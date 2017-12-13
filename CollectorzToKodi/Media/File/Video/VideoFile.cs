// <copyright file="VideoFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage video files
    /// </summary>
    public class VideoFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// indicates whether the video files is a special or not
        /// </summary>
        private bool isSpecial;

        /// <summary>
        /// list of subtitles available in video; specific to this MediaFile
        /// </summary>
        private List<SubTitleFile> subTitleFiles;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public VideoFile(Configuration configuration)
            : base(configuration)
        {
            this.isSpecial = false;
        }

        #endregion
        #region Properties

        /// <inheritdoc/>
        public override MediaFilePath Server
        {
            get
            {
                if (base.Server == null)
                {
                    base.Server = new MediaFilePath(this.Configuration);
                }

                return base.Server;
            }

            set
            {
                base.Server = value;

                this.Media.AddServer(this.Server);
            }
        }

        /// <inheritdoc/>
        public override string Filename
        {
            get
            {
                return base.Server.Filename;
            }

            set
            {
                base.Server.Filename = value;

                foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
                {
                    subTitleFile.Server.Filename = this.Server.Filename;

                    if (this.Extension != string.Empty)
                    {
                        subTitleFile.Server.Filename = this.Server.Filename.Replace(this.Extension, string.Empty);
                    }

                    subTitleFile.Server.Filename = subTitleFile.Server.Filename + subTitleFile.Extension;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the video files is a special or not
        /// </summary>
        public bool IsSpecial
        {
            get { return this.isSpecial; }
            set { this.isSpecial = value; }
        }

        /// <summary>
        /// Gets or sets list of subtitles available in video
        /// </summary>
        public List<SubTitleFile> SubTitleFiles
        {
            get
            {
                if (this.subTitleFiles == null)
                {
                    this.subTitleFiles = new List<SubTitleFile>();
                }

                return this.subTitleFiles;
            }

            set
            {
                this.subTitleFiles = value;
            }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            VideoFile videoFileClone = new VideoFile(this.Configuration);

            videoFileClone.Description = this.Description;
            videoFileClone.ServerDevicePathForPublication = this.ServerDevicePathForPublication;
            videoFileClone.Extension = this.Extension;
            videoFileClone.FileIndex = this.FileIndex;
            videoFileClone.IsSpecial = this.IsSpecial;

            foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
            {
                videoFileClone.SubTitleFiles.Add((SubTitleFile)subTitleFile.Clone());
            }

            videoFileClone.Media = this.Media;
            videoFileClone.Server = this.Server;
            videoFileClone.Server.Filename = this.Server.Filename;

            return (VideoFile)videoFileClone;
        }

        /// <summary>
        /// Sets Special from special tags present in title
        /// </summary>
        /// <param name="title">title of image, containing tags for specials</param>
        /// <returns>title of image cleaned of tags for specials</returns>
        public string OverrideSpecial(string title)
        {
            if (title.Contains("(Special)"))
            {
                this.isSpecial = true;
                title = title.Replace("(Special)", string.Empty);
            }

            return title.Trim();
        }

        /// <summary>
        /// checks XML-file from MovieCollector for SubTitles linked to the Video and sets them for this VideoFile
        /// </summary>
        /// <param name="xMLMedia">XML-file from MovieCollector</param>
        public void ReadSubTitleFile(XmlNode xMLMedia)
        {
            List<SubTitleFile> lstSubTitleFiles = new List<SubTitleFile>();

            foreach (SubTitleStream subTitle in ((Video)this.Media).SubTitles)
            {
                foreach (XmlNode xMLSubTitleStreamFile in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
                {
                    // check all links for subtitle in language
                    if ((xMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie") && xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel." + subTitle.Language + "."))
                    {
                        // create new subtitle objects
                        SrtSubTitleStream srtSubTitleFile = new SrtSubTitleStream(this.Configuration)
                        {
                            Media = this.Media,
                            SubTitle = subTitle,

                            // name and filenames
                            Description = xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty),
                            ServerDevicePathForPublication = xMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty)
                        };
                        srtSubTitleFile.ConvertFilename();

                        // check for fileIndex
                        int completeLength = srtSubTitleFile.Description.Length;
                        int subtitleLength = ("Untertitel." + subTitle.Language + ".").Length;

                        if (!int.TryParse(srtSubTitleFile.Description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out int fileIndex))
                        {
                            fileIndex = 1;
                        }

                        // subtitle file name and type
                        if (srtSubTitleFile.Extension.Contains(".srt") && this.FileIndex == fileIndex)
                        {
                            srtSubTitleFile.FileIndex = fileIndex;
                            srtSubTitleFile.ReadFromSubTitleFile();

                            lstSubTitleFiles.Add(srtSubTitleFile);
                        }
                    }
                }
            }

            this.SubTitleFiles.AddRange(lstSubTitleFiles);
        }

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            this.CreateFinalSubTitleFile();

            foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.WriteToLibrary();
            }
        }

        /// <summary>
        /// consolidates multiple SubTitleFiles into one, as one MediaFile can only have one SubTitleFile (multiple one will be overwritten due to the same filename)
        /// <remarks>creates new List with just one SubTitleFile and sets this</remarks>
        /// </summary>
        private void CreateFinalSubTitleFile()
        {
            // check, if transformation is necessary (more than one SubTitleFile)
            int numberOfSubTitleFiles = 0;

            if (this.SubTitleFiles != null)
            {
                numberOfSubTitleFiles = this.SubTitleFiles.Count;
            }

            // create new SubTitleFile
            if (numberOfSubTitleFiles > 1)
            {
                SubTitleFile firstSubTitleFile = this.SubTitleFiles[0];
                SubTitleFile extendedSubTitleFile = (SubTitleFile)firstSubTitleFile.Clone();

                for (int i = 1; i < this.SubTitleFiles.Count; i++)
                {
                    extendedSubTitleFile = ((SubTitleFile)this.SubTitleFiles[i]).CreateFinalSubTitleFile(extendedSubTitleFile);
                }

                // reset SubTitleFiles with one new File
                this.SubTitleFiles = new List<SubTitleFile>
                {
                    extendedSubTitleFile
                };
            }
        }

        #endregion
    }
}
