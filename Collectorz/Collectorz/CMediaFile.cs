
namespace Collectorz
{
    public abstract class CMediaFile
    {
        #region Attributes
        private CConfiguration configuration;
        private string description;
        private string uRL;
        private string uRLLocalFilesystem;
        private string filename;
        private string extention;
        private CMedia media;
        private int server;
        #endregion
        #region Constructor
        public CMediaFile(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.description = "";
            this.uRL = "";
            this.uRLLocalFilesystem = "";
            this.filename = "";
            this.extention = "";
            this.media = null;
        }
        #endregion
        #region Properties
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }
        public string URL
        {
            get { return this.uRL; }
            set { this.uRL = value; }
        }
        public string URLLocalFilesystem
        {
            get { return this.uRLLocalFilesystem; }
            set { this.uRLLocalFilesystem = value; }
        }
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }
        public string Extention
        {
            get { return this.extention; }
            set { this.extention = value; }
        }
        public CMedia Media
        {
            get { return this.media; }
            set { this.media = value; }
        }
        public int Server
        {
            get { return this.server; }
            set { this.server = value; }
        }
        #endregion
        #region Functions
        public abstract CMediaFile clone();
        public string convertFilename()
        {
            this.URLLocalFilesystem = this.URL;

            for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++ )
            {
                string driveLetter;
                string localPath;
                this.Configuration.ServerListsOfServers[(int)CConfiguration.ListOfServerTypes.NumberToDriveLetter].TryGetValue(i.ToString(), out driveLetter);
                this.Configuration.ServerListsOfServers[(int)CConfiguration.ListOfServerTypes.NumberToLocalPath].TryGetValue(i.ToString(), out localPath);
                
                // determine used servers from assigned driveLetters
                if (this.URL.Contains(driveLetter))
                {
                    this.Media.Server.Add(i);
                    this.Server = i;
                }

                // and replace them for local paths
                this.URLLocalFilesystem = this.URLLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
            }
        
            if (this.Media.GetType().ToString().Contains("CEpisode"))
                ((CEpisode)this.Media).Series.addServer(this.Server);


            if      (this.URLLocalFilesystem.ToLower().Contains(".m2ts"))   this.Extention = ".m2ts";
            else if (this.URLLocalFilesystem.ToLower().Contains(".m4v"))    this.Extention = ".m4v";
            else if (this.URLLocalFilesystem.ToLower().Contains(".vob"))    this.Extention = ".vob";
            else if (this.URLLocalFilesystem.ToLower().Contains(".de.srt")) this.Extention = ".de.srt";
            else if (this.URLLocalFilesystem.ToLower().Contains(".en.srt")) this.Extention = ".en.srt";
            else if (this.URLLocalFilesystem.ToLower().Contains(".jp.srt")) this.Extention = ".jp.srt";
            else if (this.URLLocalFilesystem.ToLower().Contains(".jpg"))    this.Extention = ".jpg";
            else if (this.URLLocalFilesystem.ToLower().Contains(".png"))    this.Extention = ".png";

            if (!this.Filename.Contains(this.Extention))
                this.Filename = this.Filename + this.Extention;

            return this.URLLocalFilesystem;
        }
        #endregion
    }
}
