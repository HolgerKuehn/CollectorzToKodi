// <copyright file="NfoFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage video files
    /// </summary>
    public class NfoFile : MediaFile
    {
        #region Attributes
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NfoFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public NfoFile(Configuration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties
        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            NfoFile nfoFileClone = new NfoFile(this.Configuration);
            nfoFileClone.Description = this.Description;
            nfoFileClone.URL = this.URL;
            nfoFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            nfoFileClone.Filename = this.Filename;
            nfoFileClone.Extension = this.Extension;
            nfoFileClone.Server = this.Server;
            nfoFileClone.Media = this.Media;
            nfoFileClone.FileIndex = this.FileIndex;

            return (NfoFile)nfoFileClone;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(false);
        }

        #endregion
    }
}
