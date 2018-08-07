// <copyright file="IMediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// Interface for all Collection Classes
    /// </summary>
    public interface IServerCollection : ICollection
    {
        /// <summary>
        /// Gets or sets Server containing collection
        /// </summary>
        Server Server { get; set; }
    }
}