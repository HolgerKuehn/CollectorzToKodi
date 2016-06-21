// <copyright file="CSubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;

    /// <summary>
    /// Class managing Subtitles
    /// </summary>
    public class CSubTitleFile : CMediaFile
    {
        #region Attributes

        /// <summary>
        /// SubTitle containing this SubTitleFile
        /// </summary>
        private CSubTitle subTitle;

        /// <summary>
        /// index of SubTitle file
        /// </summary>
        private int fileIndex;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CSubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.fileIndex = 1;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSubTitleFile"/> class.
        /// </summary>
        /// <param name="subTitle">SubTitle this File is assigned to</param>
        public CSubTitleFile(CSubTitle subTitle)
            : this(subTitle.Configuration)
        {
            this.subTitle = subTitle;
            this.fileIndex = 1;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SubTitle containing this SubTitleFile
        /// </summary>
        public CSubTitle SubTitle
        {
            get { return this.subTitle; }
            set { this.subTitle = value; }
        }

        /// <summary>
        /// Gets or sets file index of SubTiltle
        /// </summary>
        public int FileIndex
        {
            get { return this.fileIndex; }
            set { this.fileIndex = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override CMediaFile Clone()
        {
            CSubTitleFile subTitleFileClone = new CSubTitleFile(this.Configuration);
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.URL = this.URL;
            subTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            subTitleFileClone.Filename = this.Filename;
            subTitleFileClone.Extension = this.Extension;
            subTitleFileClone.Server = this.Server;
            subTitleFileClone.Media = this.Media;
            subTitleFileClone.SubTitle = this.SubTitle;
            subTitleFileClone.FileIndex = this.FileIndex;

            return (CMediaFile)subTitleFileClone;
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Movies
        /// </summary>
        /// <param name="movie">Movie, the SubTilte is referring to</param>
        public void SetFilename(CMovie movie)
        {
            this.Filename = movie.Filename + " part " + ("0000" + this.fileIndex.ToString()).Substring(this.fileIndex.ToString().Length) + this.Extension;
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Series
        /// </summary>
        /// <param name="series">Series, the SubTilte is referring to</param>
        public void SetFilename(CSeries series)
        {
            this.Filename = series.Filename + " part " + ("0000" + this.fileIndex.ToString()).Substring(this.fileIndex.ToString().Length) + this.Extension;
        }

        /// <summary>
        /// Writes subtitle data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        /// <remarks>If video contains SRT-subtitles, the SRT-files are created as well</remarks>
        public virtual void WriteSubTitleStreamDataToNFO(StreamWriter swrNFO)
        {
            // for generic SubTitle, no specific action is required here
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

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(false);
        }

        #endregion
    }
}
