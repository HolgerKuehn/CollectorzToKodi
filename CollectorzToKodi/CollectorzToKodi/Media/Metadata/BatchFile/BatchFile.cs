// <copyright file="BatchFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;

    /// <summary>
    /// Class to manage video files
    /// </summary>
    public abstract class BatchFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// StreamWriter representing the actual batch file
        /// </summary>
        private StreamWriter swrBatchFile;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BatchFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public BatchFile(Configuration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets StreamWriter representing the actual batch file
        /// </summary>
        public StreamWriter SwrBatchFile
        {
            get { return this.swrBatchFile; }
            set { this.swrBatchFile = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// writes Header to BatchFile
        /// </summary>
        public abstract void WriteHeader();

        #endregion
    }
}
