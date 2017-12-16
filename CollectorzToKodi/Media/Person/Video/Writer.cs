// <copyright file="Writer.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to mange Writers
    /// </summary>
    public class Writer : Person
    {
        #region Attributes
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Writer"/> class.<br/>
        /// <br/>
        /// Initializes Writer with blank values.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Writer(Configuration configuration)
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

            nfoStreamWriter.WriteLine("    <credits" + (isFirst ? " clear=\"true\"" : string.Empty) + ">" + this.Name + "</credits>");
        }

        #endregion
    }
}