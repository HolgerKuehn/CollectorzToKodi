// <copyright file="CSubTitle.cs" company="Holger Kühn">
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
    public class CSubTitle
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private CConfiguration configuration;

        /// <summary>
        /// language provides by subtitle
        /// </summary>
        private string language;

        /// <summary>
        /// Video containing subtitle
        /// </summary>
        private CVideo video;

        /// <summary>
        /// list of collected subtitle files
        /// </summary>
        private List<CSubTitleFile> subTitleFiles;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CSubTitle"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CSubTitle(CConfiguration configuration)
        {
            this.language = "Deutsch";
            this.video = null;
            this.subTitleFiles = new List<CSubTitleFile>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public CConfiguration Configuration
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
        public CVideo Video
        {
            get { return this.video; }
            set { this.video = value; }
        }

        /// <summary>
        /// Gets or sets subTitleFiles part of this SubTitle
        /// </summary>
        public List<CSubTitleFile> SubTitleFiles
        {
            get { return this.subTitleFiles; }
            set { this.subTitleFiles = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones SubTitle
        /// </summary>
        /// <returns>new instance of CSubTitle</returns>
        public virtual CSubTitle Clone()
        {
            CSubTitle subTitleClone = new CSubTitle(this.Configuration);
            subTitleClone.Language = this.Language;
            subTitleClone.Video = this.Video;

            foreach (CSubTitleFile subTitleFiles in this.SubTitleFiles)
            {
                subTitleClone.SubTitleFiles.Add((CSubTitleFile)subTitleFiles.Clone());
            }

            return subTitleClone;
        }

        /// <summary>
        /// checks XML-file from MovieCollector for SubTitles linked to the Video
        /// </summary>
        /// <param name="xMLMedia">XML-file from MovieCollector</param>
        /// <returns>List of assigned SubTitleFiles</returns>
        public virtual List<CSubTitleFile> ReadSubTitleFile(XmlNode xMLMedia)
        {
            List<CSubTitleFile> lstSubTitleFile = new List<CSubTitleFile>();

            // Reading srt-SubTitles
            SrtSubTitle srtSrtSubTitle = new SrtSubTitle(this.Configuration);
            lstSubTitleFile.AddRange(srtSrtSubTitle.ReadSubTitleFile(xMLMedia));

            return new List<CSubTitleFile>();
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Movies
        /// </summary>
        /// <param name="movie">Movie, the SubTilte is referring to</param>
        public virtual void SetFilename(CMovie movie)
        {
            foreach (CSubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.SetFilename(movie);
            }
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Series
        /// </summary>
        /// <param name="series">Series, the SubTilte is referring to</param>
        public virtual void SetFilename(CSeries series)
        {
            foreach (CSubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.SetFilename(series);
            }
        }

        /// <summary>
        /// Writes subtitle data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        /// <remarks>If video contains SRT-subtitles, the SRT-files are created as well</remarks>
        public virtual void WriteSubTitleStreamDataToNFO(StreamWriter swrNFO)
        {
            swrNFO.WriteLine("            <subtitle>");
            swrNFO.WriteLine("                <language>" + this.Language + "</language>");
            swrNFO.WriteLine("            </subtitle>");

            foreach (CSubTitleFile subTitleFile in this.SubTitleFiles)
            {
                subTitleFile.WriteSubTitleStreamDataToNFO(swrNFO);
            }
        }

        /// <summary>
        /// adds Subtitle to Shell-Script
        /// </summary>
        /// <param name="swrSH">Bash-Shell-Script</param>
        public virtual void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            foreach (CSubTitleFile subTitleFile in this.subTitleFiles)
            {
                subTitleFile.WriteSubTitleStreamDataToSH(swrSH);
            }
        }

        #endregion
    }
}
