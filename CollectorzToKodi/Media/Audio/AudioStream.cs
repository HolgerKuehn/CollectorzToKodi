// <copyright file="AudioStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// class to manage AudioStreams
    /// </summary>
    public class AudioStream
    {
        #region Attributes

        /// <summary>
        /// codec of audio stream
        /// </summary>
        private string codec;

        /// <summary>
        /// language of audio stream
        /// </summary>
        private string language;

        /// <summary>
        /// number of channels used in this audio stream
        /// </summary>
        private string numberOfChannels;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AudioStream"/> class.
        /// </summary>
        public AudioStream()
        {
            this.codec = "AC3";
            this.language = "Deutsch";
            this.numberOfChannels = "2";
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets codec of audio stream
        /// </summary>
        public string Codec
        {
            get { return this.codec; }
            set { this.codec = value; }
        }

        /// <summary>
        /// Gets or sets language of audio stream
        /// </summary>
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        /// <summary>
        /// Gets or sets number of channels used in this audio stream
        /// </summary>
        public string NumberOfChannels
        {
            get { return this.numberOfChannels; }
            set { this.numberOfChannels = value; }
        }

        #endregion
        #region Functions

        #endregion
    }
}
