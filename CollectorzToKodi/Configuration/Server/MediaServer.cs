// <copyright file="MediaServer.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class MediaServer : Server
    {
        #region Attributes

        /// <summary>
        /// media containing file
        /// </summary>
        private Media media;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaServer"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        /// <param name="number">Number of MediaServer represented by object</param>
        public MediaServer(Configuration configuration, int number)
            : base (configuration, number)
        {
            this.media = null;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets media containing file
        /// </summary>
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaServer object
        /// </summary>
        /// <returns>clone of current MediaServer object</returns>
        public override Server Clone()
        {
            MediaServer MediaServerClone = new MediaServer(this.Configuration, this.Number);
            MediaServerClone.Configuration = this.Configuration;
            MediaServerClone.Media = this.Media;
            MediaServerClone.Path = this.Path.Clone();

            return MediaServerClone;
        }

        #endregion
    }
}
