// <copyright file="SubTitle.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing SubTitles
    /// </summary>
    public class SubTitle
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// language provides by subtitle
        /// </summary>
        private string language;

        /// <summary>
        /// Video containing subtitle
        /// </summary>
        private Video video;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTitle"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SubTitle(Configuration configuration)
        {
            this.language = "Deutsch";
            this.video = null;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets language provides by subtitle
        /// </summary>
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        /// <summary>
        /// Gets or sets video containing file
        /// </summary>
        public Video Video
        {
            get { return this.video; }
            set { this.video = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones SubTitle
        /// </summary>
        /// <returns>new instance of CSubTitle</returns>
        public virtual SubTitle Clone()
        {
            SubTitle subTitleClone = new SubTitle(this.Configuration);
            subTitleClone.Language = this.Language;
            subTitleClone.Video = this.Video;

            return (SubTitle)subTitleClone;
        }

        /// <summary>
        /// Writes subtitle data to provided NFO file
        /// </summary>
        /// <param name="nfoStreamWriter">NFO file that the stream information should be added to</param>
        /// <remarks>If video contains SRT-subtitles, the SRT-files are created as well</remarks>
        public virtual void WriteSubTitleStreamDataToLibrary(StreamWriter nfoStreamWriter)
        {
            nfoStreamWriter.WriteLine("            <subtitle>");
            nfoStreamWriter.WriteLine("                <language>" + this.Language + "</language>");
            nfoStreamWriter.WriteLine("            </subtitle>");
        }

        #endregion
    }
}
