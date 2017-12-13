// <copyright file="MediaFilePath.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class MediaFilePath
    {
        #region Attributes

        /// <summary>
        /// name of media file
        /// </summary>
        private string filename;

        /// <summary>
        /// extension for original file
        /// </summary>
        private string extension;

        /// <summary>
        /// windows path to stored content
        /// </summary>
        private string windowsPath;

        /// <summary>
        /// windows path to stored content, used during publication
        /// </summary>
        private string windowsPathForPublication;

        /// <summary>
        /// device path to stored content, used during publication
        /// </summary>
        private string devicePathForPublication;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFilePath"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaFilePath(Configuration configuration)
        {
            this.windowsPath = string.Empty;
            this.windowsPathForPublication = string.Empty;
            this.devicePathForPublication = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets name of Media
        /// </summary>
        public string Filename
        {
            get { return this.extension; }
            set { this.extension = value; }
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
        /// Gets or sets windows path to stored content
        /// </summary>
        public string WindowsPath
        {
            get { return this.windowsPath; }
            set { this.windowsPath = value; }
        }

        /// <summary>
        /// Gets or sets windows path to stored content
        /// </summary>
        public string WindowsPathForPublication
        {
            get { return this.windowsPathForPublication; }
            set { this.windowsPathForPublication = value; }
        }

        /// <summary>
        /// Gets or sets device path to stored content
        /// </summary>
        public string DevicePathForPublication
        {
            get { return this.devicePathForPublication; }
            set { this.devicePathForPublication = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaFilePath object
        /// </summary>
        /// <returns>clone of current MediaFilePath object</returns>
        public override Server Clone()
        {
            MediaFilePath mediaPathClone = new MediaFilePath(this.Configuration);
            mediaPathClone.Configuration = this.Configuration;
            mediaPathClone.WindowsPath = this.WindowsPath;
            mediaPathClone.WindowsPathForPublication = this.WindowsPathForPublication;
            mediaPathClone.DevicePathForPublication = this.DevicePathForPublication;
            mediaPathClone.WindowsPathToDestination = this.WindowsPathToDestination;
            mediaPathClone.DevicePathToDestination = this.DevicePathToDestination;

            return mediaPathClone;
        }

        #endregion
    }
}
