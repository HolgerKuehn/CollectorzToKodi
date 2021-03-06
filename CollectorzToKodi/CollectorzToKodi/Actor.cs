﻿// <copyright file="Actor.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to mange Actors
    /// </summary>
    public class Actor : Person
    {
        #region Attributes

        /// <summary>
        /// role played by actor
        /// </summary>
        private string role;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class.<br/>
        /// <br/>
        /// Initializes actor with blank values.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Actor(Configuration configuration)
            : base(configuration)
        {
            this.role = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets role played by actor
        /// </summary>
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override void ReadPerson(XmlNode xmlActor)
        {
            // extract person and set basic properties
            XmlNode xmlPerson = xmlActor.XMLReadSubnode("person");
            base.ReadPerson(xmlPerson);

            // add role
            this.Role = xmlActor.XMLReadSubnode("character").XMLReadInnerText(string.Empty);
        }

        /// <inheritdoc/>
        public override void WritePerson(StreamWriter swrNFO, bool isFirst = false)
        {
            swrNFO.WriteLine("    <actor" + (isFirst ? " clear=\"true\"" : string.Empty) + ">");
            swrNFO.WriteLine("        <role>" + this.Role + "</role>");

            base.WritePerson(swrNFO, isFirst);

            swrNFO.WriteLine("    </actor>");
        }

        #endregion
    }
}