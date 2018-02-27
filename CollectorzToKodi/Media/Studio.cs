// <copyright file="Studio.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage studios
    /// </summary>
    public class Studio
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
        /// Name of studio
        /// </summary>
        private string name;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Studio"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Studio(Configuration configuration)
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
        /// Gets or sets name of studio
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of studio from XML-node representing a studio
        /// </summary>
        /// <param name="xmlStudio">xml-node representing a studio</param>
        public virtual void ReadFromXml(XmlNode xmlStudio)
        {
            this.Name = xmlStudio.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
        }

        /// <summary>
        /// exports Studio
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
