using System;
using System.Collections.Generic;

/// <summary>
/// Namespace for managing .nfo-export from Collectorz-Programs <br/>
/// </summary>
namespace Collectorz
{
    /// <summary>
    /// SRT-Entry
    /// </summary>
    class CSrtSubTitleFileEntry
    {
        #region Attributes
        private int entryNumber;
        private TimeSpan startTime;
        private TimeSpan endTime;
        private TimeSpan offsetTime;
        private string timeExtentions;
        private List<string> subTitleLines;
        #endregion
        #region Constructor
        public CSrtSubTitleFileEntry()
        { 
            this.entryNumber = 0;
            TimeSpan.TryParse("00:00:00", out this.startTime);
            TimeSpan.TryParse("00:00:00", out this.endTime);
            this.timeExtentions = "";
            this.subTitleLines = new List<string>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// SRT-Entry-Number
        /// </summary>
        public int EntryNumber
        {
            get { return entryNumber; }
            set { entryNumber = value; }
        }
        /// <summary>
        /// Time to start the subtitle entry
        /// </summary>
        public TimeSpan StartTime
        {
            get { return startTime; }
            set { startTime = value; }
        }
        /// <summary>
        /// Time to stop the subtitle entry
        /// </summary>
        public TimeSpan EndTime
        {
            get { return endTime; }
            set { endTime = value; }
        }
        /// <summary>
        /// Time to stop the subtitle entry
        /// </summary>
        public TimeSpan OffsetTime
        {
            get { return offsetTime; }
            set { offsetTime = value; }
        }
        /// <summary>
        /// additional Entries; keep unchanged
        /// </summary>
        public string TimeExtentions
        {
            get { return timeExtentions; }
            set { timeExtentions = value; }
        }
        /// <summary>
        /// subtitle lines
        /// </summary>
        public List<string> SubTitleLines
        {
            get { return subTitleLines; }
            set { subTitleLines = value; }
        }
        #endregion
        #region Functions

        #endregion
    }
}
