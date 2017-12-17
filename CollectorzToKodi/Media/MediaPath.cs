﻿// <copyright file="MediaPath.cs" company="Holger Kühn">
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
        /// name of Media folder or name of MediaFile
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

        /// <summary>
        /// device filename on destination
        /// </summary>
        private string deviceFilenameOnDestination;

        /// <summary>
        /// device path in UMC notation for NFO Files
        /// </summary>
        private string deviceUmcPathToDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPath"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaPath(Configuration configuration)
        {
            this.configuration = configuration;
            this.filename = string.Empty;
            this.windowsPathToDestination = string.Empty;
            this.devicePathToDestination = string.Empty;
            this.deviceFilenameOnDestination = string.Empty;
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
        /// Gets or sets name of Media
        /// </summary>
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
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
        /// Gets or sets device path to destination
        /// </summary>
        public string DeviceFilenameOnDestination
        {
            get { return this.deviceFilenameOnDestination; }
            set { this.deviceFilenameOnDestination = value; }
        }

        /// <summary>
        /// Gets or sets device path in UMC notation for NFO Files
        /// </summary>
        public string DeviceUmcPathToDestination
        {
            get { return this.deviceUmcPathToDestination; }
            set { this.deviceUmcPathToDestination = value; }
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
            mediaPathClone.Filename = this.Filename;
            mediaPathClone.windowsPathToDestination = this.windowsPathToDestination;
            mediaPathClone.devicePathToDestination = this.devicePathToDestination;
            mediaPathClone.deviceFilenameOnDestination = this.deviceFilenameOnDestination;
            mediaPathClone.deviceUmcPathToDestination = this.deviceUmcPathToDestination;

            return mediaPathClone;
        }

        #endregion
    }
}
