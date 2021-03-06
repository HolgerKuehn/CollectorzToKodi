﻿// <copyright file="MediaFile.cs" company="Holger Kühn">
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
            this.uRL = string.Empty;
            this.uRLLocalFilesystem = string.Empty;
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
        public Media Media
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
        /// converts mapped drives to network names and adds used server to media
        /// </summary>
        /// <returns>URLLocalFilesystem</returns>
        public abstract string ConvertFilename();

        /// <summary>
        /// converts mapped drives to network names and adds used server to media
        /// </summary>
        /// <param name="setMediaServer">defines, if Server is set for Media containing this MediaFile</param>
        /// <returns>URLLocalFilesystem</returns>
        protected string ConvertFilename(bool setMediaServer = false)
        {
            if (this.URL == string.Empty || this.URL.Contains(".m2ts") || this.URL.Contains(".m4v"))
            {
                return string.Empty; // ###
            }

            this.URLLocalFilesystem = this.URL;

            if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
            {
                this.URLLocalFilesystem = this.URLLocalFilesystem.ReplaceAll("\\", "/");
            }

            for (int i = 0; i < this.Configuration.ServerNumberOfServers; i++)
            {
                string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
                string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaStorage][i.ToString()];

                // determine used servers from assigned driveLetters
                if (this.URL.StartsWith(driveLetter.Trim() + ":", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    if (setMediaServer)
                    {
                        this.Media.AddServer(i);
                    }

                    this.Server = i;
                }

                // and replace them for local paths
                this.URLLocalFilesystem = this.URLLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
            }

            // determine file extension
            string extension = this.URLLocalFilesystem.ToLower().RightOfLast(".");
            string filename = this.URLLocalFilesystem.ToLower().LeftOfLast(".");

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

                    if (extension == "forced.srt")
                    {
                        extension = filename.RightOfLast(".") + "." + extension;
                        filename = filename.LeftOfLast(".");
                    }

                    switch (extension)
                    {
                        case "de.srt":
                        case "en.srt":
                        case "jp.srt":
                        case "de.forced.srt":
                        case "en.forced.srt":
                        case "jp.forced.srt":
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

            return this.URLLocalFilesystem;
        }

        #endregion
    }
}
