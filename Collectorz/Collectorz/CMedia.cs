// <copyright file="CMedia.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace Collectorz
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Base class for media content
    /// </summary>
    public abstract class CMedia
    {
        #region Attributes

        /// <summary>
        /// Current configuration of Collectorz
        /// </summary>
        private CConfiguration configuration;

        /// <summary>
        /// Title of media
        /// </summary>
        private string title;

        /// <summary>
        /// Title of media used for sorting
        /// </summary>
        private string titleSort;

        /// <summary>
        /// Original title of media
        /// </summary>
        private string titleOriginal;

        /// <summary>
        /// Group of media; eg. album for music or sets for movies
        /// </summary>
        private string mediaGroup;

        /// <summary>
        /// Rating of media
        /// </summary>
        private string rating;

        /// <summary>
        /// Publishing year of media
        /// </summary>
        private string publishingYear;

        /// <summary>
        /// Publishing date of media
        /// </summary>
        private string publishingDate;

        /// <summary>
        /// content of media, eg. song-text for music or plot for movie
        /// </summary>
        private string content;

        /// <summary>
        /// Run time of media
        /// </summary>
        private string runTime;

        /// <summary>
        /// Images associated with media
        /// </summary>
        private List<CImageFile> images;

        /// <summary>
        /// Country of media
        /// </summary>
        private string country;

        /// <summary>
        /// List genres of media
        /// </summary>
        private List<string> genres;

        /// <summary>
        /// List of studios of media
        /// </summary>
        private List<string> studios;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMedia"/> class.
        /// </summary>
        /// <param name="configuration">Current configuration of Collectorz</param>
        public CMedia(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.title = string.Empty;
            this.titleSort = string.Empty;
            this.titleOriginal = string.Empty;
            this.mediaGroup = string.Empty;
            this.rating = string.Empty;
            this.publishingYear = string.Empty;
            this.publishingDate = string.Empty;
            this.content = string.Empty;
            this.runTime = string.Empty;
            this.images = new List<CImageFile>();
            this.country = string.Empty;
            this.genres = new List<string>();
            this.studios = new List<string>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of Collectorz
        /// </summary>
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets title of media
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets or sets title of media used for sorting
        /// </summary>
        public string TitleSort
        {
            get { return this.titleSort; }
            set { this.titleSort = value; }
        }

        /// <summary>
        /// Gets or sets Original title of media
        /// </summary>
        public string TitleOriginal
        {
            get { return this.titleOriginal; }
            set { this.titleOriginal = value; }
        }

        /// <summary>
        /// Gets or sets group of media; eg. album for music or sets for movies
        /// </summary>
        public string MediaGroup
        {
            get { return this.mediaGroup; }
            set { this.mediaGroup = value; }
        }

        /// <summary>
        /// Gets or sets rating of media
        /// </summary>
        public string Rating
        {
            get { return this.rating; }
            set { this.rating = value; }
        }

        /// <summary>
        /// Gets or sets publishing year of media
        /// </summary>
        public string PublishingYear
        {
            get { return this.publishingYear; }
            set { this.publishingYear = value; }
        }

        /// <summary>
        /// Gets or sets publishing date of media
        /// </summary>
        public string PublishingDate
        {
            get { return this.publishingDate; }
            set { this.publishingDate = value; }
        }

        /// <summary>
        /// Gets or sets content of media, eg. song-text for music or plot for movie
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets run time of media
        /// </summary>
        public string RunTime
        {
            get { return this.runTime; }
            set { this.runTime = value; }
        }

        /// <summary>
        /// Gets or sets images associated with media
        /// </summary>
        public List<CImageFile> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        /// <summary>
        /// Gets or sets country of media
        /// </summary>
        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        /// <summary>
        /// Gets or sets list genres of media
        /// </summary>
        public List<string> Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }

        /// <summary>
        /// Gets or sets list of studios of media
        /// </summary>
        public List<string> Studios
        {
            get { return this.studios; }
            set { this.studios = value; }
        }

        #endregion
        #region Functions
        #endregion
    }
}
