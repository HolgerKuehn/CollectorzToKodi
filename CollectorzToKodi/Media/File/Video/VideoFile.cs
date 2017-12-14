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
        private List<SubTitleStream> subTitleStreams;

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
        public List<SubTitleStream> SubTitleFiles
        {
            get
            {
                if (this.subTitleStreams == null)
                {
                    this.subTitleStreams = new List<SubTitleStream>();
                }

                return this.subTitleStreams;
            }

            set
            {
                this.subTitleStreams = value;
            }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            VideoFile videoFileClone = new VideoFile(this.Configuration);
            
            // MediaFile
            videoFileClone.Description = this.Description;
            videoFileClone.MediaFilePath = this.MediaFilePath.Clone();
            videoFileClone.Media = this.Media;
            videoFileClone.Server = this.Server.Clone();
            videoFileClone.FileIndex = this.FileIndex;

            // VideoFile
            videoFileClone.IsSpecial = this.IsSpecial;

            foreach (SubTitleStream subTitleFile in this.SubTitleFiles)
            {
                videoFileClone.SubTitleFiles.Add((SubTitleStream)subTitleFile.Clone());
            }

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
            List<SubTitleStream> lstSubTitleFiles = new List<SubTitleStream>();

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

            foreach (SubTitleStream subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.WriteToLibrary();
            }
        }

        /// <summary>
        /// consolidates multiple SubTitleFiles into one, as one MediaFile can only have one SubTitleStream (multiple one will be overwritten due to the same filename)
        /// <remarks>creates new List with just one SubTitleStream and sets this</remarks>
        /// </summary>
        private void CreateFinalSubTitleFile()
        {
            // check, if transformation is necessary (more than one SubTitleStream)
            int numberOfSubTitleFiles = 0;

            if (this.SubTitleFiles != null)
            {
                numberOfSubTitleFiles = this.SubTitleFiles.Count;
            }

            // create new SubTitleStream
            if (numberOfSubTitleFiles > 1)
            {
                SubTitleStream firstSubTitleFile = this.SubTitleFiles[0];
                SubTitleStream extendedSubTitleFile = (SubTitleStream)firstSubTitleFile.Clone();

                for (int i = 1; i < this.SubTitleFiles.Count; i++)
                {
                    extendedSubTitleFile = ((SubTitleStream)this.SubTitleFiles[i]).CreateFinalSubTitleFile(extendedSubTitleFile);
                }

                // reset SubTitleFiles with one new File
                this.SubTitleFiles = new List<SubTitleStream>
                {
                    extendedSubTitleFile
                };
            }
        }

        #endregion
    }
}
