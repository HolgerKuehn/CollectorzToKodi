// <copyright file="MetadataFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Class to manage video files
    /// </summary>
    public abstract class MetadataFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// StreamWriter representing the actual batch file
        /// </summary>
        private StreamWriter streamWriter;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MetadataFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MetadataFile(Configuration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets StreamWriter representing the actual batch file
        /// </summary>
        public StreamWriter StreamWriter
        {
            get { return this.streamWriter; }
            set { this.streamWriter = value; }
        }

        #endregion
        #region Functions
        #endregion
    }
}
