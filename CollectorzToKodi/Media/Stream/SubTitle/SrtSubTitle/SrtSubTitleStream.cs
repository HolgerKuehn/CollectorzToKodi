// <copyright file="SrtSubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
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
    public class SrtSubTitleStream : TypeSubTitleStream
    {
        #region Attributes

        /// <summary>
        /// SRT-Entries
        /// </summary>
        private List<SrtSubTitleStreamEntry> subTitleEntries;

        /// <summary>
        /// basic time offset for subtitle entries
        /// </summary>
        private TimeSpan offsetTime;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SrtSubTitleStream"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SrtSubTitleStream(Configuration configuration)
            : base(configuration)
        {
            this.subTitleEntries = new List<SrtSubTitleStreamEntry>();

            TimeSpan.TryParse("00:00:00.000", out TimeSpan offsetTime);
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SRT-Entries
        /// </summary>
        public List<SrtSubTitleStreamEntry> SubTitleEntries
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
            SrtSubTitleStream srtSubTitleFileClone = new SrtSubTitleStream(this.Configuration);
            srtSubTitleFileClone.Description = this.Description;
            srtSubTitleFileClone.UrlForMediaStorage = this.UrlForMediaStorage;
            srtSubTitleFileClone.Extension = this.Extension;
            srtSubTitleFileClone.SubTitle = this.SubTitle;
            srtSubTitleFileClone.SubTitleEntries.AddRange(this.SubTitleEntries);
            srtSubTitleFileClone.FileIndex = this.FileIndex;
            srtSubTitleFileClone.OffsetTime = this.OffsetTime;

            srtSubTitleFileClone.Media = this.Media;
            srtSubTitleFileClone.Server = this.Server;
            srtSubTitleFileClone.MediaPath.Filename = this.MediaPath.Filename;

            return (SrtSubTitleStream)srtSubTitleFileClone;
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            // perform action from base-class
            base.WriteToLibrary();

            // generating srt-files, as this is one
            StreamWriter swrSrtSubTitle = new StreamWriter(this.Configuration.MovieCollectorWindowsPathToXmlExportPath + this.MediaPath.Filename, false, Encoding.UTF8, 512);

            int entryNumber = 0;

            foreach (SrtSubTitleStreamEntry srtSubTitleFileEntry in this.SubTitleEntries)
            {
                // resets Entry-Number
                entryNumber++;
                srtSubTitleFileEntry.EntryNumber = entryNumber;

                // write entry to file
                srtSubTitleFileEntry.WriteSrtSubTitleStreamDataToSRT(swrSrtSubTitle);
            }

            swrSrtSubTitle.Close();
        }

        /// <inheritdoc/>
        public override SubTitleFile CreateFinalSubTitleFile(SubTitleFile subTitleFile)
        {
            SrtSubTitleStream srtTitleFile = (SrtSubTitleStream)subTitleFile;
            srtTitleFile.SubTitleEntries.AddRange(this.SubTitleEntries);

            return srtTitleFile;
        }

        /// <summary>
        /// transfers actualized data from subTitleFile
        /// </summary>
        public void ReadFromSubTitleFile()
        {
            // reading basic information
            string strOffsetTime = this.Description.RightOf("(Offset ").LeftOf(")");
            this.Description = this.Description.Replace("(Offset " + strOffsetTime + ")", string.Empty);

            TimeSpan.TryParse(strOffsetTime, out TimeSpan timOffsetTime);
            this.OffsetTime = timOffsetTime;

            // reading subtitle content
            if (this.UrlForMediaStorage == string.Empty || !File.Exists(this.UrlForMediaStorage))
            {
                return;
            }

            using (StreamReader srdSrtFile = new StreamReader(this.UrlForMediaStorage, Encoding.UTF8))
            {
                Configuration.SrtSubTitleLineType lineType = default(Configuration.SrtSubTitleLineType);
                SrtSubTitleStreamEntry srtSubTitleFileEntry = new SrtSubTitleStreamEntry();
                lineType = Configuration.SrtSubTitleLineType.EntryNumber;

                while (true)
                {
                    string srtLine = srdSrtFile.ReadLine();

                    // end of file
                    if (srtLine == null)
                    {
                        break;
                    }

                    // additional empty lines (on end); just skipping
                    if (srtLine == string.Empty && lineType == Configuration.SrtSubTitleLineType.EntryNumber)
                    {
                        continue;
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

                        TimeSpan.TryParse(strStartTime, out TimeSpan timStartTime);
                        TimeSpan.TryParse(strEndTime, out TimeSpan timEndTime);

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
                        srtSubTitleFileEntry = new SrtSubTitleStreamEntry();
                        lineType = Configuration.SrtSubTitleLineType.EntryNumber;
                    }
                }
            }
        }

        #endregion
    }
}
