// <copyright file="MediaFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Xml;

    /// <summary>
    /// base type for media files, e.g. images, videos, etc.
    /// </summary>
    public abstract class MediaFile
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// description of media file
        /// </summary>
        private string description;

        /// <summary>
        /// path of MediaFile
        /// </summary>
        private MediaFilePath mediaFilePath;

        /// <summary>
        /// media containing file
        /// </summary>
        private Media media;

        /// <summary>
        /// list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        private Server server;

        /// <summary>
        /// index of MediaFile file
        /// </summary>
        private int fileIndex;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaFile(Configuration configuration)
        {
            this.configuration = configuration;
            this.description = string.Empty;
            this.media = null;
            this.mediaFilePath = null;
            this.server = null;
            this.fileIndex = 1;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets description of media file
        /// </summary>
        public string Description
        {
            get { return this.description; }
            set { this.description = value; }
        }

        /// <summary>
        /// Gets or sets path to Media
        /// </summary>
        public virtual MediaFilePath MediaFilePath
        {
            get
            {
                if (this.mediaFilePath == null)
                {
                    this.mediaFilePath = new MediaFilePath(this.Configuration);
                }

                return this.mediaFilePath;
            }

            set
            {
                this.mediaFilePath = value;
            }
        }

        /// <summary>
        /// Gets or sets media containing file
        /// </summary>
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        public virtual Server Server
        {
            get { return this.server; }
            set { this.server = value; }
        }

        /// <summary>
        /// Gets or sets file index of MediaFile
        /// </summary>
        public int FileIndex
        {
            get { return this.fileIndex; }
            set { this.fileIndex = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones MediaFile object completely
        /// </summary>
        /// <returns>new instance of MediaFile</returns>
        public abstract MediaFile Clone();

        /// <summary>
        /// Reads XML-files into media collection
        /// </summary>
        /// <param name="xMLMedia">part of XML export representing Movie, Series, Episode or Music</param>
        public abstract void ReadFromXml(XmlNode xMLMedia);

        /// <summary>
        /// delete from Library
        /// </summary>
        public abstract void DeleteFromLibrary();

        /// <summary>
        /// exports Library to Disk
        /// </summary>
        public abstract void WriteToLibrary();

        #endregion
    }
}
