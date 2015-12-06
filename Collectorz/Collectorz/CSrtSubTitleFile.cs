using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

/// <summary>
/// Namespace for managing .nfo-export from Collectorz-Programs <br/>
/// </summary>
namespace Collectorz
{
    /// <summary>
    /// Extension for SRT-Files
    /// </summary>
    class CSrtSubTitleFile : CSrtSubTitleFileCollection
    {
        #region Attributes
        private List<CSrtSubTitleFileEntry> subTitleEntries;
        private TimeSpan offsetTime;
        #endregion
        #region Constructor
        public CSrtSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.subTitleEntries = new List<CSrtSubTitleFileEntry>();

            TimeSpan offsetTime;
            TimeSpan.TryParse("00:00:00.000", out offsetTime);
        }
        public CSrtSubTitleFile(CSubTitleFile subTitleFile)
            : this(subTitleFile.Configuration)
        {
            this.Language = subTitleFile.Language;
            this.Description = subTitleFile.Description;
            this.URL = subTitleFile.URL;
            this.URLLocalFilesystem = subTitleFile.URLLocalFilesystem;
            this.Filename = subTitleFile.Filename;
            this.Extention = subTitleFile.Extention;
            this.Server = subTitleFile.Server;
            this.Media = subTitleFile.Media;
        }
        #endregion
        #region Properties
        /// <summary>
        /// SRT-Entries
        /// </summary>
        public List<CSrtSubTitleFileEntry> SubTitleEntries
        {
            get { return subTitleEntries; }
            set { subTitleEntries = value; }
        }
        /// <summary>
        /// Time to stop the subtitle entry
        /// </summary>
        private TimeSpan OffsetTime
        {
            get { return offsetTime; }
            set { offsetTime = value; }
        }
        #endregion
        #region Functions
        /// <summary>
        /// transfers actualized data from subTitleFile
        /// </summary>
        /// <param name="subTitleFile"></param>
        public void readFromSubTitleFile(CSubTitleFile subTitleFile, int fileIndex)
        {
            TimeSpan timOffsetTime;
            string strOffsetTime;

            // reading basic information
            this.Description = subTitleFile.Description;
            strOffsetTime = this.Description.RightOf("(Offset ").LeftOf(")");
            this.Description = this.Description.Replace("(Offset " + strOffsetTime + ")", "");

            this.URL = subTitleFile.URL;
            this.URLLocalFilesystem = subTitleFile.URLLocalFilesystem;
            this.Filename = subTitleFile.Filename;
            this.Extention = subTitleFile.Extention;
            this.FileIndex = fileIndex;

            TimeSpan.TryParse(strOffsetTime, out timOffsetTime);

            this.OffsetTime = timOffsetTime;


            // reading subtitle content
            if (this.URL == "" || !File.Exists(this.URL))
                return;

            using (StreamReader srdSrtFile = new StreamReader(this.URL, Encoding.UTF8))
            {
                CConfiguration.SrtSubTitleLineType lineType = new CConfiguration.SrtSubTitleLineType();
                CSrtSubTitleFileEntry srtSubTitleFileEntry = new CSrtSubTitleFileEntry();
                lineType = CConfiguration.SrtSubTitleLineType.EntryNumber;

                while (true)
                {
                    string srtLine = srdSrtFile.ReadLine();
                    if (srtLine == null) break;

                    srtSubTitleFileEntry.OffsetTime = this.OffsetTime;

                    // end of SubTitleLines
                    if (lineType == CConfiguration.SrtSubTitleLineType.SubTitles && srtLine == "")
                    {
                        this.SubTitleEntries.Add(srtSubTitleFileEntry);
                        lineType = CConfiguration.SrtSubTitleLineType.EmptyLine;
                    }


                    // SubTitleLines
                    if (lineType == CConfiguration.SrtSubTitleLineType.SubTitles && srtLine != "")
                    {
                        srtSubTitleFileEntry.SubTitleLines.Add(srtLine);
                        lineType = CConfiguration.SrtSubTitleLineType.SubTitles;
                    }


                    // second Line -> Times
                    if (lineType == CConfiguration.SrtSubTitleLineType.Times)
                    {
                        string strStartTime = srtLine.Substring(0, 12).Replace(",", ".");
                        string strEndTime = srtLine.Substring(17, 12).Replace(",", ".");
                        string timeExtentions = "";
                        if (srtLine.Length > 30)
                            timeExtentions = srtLine.Substring(30, srtLine.Length);

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
                        srtLine = srtLine.Replace("(Offset " + strOffsetTime + ")", "");

                        if (strOffsetTime != "")
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
        /// writes new srt-file
        /// </summary>
        public override void writeSrtSubTitleStreamDataToSRT()
        {
            using (StreamWriter swrSrtSubTitle = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename, false, Encoding.UTF8, 512))
            {
                int entryNumber = 0;
                TimeSpan startTime;
                TimeSpan endTime;

                foreach (CSrtSubTitleFileEntry srtSubTitleFileEntry in this.SubTitleEntries)
                {
                    entryNumber++;
                    startTime = srtSubTitleFileEntry.StartTime.Add(srtSubTitleFileEntry.OffsetTime);
                    endTime = srtSubTitleFileEntry.EndTime.Add(srtSubTitleFileEntry.OffsetTime);

                    swrSrtSubTitle.WriteLine(entryNumber);
                    swrSrtSubTitle.WriteLine(startTime.ToString(@"hh\:mm\:ss\.fff").Replace(".", ",") + " --> " + endTime.ToString(@"hh\:mm\:ss\.fff").Replace(".", ",") + (srtSubTitleFileEntry.TimeExtentions == "" ? "" : " ") + srtSubTitleFileEntry.TimeExtentions);

                    foreach (string subTitleLine in srtSubTitleFileEntry.SubTitleLines)
                        swrSrtSubTitle.WriteLine(subTitleLine);

                    swrSrtSubTitle.WriteLine("");
                }
            }
        }
        /// <summary>
        /// writes subtitle to SH
        /// </summary>
        /// <param name="swrSH"></param>
        public override void writeSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            if (this.Filename != "")
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + "\" \"" + this.Filename + "\"");
        }
        #endregion
    }
}
