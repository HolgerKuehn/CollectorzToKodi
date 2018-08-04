// <copyright file="ShFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Class to manage video files
    /// </summary>
    public class ShFile : BatchFile
    {
        #region Attributes
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ShFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public ShFile(Configuration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties
        #endregion
        #region Functions

        /// <inheritdoc/>>
        public override MediaFile Clone()
        {
            throw new System.NotImplementedException();
        }

        /// <inheritdoc/>>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
        }

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            // SH file header
            this.StreamWriter.WriteLine("#!/bin/bash");
            this.StreamWriter.WriteLine(string.Empty);
        }

        #endregion
    }
}
