// <copyright file="SubTitleStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// class to manage SubTitleStreams
    /// </summary>
    public class SubTitleStream : MediaStream
    {
        #region Attributes

        /// <summary>
        /// language provides by subtitle
        /// </summary>
        private string language;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTitleStream"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SubTitleStream(Configuration configuration)
            : base(configuration)
        {
            this.language = "Deutsch";
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets language provides by subtitle
        /// </summary>
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>>
        public override MediaStream Clone()
        {
            SubTitleStream subTitleStreamClone = new SubTitleStream(this.Configuration);

            // MediaStream
            subTitleStreamClone.Media = this.Media;

            // SubTitleStream
            subTitleStreamClone.Language = this.Language;

            return (SubTitleStream)subTitleStreamClone;
        }

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;

            nfoStreamWriter.WriteLine("            <subtitle>");
            nfoStreamWriter.WriteLine("                <language>" + this.Language + "</language>");
            nfoStreamWriter.WriteLine("            </subtitle>");
        }

        #endregion
    }
}
