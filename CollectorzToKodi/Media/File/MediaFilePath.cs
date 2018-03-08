// <copyright file="MediaFilePath.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class MediaFilePath : MediaPath
    {
        #region Attributes

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

        /// <summary>
        /// windows path to destination
        /// </summary>
        private string windowsPathToDestination;

        /// <summary>
        /// device path to destination
        /// </summary>
        private string devicePathToDestination;

        /// <summary>
        /// device filename on destination
        /// </summary>
        private string deviceFilenameOnDestination;

        /// <summary>
        /// device path in UMC notation for NFO Files
        /// </summary>
        private string deviceUncPathToDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFilePath"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaFilePath(Configuration configuration)
            : base(configuration)
        {
            this.extension = string.Empty;
            this.windowsPath = string.Empty;
            this.windowsPathForPublication = string.Empty;
            this.devicePathForPublication = string.Empty;
            this.windowsPathToDestination = string.Empty;
            this.devicePathToDestination = string.Empty;
            this.deviceFilenameOnDestination = string.Empty;
        }

        #endregion
        #region Properties

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
        public override MediaPath Clone()
        {
            MediaFilePath mediaFilePathClone = new MediaFilePath(this.Configuration);
            mediaFilePathClone.Filename = this.Filename;
            mediaFilePathClone.Extension = this.Extension;
            mediaFilePathClone.WindowsPath = this.WindowsPath;
            mediaFilePathClone.WindowsPathForPublication = this.WindowsPathForPublication;
            mediaFilePathClone.DevicePathForPublication = this.DevicePathForPublication;
            mediaFilePathClone.WindowsPathToDestination = this.WindowsPathToDestination;
            mediaFilePathClone.DevicePathToDestination = this.DevicePathToDestination;
            mediaFilePathClone.DeviceFilenameOnDestination = this.DeviceFilenameOnDestination;
            mediaFilePathClone.DeviceUncPathToDestination = this.DeviceUncPathToDestination;

            return mediaFilePathClone;
        }

        #endregion
    }
}
