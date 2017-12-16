// <copyright file="VideoFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
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
        public List<SubTitleStream> SubTitleStreams
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

            foreach (SubTitleStream subTitleFile in this.SubTitleStreams)
            {
                videoFileClone.SubTitleStreams.Add((SubTitleStream)subTitleFile.Clone());
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

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            List<SubTitleStream> lstSubTitleSteams = new List<SubTitleStream>();

            foreach (SubTitleStream subTitleStream in ((Video)this.Media).SubTitles)
            {
                // SrtSubTitleStream for language
                SrtSubTitleStream srtSubTitleStream = new SrtSubTitleStream(this.Configuration);
                srtSubTitleStream.Media = this.Media;
                srtSubTitleStream.Language = subTitleStream.Language;
                srtSubTitleStream.SourceSubTitleFiles = null;
                srtSubTitleStream.DestinationSubTitleFile = null;

                // search Links for SubTitleFiles for SubTitleStream
                foreach (XmlNode xMLSubTitleStream in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
                {
                    // check all links for subtitle in language
                    if ((xMLSubTitleStream.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie") && xMLSubTitleStream.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel." + subTitleStream.Language + "."))
                    {
                        string description = xMLSubTitleStream.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                        string windowsPath = xMLSubTitleStream.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                        int completeLength = 0;
                        int subtitleLength = 0;
                        int fileIndex;

                        // checking, if it's an srt SubTitle
                        if (windowsPath.EndsWith(".srt"))
                        {
                            // create new SubTitleFile
                            SubTitleFile srtSubTitleFile = new SubTitleFile(this.Configuration);

                            // MediaFile
                            srtSubTitleFile.Description = description;
                            srtSubTitleFile.Media = this.Media;
                            srtSubTitleFile.Server = this.Server;
                            srtSubTitleFile.MediaFilePath.WindowsPath = windowsPath;
                            srtSubTitleFile.MediaFilePath.Filename = this.MediaFilePath.Filename;
                            srtSubTitleFile.FileIndex = this.FileIndex;

                            // SubTitleFile
                            srtSubTitleFile.SubTitleStream = srtSubTitleStream;

                            // check for fileIndex
                            completeLength = description.Length;
                            subtitleLength = ("Untertitel." + subTitleStream.Language + ".").Length;

                            if (!int.TryParse(description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out fileIndex))
                            {
                                fileIndex = 1;
                            }

                            if (this.FileIndex == fileIndex)
                            {
                                srtSubTitleStream.SourceSubTitleFiles.Add(srtSubTitleFile);
                            }
                        }

                        /* else if windowsPath.EndsWith(".ass") {} */
                    }
                }

                // add SubTitleStreams per Language
                lstSubTitleSteams.Add(srtSubTitleStream);
            }

            // assign SubTitleStreams to VideoFile
            this.SubTitleStreams.AddRange(lstSubTitleSteams);

            // read SubTitleFiles from SourceFiles
            foreach (SubTitleStream subTitleStream in this.SubTitleStreams)
            {
                subTitleStream.ReadFromXml(xMLMedia);
            }
        }

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            foreach (SubTitleStream subTitleStream in this.SubTitleStreams)
            {
                subTitleStream.WriteToLibrary();
            }
        }

        #endregion
    }
}
