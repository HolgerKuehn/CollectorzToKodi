// <copyright file="CSrtSubTitleFileCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class collects multiple SRT-subtitle files for one (stacked) movie
    /// </summary>
    public class CSrtSubTitleFileCollection : CSubTitleFile
    {
        #region Attributes

        /// <summary>
        /// list of collected subtitle files
        /// </summary>
        private List<CSrtSubTitleFile> subTitleFiles;

        /// <summary>
        /// index of SRT-subtitle file in collection
        /// </summary>
        private int fileIndex;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CSrtSubTitleFileCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CSrtSubTitleFileCollection(CConfiguration configuration)
            : base(configuration)
        {
            this.subTitleFiles = new List<CSrtSubTitleFile>();
            this.fileIndex = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSrtSubTitleFileCollection"/> class.
        /// converts plain subtitle into SRT-subtitle
        /// </summary>
        /// <param name="subTitleFile">plain SRT-subtitle to be converted to SRT-subtitle</param>
        public CSrtSubTitleFileCollection(CSubTitleFile subTitleFile)
            : this(subTitleFile.Configuration)
        {
            this.Language = subTitleFile.Language;
            this.Description = subTitleFile.Description;
            this.URL = subTitleFile.URL;
            this.URLLocalFilesystem = subTitleFile.URLLocalFilesystem;
            this.Filename = subTitleFile.Filename;
            this.Extension = subTitleFile.Extension;
            this.Server = subTitleFile.Server;
            this.Media = subTitleFile.Media;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SRT-Entries
        /// </summary>
        public List<CSrtSubTitleFile> SubTitleFiles
        {
            get { return this.subTitleFiles; }
            set { this.subTitleFiles = value; }
        }

        /// <summary>
        /// Gets or sets file index of SRT-SubTiltle
        /// </summary>
        public int FileIndex
        {
            get { return this.fileIndex; }
            set { this.fileIndex = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// writes new srt-file
        /// </summary>
        public virtual void WriteSrtSubTitleStreamDataToSRT()
        {
            foreach (CSrtSubTitleFile srtSubTitleFile in this.subTitleFiles)
            {
                if (srtSubTitleFile != null)
                {
                    srtSubTitleFile.WriteSrtSubTitleStreamDataToSRT();
                }
            }
        }

        /// <summary>
        /// writes subtitle to SH
        /// </summary>
        /// <param name="swrSH">StreamWriter used to write script to</param>
        public override void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            foreach (CSrtSubTitleFile srtSubTitleFile in this.subTitleFiles)
            {
                if (srtSubTitleFile != null)
                {
                    srtSubTitleFile.WriteSubTitleStreamDataToSH(swrSH);
                }
            }
        }

        #endregion
    }
}
