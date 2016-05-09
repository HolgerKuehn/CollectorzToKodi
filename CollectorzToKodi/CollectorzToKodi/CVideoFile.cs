// <copyright file="CVideoFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// Class to manage video files
    /// </summary>
    public class CVideoFile : CMediaFile
    {
        #region Attributes

        /// <summary>
        /// indicates whether the video files is a special or not
        /// </summary>
        private bool isSpecial;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CVideoFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CVideoFile(CConfiguration configuration)
            : base(configuration)
        {
            this.isSpecial = false;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether the video files is a special or not
        /// </summary>
        public bool IsSpecial
        {
            get { return this.isSpecial; }
            set { this.isSpecial = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override CMediaFile Clone()
        {
            CVideoFile videoFileClone = new CVideoFile(this.Configuration);
            videoFileClone.IsSpecial = this.IsSpecial;
            videoFileClone.Description = this.Description;
            videoFileClone.URL = this.URL;
            videoFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            videoFileClone.Filename = this.Filename;
            videoFileClone.Extension = this.Extension;
            videoFileClone.Server = this.Server;
            videoFileClone.Media = this.Media;

            return (CMediaFile)videoFileClone;
        }

        /// <inheritdoc/>
        public override string ConvertFilename()
        {
            return this.ConvertFilename(true);
        }

        /// <summary>
        /// Sets Special from special tags present in title
        /// </summary>
        /// <param name="title">title of image, containing tags for specials</param>
        /// <returns>title of image cleaned of tags for specials</returns>
        public string OverrideSpecial(string title)
        {
            if (title.Contains("(Special)"))
            {
                this.isSpecial = true;
                title = title.Replace("(Special)", string.Empty);
            }

            return title.Trim();
        }

        #endregion
    }
}
