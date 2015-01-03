namespace Collectorz
{
    public class CVideoFile : CMediaFile
    {
        #region Attributes
        private bool isSpecial;
        #endregion
        #region Constructor
        public CVideoFile()
            : base()
        {
            isSpecial = false;
        }
        #endregion
        #region Properties
        public bool IsSpecial
        {
            get { return isSpecial; }
            set { isSpecial = value; }
        }
        #endregion
        #region Functions
        public override CMediaFile clone()
        {
            CVideoFile videoFileClone = new CVideoFile();
            videoFileClone.IsSpecial = this.IsSpecial;
            videoFileClone.Description = this.Description;
            videoFileClone.URL = this.URL;
            videoFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            videoFileClone.Filename = this.Filename;
            videoFileClone.Extention = this.Extention;
            videoFileClone.Server = this.Server;
            videoFileClone.Media = this.Media;

            return (CMediaFile)videoFileClone;
        }
        public string overrideSpecial(string title)
        {
            if (title.Contains("(Special)"))
                this.isSpecial = true;

            title = title.Replace("(Special)", "");

            return title.Trim();
        }
        #endregion
    }
}
