// <copyright file="SrtSubTitleStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Extension for SRT-Files
    /// </summary>
    public class SrtSubTitleStream : TypeSubTitleStream
    {
        #region Attributes

        /// <summary>
        /// SRT-Entries
        /// </summary>
        private List<SrtSubTitleStreamEntry> srtSubTitleEntries;

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
            this.srtSubTitleEntries = new List<SrtSubTitleStreamEntry>();

            TimeSpan.TryParse("00:00:00.000", out TimeSpan offsetTime);
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SRT-Entries
        /// </summary>
        public List<SrtSubTitleStreamEntry> SrtSubTitleEntries
        {
            get { return this.srtSubTitleEntries; }
            set { this.srtSubTitleEntries = value; }
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
        public override MediaStream Clone()
        {
            SrtSubTitleStream srtSubTitleFileClone = new SrtSubTitleStream(this.Configuration);

            // MediaStream
            srtSubTitleFileClone.Media = this.Media;

            // SubTitleStream
            srtSubTitleFileClone.Language = this.Language;

            // SrtSubTitleStream
            srtSubTitleFileClone.SrtSubTitleEntries = this.SrtSubTitleEntries;
            srtSubTitleFileClone.OffsetTime = this.OffsetTime;

            return srtSubTitleFileClone;
        }

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            foreach (SubTitleFile srtSubTitleFile in this.SourceSubTitleFiles)
            {
                // checking, if file exists
                if (!File.Exists(srtSubTitleFile.MediaFilePath.WindowsPathForPublication))
                {
                    return;
                }

                // reading basic information
                string strOffsetTime = srtSubTitleFile.Description.RightOf("(Offset ").LeftOf(")");
                srtSubTitleFile.Description = srtSubTitleFile.Description.Replace("(Offset " + strOffsetTime + ")", string.Empty);

                TimeSpan.TryParse(strOffsetTime, out TimeSpan timOffsetTime);
                this.OffsetTime = timOffsetTime;

                using (StreamReader srdSrtFile = new StreamReader(srtSubTitleFile.MediaFilePath.WindowsPathForPublication, Encoding.UTF8))
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
                            this.SrtSubTitleEntries.Add(srtSubTitleFileEntry);
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
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            // generating srt-files, as this is one
            StreamWriter swrSrtSubTitle = new StreamWriter(this.DestinationSubTitleFile.MediaFilePath.WindowsPathForPublication, false, Encoding.UTF8, 512);

            int entryNumber = 0;

            foreach (SrtSubTitleStreamEntry srtSubTitleFileEntry in this.SrtSubTitleEntries)
            {
                // resets Entry-Number
                entryNumber++;
                srtSubTitleFileEntry.EntryNumber = entryNumber;

                // write entry to file
                srtSubTitleFileEntry.WriteToLibrary(swrSrtSubTitle);
            }

            swrSrtSubTitle.Close();
        }

        #endregion
    }
}
