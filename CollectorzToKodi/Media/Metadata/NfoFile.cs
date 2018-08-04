// <copyright file="NfoFile.cs" company="Holger Kühn">
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
    public class NfoFile : MetadataFile
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

        ///// <inheritdoc/>
        //public override Media Media
        //{
        //    get
        //    {
        //        return base.Media;
        //    }

        //    set
        //    {
        //        base.Media = value;
        //        this.MediaFilePath.Filename = this.Media.MediaPath.Filename + ".nfo";
        //        this.MediaFilePath.WindowsPath = this.Configuration.MovieCollectorWindowsPathToXmlExportPath + this.MediaFilePath.Filename;
        //        this.MediaFilePath.WindowsPathForPublication = this.MediaFilePath.WindowsPath;

        //        this.StreamWriter = new StreamWriter(this.MediaFilePath.WindowsPathForPublication, false, Encoding.UTF8, 512);
        //    }
        //}

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            NfoFile nfoFileClone = new NfoFile(this.Configuration);
            nfoFileClone.Description = this.Description;
            nfoFileClone.FileIndex = this.FileIndex;

            nfoFileClone.Media = this.Media;
            nfoFileClone.Server = this.Server;
            //nfoFileClone.MediaFilePath = (MediaFilePath)this.MediaFilePath.Clone();

            return nfoFileClone;
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
        }

        #endregion
    }
}
