// <copyright file="ImageFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// Class managing images
    /// </summary>
    public class ImageFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// type of image
        /// </summary>
        private Configuration.ImageType imageType;

        /// <summary>
        /// season the image is assigned to
        /// </summary>
        private string season;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public ImageFile(Configuration configuration)
            : base(configuration)
        {
            this.imageType = Configuration.ImageType.Unknown;
            this.season = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets type of image
        /// </summary>
        public Configuration.ImageType ImageType
        {
            get { return this.imageType; }
            set { this.imageType = value; }
        }

        /// <summary>
        /// Gets or sets season the image is assigned to
        /// </summary>
        public string Season
        {
            get { return this.season; }
            set { this.season = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            ImageFile imageFileClone = new ImageFile(this.Configuration);
            imageFileClone.ImageType = this.ImageType;
            imageFileClone.Season = this.Season;
            imageFileClone.Description = this.Description;
            imageFileClone.UrlForMediaStorage = this.UrlForMediaStorage;
            imageFileClone.Extension = this.Extension;

            imageFileClone.Media = this.Media;
            imageFileClone.Server = this.Server;
            imageFileClone.Filename = this.Filename;

            return (ImageFile)imageFileClone;
        }

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
        }

        /// <summary>
        /// Sets season from special tags present in title
        /// </summary>
        /// <param name="title">title of image, containing tags for season</param>
        /// <returns>title of image cleaned of tags for season</returns>
        public string OverrideSeason(string title)
        {
            this.Season = "0";

            if (title.Contains("(S"))
            {
                this.Season = title.RightOf("(S").LeftOf(")");
                title = title.Replace("(S" + this.Season + ")", string.Empty);
            }

            return title.Trim();
        }

        #endregion
    }
}
