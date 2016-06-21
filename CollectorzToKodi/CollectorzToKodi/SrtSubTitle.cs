// <copyright file="SrtSubTitle.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing SrtSubTitles
    /// </summary>
    public class SrtSubTitle : CSubTitle
    {
        #region Attributes
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SrtSubTitle"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SrtSubTitle(CConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties
        #endregion
        #region Functions

        /// <summary>
        /// clones SubTitle
        /// </summary>
        /// <returns>new instance of SrtSubTitle</returns>
        public override CSubTitle Clone()
        {
            return base.Clone();
        }

        /// <inheritdoc/>
        public override List<CSubTitleFile> ReadSubTitleFile(XmlNode xMLMedia)
        {
            List<CSubTitleFile> lstSubTitleFiles = new List<CSubTitleFile>();

            foreach (XmlNode xMLSubTitleStreamFile in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                // check all links for subtitle in language
                if ((xMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie") && xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel." + this.Language + "."))
                {
                    // create new subtitle objects (generic Subtitle or SRT-Subtitle)
                    CSrtSubTitleFile srtSubTitleFile = new CSrtSubTitleFile(this.Configuration);

                    // name and filenames
                    srtSubTitleFile.Description = xMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                    srtSubTitleFile.URL = xMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                    srtSubTitleFile.ConvertFilename();

                    // check for fileIndex
                    int completeLength = srtSubTitleFile.Description.Length;
                    int subtitleLength = ("Untertitel." + this.Language + ".").Length;
                    int fileIndex = 1;

                    if (!int.TryParse(srtSubTitleFile.Description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out fileIndex))
                    {
                        fileIndex = 1;
                    }

                    // subtitle file name and type
                    if (srtSubTitleFile.Extension.Contains(".srt"))
                    {
                        while (lstSubTitleFiles.Count < fileIndex)
                        {
                            lstSubTitleFiles.Add(null);
                        }

                        if (lstSubTitleFiles[fileIndex - 1] == null)
                        {
                            lstSubTitleFiles[fileIndex - 1] = new CSrtSubTitleFile(this.Configuration);
                        }

                        ((CSrtSubTitleFile)lstSubTitleFiles[fileIndex - 1]).ReadFromSubTitleFile(srtSubTitleFile, fileIndex);
                    }
                }
            }

            return lstSubTitleFiles;
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Movies
        /// </summary>
        /// <param name="movie">Movie, the SubTilte is referring to</param>
        public override void SetFilename(CMovie movie)
        {
            base.SetFilename(movie);
        }

        /// <summary>
        /// sets filename for SubTitleFile suitable for Series
        /// </summary>
        /// <param name="series">Series, the SubTilte is referring to</param>
        public override void SetFilename(CSeries series)
        {
            base.SetFilename(series);
        }

        /// <summary>
        /// Writes subtitle data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        /// <remarks>If video contains SRT-subtitles, the SRT-files are created as well</remarks>
        public override void WriteSubTitleStreamDataToNFO(StreamWriter swrNFO)
        {
            base.WriteSubTitleStreamDataToNFO(swrNFO);
        }

        /// <summary>
        /// adds Subtitle to Shell-Script
        /// </summary>
        /// <param name="swrSH">Bash-Shell-Script</param>
        public override void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            base.WriteSubTitleStreamDataToSH(swrSH);
        }

        #endregion
    }
}
