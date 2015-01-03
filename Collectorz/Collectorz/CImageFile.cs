namespace Collectorz
{
    public class CImageFile : CMediaFile
    {
        #region Attributes
        private CConstants.ImageType imageType;
        private string season;
        #endregion
        #region Constructor
        public CImageFile()
            : base()
        {
            imageType = CConstants.ImageType.unknown;
            season = "";
        }
        #endregion
        #region Properties
        public CConstants.ImageType ImageType
        {
            get { return imageType; }
            set { imageType = value; }
        }
        public string Season
        {
            get { return season; }
            set { season = value; }
        }
        #endregion
        #region Functions
        public override CMediaFile clone()
        {
            CImageFile imageFileClone = new CImageFile();
            imageFileClone.ImageType = this.ImageType;
            imageFileClone.Season = this.Season;
            imageFileClone.Description = this.Description;
            imageFileClone.URL = this.URL;
            imageFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            imageFileClone.Filename = this.Filename;
            imageFileClone.Extention = this.Extention;
            imageFileClone.Server = this.Server;
            imageFileClone.Media = this.Media;

            return (CMediaFile)imageFileClone;
        }
        public string overrideSeason(string title)
        {
            this.Season = "0";

            if (title.Contains("(S"))
                this.Season = title.RightOf("(S").LeftOf(")");

            title = title.Replace("(S" + this.Season + ")", "");

            return title.Trim();
        }
        #endregion
    }
}
