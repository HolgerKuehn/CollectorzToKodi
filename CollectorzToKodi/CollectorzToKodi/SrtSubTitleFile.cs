// <copyright file="SrtSubTitleFile.cs" company="Holger Kühn">
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
    public class SrtSubTitleFile : SubTitleFile
    {
        #region Attributes

        /// <summary>
        /// SRT-Entries
        /// </summary>
        private List<SrtSubTitleFileEntry> subTitleEntries;

        /// <summary>
        /// basic time offset for subtitle entries
        /// </summary>
        private TimeSpan offsetTime;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SrtSubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SrtSubTitleFile(Configuration configuration)
            : base(configuration)
        {
            this.subTitleEntries = new List<SrtSubTitleFileEntry>();

            TimeSpan offsetTime;
            TimeSpan.TryParse("00:00:00.000", out offsetTime);
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SRT-Entries
        /// </summary>
        public List<SrtSubTitleFileEntry> SubTitleEntries
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

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            SrtSubTitleFile srtSubTitleFileClone = new SrtSubTitleFile(this.Configuration);
            srtSubTitleFileClone.Description = this.Description;
            srtSubTitleFileClone.URL = this.URL;
            srtSubTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            srtSubTitleFileClone.Filename = this.Filename;
            srtSubTitleFileClone.Extension = this.Extension;
            srtSubTitleFileClone.Server = this.Server;
            srtSubTitleFileClone.Media = this.Media;
            srtSubTitleFileClone.SubTitle = this.SubTitle;
            srtSubTitleFileClone.FileIndex = this.FileIndex;

            // clone itself if necessary
            srtSubTitleFileClone.SubTitleEntries = this.SubTitleEntries;
            srtSubTitleFileClone.OffsetTime = this.OffsetTime;

            return (SrtSubTitleFile)srtSubTitleFileClone;
        }

        /// <summary>
        /// transfers actualized data from subTitleFile
        /// </summary>
        /// <param name="subTitleFile">Subtitle file added to Media</param>
        /// <param name="fileIndex">Index of multiple files</param>
        public void ReadFromSubTitleFile(SubTitleFile subTitleFile, int fileIndex)
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
                Configuration.SrtSubTitleLineType lineType = default(Configuration.SrtSubTitleLineType);
                SrtSubTitleFileEntry srtSubTitleFileEntry = new SrtSubTitleFileEntry();
                lineType = Configuration.SrtSubTitleLineType.EntryNumber;

                while (true)
                {
                    string srtLine = srdSrtFile.ReadLine();
                    if (srtLine == null)
                    {
                        break;
                    }

                    srtSubTitleFileEntry.OffsetTime = this.OffsetTime;

                    // end of SubTitleLines
                    if (lineType == Configuration.SrtSubTitleLineType.SubTitles && srtLine == string.Empty)
                    {
                        this.SubTitleEntries.Add(srtSubTitleFileEntry);
                        lineType = Configuration.SrtSubTitleLineType.EmptyLine;
                    }

                    // SubTitleLines
                    if (lineType == Configuration.SrtSubTitleLineType.SubTitles && srtLine != string.Empty)
                    {
                        srtSubTitleFileEntry.SubTitleLines.Add(srtLine);
                        lineType = Configuration.SrtSubTitleLineType.SubTitles;
                    }

                    // second Line -> Times
                    if (lineType == Configuration.SrtSubTitleLineType.Times)
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

                        lineType = Configuration.SrtSubTitleLineType.SubTitles;
                    }

                    // first Line -> EntryNumber
                    if (lineType == Configuration.SrtSubTitleLineType.EntryNumber)
                    {
                        strOffsetTime = srtLine.RightOf("(Offset ").LeftOf(")");
                        srtLine = srtLine.Replace("(Offset " + strOffsetTime + ")", string.Empty);

                        if (strOffsetTime != string.Empty)
                        {
                            TimeSpan.TryParse(strOffsetTime, out timOffsetTime);
                            this.OffsetTime = timOffsetTime;
                        }

                        srtSubTitleFileEntry.EntryNumber = int.Parse(srtLine);
                        lineType = Configuration.SrtSubTitleLineType.Times;
                    }

                    if (lineType == Configuration.SrtSubTitleLineType.EmptyLine)
                    {
                        srtSubTitleFileEntry = new SrtSubTitleFileEntry();
                        lineType = Configuration.SrtSubTitleLineType.EntryNumber;
                    }
                }
            }
        }

        /// <inheritdoc/>
        public override void WriteSubTitleStreamDataToNFO(StreamWriter swrNFO)
        {
            // perform action from base-class
            base.WriteSubTitleStreamDataToNFO(swrNFO);
        }

        /// <inheritdoc/>
        public override void WriteSubTitleToSH(StreamWriter swrSH)
        {
            base.WriteSubTitleToSH(swrSH);

            // generating srt-files, as this is one
            using (StreamWriter swrSrtSubTitle = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename, false, Encoding.UTF8, 512))
            {
                int entryNumber = 0;

                foreach (SrtSubTitleFileEntry srtSubTitleFileEntry in this.SubTitleEntries)
                {
                    // resets Entry-Number
                    entryNumber++;
                    srtSubTitleFileEntry.EntryNumber = entryNumber;

                    // write entry to file
                    srtSubTitleFileEntry.WriteSrtSubTitleStreamDataToSRT(swrSrtSubTitle);
                }
            }
        }

        #endregion
    }
}
