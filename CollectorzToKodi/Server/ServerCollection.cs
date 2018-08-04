// <copyright file="ServerCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// Class to manage List of servers
    /// </summary>
    public class ServerCollection : IMediaCollection
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
        /// Name of mediaLanguage
        /// </summary>
        private List<Server> servers;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public ServerCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.servers = new List<Server>();
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
        /// Gets or sets name of mediaLanguage
        /// </summary>
        public List<Server> Servers
        {
            get { return this.servers; }
            set { this.servers = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of Server
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

        #endregion
    }
}