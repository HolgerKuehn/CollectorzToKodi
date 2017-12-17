// <copyright file="SubTitleFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing Subtitles
    /// </summary>
    public class SubTitleFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// SubTitle containing this SubTitleFile
        /// </summary>
        private SubTitleStream subTitleStream;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SubTitleFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SubTitleFile(Configuration configuration)
            : base(configuration)
        {
            this.subTitleStream = null;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets SubTitle containing this SubTitleFile
        /// </summary>
        public SubTitleStream SubTitleStream
        {
            get { return this.subTitleStream; }
            set { this.subTitleStream = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            SubTitleFile subTitleFileClone = new SubTitleFile(this.Configuration);

            // MediaFile
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.MediaFilePath = (MediaFilePath)this.MediaFilePath.Clone();
            subTitleFileClone.Media = this.Media;
            subTitleFileClone.Server = this.Server.Clone();
            subTitleFileClone.FileIndex = this.FileIndex;

            // subTitleFile
            subTitleFileClone.subTitleStream = this.subTitleStream;

            return subTitleFileClone;
        }

        /// <inheritdoc/>
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
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Media.Server[0].Number].StreamWriter;

            // copy SubTitleFile to destination, as it's converted during Export (Offset-times, etc.)
            // valid for all types of SubTitles
            if (this.MediaFilePath.DevicePathForPublication != string.Empty)
            {
                bfStreamWriter.WriteLine("/bin/cp \"" + this.MediaFilePath.DevicePathForPublication + "\"" + this.MediaFilePath.Filename + "\"");
            }
        }

        #endregion
    }
}
