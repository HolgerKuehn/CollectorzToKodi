// <copyright file="Path.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class Path
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// windows path to destination
        /// </summary>
        private string windowsPathToDestination;

        /// <summary>
        /// device path to destination
        /// </summary>
        private string devicePathToDestination;

        /// <summary>
        /// device path in UMC notation for NFO Files
        /// </summary>
        private string deviceUncPathToDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Path"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Path(Configuration configuration)
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

        /// <summary>
        /// Gets or sets device path in UMC notation for NFO Files
        /// </summary>
        public string DeviceUncPathToDestination
        {
            get { return this.deviceUncPathToDestination; }
            set { this.deviceUncPathToDestination = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones Path object
        /// </summary>
        /// <returns>clone of current Path object</returns>
        public virtual Path Clone()
        {
            Path mediaPathClone = new Path(this.Configuration);
            mediaPathClone.windowsPathToDestination = this.windowsPathToDestination;
            mediaPathClone.devicePathToDestination = this.devicePathToDestination;
            mediaPathClone.deviceUncPathToDestination = this.deviceUncPathToDestination;

            return mediaPathClone;
        }

        #endregion
    }
}
