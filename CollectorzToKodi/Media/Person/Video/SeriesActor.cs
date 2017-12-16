// <copyright file="SeriesActor.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Xml;

    /// <summary>
    /// class to mange Actors in Series
    /// </summary>
    public class SeriesActor : Actor
    {
        #region Attributes

        /// <summary>
        /// season the actor is playing in
        /// </summary>
        private List<string> seasons;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SeriesActor"/> class.<br/>
        /// <br/>
        /// Initializes SeriesActor with blank values.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public SeriesActor(Configuration configuration)
            : base(configuration)
        {
            this.seasons = new List<string>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets season the actor is playing in
        /// </summary>
        public List<string> Seasons
        {
            get { return this.seasons; }
            set { this.seasons = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Sets Season for actor, as specified in role
        /// </summary>
        /// <param name="role">role played by actor</param>
        /// <returns>role without special tags containing meta-data for season</returns>
        /// <remarks>Meta-Tag available are:<br />
        /// (S###) - defines season the actor should be displayed in
        /// </remarks>
        public string OverrideSeason(string role)
        {
            if (this.Seasons == null)
            {
                this.Seasons = new List<string>();
            }

            if (this.Seasons.Count == 0)
            {
                this.Seasons.Add(string.Empty);
            }

            string seasonIdentifierLeft = this.Configuration.MovieCollectorSeason.LeftOf("<Season>");
            string seasonIdentifierRight = this.Configuration.MovieCollectorSeason.RightOf("<Season>");
            string season = string.Empty;

            if (role.Contains(seasonIdentifierLeft))
            {
                season = role.RightOfLast(seasonIdentifierLeft).LeftOf(seasonIdentifierRight).ReplaceAll(" ", string.Empty);
                role = role.LeftOfLast(seasonIdentifierLeft);
            }

            this.Seasons = season.Split(",");

            return role.Trim();
        }

        /// <inheritdoc/>
        public override void ReadPersonFromXml(XmlNode xmlActor)
        {
            // extract actor from xmlActor
            base.ReadPersonFromXml(xmlActor);

            // extract Seasons from role
            this.Role = this.OverrideSeason(this.Role);
        }

        #endregion
    }
}