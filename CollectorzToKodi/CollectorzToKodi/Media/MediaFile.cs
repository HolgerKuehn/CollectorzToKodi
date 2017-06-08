// <copyright file="MediaFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// base type for media files, e.g. images, videos, etc.
    /// </summary>
    public abstract class MediaFile
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// description of media file
        /// </summary>
        private string description;

        /// <summary>
        /// url or uri of file; used for storage
        /// </summary>
        private string urlForMediaStorage;

        /// <summary>
        /// path and filename on local machine; used for storage
        /// </summary>
        private string urlForMediaStorageLocalFilesystem;

        /// <summary>
        /// url or uri of file; used for publication
        /// </summary>
        private string urlForMediaPublication;

        /// <summary>
        /// path and filename on local machine; used for publication
        /// </summary>
        private string urlForMediaPublicationLocalFilesystem;

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
        private Media media;

        /// <summary>
        /// list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        private int server;

        /// <summary>
        /// index of MediaFile file
        /// </summary>
        private int fileIndex;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaFile(Configuration configuration)
        {
            this.configuration = configuration;
            this.description = string.Empty;
            this.urlForMediaStorage = string.Empty;
            this.urlForMediaStorageLocalFilesystem = string.Empty;
            this.urlForMediaPublication = string.Empty;
            this.urlForMediaPublicationLocalFilesystem = string.Empty;
            this.filename = string.Empty;
            this.extension = string.Empty;
            this.media = null;
            this.fileIndex = 1;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
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
        /// Gets or sets url or uri of file; used for media storage
        /// </summary>
        public virtual string UrlForMediaStorage
        {
            get
            {
                return this.urlForMediaStorage;
            }

            set
            {
                this.urlForMediaStorage = value;

                this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorage;

                if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
                {
                    this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorageLocalFilesystem.ReplaceAll("\\", "/");
                }

                for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++)
                {
                    string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
                    string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaStorage][i.ToString()];

                    // determine used servers from assigned driveLetters
                    if (this.UrlForMediaStorage.StartsWith(driveLetter.Trim() + ":", true, System.Globalization.CultureInfo.CurrentCulture))
                    {
                        this.Server = i;
                    }

                    // and replace them for local paths
                    this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorageLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
                }

                // determine file extension
                string extension = this.urlForMediaStorage.ToLower().RightOfLast(".");
                string filename = this.urlForMediaStorage.ToLower().LeftOfLast(".");

                if (extension == "jpeg")
                {
                    extension = "jpg";
                }

                switch (extension)
                {
                    case "m2ts":
                    case "m4v":
                    case "mkv":
                    case "mp4":
                    case "mpg":
                    case "vob":

                    case "gif":
                    case "jpg":
                    case "png":
                        this.Extension = "." + extension;
                        break;

                    case "srt":
                        extension = filename.RightOfLast(".") + "." + extension;
                        filename = filename.LeftOfLast(".");

                        switch (extension)
                        {
                            case "de.srt":
                            case "en.srt":
                            case "jp.srt":
                                this.Extension = "." + extension;
                                break;
                        }

                        break;

                    default:
                        throw new System.NotImplementedException("Extension \"" + extension + "\" is not supported yet.");
                }

                if (!this.Filename.Contains(this.Extension))
                {
                    this.Filename = this.Filename + this.Extension;
                }
            }
        }

        /// <summary>
        /// Gets or sets path and filename on local machine; used for media storage
        /// </summary>
        public string UrlForMediaStorageLocalFilesystem
        {
            get { return this.urlForMediaStorageLocalFilesystem; }
            set { this.urlForMediaStorageLocalFilesystem = value; }
        }

        /// <summary>
        /// Gets or sets url or uri of file; used for media publication
        /// </summary>
        public string UrlForMediaPublication
        {
            get
            {
                return this.urlForMediaPublication;
            }

            set
            {
                this.urlForMediaPublication = value;

                this.urlForMediaPublicationLocalFilesystem = this.urlForMediaPublication;

                if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
                {
                    this.urlForMediaPublicationLocalFilesystem = this.urlForMediaPublicationLocalFilesystem.ReplaceAll("\\", "/");
                }

                for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++)
                {
                    string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
                    string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaPublication][i.ToString()];

                    // replace drive-letter with local paths
                    this.urlForMediaPublicationLocalFilesystem = this.urlForMediaPublicationLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
                }
            }
        }

        /// <summary>
        /// Gets or sets path and filename on local machine; used for media publication
        /// </summary>
        public string UrlForMediaPublicationLocalFilesystem
        {
            get { return this.urlForMediaPublicationLocalFilesystem; }
            set { this.urlForMediaPublicationLocalFilesystem = value; }
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
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        public virtual int Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        /// <summary>
        /// Gets or sets file index of SubTiltle
        /// </summary>
        public int FileIndex
        {
            get { return this.fileIndex; }
            set { this.fileIndex = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaFile object completely
        /// </summary>
        /// <returns>new instance of MediaFile</returns>
        public abstract MediaFile Clone();

        /// <summary>
        /// exports Library to Disk
        /// </summary>
        public abstract void WriteToLibrary();

        #endregion
    }
}
