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

        /// <inheritdoc/>
        public override Server Server
        {
            get
            {
                return base.Server;
            }

            set
            {
                base.Server = value;
                this.MediaFilePath.Filename = "NFO" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][this.Server.Number.ToString()] + "Win.sh";
                this.MediaFilePath.WindowsPath = this.Configuration.MovieCollectorWindowsPathToXmlExportPath + this.MediaFilePath.Filename;
                this.MediaFilePath.WindowsPathForPublication = this.MediaFilePath.WindowsPath;

                this.StreamWriter = new StreamWriter(this.MediaFilePath.WindowsPathForPublication, false, Encoding.UTF8, 512);
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
            shFileClone.FileIndex = this.FileIndex;
            shFileClone.StreamWriter = null;

            shFileClone.Media = this.Media;
            shFileClone.Server = this.Server;
            shFileClone.MediaFilePath = (MediaFilePath)this.MediaFilePath.Clone();

            return shFileClone;
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
