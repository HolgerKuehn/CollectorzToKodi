// <copyright file="Actor.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// class to mange Actors
    /// </summary>
    public class Actor : Person
    {
        #region Attributes

        /// <summary>
        /// role played by actor
        /// </summary>
        private string role;

        /// <summary>
        /// url to profile
        /// </summary>
        private string url;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Actor"/> class.<br/>
        /// <br/>
        /// Initializes actor with blank values.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Actor(Configuration configuration)
            : base(configuration)
        {
            this.role = string.Empty;
            this.url = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets role played by actor
        /// </summary>
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }

        /// <summary>
        /// Gets or sets url to profile of actor
        /// </summary>
        public string URL
        {
            get { return this.url; }
            set { this.url = value; }
        }

        #endregion
        #region Functions
        #endregion
    }
}