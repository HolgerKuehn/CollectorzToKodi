// <copyright file="SubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Class managing Subtitles
    /// </summary>
    public class SubTitleFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// SubTitle containing this SubTitleFile
        /// </summary>
        private SubTitle subTitle;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SubTitleFile(Configuration configuration)
            : base(configuration)
        {
            this.subTitle = null;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SubTitle containing this SubTitleFile
        /// </summary>
        public SubTitle SubTitle
        {
            get { return this.subTitle; }
            set { this.subTitle = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            SubTitleFile subTitleFileClone = new SubTitleFile(this.Configuration);
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.UrlForMediaStorage = this.UrlForMediaStorage;
            subTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            subTitleFileClone.Filename = this.Filename;
            subTitleFileClone.Extension = this.Extension;
            subTitleFileClone.Server = this.Server;
            subTitleFileClone.Media = this.Media;
            subTitleFileClone.SubTitle = this.SubTitle;
            subTitleFileClone.FileIndex = this.FileIndex;

            return (SubTitleFile)subTitleFileClone;
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
        public virtual void WriteSubTitleToSH(StreamWriter swrSH)
        {
            // copy SubTitleFile to destination, as it's converted during Export (Offset-times, etc.)
            // valid for all types of SubTitles
            if (this.Filename != string.Empty)
            {
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + "\" \"" + this.Filename + "\"");
            }
        }

        /// <summary>
        /// consolidates multiple SubTitleFiles into one, as one MediaFile can only have one SubTitleFile (multiple one will be overwritten due to the same filename)
        /// <remarks>creates new List with just one SubTitleFile and sets this</remarks>
        /// </summary>
        /// <param name="subTitleFile">subTitleFile that should be extended</param>
        /// <returns>new subTitleFile extended by this object</returns>
        public virtual SubTitleFile CreateFinalSubTitleFile(SubTitleFile subTitleFile)
        {
            // leave object unchanged for generic SubTitle
            return subTitleFile;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(false);
        }

        #endregion
    }
}
