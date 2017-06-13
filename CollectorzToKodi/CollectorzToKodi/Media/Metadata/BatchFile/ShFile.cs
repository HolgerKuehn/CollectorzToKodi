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
                this.UrlForMediaStorage = this.Configuration.MovieCollectorUrlForXMLExportPath + this.Filename;

                this.StreamWriter = new StreamWriter(this.UrlForMediaStorage, false, Encoding.UTF8, 512);
                this.WriteToLibrary();
            }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            ShFile shFileClone = new ShFile(this.Configuration);
            shFileClone.Description = this.Description;
            shFileClone.UrlForMediaStorage = this.UrlForMediaStorage;
            shFileClone.Extension = this.Extension;
            shFileClone.FileIndex = this.FileIndex;
            shFileClone.StreamWriter = null;

            shFileClone.Media = this.Media;
            shFileClone.Server = this.Server;
            shFileClone.Filename = this.Filename;

            return (ShFile)shFileClone;
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
