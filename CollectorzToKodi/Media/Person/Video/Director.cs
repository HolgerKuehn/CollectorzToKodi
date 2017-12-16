// <copyright file="Director.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to mange Directors
    /// </summary>
    public class Director : Person
    {
        #region Attributes
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Director"/> class.<br/>
        /// <br/>
        /// Initializes Director with blank values.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Director(Configuration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties
        #endregion
        #region Functions

        /// <inheritdoc/>
        public override void WritePersonToLibrary(bool isFirst = false)
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;

            nfoStreamWriter.WriteLine("    <director" + (isFirst ? " clear=\"true\"" : string.Empty) + ">" + this.Name + "</director>");
        }

        #endregion
    }
}