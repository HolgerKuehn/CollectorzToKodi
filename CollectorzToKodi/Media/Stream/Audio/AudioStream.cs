// <copyright file="AudioStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to manage AudioStreams
    /// </summary>
    public class AudioStream : MediaStream
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
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public AudioStream(Configuration configuration)
            : base(configuration)
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

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            string displayname = xMLAudio.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);

            this.Codec = "AC3";
            this.Language = displayname.RightOf("[").LeftOf("]");

            if (displayname.LeftOf("[").Contains("2.0") || displayname.LeftOf("[").Contains("Stereo"))
            {
                this.NumberOfChannels = "2";
            }

            if (displayname.LeftOf("[").Contains("5.1"))
            {
                this.NumberOfChannels = "6";
            }
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
        }

        #endregion
    }
}
