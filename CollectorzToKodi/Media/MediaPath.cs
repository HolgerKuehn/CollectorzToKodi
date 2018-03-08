// <copyright file="MediaPath.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class MediaPath : Path
    {
        #region Attributes

        /// <summary>
        /// name of Media folder or name of MediaFile
        /// </summary>
        private string filename;

        /// <summary>
        /// device filename on destination
        /// </summary>
        private string deviceFilenameOnDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaPath"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaPath(Configuration configuration)
            : base(configuration)
        {
            this.filename = string.Empty;
            this.deviceFilenameOnDestination = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets name of Media
        /// </summary>
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }

        /// <summary>
        /// Gets or sets device path to destination
        /// </summary>
        public string DeviceFilenameOnDestination
        {
            get { return this.deviceFilenameOnDestination; }
            set { this.deviceFilenameOnDestination = value; }
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
            mediaPathClone.WindowsPathToDestination = this.WindowsPathToDestination;
            mediaPathClone.DevicePathToDestination = this.DevicePathToDestination;
            mediaPathClone.DeviceFilenameOnDestination = this.DeviceFilenameOnDestination;
            mediaPathClone.DeviceUncPathToDestination = this.DeviceUncPathToDestination;

            return mediaPathClone;
        }

        #endregion
    }
}
