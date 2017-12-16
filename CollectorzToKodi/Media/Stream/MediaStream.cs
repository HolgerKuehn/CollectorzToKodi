// <copyright file="MediaStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Xml;

    /// <summary>
    /// class to manage MediaStreams
    /// </summary>
    public abstract class MediaStream
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// media containing file
        /// </summary>
        private Media media;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaStream"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaStream(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
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
        /// Gets or sets media containing file
        /// </summary>
        public Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaStream
        /// </summary>
        /// <returns>new instance of CSubTitle</returns>
        public abstract MediaStream Clone();

        /// <summary>
        /// Reads XML-files into media collection
        /// </summary>
        /// <param name="xMLMedia">part of XML export representing Movie, Series, Episode or Music</param>
        public abstract void ReadFromXml(XmlNode xMLMedia);

        /// <summary>
        /// exports Library to Disk
        /// </summary>
        public abstract void WriteToLibrary();

        #endregion
    }
}
