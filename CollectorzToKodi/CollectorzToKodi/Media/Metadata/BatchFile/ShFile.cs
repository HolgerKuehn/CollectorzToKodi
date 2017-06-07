// <copyright file="ShFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Text;

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

        /// <inheritdoc/>
        public override int Server
        {
            get
            {
                return base.Server;
            }

            set
            {
                base.Server = value;
                this.Filename = "NFO" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][this.Server.ToString()] + "Win.sh";
                this.URL = this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename;

                this.StreamWriter = new StreamWriter(this.URL, false, Encoding.UTF8, 512);
                this.ExportLibrary();
            }
        }

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
            shFileClone.StreamWriter = this.StreamWriter;

            return (ShFile)shFileClone;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(false);
        }

        /// <inheritdoc/>
        public override void ExportLibrary()
        {
            // SH file header
            this.StreamWriter.WriteLine("#!/bin/bash");
            this.StreamWriter.WriteLine(string.Empty);
        }

        #endregion
    }
}
