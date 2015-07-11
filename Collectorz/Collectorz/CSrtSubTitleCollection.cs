using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectorz
{
    class CSrtSubTitleFileCollection : CSubTitleFile
    {
        #region Attributes
        private List<CSrtSubTitleFile> subTitleFiles;
        private int fileIndex;
        #endregion
        #region Constructor
        public CSrtSubTitleFileCollection(CConfiguration configuration)
            : base(configuration)
        {
            this.subTitleFiles = new List<CSrtSubTitleFile>();
            this.fileIndex = 1;
        }
        public CSrtSubTitleFileCollection(CSubTitleFile subTitleFile)
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
        public List<CSrtSubTitleFile> SubTitleFiles
        {
            get { return subTitleFiles; }
            set { subTitleFiles = value; }
        }
        /// <summary>
        /// file index of srt-SubTiltle
        /// </summary>
        public int FileIndex
        {
            get { return this.fileIndex; }
            set { this.fileIndex = value; }
        }
        #endregion
        #region Functions
        /// <summary>
        /// writes new srt-file
        /// </summary>
        public virtual void writeSrtSubTitleStreamDataToSRT()
        {
            foreach (CSrtSubTitleFile subTitleFile in this.subTitleFiles)
                if (subTitleFile != null)
                    subTitleFile.writeSrtSubTitleStreamDataToSRT();
        }
        /// <summary>
        /// writes subtitle to SH
        /// </summary>
        /// <param name="swrSH"></param>
        public override void writeSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            foreach (CSrtSubTitleFile subTitleFile in this.subTitleFiles)
                if (subTitleFile != null)
                    subTitleFile.writeSubTitleStreamDataToSH(swrSH);
        }
        #endregion
    }
}
