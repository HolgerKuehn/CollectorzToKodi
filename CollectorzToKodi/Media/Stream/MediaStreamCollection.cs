// <copyright file="MediaStreamCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage List of MediaStreams
    /// </summary>
    public abstract class MediaStreamCollection : IMediaCollection
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// media containing person
        /// </summary>
        private Media media;

        /// <summary>
        /// Name of MediaStream
        /// </summary>
        private List<MediaStream> mediaStreams;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaStreamCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaStreamCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.MediaStreams = new List<MediaStream>();
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
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets name of MediaStream
        /// </summary>
        public List<MediaStream> MediaStreams
        {
            get { return this.mediaStreams; }
            set { this.mediaStreams = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of MediaStream from XML-node representing a MediaStream
        /// </summary>
        /// <param name="xmlMediaStreams">xml-node representing a list of MediaStream</param>
        public abstract void ReadFromXml(XmlNode xmlMediaStreams);

        /// <summary>
        /// delete from Library
        /// </summary>
        public abstract void DeleteFromLibrary();

        /// <summary>
        /// exports MediaStream
        /// </summary>
        public abstract void WriteToLibrary();

        #endregion
    }
}