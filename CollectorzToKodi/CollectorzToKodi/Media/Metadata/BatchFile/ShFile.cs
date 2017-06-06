// <copyright file="ShFile.cs" company="Holger Kühn">
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

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            ShFile shFileClone = new ShFile(this.Configuration);
            shFileClone.Description = this.Description;
            shFileClone.URL = this.URL;
            shFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            shFileClone.Filename = this.Filename;
            shFileClone.Extension = this.Extension;
            shFileClone.Server = this.Server;
            shFileClone.Media = this.Media;
            shFileClone.FileIndex = this.FileIndex;
            shFileClone.SwrBatchFile = this.SwrBatchFile;

            return (ShFile)shFileClone;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(false);
        }

        /// <inheritdoc/>
        public override void WriteHeader()
        {
            // SH file header
            this.SwrBatchFile.WriteLine("#!/bin/bash");
            this.SwrBatchFile.WriteLine(string.Empty);
        }

        #endregion
    }
}
