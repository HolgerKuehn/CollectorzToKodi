// <copyright file="ICollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Xml;

    /// <summary>
    /// Interface for all Collection Classes
    /// </summary>
    public interface ICollection
    {
        /// <summary>
        /// Gets current configuration of CollectorzToKodi
        /// </summary>
        Configuration Configuration { get; }

        /// <summary>
        /// extracts properties from XML-node
        /// </summary>
        /// <param name="xmlGenres">xml-node representing baseclass for collection</param>
        void ReadFromXml(XmlNode xmlGenres);

        /// <summary>
        /// delete from Library
        /// </summary>
        void DeleteFromLibrary();

        /// <summary>
        /// exports Collection
        /// </summary>
        void WriteToLibrary();
    }
}