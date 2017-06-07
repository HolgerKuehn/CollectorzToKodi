// <copyright file="BatchFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// Class to manage video files
    /// </summary>
    public abstract class BatchFile : MediaFile
    {
        #region Attributes
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
        #endregion
        #region Functions
        #endregion
    }
}
