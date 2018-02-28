// <copyright file="PersonCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage List of Persons
    /// </summary>
    public abstract class PersonCollection : IMediaCollection
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
        /// Name of Person
        /// </summary>
        private List<Person> persons;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public PersonCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.Persons = new List<Person>();
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
        /// Gets or sets name of Person
        /// </summary>
        public List<Person> Persons
        {
            get { return this.Persons; }
            set { this.Persons = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of Person from XML-node representing a Person
        /// </summary>
        /// <param name="xmlPersons">xml-node representing a list of Person</param>
        public abstract void ReadFromXml(XmlNode xmlPersons);

        /// <summary>
        /// delete from Library
        /// </summary>
        public abstract void DeleteFromLibrary();

        /// <summary>
        /// exports Person
        /// </summary>
        public abstract void WriteToLibrary();

        #endregion
    }
}