// <copyright file="StudioCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage List of studios
    /// </summary>
    public class StudioCollection
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
        private List<Studio> studios;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StudioCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public StudioCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.studios = new List<Studio>();
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
        public List<Studio> Studios
        {
            get { return this.studios; }
            set { this.studios = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of studio from XML-node representing a studio
        /// </summary>
        /// <param name="xmlStudios">xml-node representing a list of studio</param>
        public virtual void ReadFromXml(XmlNode xmlStudios)
        {
            foreach (XmlNode xmlStudio in xmlStudios.XMLReadSubnodes("studio"))
            {
                Studio studio = new Studio(this.Configuration);
                studio.ReadFromXml(xmlStudio);
                this.Studios.Add(studio);
            }
        }

        /// <summary>
        /// exports Studio
        /// </summary>
        public virtual void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Media.Server[0].Number].StreamWriter;
            int i = 0;

            foreach (Studio studio in this.Studios)
            {
                i++;
                nfoStreamWriter.Write("    <studio" + (i == 1 ? " clear=\"true\"" : string.Empty) + ">");
                studio.WriteToLibrary();
                nfoStreamWriter.WriteLine("</studio>");
            }
        }

        #endregion
    }
}