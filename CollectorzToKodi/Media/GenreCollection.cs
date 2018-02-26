// <copyright file="GenreCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class to manage List of genres
    /// </summary>
    public class GenreCollection
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// media containing person
        /// </summary>
        private Media media;

        /// <summary>
        /// Name of genre
        /// </summary>
        private List<Genre> genres;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="GenreCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public GenreCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.media = null;
            this.genres = new List<Genre>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets media containing file
        /// </summary>
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets name of genre
        /// </summary>
        public List<Genre> Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// extracts properties of genre from XML-node representing a genre
        /// </summary>
        /// <param name="xmlGenres">xml-node representing a list of genre</param>
        public virtual void ReadFromXml(XmlNode xmlGenres)
        {
            foreach (XmlNode xmlGenre in xmlGenres.XMLReadSubnodes("genre"))
            {
                Genre genre = new Genre(this.Configuration);
                genre.ReadFromXml(xmlGenre);
                this.Genres.Add(genre);
            }
        }

        /// <summary>
        /// exports Genre
        /// </summary>
        public virtual void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Media.Server[0].Number].StreamWriter;
            int i = 0;

            foreach (Genre genre in this.Genres)
            {
                i++;
                nfoStreamWriter.Write("    <genre" + (i == 1 ? " clear=\"true\"" : string.Empty) + ">");
                genre.WriteToLibrary();
                nfoStreamWriter.WriteLine("</genre>");
            }
        }

        #endregion
    }
}