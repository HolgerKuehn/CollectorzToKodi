/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    public class CImageFile : CMediaFile
    {
        #region Attributes
        private CConfiguration.ImageType imageType;
        private string season;
        #endregion
        #region Constructor
        public CImageFile(CConfiguration configuration)
            : base(configuration)
        {
            this.imageType = CConfiguration.ImageType.unknown;
            this.season = "";
        }
        #endregion
        #region Properties
        public CConfiguration.ImageType ImageType
        {
            get { return this.imageType; }
            set { this.imageType = value; }
        }
        public string Season
        {
            get { return this.season; }
            set { this.season = value; }
        }
        #endregion
        #region Functions
        public override CMediaFile clone()
        {
            CImageFile imageFileClone = new CImageFile(this.Configuration);
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
