// <copyright file="Person.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to manage persons from CollectorzToKodi-Programs<br/>
    /// </summary>
    public abstract class Person
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// full name displayed in Kodi<br/>
        /// </summary>
        private string name;

        /// <summary>
        /// url to profile
        /// </summary>
        private string url;

        /// <summary>
        /// link to thumb of person<br/>
        /// can hold url or local-file link to thumb<br/>
        /// </summary>
        private string thumb;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Person(Configuration configuration)
        {
            this.configuration = configuration;
            this.name = string.Empty;
            this.url = string.Empty;
            this.thumb = string.Empty;
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
        /// Gets or sets full name displayed in Kodi
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets url to profile of actor
        /// </summary>
        public string URL
        {
            get { return this.url; }
            set { this.url = value; }
        }

        /// <summary>
        /// Gets or sets link to thumb of person<br/>
        /// can hold url or local-file link to thumb
        /// </summary>
        public string Thumb
        {
            get { return this.thumb; }
            set { this.thumb = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of person from XML-node representing a person
        /// </summary>
        /// <param name="xmlPerson">xml-node representing a person</param>
        public virtual void ReadPerson(XmlNode xmlPerson)
        {
            this.Name = xmlPerson.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
            this.URL = xmlPerson.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
            this.Thumb = xmlPerson.XMLReadSubnode("imageurl").XMLReadInnerText(string.Empty);
        }

        /// <summary>
        /// Writes person to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the image information should be added to</param>
        /// <param name="isFirst">states, weather the written Person is the fist in the generated list or not; resets list in Kodi, if so</param>
        public virtual void WritePerson(StreamWriter swrNFO, bool isFirst = false)
        {
            swrNFO.WriteLine("        <name>" + this.Name + "</name>");

            if (this.URL.StartsWith("http"))
            {
                swrNFO.WriteLine("        <url>" + this.URL + "</url>");
            }

            if (this.Thumb.StartsWith("http"))
            {
                swrNFO.WriteLine("        <thumb>" + this.Thumb + "</thumb>");
            }
        }

        #endregion
    }
}
