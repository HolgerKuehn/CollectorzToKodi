// <copyright file="CSrtSubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;

    /// <summary>
    /// Extension for SRT-Files
    /// </summary>
    public class CSrtSubTitleFile : CSrtSubTitleFileCollection
    {
        #region Attributes

        /// <summary>
        /// SRT-Entries
        /// </summary>
        private List<CSrtSubTitleFileEntry> subTitleEntries;

        /// <summary>
        /// basic time offset for subtitle entries
        /// </summary>
        private TimeSpan offsetTime;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CSrtSubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CSrtSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.subTitleEntries = new List<CSrtSubTitleFileEntry>();

            TimeSpan offsetTime;
            TimeSpan.TryParse("00:00:00.000", out offsetTime);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSrtSubTitleFile"/> class.
        /// converts plain subtitle into SRT-subtitle
        /// </summary>
        /// <param name="subTitleFile">plain SRT-subtitle to be converted to SRT-subtitle</param>
        public CSrtSubTitleFile(CSubTitleFile subTitleFile)
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
        public List<CSrtSubTitleFileEntry> SubTitleEntries
        {
            get { return this.subTitleEntries; }
            set { this.subTitleEntries = value; }
        }

        /// <summary>
        /// Gets or sets basic time offset for subtitle entries
        /// </summary>
        private TimeSpan OffsetTime
        {
            get { return this.offsetTime; }
            set { this.offsetTime = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// transfers actualized data from subTitleFile
        /// </summary>
        /// <param name="subTitleFile">Subtitle file added to Media</param>
        /// <param name="fileIndex">Index of multiple files</param>
        public void ReadFromSubTitleFile(CSubTitleFile subTitleFile, int fileIndex)
        {
            TimeSpan timOffsetTime;
            string strOffsetTime;

            // reading basic information
            this.Description = subTitleFile.Description;
            strOffsetTime = this.Description.RightOf("(Offset ").LeftOf(")");
            this.Description = this.Description.Replace("(Offset " + strOffsetTime + ")", string.Empty);

            this.URL = subTitleFile.URL;
            this.URLLocalFilesystem = subTitleFile.URLLocalFilesystem;
            this.Filename = subTitleFile.Filename;
            this.Extension = subTitleFile.Extension;
            this.FileIndex = fileIndex;

            TimeSpan.TryParse(strOffsetTime, out timOffsetTime);

            this.OffsetTime = timOffsetTime;

            // reading subtitle content
            if (this.URL == string.Empty || !File.Exists(this.URL))
            {
                return;
            }

            using (StreamReader srdSrtFile = new StreamReader(this.URL, Encoding.UTF8))
            {
                CConfiguration.SrtSubTitleLineType lineType = default(CConfiguration.SrtSubTitleLineType);
                CSrtSubTitleFileEntry srtSubTitleFileEntry = new CSrtSubTitleFileEntry();
                lineType = CConfiguration.SrtSubTitleLineType.EntryNumber;

                while (true)
                {
                    string srtLine = srdSrtFile.ReadLine();
                    if (srtLine == null)
                    {
                        break;
                    }

                    srtSubTitleFileEntry.OffsetTime = this.OffsetTime;

                    // end of SubTitleLines
                    if (lineType == CConfiguration.SrtSubTitleLineType.SubTitles && srtLine == string.Empty)
                    {
                        this.SubTitleEntries.Add(srtSubTitleFileEntry);
                        lineType = CConfiguration.SrtSubTitleLineType.EmptyLine;
                    }

                    // SubTitleLines
                    if (lineType == CConfiguration.SrtSubTitleLineType.SubTitles && srtLine != string.Empty)
                    {
                        srtSubTitleFileEntry.SubTitleLines.Add(srtLine);
                        lineType = CConfiguration.SrtSubTitleLineType.SubTitles;
                    }

                    // second Line -> Times
                    if (lineType == CConfiguration.SrtSubTitleLineType.Times)
                    {
                        string strStartTime = srtLine.Substring(0, 12).Replace(",", ".");
                        string strEndTime = srtLine.Substring(17, 12).Replace(",", ".");
                        string timeExtentions = string.Empty;
                        if (srtLine.Length > 30)
                        {
                            timeExtentions = srtLine.Substring(30, srtLine.Length);
                        }

                        TimeSpan timStartTime;
                        TimeSpan timEndTime;

                        TimeSpan.TryParse(strStartTime, out timStartTime);
                        TimeSpan.TryParse(strEndTime, out timEndTime);

                        srtSubTitleFileEntry.StartTime = timStartTime;
                        srtSubTitleFileEntry.EndTime = timEndTime;
                        srtSubTitleFileEntry.TimeExtentions = timeExtentions;

                        lineType = CConfiguration.SrtSubTitleLineType.SubTitles;
                    }

                    // first Line -> EntryNumber
                    if (lineType == CConfiguration.SrtSubTitleLineType.EntryNumber)
                    {
                        strOffsetTime = srtLine.RightOf("(Offset ").LeftOf(")");
                        srtLine = srtLine.Replace("(Offset " + strOffsetTime + ")", string.Empty);

                        if (strOffsetTime != string.Empty)
                        {
                            TimeSpan.TryParse(strOffsetTime, out timOffsetTime);
                            this.OffsetTime = timOffsetTime;
                        }

                        srtSubTitleFileEntry.EntryNumber = int.Parse(srtLine);
                        lineType = CConfiguration.SrtSubTitleLineType.Times;
                    }

                    if (lineType == CConfiguration.SrtSubTitleLineType.EmptyLine)
                    {
                        srtSubTitleFileEntry = new CSrtSubTitleFileEntry();
                        lineType = CConfiguration.SrtSubTitleLineType.EntryNumber;
                    }
                }
            }
        }

        /// <summary>
        /// writes new SRT-file
        /// </summary>
        public override void WriteSrtSubTitleStreamDataToSRT()
        {
            using (StreamWriter swrSrtSubTitle = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename, false, Encoding.UTF8, 512))
            {
                int entryNumber = 0;

                foreach (CSrtSubTitleFileEntry srtSubTitleFileEntry in this.SubTitleEntries)
                {
                    // resets Entry-Number
                    entryNumber++;
                    srtSubTitleFileEntry.EntryNumber = entryNumber;

                    // write entry to file
                    srtSubTitleFileEntry.WriteSrtSubTitleStreamDataToSRT(swrSrtSubTitle);
                }
            }
        }

        /// <summary>
        /// writes subtitle to SH
        /// </summary>
        /// <param name="swrSH">StreamWriter used to write script to</param>
        public override void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            if (this.Filename != string.Empty)
            {
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/CollectorzToKodi.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + "\" \"" + this.Filename + "\"");
            }
        }

        #endregion
    }
}
