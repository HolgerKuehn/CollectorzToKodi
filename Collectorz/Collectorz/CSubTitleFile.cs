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
        public void checkForSubTitleStreamFile(XmlNode XMLMedia)
        {
            foreach (XmlNode XMLSubTitleStreamFile in XMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if ((XMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText("") == "Movie") && XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("").Contains("Untertitel." + this.Language + "."))
                {
                    this.Description = XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("");
                    this.URL = XMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText("");
                    this.convertFilename();
                    this.Filename = this.Media.Filename + this.Extention;
                }
            }
        }
        #endregion
    }
}
