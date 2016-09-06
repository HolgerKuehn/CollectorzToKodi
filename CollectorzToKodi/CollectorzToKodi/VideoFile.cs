// <copyright file="VideoFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
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
            videoFileClone.IsSpecial = this.IsSpecial;
            videoFileClone.Description = this.Description;
            videoFileClone.URL = this.URL;
            videoFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            videoFileClone.Filename = this.Filename;
            videoFileClone.Extension = this.Extension;
            videoFileClone.Server = this.Server;
            videoFileClone.Media = this.Media;
            videoFileClone.FileIndex = this.FileIndex;

            foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
            {
                videoFileClone.SubTitleFiles.Add((SubTitleFile)subTitleFile.Clone());
            }

            return (VideoFile)videoFileClone;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            string filename = this.ConvertFilename(true);
            this.SetFilename();
            return filename;
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

            foreach (SubTitle subTitle in ((Video)this.Media).SubTitles)
            {
                foreach (XmlNode xMLSubTitleStreamFile in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
                {
                    // check all links for subtitle in language
                    if ((xMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie") && xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel." + subTitle.Language + "."))
                    {
                        // create new subtitle objects
                        SrtSubTitleFile srtSubTitleFile = new SrtSubTitleFile(this.Configuration);
                        srtSubTitleFile.Media = this.Media;
                        srtSubTitleFile.SubTitle = subTitle;

                        // name and filenames
                        srtSubTitleFile.Description = xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                        srtSubTitleFile.URL = xMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                        srtSubTitleFile.ConvertFilename();

                        // check for fileIndex
                        int completeLength = srtSubTitleFile.Description.Length;
                        int subtitleLength = ("Untertitel." + subTitle.Language + ".").Length;
                        int fileIndex = 1;

                        if (!int.TryParse(srtSubTitleFile.Description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out fileIndex))
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

        /// <summary>
        /// adds Subtitle to Shell-Script
        /// </summary>
        /// <param name="swrSH">Bash-Shell-Script</param>
        public void WriteSubTitleToSH(StreamWriter swrSH)
        {
            this.CreateFinalSubTitleFile();

            foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.WriteSubTitleToSH(swrSH);
            }
        }

        /// <summary>
        /// consolidates multiple SubTitleFiles into one, as one MediaFile can only have one SubTitleFile (multiple one will be overwritten due to the same filename)
        /// <remarks>creates new List with just one SubTitleFile and sets this</remarks>
        /// </summary>
        private void CreateFinalSubTitleFile()
        {
            // check, if transformation is necessary (more than one SubTitleFiles)
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
                this.SubTitleFiles = new List<SubTitleFile>();
                this.SubTitleFiles.Add(extendedSubTitleFile);
            }
        }

        /// <summary>
        /// sets Filenames according to media-type for all corresponding files
        /// </summary>
        private void SetFilename()
        {
            foreach (SubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.Filename = this.Filename;

                if (this.Extension != string.Empty)
                {
                    subTitleFile.Filename = this.Filename.Replace(this.Extension, string.Empty);
                }

                subTitleFile.Filename = subTitleFile.Filename + subTitleFile.Extension;
            }
        }

        #endregion
    }
}
