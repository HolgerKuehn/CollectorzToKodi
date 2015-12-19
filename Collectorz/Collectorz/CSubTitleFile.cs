// <copyright file="CSubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace Collectorz
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing Subtitles
    /// </summary>
    public class CSubTitleFile : CMediaFile
    {
        #region Attributes

        /// <summary>
        /// language provides by subtitle
        /// </summary>
        private string language;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CSubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for Collectorz programs and Kodi</param>
        public CSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.language = "Deutsch";
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets language provides by subtitle
        /// </summary>
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override CMediaFile Clone()
        {
            CSubTitleFile subTitleFileClone = new CSubTitleFile(this.Configuration);
            subTitleFileClone.Language = this.Language;
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.URL = this.URL;
            subTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            subTitleFileClone.Filename = this.Filename;
            subTitleFileClone.Extension = this.Extension;
            subTitleFileClone.Server = this.Server;
            subTitleFileClone.Media = this.Media;

            return (CMediaFile)subTitleFileClone;
        }

        /// <summary>
        /// checks XML-file from MovieCollector for Subtitles linked to the Video
        /// </summary>
        /// <param name="xMLMedia">XML-file from MovieCollector</param>
        /// <returns>Subtitle</returns>
        public CMediaFile CheckForSubTitleStreamFile(XmlNode xMLMedia)
        {
            CSrtSubTitleFileCollection srtSubTitleFileCollection = null;
            CSubTitleFile subTitleFile = null;
            bool isSrtSubTitleFile = false;

            foreach (XmlNode xMLSubTitleStreamFile in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                // check all links for subtitle in language
                if ((xMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie") && xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel." + this.Language + "."))
                {
                    isSrtSubTitleFile = false;

                    // create new subtitle objects (generic Subtitle or SRT-Subtitle)
                    subTitleFile = (CSubTitleFile)this.Clone();

                    // name and filenames
                    subTitleFile.Description = xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                    subTitleFile.URL = xMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                    subTitleFile.ConvertFilename();

                    // check for fileIndex
                    int completeLength = subTitleFile.Description.Length;
                    int subtitleLength = ("Untertitel." + this.Language + ".").Length;
                    int fileIndex = 1;

                    if (!int.TryParse(subTitleFile.Description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out fileIndex))
                    {
                        fileIndex = 1;
                    }

                    // subtitle file name and type
                    if (subTitleFile.Extension.Contains(".srt"))
                    {
                        isSrtSubTitleFile = true;
                        if (srtSubTitleFileCollection == null)
                        {
                            srtSubTitleFileCollection = new CSrtSubTitleFileCollection(this);
                        }

                        subTitleFile.Filename = subTitleFile.Media.Filename + " part " + ("0000" + fileIndex.ToString()).Substring(fileIndex.ToString().Length) + subTitleFile.Extension;

                        while (srtSubTitleFileCollection.SubTitleFiles.Count < fileIndex)
                        {
                            srtSubTitleFileCollection.SubTitleFiles.Add(null);
                        }

                        if (srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1] == null)
                        {
                            srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1] = new CSrtSubTitleFile(subTitleFile);
                        }

                        srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1].ReadFromSubTitleFile(subTitleFile, fileIndex);
                    }
                }
            }

            // collecting subs and return them as CMediaFiles
            if (isSrtSubTitleFile)
            {
                return (CMediaFile)srtSubTitleFileCollection;
            }
            else
            {
                return (CMediaFile)subTitleFile;
            }
        }

        /// <summary>
        /// adds Subtitle to Shell-Script
        /// </summary>
        /// <param name="swrSH">Bash-Shell-Script</param>
        public virtual void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            if (this.Filename != string.Empty)
            {
                swrSH.WriteLine("/bin/ln -s \"" + this.URLLocalFilesystem + "\" \"" + this.Filename + "\"");
            }
        }

        #endregion
    }
}
