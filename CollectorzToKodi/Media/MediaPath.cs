// <copyright file="MediaPath.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class MediaPath
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// filename without extension
        /// </summary>
        private string filename;

        /// <summary>
        /// windows path to destination
        /// </summary>
        private string windowsPathToDestination;

        /// <summary>
        /// device path to destination
        /// </summary>
        private string devicePathToDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPath"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaPath(Configuration configuration)
        {
            this.configuration = configuration;
            this.windowsPathToDestination = string.Empty;
            this.devicePathToDestination = string.Empty;
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
        /// Gets or sets filename without extension
        /// </summary>
        public virtual string Filename
        {
            get
            {
                return this.filename;
            }

            set
            {
                this.filename = value;

                this.UrlForMediaPublicationLocalFilesystem = this.Media.UrlForMediaPublicationLocalFilesystem;
            }
        }

        /// <summary>
        /// Gets or sets windows path to destination
        /// </summary>
        public string WindowsPathToDestination
        {
            get { return this.windowsPathToDestination; }
            set { this.windowsPathToDestination = value; }
        }

        /// <summary>
        /// Gets or sets device path to destination
        /// </summary>
        public string DevicePathToDestination
        {
            get { return this.devicePathToDestination; }
            set { this.devicePathToDestination = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaPath object
        /// </summary>
        /// <returns>clone of current MediaPath object</returns>
        public virtual MediaPath Clone()
        {
            MediaPath mediaPathClone = new MediaPath(this.Configuration);
            mediaPathClone.configuration = this.configuration;
            mediaPathClone.windowsPathToDestination = this.windowsPathToDestination;
            mediaPathClone.devicePathToDestination = this.devicePathToDestination;

            return mediaPathClone;
        }

        ///// <summary>
        ///// Gets or sets url or uri of file; used for media storage
        ///// </summary>
        //public virtual string UrlForMediaStorage
        //{
        //    get
        //    {
        //        return this.urlForMediaStorage;
        //    }

        //    set
        //    {
        //        this.urlForMediaStorage = value;

        //        this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorage;

        //        if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
        //        {
        //            this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorageLocalFilesystem.ReplaceAll("\\", "/");
        //        }

        //        for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++)
        //        {
        //            string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
        //            string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaStorage][i.ToString()];

        //            // determine used servers from assigned driveLetters
        //            if (this.UrlForMediaStorage.StartsWith(driveLetter.Trim() + ":", true, System.Globalization.CultureInfo.CurrentCulture))
        //            {
        //                this.Server = i;
        //            }

        //            // and replace them for local paths
        //            this.urlForMediaStorageLocalFilesystem = this.urlForMediaStorageLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
        //        }

        //        // determine file extension
        //        string extension = this.urlForMediaStorage.ToLower().RightOfLast(".");
        //        string filename = this.urlForMediaStorage.ToLower().LeftOfLast(".");

        //        if (extension == "jpeg")
        //        {
        //            extension = "jpg";
        //        }

        //        switch (extension)
        //        {
        //            case "m2ts":
        //            case "m4v":
        //            case "mkv":
        //            case "mp4":
        //            case "mpg":
        //            case "vob":

        //            case "gif":
        //            case "jpg":
        //            case "png":
        //                this.Extension = "." + extension;
        //                break;

        //            case "srt":
        //                extension = filename.RightOfLast(".") + "." + extension;
        //                filename = filename.LeftOfLast(".");

        //                switch (extension)
        //                {
        //                    case "de.srt":
        //                    case "en.srt":
        //                    case "jp.srt":
        //                        this.Extension = "." + extension;
        //                        break;
        //                }

        //                break;

        //            default:
        //                throw new System.NotImplementedException("Extension \"" + extension + "\" is not supported yet.");
        //        }

        //        if (!this.Filename.Contains(this.Extension))
        //        {
        //            this.Filename = this.Filename + this.Extension;
        //        }
        //    }
        //}

        #endregion
    }
}
