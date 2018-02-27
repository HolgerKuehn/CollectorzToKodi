// <copyright file="MediaLanguageCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage List of mediaLanguages
    /// </summary>
    public class MediaLanguageCollection
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
        private List<MediaLanguage> mediaLanguages;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaLanguageCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaLanguageCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.mediaLanguages = new List<MediaLanguage>();
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
        public List<MediaLanguage> MediaLanguages
        {
            get { return this.mediaLanguages; }
            set { this.mediaLanguages = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of mediaLanguage from XML-node representing a mediaLanguage
        /// </summary>
        /// <param name="xmlMediaLanguages">xml-node representing a list of mediaLanguage</param>
        public virtual void ReadFromXml(XmlNode xmlMediaLanguages)
        {
            foreach (XmlNode xmlMediaLanguage in xmlMediaLanguages.XMLReadSubnodes("mediaLanguage"))
            {
                MediaLanguage mediaLanguage = new MediaLanguage(this.Configuration);
                mediaLanguage.ReadFromXml(xmlMediaLanguage);
                this.MediaLanguages.Add(mediaLanguage);
            }
        }

        /// <summary>
        /// exports MediaLanguage
        /// </summary>
        public virtual void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Media.Server[0].Number].StreamWriter;
            int i = 0;

            foreach (MediaLanguage mediaLanguage in this.MediaLanguages)
            {
                i++;
                nfoStreamWriter.Write("    <mediaLanguage" + (i == 1 ? " clear=\"true\"" : string.Empty) + ">");
                mediaLanguage.WriteToLibrary();
                nfoStreamWriter.WriteLine("</mediaLanguage>");
            }
        }

        #endregion
    }
}