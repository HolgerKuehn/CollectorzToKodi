// <copyright file="CMediaFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace Collectorz
{
    /// <summary>
    /// base type for media files, e.g. images, videos, etc.
    /// </summary>
    public abstract class CMediaFile
    {
        #region Attributes

        /// <summary>
        /// current configuration of Collectorz
        /// </summary>
        private CConfiguration configuration;

        /// <summary>
        /// description of media file
        /// </summary>
        private string description;

        /// <summary>
        /// url or uri of file
        /// </summary>
        private string uRL;

        /// <summary>
        /// path and filename on local machine
        /// </summary>
        private string uRLLocalFilesystem;

        /// <summary>
        /// filename without extension
        /// </summary>
        private string filename;

        /// <summary>
        /// extension for original file
        /// </summary>
        private string extension;

        /// <summary>
        /// media containing file
        /// </summary>
        private CMedia media;

        /// <summary>
        /// list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        private int server;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMediaFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for Collectorz programs and Kodi</param>
        public CMediaFile(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.description = string.Empty;
            this.uRL = string.Empty;
            this.uRLLocalFilesystem = string.Empty;
            this.filename = string.Empty;
            this.extension = string.Empty;
            this.media = null;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of Collectorz
        /// </summary>
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets description of media file
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// Gets or sets url or uri of file
        /// </summary>
        public string URL
        {
            get { return this.uRL; }
            set { this.uRL = value; }
        }

        /// <summary>
        /// Gets or sets path and filename on local machine
        /// </summary>
        public string URLLocalFilesystem
        {
            get { return this.uRLLocalFilesystem; }
            set { this.uRLLocalFilesystem = value; }
        }

        /// <summary>
        /// Gets or sets filename without extension
        /// </summary>
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }

        /// <summary>
        /// Gets or sets extension for original file
        /// </summary>
        public string Extension
        {
            get { return this.extension; }
            set { this.extension = value; }
        }

        /// <summary>
        /// Gets or sets media containing file
        /// </summary>
        public CMedia Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        public int Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaFile object completely
        /// </summary>
        /// <returns>new instance of MediaFile</returns>
        public abstract CMediaFile Clone();

        /// <summary>
        /// converts mapped drives to network names and adds used server to media
        /// </summary>
        /// <returns>URLLocalFilesystem</returns>
        public string ConvertFilename()
        {
            this.URLLocalFilesystem = this.URL;

            if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
            {
                this.URLLocalFilesystem = this.URLLocalFilesystem.ReplaceAll("\\", "/");
            }

            for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++)
            {
                string driveLetter = this.Configuration.ServerListsOfServers[(int)CConfiguration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
                string localPath = this.Configuration.ServerListsOfServers[(int)CConfiguration.ListOfServerTypes.NumberToLocalPath][i.ToString()];

                // determine used servers from assigned driveLetters
                if (this.URL.StartsWith(driveLetter.Trim() + ":", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    if (!this.Media.Server.Contains(i))
                    {
                        this.Media.Server.Add(i);
                    }

                    this.Server = i;
                }

                // and replace them for local paths
                this.URLLocalFilesystem = this.URLLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
            }

            if (this.Media.GetType().ToString().Contains("CEpisode"))
            {
                ((CEpisode)this.Media).Series.AddServer(this.Server);
            }

            // determine file extension
            if (this.URLLocalFilesystem.ToLower().Contains(".m2ts"))
            {
                this.Extension = ".m2ts";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".m4v"))
            {
                this.Extension = ".m4v";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".vob"))
            {
                this.Extension = ".vob";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".de.srt"))
            {
                this.Extension = ".de.srt";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".en.srt"))
            {
                this.Extension = ".en.srt";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".jp.srt"))
            {
                this.Extension = ".jp.srt";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".jpg"))
            {
                this.Extension = ".jpg";
            }
            else if (this.URLLocalFilesystem.ToLower().Contains(".png"))
            {
                this.Extension = ".png";
            }

            if (!this.Filename.Contains(this.Extension))
            {
                this.Filename = this.Filename + this.Extension;
            }

            return this.URLLocalFilesystem;
        }

        #endregion
    }
}
