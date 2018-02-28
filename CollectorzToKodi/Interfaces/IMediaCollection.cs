// <copyright file="IMediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Xml;

    /// <summary>
    /// Interface for all Collection Classes
    /// </summary>
    public interface IMediaCollection : ICollection
    {
        /// <summary>
        /// Gets or sets media containing collection
        /// </summary>
        Media Media { get; set; }
    }
}