using System.IO;
using System.Xml;

namespace Collectorz
{
    public class CSubTitleFile : CMediaFile
    {
        #region Attributes
        private string language;
        #endregion
        #region Constructor
        public CSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.language = "Deutsch";
        }
        #endregion
        #region Properties
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        #endregion
        #region Functions
        public override CMediaFile clone()
        {
            CSubTitleFile subTitleFileClone = new CSubTitleFile(this.Configuration);
            subTitleFileClone.Language = this.Language;
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.URL = this.URL;
            subTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            subTitleFileClone.Filename = this.Filename;
            subTitleFileClone.Extention = this.Extention;
            subTitleFileClone.Server = this.Server;
            subTitleFileClone.Media = this.Media;

            return (CMediaFile)subTitleFileClone;
        }
        public CMediaFile checkForSubTitleStreamFile(XmlNode XMLMedia)
        {
            CSubTitleFile subTitleFile = (CSubTitleFile)this.clone();
            CSrtSubTitleFile srtSubTitleFile = new CSrtSubTitleFile(this);
            bool isSrtSubTitleFile = false;

            foreach (XmlNode XMLSubTitleStreamFile in XMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if ((XMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText("") == "Movie") && XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("").Contains("Untertitel." + this.Language + "."))
                {
                    subTitleFile.Description = XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("");
                    subTitleFile.URL = XMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText("");
                    subTitleFile.convertFilename();
                    subTitleFile.Filename = subTitleFile.Media.Filename + subTitleFile.Extention;
                    srtSubTitleFile.readFromSubTitleFile(subTitleFile);

                    if (srtSubTitleFile.Extention.Contains(".srt"))
                    {
                        isSrtSubTitleFile = true;
                        srtSubTitleFile.readSrtFile();
                    }
                }
            }

            if (isSrtSubTitleFile)
                return srtSubTitleFile;
            else
                return subTitleFile;
        }
        public void writeSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            if (this.Filename != "")
                swrSH.WriteLine("/bin/ln -s \"" + this.URLLocalFilesystem + "\" \"" + this.Filename + "\"");
        }
        #endregion
    }
}
