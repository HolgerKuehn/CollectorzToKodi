// <copyright file="CActor.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace Collectorz
{
    /// <summary>
    /// class to mange Actors
    /// </summary>
    public class CActor : CPerson
    {
        #region Attributes

        /// <summary>
        /// role played by actor
        /// </summary>
        private string role;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CActor"/> class.<br/>
        /// <br/>
        /// Initializes actor with blank values.
        /// </summary>
        public CActor()
            : base()
        {
            this.role = string.Empty;
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

        #endregion
    }
}