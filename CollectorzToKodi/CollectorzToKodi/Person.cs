// <copyright file="Person.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// class to manage persons from CollectorzToKodi-Programs<br/>
    /// </summary>
    public class Person
    {
        #region Attributes

        /// <summary>
        /// full name displayed in Kodi<br/>
        /// </summary>
        private string name;

        /// <summary>
        /// link to thumb of person<br/>
        /// can hold url or local-file link to thumb<br/>
        /// </summary>
        private string thumb;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        public Person()
        {
            this.name = string.Empty;
            this.thumb = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets full name displayed in Kodi
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets link to thumb of person<br/>
        /// can hold url or local-file link to thumb
        /// </summary>
        public string Thumb
        {
            get { return this.thumb; }
            set { this.thumb = value; }
        }

        #endregion
        #region Functions
        #endregion
    }
}
