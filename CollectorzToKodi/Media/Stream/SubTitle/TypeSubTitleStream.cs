// <copyright file="TypeSubTitleStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;

    /// <summary>
    /// class to manage TypeSubTitleStreams
    /// </summary>
    public abstract class TypeSubTitleStream : SubTitleStream
    {
        #region Attributes

        /// <summary>
        /// source files of SubTitle
        /// </summary>
        private List<SubTitleFile> sourceSubTitleFiles;

        /// <summary>
        /// destination file of SubTitle
        /// </summary>
        private SubTitleFile destinationSubTitleFile;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TypeSubTitleStream"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public TypeSubTitleStream(Configuration configuration)
            : base(configuration)
        {
            this.destinationSubTitleFile = new SubTitleFile(configuration);
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets list of source files of SubTitle
        /// </summary>
        public List<SubTitleFile> SourceSubTitleFiles
        {
            get
            {
                if (this.sourceSubTitleFiles == null)
                {
                    this.sourceSubTitleFiles = new List<SubTitleFile>();
                }

                return this.sourceSubTitleFiles;
            }

            set
            {
                this.sourceSubTitleFiles = value;
            }
        }

        /// <summary>
        /// Gets or sets list of destination file of SubTitle
        /// </summary>
        public SubTitleFile DestinationSubTitleFile
        {
            get { return this.destinationSubTitleFile; }
            set { this.destinationSubTitleFile = value; }
        }
        #endregion
        #region Functions
        #endregion
    }
}
