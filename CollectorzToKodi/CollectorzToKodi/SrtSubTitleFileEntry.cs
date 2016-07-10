// <copyright file="SrtSubTitleFileEntry.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// SRT-Entry
    /// </summary>
    public class SrtSubTitleFileEntry
    {
        #region Attributes

        /// <summary>
        /// SRT-Entry-Number
        /// </summary>
        private int entryNumber;

        /// <summary>
        /// Time to start the subtitle entry
        /// </summary>
        private TimeSpan startTime;

        /// <summary>
        /// Time to stop the subtitle entry
        /// </summary>
        private TimeSpan endTime;

        /// <summary>
        /// Offset for Times in this subtitle entry
        /// </summary>
        private TimeSpan offsetTime;

        /// <summary>
        /// additional Entries; kept unchanged
        /// </summary>
        private string timeExtentions;

        /// <summary>
        /// subtitle lines
        /// </summary>
        private List<string> subTitleLines;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SrtSubTitleFileEntry"/> class.
        /// </summary>
        public SrtSubTitleFileEntry()
        {
            this.entryNumber = 0;
            TimeSpan.TryParse("00:00:00", out this.startTime);
            TimeSpan.TryParse("00:00:00", out this.endTime);
            this.timeExtentions = string.Empty;
            this.subTitleLines = new List<string>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SRT-Entry-Number
        /// </summary>
        public int EntryNumber
        {
            get { return this.entryNumber; }
            set { this.entryNumber = value; }
        }

        /// <summary>
        /// Gets or sets Time to start the subtitle entry
        /// </summary>
        public TimeSpan StartTime
        {
            get { return this.startTime; }
            set { this.startTime = value; }
        }

        /// <summary>
        /// Gets or sets Time to stop the subtitle entry
        /// </summary>
        public TimeSpan EndTime
        {
            get { return this.endTime; }
            set { this.endTime = value; }
        }

        /// <summary>
        /// Gets or sets Offset for Times in this subtitle entry
        /// </summary>
        public TimeSpan OffsetTime
        {
            get { return this.offsetTime; }
            set { this.offsetTime = value; }
        }

        /// <summary>
        /// Gets or sets additional Entries; kept unchanged
        /// </summary>
        public string TimeExtentions
        {
            get { return this.timeExtentions; }
            set { this.timeExtentions = value; }
        }

        /// <summary>
        /// Gets or sets subtitle lines
        /// </summary>
        public List<string> SubTitleLines
        {
            get { return this.subTitleLines; }
            set { this.subTitleLines = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Writes srtSubTitle entry to provided file
        /// </summary>
        /// <param name="swrSrtSubTitle">StreamWriter for new SRT subtitle file</param>
        public void WriteSrtSubTitleStreamDataToSRT(StreamWriter swrSrtSubTitle)
        {
            TimeSpan startTime;
            TimeSpan endTime;

            startTime = this.StartTime.Add(this.OffsetTime);
            endTime = this.EndTime.Add(this.OffsetTime);

            swrSrtSubTitle.WriteLine(this.entryNumber);
            swrSrtSubTitle.WriteLine(startTime.ToString(@"hh\:mm\:ss\.fff").Replace(".", ",") + " --> " + endTime.ToString(@"hh\:mm\:ss\.fff").Replace(".", ",") + (this.TimeExtentions == string.Empty ? string.Empty : " ") + this.TimeExtentions);

            foreach (string subTitleLine in this.SubTitleLines)
            {
                swrSrtSubTitle.WriteLine(subTitleLine);
            }

            swrSrtSubTitle.WriteLine(string.Empty);
        }

        #endregion
    }
}
