// <copyright file="MediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// Class to manage List of media
    /// </summary>
    public class MediaCollection : IServerCollection
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// Server containing Collection
        /// </summary>
        private Server server;

        /// <summary>
        /// list of media
        /// </summary>
        private List<Media> media;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = new List<Media>();
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
        /// Gets or sets Server containing Collection
        /// </summary>
        public Server Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        /// <summary>
        /// Gets or sets name of mediaLanguage
        /// </summary>
        public List<Media> Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of Media
        /// </summary>
        /// <param name="xmlNode">xml-node</param>
        public virtual void ReadFromXml(XmlNode xmlNode)
        {
        }

        /// <summary>
        /// delete from Library
        /// </summary>
        public void DeleteFromLibrary()
        {
        }

        /// <summary>
        /// exports MediaLanguage
        /// </summary>
        public virtual void WriteToLibrary()
        {
        }

        /// <summary>
        /// clones MediaColection
        /// </summary>
        /// <returns></returns>
        public virtual MediaCollection Clone()
        {
            MediaCollection mediaCollectionClone = new MediaCollection(this.Configuration);
            mediaCollectionClone.Server = this.Server;

            foreach (Media media in this.Media)
            {
                mediaCollectionClone.Media.Add(media.Clone());
            }

            return mediaCollectionClone;
        }

        #endregion
    }
}