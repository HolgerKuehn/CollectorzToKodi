
namespace Collectorz
{
    public abstract class CMediaFile
    {
        #region Attributes
        private string description;
        private string uRL;
        private string uRLLocalFilesystem;
        private string filename;
        private string extention;
        private CMedia media;
        private CConstants.ServerList server;
        #endregion
        #region Constructor
        public CMediaFile()
        {
            description = "";
            uRL = "";
            uRLLocalFilesystem = "";
            filename = "";
            extention = "";
            media = null;
        }
        #endregion
        #region Properties
        public string Description
        {
            get { return description; }
            set { description = value; }
        }
        public string URL
        {
            get { return uRL; }
            set { uRL = value; }
        }
        public string URLLocalFilesystem
        {
            get { return uRLLocalFilesystem; }
            set { uRLLocalFilesystem = value; }
        }
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        public string Extention
        {
            get { return extention; }
            set { extention = value; }
        }
        public CMedia Media
        {
            get { return media; }
            set { media = value; }
        }
        public CConstants.ServerList Server
        {
            get { return server; }
            set { server = value; }
        }
        #endregion
        #region Functions
        public abstract CMediaFile clone();
        public static string convertFilename(string URL)
        {
            URL = URL.Replace("\\", "/");
            URL = URL.Replace("U:", "/share/Video");
            URL = URL.Replace("V:", "/share/Video");
            URL = URL.Replace("P:", "/share/XBMC/SHIRYOUSOOCHI/Programme");

            return URL;
        }
        public string convertFilename()
        {
            if (this.URL.Contains("U:\\"))
            {
                this.Media.Server.Add(CConstants.ServerList.EIZOUSOOCHI);
                this.Server = CConstants.ServerList.EIZOUSOOCHI;
            }
            else if (this.URL.Contains("V:\\"))
            {
                this.Media.Server.Add(CConstants.ServerList.JOUSETSUSOOCHI);
                this.Server = CConstants.ServerList.JOUSETSUSOOCHI;
            }

            if (this.Media.GetType().ToString().Contains("CEpisode"))
                ((CEpisode)this.Media).Series.addServer(this.Server);

            this.URLLocalFilesystem = CMediaFile.convertFilename(this.URL);

            if (this.URLLocalFilesystem.Contains(".m2ts")) this.Extention = ".m2ts";
            else if (this.URLLocalFilesystem.Contains(".m4v")) this.Extention = ".m4v";
            else if (this.URLLocalFilesystem.Contains(".vob")) this.Extention = ".vob";
            else if (this.URLLocalFilesystem.Contains(".de.srt")) this.Extention = ".de.srt";
            else if (this.URLLocalFilesystem.Contains(".en.srt")) this.Extention = ".en.srt";
            else if (this.URLLocalFilesystem.Contains(".jp.srt")) this.Extention = ".jp.srt";
            else if (this.URLLocalFilesystem.Contains(".jpg")) this.Extention = ".jpg";

            if (!this.Filename.Contains(this.Extention))
                this.Filename = this.Filename + this.Extention;

            return this.URLLocalFilesystem;
        }
        #endregion
    }
}
