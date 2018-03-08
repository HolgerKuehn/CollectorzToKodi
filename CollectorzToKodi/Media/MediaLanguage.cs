﻿// <copyright file="MediaLanguage.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage mediaLanguages
    /// </summary>
    public class MediaLanguage
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
        private string name;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLanguage"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaLanguage(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.name = string.Empty;
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
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of mediaLanguage from XML-node representing a mediaLanguage
        /// </summary>
        /// <param name="xmlMediaLanguage">xml-node representing a mediaLanguage</param>
        public virtual void ReadFromXml(XmlNode xmlMediaLanguage)
        {
            this.Name = xmlMediaLanguage.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
        }

        /// <summary>
        /// exports MediaLanguage
        /// </summary>
        public virtual void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Media.Server[0].Number].StreamWriter;

            nfoStreamWriter.Write(this.Name);
        }

        #endregion
    }
}