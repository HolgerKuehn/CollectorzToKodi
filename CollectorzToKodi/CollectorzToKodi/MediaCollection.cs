// <copyright file="MediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

// <copyright file="CMediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>
namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// provides methods to handle the complete media-collection (e.g. movies and series) from MovieCollector
    /// </summary>
    public class MediaCollection
    {
        #region Attributes

        /// <summary>
        /// stores configuration for MediaCollection
        /// </summary>
        private readonly Configuration configuration;

        /// <summary>
        /// stores MediaCollection for movies
        /// </summary>
        private readonly List<Movie> movieCollection;

        /// <summary>
        /// stores MediaCollection for series
        /// </summary>
        private readonly List<Series> seriesCollection;

        /// <summary>
        /// stores Collection grouped by MediaGroup
        /// </summary>
        private List<Series> seriesCollectionGroupedByMediaGroup;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MediaCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public MediaCollection(Configuration configuration)
        {
            this.configuration = configuration;
            this.movieCollection = new List<Movie>();
            this.seriesCollection = new List<Series>();

            this.seriesCollectionGroupedByMediaGroup = new List<Series>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets current configuration used by MediaCollection
        /// </summary>
        /// <returns>current configuration used by MediaCollection</returns>
        public Configuration Configuration
        {
            get { return this.configuration; }
        }

        /// <summary>
        /// Gets current MovieCollection used by MediaCollection
        /// </summary>
        /// <returns>current MovieCollection used by MediaCollection</returns>
        public List<Movie> MovieCollection
        {
            get { return this.movieCollection; }
        }

        /// <summary>
        /// Gets current SeriesCollection used by MediaCollection
        /// </summary>
        /// <returns>current SeriesCollection used by MediaCollection</returns>
        public List<Series> SeriesCollection
        {
            get { return this.seriesCollection; }
        }

        /// <summary>
        /// Gets or sets stored Collection grouped by MediaGroup
        /// </summary>
        private List<Series> SeriesCollectionGroupedByMediaGroup
        {
            get { return this.seriesCollectionGroupedByMediaGroup; }
            set { this.seriesCollectionGroupedByMediaGroup = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// reads the MovieCollectoz XML export into MediaCollection
        /// </summary>
        /// <remarks>
        ///     requires user defined field:<br/>
        ///     "XBMC Movie" - true if entry is to be interpreted as a movie<br/>
        ///     "XBMC Series" - true if entry is to be interpreted as a series<br/>
        /// </remarks>
        /// <param name="eingabeXML">File that was used while export from MovieCollector - complete local path</param>
        public void ReadXML(string eingabeXML)
        {
            bool xMLMovieIsMovie = false;
            bool xMLMovieIsSeries = false;
            Video media = null;
            foreach (XmlNode xMLMovie in BaseClassExtention.XMLReadFile(eingabeXML, "movieinfo").XMLReadSubnode("movielist").XMLReadSubnodes("movie"))
            {
                #region evaluate Type and create media-object
                foreach (XmlNode xMLUserDefinedValue in xMLMovie.XMLReadSubnode("userdefinedvalues").XMLReadSubnodes("userdefinedvalue"))
                {
                    if (xMLUserDefinedValue.XMLReadSubnode("userdefinedfield").XMLReadSubnode("label").XMLReadInnerText(string.Empty) == "XBMC Movie")
                    {
                        xMLMovieIsMovie = xMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText(string.Empty) == "Yes" || xMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText(string.Empty) == "Ja";
                    }

                    if (xMLUserDefinedValue.XMLReadSubnode("userdefinedfield").XMLReadSubnode("label").XMLReadInnerText(string.Empty) == "XBMC Serie")
                    {
                        xMLMovieIsSeries = xMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText(string.Empty) == "Yes" || xMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText(string.Empty) == "Ja";
                    }
                }

                if (xMLMovieIsMovie)
                {
                    if (!this.Configuration.KodiExportMovieAsSeries)
                    {
                        media = new Movie(this.Configuration);
                    }
                    else
                    {
                        xMLMovieIsMovie = false;
                        xMLMovieIsSeries = true;
                    }
                }

                if (xMLMovieIsSeries)
                {
                    media = new Series(this.Configuration);
                }

                if (media == null)
                {
                    continue; // TODO: ### write error log
                }

                #endregion
                #region Media (Movie & Series)

                media.Title = xMLMovie.XMLReadSubnode("title").XMLReadInnerText(string.Empty);
                media.Title = media.OverrideVideoStreamData(media.Title);
                media.PublishingYear = xMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("year").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                media.TitleSort = xMLMovie.XMLReadSubnode("titlesort").XMLReadInnerText(string.Empty);
                media.TitleOriginal = xMLMovie.XMLReadSubnode("originaltitle").XMLReadInnerText(string.Empty);
                media.MediaGroup = xMLMovie.XMLReadSubnode("series").XMLReadSubnode("displayname").XMLReadInnerText(media.Title);
                media.Rating = xMLMovie.XMLReadSubnode("imdbrating").XMLReadInnerText(string.Empty);
                media.Content = xMLMovie.XMLReadSubnode("plot").XMLReadInnerText(string.Empty);
                media.RunTime = xMLMovie.XMLReadSubnode("runtime").XMLReadInnerText(string.Empty);
                media.MPAA = xMLMovie.XMLReadSubnode("mpaarating").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                media.IMDbId = "tt" + xMLMovie.XMLReadSubnode("imdbnum").XMLReadInnerText(string.Empty);
                media.Country = xMLMovie.XMLReadSubnode("country").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                media.ReadImages(xMLMovie);
                media.ReadGenre(xMLMovie);
                media.ReadStudio(xMLMovie);
                media.ReadCrew(xMLMovie);
                media.ReadCast(xMLMovie);
                media.ReadStreamData(xMLMovie);

                #endregion
                #region Movie

                if (xMLMovieIsMovie)
                {
                    ((Movie)media).VideoIndex = 1;
                    ((Movie)media).ReadMediaFiles(xMLMovie);
                    media.PublishingDate = xMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.PublishingYear);
                    media.PlayCount = (xMLMovie.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Yes" || xMLMovie.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Ja") ? "1" : "0";
                    media.PlayDate = xMLMovie.XMLReadSubnode("viewingdate").XMLReadSubnode("date").XMLReadInnerText(string.Empty);

                    if (media.HasSpecials() && media.MediaGroup == string.Empty)
                    {
                        media.MediaGroup = media.Title;
                    }

                    this.MovieCollection.Add((Movie)media);
                }

                #endregion
                #region Series

                if (xMLMovieIsSeries)
                {
                    // releasedate and number episodes in series
                    media.PublishingDate = xMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.PublishingYear);
                    ((Series)media).NumberOfTotalEpisodes = (int)int.Parse(xMLMovie.XMLReadSubnode("chapters").XMLReadInnerText(string.Empty));

                    int videoIndex = 0;

                    // episodes
                    foreach (XmlNode xMLSeriesDisc in xMLMovie.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
                    {
                        Episode seriesDiscEpisode = new Episode(this.Configuration);
                        seriesDiscEpisode.Series = (Series)media;
                        seriesDiscEpisode.ExtractSeriesData((Series)media);
                        seriesDiscEpisode.OverrideSeason(seriesDiscEpisode.OverrideVideoStreamData(xMLSeriesDisc.XMLReadSubnode("title").XMLReadInnerText(string.Empty)), false);

                        foreach (XmlNode xMLEpisode in xMLSeriesDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                        {
                            Episode episode = new Episode(this.Configuration);
                            videoIndex++;
                            episode.VideoIndex = videoIndex;

                            // reset Episode-Attributes to Disc-Attributes
                            episode.ExtractSeriesData(seriesDiscEpisode);

                            // read Episode-Details
                            episode.Series = (Series)media;
                            episode.Title = episode.OverrideSeason(episode.OverrideVideoStreamData(xMLEpisode.XMLReadSubnode("title").XMLReadInnerText(string.Empty)), true);
                            episode.TitleSort = episode.Title;
                            episode.TitleOriginal = episode.Title;
                            episode.MediaGroup = episode.Series.MediaGroup;
                            episode.Content = xMLEpisode.XMLReadSubnode("plot").XMLReadInnerText(string.Empty);
                            episode.RunTime = xMLEpisode.XMLReadSubnode("runtimeminutes").XMLReadInnerText(string.Empty);
                            episode.PublishingDate = xMLEpisode.XMLReadSubnode("firstairdate").XMLReadSubnode("date").XMLReadInnerText(episode.Series.PublishingDate);
                            episode.PlayCount = xMLEpisode.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Yes" || xMLEpisode.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Ja" ? "1" : "0";
                            episode.PlayDate = xMLEpisode.XMLReadSubnode("viewingdate").XMLReadSubnode("date").XMLReadInnerText(string.Empty);
                            episode.Genres = episode.Series.Genres;
                            episode.Studios = episode.Series.Studios;
                            episode.AudioStreams = episode.Series.AudioStreams;
                            episode.SubTitles = episode.Series.SubTitles;
                            episode.ReadCrew(xMLEpisode);
                            episode.ReadCast(xMLEpisode);
                            episode.ReadMediaFiles(xMLEpisode);
                            episode.ReadImages(xMLEpisode);

                            ((Series)media).Episodes.Add(episode);
                        }
                    }

                    // add SubTitles on series-level
                    ((Series)media).ReadMediaFiles(xMLMovie);

                    this.SeriesCollection.Add((Series)media);
                }

                #endregion
                #region Media (Movie & Series)

                media.SetFilename();

                #endregion
            }

            this.GroupByMediaGroup();
        }

        /// <summary>
        /// list of movies stored at the specified server
        /// </summary>
        /// <param name="server">Server containing the Movie; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <returns>new list of movies stored at the specified server</returns>
        public List<Movie> ListMovieCollectionPerServer(int server)
        {
            List<Movie> movieCollectionPerServer = new List<Movie>();

            foreach (Movie movie in this.ListMovieCollectionPerServer(server, false))
            {
                if (movie != null)
                {
                    movieCollectionPerServer.Add(movie);
                }
            }

            foreach (Movie movie in this.ListMovieCollectionPerServer(server, true))
            {
                if (movie != null)
                {
                    movie.Title = movie.Title + " (Specials)";
                    movie.TitleSort = movie.TitleSort + " (Specials)";
                    movie.TitleOriginal = movie.TitleOriginal + " (Specials)";
                    movie.Filename = movie.Filename + " (Specials)";
                    movieCollectionPerServer.Add(movie);
                }
            }

            return movieCollectionPerServer;
        }

        /// <summary>
        /// list of movies stored at the specified server
        /// </summary>
        /// <param name="server">Server containing the Movie; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <param name="isSpecial">value indicating whether Specials or regular movies should be returned</param>
        /// <returns>new list of movies stored at the specified server</returns>
        public List<Movie> ListMovieCollectionPerServer(int server, bool isSpecial)
        {
            List<Movie> movieCollectionPerServer = new List<Movie>();
            foreach (Movie movie in this.ClonePerLanguage(this.MovieCollection))
            {
                bool addMovie = false;
                foreach (int serverList in movie.Server)
                {
                    if (serverList.Equals(server))
                    {
                        addMovie = true;
                    }
                }

                if (movie.MediaFiles.Count > 0 && addMovie)
                {
                    Movie movieClone = (Movie)movie.Clone(server, isSpecial);
                    movieCollectionPerServer.Add(movieClone);
                }
            }

            return movieCollectionPerServer;
        }

        /// <summary>
        /// list of series stored at the specified server
        /// </summary>
        /// <param name="server">Server containing the Movie; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <returns>new list of series stored at the specified server</returns>
        public List<Series> ListSeriesCollectionPerServer(int server)
        {
            List<Series> lstSeriesCollectionPerServer = new List<Series>();

            // list Series for Server
            foreach (Series series in this.SeriesCollectionGroupedByMediaGroup)
            {
                bool addSeries = false;
                foreach (int serverList in series.Server)
                {
                    if (serverList.Equals(server))
                    {
                        addSeries = true;
                    }
                }

                if (addSeries)
                {
                    Series seriesClone = (Series)series.Clone(server);
                    lstSeriesCollectionPerServer.Add(seriesClone);
                }
            }

            return lstSeriesCollectionPerServer;
        }

        /// <summary>
        /// list movies, duplicated by language, if multiple video file are used
        /// </summary>
        /// <param name="movieCollection">List of movies, that should be checked for multiple languages</param>
        /// <returns>new list of movies, duplicated by language, if multiple video file are used</returns>
        private List<Movie> ClonePerLanguage(List<Movie> movieCollection)
        {
            List<Movie> movieCollectionPerLanguage = new List<Movie>();

            foreach (Movie movie in movieCollection)
            {
                foreach (string movieLanguage in movie.MediaLanguages)
                {
                    Movie movieClone = (Movie)movie.Clone();
                    List<string> mediaLanguagesToBeReplaced = new List<string>();
                    mediaLanguagesToBeReplaced.Add(movie.MediaLanguages[0]);
                    movieClone.ClonePerLanguage(mediaLanguagesToBeReplaced, movieLanguage);
                    movieCollectionPerLanguage.Add(movieClone);
                }
            }

            return movieCollectionPerLanguage;
        }

        /// <summary>
        /// list series, duplicated by language, if multiple video file are used
        /// </summary>
        /// <param name="seriesCollection">List of series, that should be checked for multiple languages</param>
        /// <returns>new list of series, duplicated by language, if multiple video file are used</returns>
        private List<Series> ClonePerLanguage(List<Series> seriesCollection)
        {
            List<Series> seriesCollectionPerLanguage = new List<Series>();

            foreach (Series series in seriesCollection)
            {
                foreach (string seriesLanguage in series.MediaLanguages)
                {
                    Series seriesClone = (Series)series.Clone();
                    List<string> mediaLanguagesToBeReplaced;

                    mediaLanguagesToBeReplaced = new List<string>();
                    mediaLanguagesToBeReplaced.Add(series.MediaLanguages[0]);  // series title
                    seriesClone.ClonePerLanguage(mediaLanguagesToBeReplaced, seriesLanguage);

                    foreach (Episode episodeClone in seriesClone.Episodes)
                    {
                        mediaLanguagesToBeReplaced = new List<string>();
                        mediaLanguagesToBeReplaced.Add(series.MediaLanguages[0]);  // series title
                        mediaLanguagesToBeReplaced.Add(episodeClone.MediaLanguages[0]); // and episode data, when available languages change during series

                        episodeClone.ClonePerLanguage(mediaLanguagesToBeReplaced, seriesLanguage);
                    }

                    seriesCollectionPerLanguage.Add(seriesClone);
                }
            }

            return seriesCollectionPerLanguage;
        }

        /// <summary>
        /// groups multiple Series into one according by MediaGroup
        /// </summary>
        private void GroupByMediaGroup()
        {
            List<Series> lstSeriesCollection = new List<Series>();
            List<Series> lstSeriesCollectionPerMediaGroup = new List<Series>();
            Series serSeriesPerMediaGroup = new Series(this.Configuration);
            Episode epiEpisodePerMediaGroup = new Episode(this.Configuration);
            string strActiveMediaGroup = string.Empty;
            int intSeasonOffset = 0;
            int intDisplaySeasonOffset = 0;
            int intEpisodeOffset = 0;
            int intSpecialsOffset = 0;
            int intDisplayEpisodeOffset = 0;

            lstSeriesCollection = this.ClonePerLanguage(this.SeriesCollection);

            // create new List of Series with MediaGroup name
            foreach (Series serSeries in lstSeriesCollection.OrderBy(o => o.MediaGroup).ThenBy(o => o.TitleSort).ToList())
            {
                // create new Series, when a different MediaGroup is present
                if (serSeries.MediaGroup != strActiveMediaGroup)
                {
                    serSeriesPerMediaGroup = (Series)serSeries.Clone();
                    strActiveMediaGroup = serSeries.MediaGroup;
                    intSeasonOffset = 0;
                    intDisplaySeasonOffset = 0;
                    intEpisodeOffset = 0;
                    intSpecialsOffset = 0;
                    intDisplayEpisodeOffset = 0;

                    lstSeriesCollectionPerMediaGroup.Add(serSeriesPerMediaGroup);
                }
                else
                {
                    serSeriesPerMediaGroup.Title = serSeriesPerMediaGroup.MediaGroup;
                    intSeasonOffset = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count - 1;
                    intDisplaySeasonOffset = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count - 1;
                    intEpisodeOffset = serSeriesPerMediaGroup.NumberOfEpisodes;
                    intSpecialsOffset = serSeriesPerMediaGroup.NumberOfSpecials;
                    intDisplayEpisodeOffset = serSeriesPerMediaGroup.Episodes.Count;

                    foreach (Episode epiEpisode in serSeries.Episodes)
                    {
                        // create new Episode and set reference to series
                        epiEpisodePerMediaGroup = (Episode)epiEpisode.Clone();
                        epiEpisodePerMediaGroup.Series = serSeriesPerMediaGroup;

                        // set DisplaySeason, DisplayEpisode and Season for new Episode
                        epiEpisodePerMediaGroup.DisplaySeason = (int.Parse(epiEpisodePerMediaGroup.DisplaySeason) + intDisplaySeasonOffset).ToString();
                        epiEpisodePerMediaGroup.DisplayEpisode = (int.Parse(epiEpisodePerMediaGroup.DisplayEpisode) + intDisplayEpisodeOffset).ToString();
                        epiEpisodePerMediaGroup.ActualSeason = epiEpisodePerMediaGroup.ActualSeason == "0" /* Special */ ? "0" : (int.Parse(epiEpisodePerMediaGroup.ActualSeason) + intSeasonOffset).ToString();

                        // add new Season to NumberOfEpisodesPerSeason if necessary
                        int intSeason = int.Parse(epiEpisodePerMediaGroup.ActualSeason);
                        while (serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count < intSeason + 1)
                        {
                            serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Add(0);
                        }

                        // set new Numbers per Series
                        serSeriesPerMediaGroup.NumberOfEpisodes = serSeriesPerMediaGroup.NumberOfEpisodes + (!epiEpisodePerMediaGroup.IsSpecial ? 1 : 0);
                        serSeriesPerMediaGroup.NumberOfSpecials = serSeriesPerMediaGroup.NumberOfSpecials + (epiEpisodePerMediaGroup.IsSpecial ? 1 : 0);
                        serSeriesPerMediaGroup.NumberOfEpisodesPerSeason[intSeason] = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason[intSeason] + 1;
                        serSeriesPerMediaGroup.NumberOfTotalEpisodes = serSeriesPerMediaGroup.NumberOfTotalEpisodes + 1;

                        // set Episode-Number accordingly
                        epiEpisodePerMediaGroup.ActualEpisode = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason[intSeason].ToString();

                        // add Server for serSeriesPerMediaGroup
                        serSeriesPerMediaGroup.AddServer(epiEpisode.Server);

                        // TODO: Add cover as season-art

                        // set VideoIndex and FileIndex for Episode and associated Files
                        epiEpisodePerMediaGroup.VideoIndex = epiEpisodePerMediaGroup.VideoIndex + intDisplayEpisodeOffset;
                        foreach (VideoFile videoFile in epiEpisodePerMediaGroup.MediaFiles)
                        {
                            videoFile.FileIndex = videoFile.FileIndex + intDisplayEpisodeOffset;

                            foreach (SubTitleFile subTitleFile in videoFile.SubTitleFiles)
                            {
                                subTitleFile.FileIndex = subTitleFile.FileIndex + intDisplayEpisodeOffset;
                            }
                        }

                        // add episode to new series
                        serSeriesPerMediaGroup.Episodes.Add(epiEpisodePerMediaGroup);
                    }

                    serSeriesPerMediaGroup.SetFilename();
                }
            }

            this.seriesCollectionGroupedByMediaGroup = lstSeriesCollectionPerMediaGroup;
        }

        #endregion
    }
}
