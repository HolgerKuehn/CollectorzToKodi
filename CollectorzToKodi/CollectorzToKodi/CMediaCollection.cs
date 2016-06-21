// <copyright file="CMediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

// <copyright file="CMediaCollection.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>
namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// provides methods to handle the complete media-collection (e.g. movies and series) from MovieCollector
    /// </summary>
    public class CMediaCollection
    {
        #region Attributes

        /// <summary>
        /// stores configuration for MediaCollection
        /// </summary>
        private readonly CConfiguration configuration;

        /// <summary>
        /// stores MediaCollection for movies
        /// </summary>
        private readonly List<CMovie> movieCollection;

        /// <summary>
        /// stores MediaCollection for series
        /// </summary>
        private readonly List<CSeries> seriesCollection;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMediaCollection"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CMediaCollection(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.movieCollection = new List<CMovie>();
            this.seriesCollection = new List<CSeries>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets current configuration used by MediaCollection
        /// </summary>
        /// <returns>current configuration used by MediaCollection</returns>
        public CConfiguration Configuration
        {
            get { return this.configuration; }
        }

        /// <summary>
        /// Gets current MovieCollection used by MediaCollection
        /// </summary>
        /// <returns>current MovieCollection used by MediaCollection</returns>
        public List<CMovie> MovieCollection
        {
            get { return this.movieCollection; }
        }

        /// <summary>
        /// Gets current SeriesCollection used by MediaCollection
        /// </summary>
        /// <returns>current SeriesCollection used by MediaCollection</returns>
        public List<CSeries> SeriesCollection
        {
            get { return this.seriesCollection; }
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
            CVideo media = null;
            foreach (XmlNode xMLMovie in CBaseClassExtention.XMLReadFile(eingabeXML, "movieinfo").XMLReadSubnode("movielist").XMLReadSubnodes("movie"))
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
                        media = new CMovie(this.Configuration);
                    }
                    else
                    {
                        xMLMovieIsMovie = false;
                        xMLMovieIsSeries = true;
                    }
                }

                if (xMLMovieIsSeries)
                {
                    media = new CSeries(this.Configuration);
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
                media.Filename = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.Convert(Encoding.UTF8, Encoding.ASCII, System.Text.Encoding.UTF8.GetBytes(media.Title + " (" + media.PublishingYear + ")"))).Replace("?", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty).Trim();
                media.TitleSort = xMLMovie.XMLReadSubnode("titlesort").XMLReadInnerText(string.Empty);
                media.TitleOriginal = xMLMovie.XMLReadSubnode("originaltitle").XMLReadInnerText(string.Empty);
                media.MediaGroup = xMLMovie.XMLReadSubnode("series").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
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
                    ((CMovie)media).ReadMediaFiles(xMLMovie);
                    media.PublishingDate = xMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.PublishingYear);
                    media.PlayCount = (xMLMovie.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Yes" || xMLMovie.XMLReadSubnode("seenit").XMLReadInnerText(string.Empty) == "Ja") ? "1" : "0";
                    media.PlayDate = xMLMovie.XMLReadSubnode("viewingdate").XMLReadSubnode("date").XMLReadInnerText(string.Empty);

                    if (media.HasSpecials() && media.MediaGroup == string.Empty)
                    {
                        media.MediaGroup = media.Title;
                    }

                    this.MovieCollection.Add((CMovie)media);
                }

                #endregion
                #region Series

                if (xMLMovieIsSeries)
                {
                    // releasedate and number episodes in series
                    media.PublishingDate = xMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.PublishingYear);
                    ((CSeries)media).NumberOfTotalEpisodes = (int)int.Parse(xMLMovie.XMLReadSubnode("chapters").XMLReadInnerText(string.Empty));

                    // episodes
                    foreach (XmlNode xMLSeriesDisc in xMLMovie.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
                    {
                        CEpisode seriesDiscEpisode = new CEpisode(this.Configuration);
                        seriesDiscEpisode.Series = (CSeries)media;
                        seriesDiscEpisode.ExtractSeriesData((CSeries)media);
                        seriesDiscEpisode.OverrideSeason(seriesDiscEpisode.OverrideVideoStreamData(xMLSeriesDisc.XMLReadSubnode("title").XMLReadInnerText(string.Empty)), false);

                        foreach (XmlNode xMLEpisode in xMLSeriesDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                        {
                            CEpisode episode = new CEpisode(this.Configuration);

                            // reset Episode-Attributes to Disc-Attributes
                            episode.ExtractSeriesData(seriesDiscEpisode);

                            // read Episode-Details
                            episode.Series = (CSeries)media;
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

                            // SubTitle der Episode ableiten
                            episode.SubTitleStreams = episode.Series.SubTitleStreams;
                            episode.ReadCrew(xMLEpisode);
                            episode.ReadCast(xMLEpisode);
                            episode.ReadMediaFiles(xMLEpisode);
                            episode.ReadImages(xMLEpisode);

                            ((CSeries)media).Episodes.Add(episode);
                        }
                    }

                    this.SeriesCollection.Add((CSeries)media);
                }

                #endregion
            }
        }

        /// <summary>
        /// list of movies stored at the specified server
        /// </summary>
        /// <param name="server">Server containing the Movie; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <returns>new list of movies stored at the specified server</returns>
        public List<CMovie> ListMovieCollectionPerServer(int server)
        {
            List<CMovie> movieCollectionPerServer = new List<CMovie>();

            foreach (CMovie movie in this.ListMovieCollectionPerServer(server, false))
            {
                if (movie != null)
                {
                    movieCollectionPerServer.Add(movie);
                }
            }

            foreach (CMovie movie in this.ListMovieCollectionPerServer(server, true))
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
        public List<CMovie> ListMovieCollectionPerServer(int server, bool isSpecial)
        {
            List<CMovie> movieCollectionPerServer = new List<CMovie>();
            foreach (CMovie movie in this.ClonePerLanguage(this.MovieCollection))
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
                    CMovie movieClone = (CMovie)movie.Clone(server, isSpecial);
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
        public List<CSeries> ListSeriesCollectionPerServer(int server)
        {
            List<CSeries> lstSeriesCollection = new List<CSeries>();
            List<CSeries> lstSeriesCollectionPerMediaGroup = new List<CSeries>();
            List<CSeries> lstSeriesCollectionPerServer = new List<CSeries>();
            CSeries serSeriesPerMediaGroup = new CSeries(this.Configuration);
            CEpisode epiEpisodePerMediaGroup = new CEpisode(this.Configuration);
            string strActiveMediaGroup = string.Empty;
            int intSeasonOffset = 0;
            int intDisplaySeasonOffset = 0;
            int intEpisodeOffset = 0;

            lstSeriesCollection = this.ClonePerLanguage(this.SeriesCollection);

            // create new List of Series with MediaGroup name
            foreach (CSeries serSeries in lstSeriesCollection.OrderBy(o => o.MediaGroup).ThenBy(o => o.TitleSort).ToList())
            {
                // create new Series, when a different MediaGroup is present
                if (serSeries.MediaGroup != strActiveMediaGroup)
                {
                    serSeriesPerMediaGroup = (CSeries)serSeries.Clone();
                    strActiveMediaGroup = serSeries.MediaGroup;
                    intSeasonOffset = 0;
                    intDisplaySeasonOffset = 0;
                    intEpisodeOffset = 0;
                }
                else
                {
                    serSeriesPerMediaGroup.Title = serSeriesPerMediaGroup.MediaGroup;
                    intSeasonOffset = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count;
                    intDisplaySeasonOffset = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count;
                    intEpisodeOffset = serSeriesPerMediaGroup.Episodes.Count;

                    foreach (CEpisode epiEpisode in serSeries.Episodes)
                    {
                        epiEpisodePerMediaGroup = (CEpisode)epiEpisode.Clone();

                        epiEpisodePerMediaGroup.DisplaySeason = (int.Parse(epiEpisodePerMediaGroup.DisplaySeason) + intDisplaySeasonOffset).ToString();
                        epiEpisodePerMediaGroup.Episode = (int.Parse(epiEpisodePerMediaGroup.Episode) + intEpisodeOffset).ToString();

                        epiEpisodePerMediaGroup.Season = epiEpisodePerMediaGroup.Season == "0" ? "0" : (int.Parse(epiEpisodePerMediaGroup.Season) + intSeasonOffset).ToString();
                        epiEpisodePerMediaGroup.Series = serSeriesPerMediaGroup;

                        serSeriesPerMediaGroup.Episodes.Add(epiEpisodePerMediaGroup);
                        serSeriesPerMediaGroup.NumberOfEpisodes = serSeriesPerMediaGroup.NumberOfEpisodes + (!epiEpisodePerMediaGroup.IsSpecial ? 1 : 0);

                        int intSeason = int.Parse(epiEpisodePerMediaGroup.Season);
                        while (serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Count < intSeason + 1)
                        {
                            serSeriesPerMediaGroup.NumberOfEpisodesPerSeason.Add(0);
                        }

                        serSeriesPerMediaGroup.NumberOfEpisodesPerSeason[intSeason] = serSeriesPerMediaGroup.NumberOfEpisodesPerSeason[intSeason] + 1;
                        serSeriesPerMediaGroup.NumberOfSpecials = serSeriesPerMediaGroup.NumberOfSpecials + (epiEpisodePerMediaGroup.IsSpecial ? 1 : 0);
                        serSeriesPerMediaGroup.NumberOfTotalEpisodes = serSeriesPerMediaGroup.NumberOfTotalEpisodes + 1;

                           // epiEpisodePerMediaGroup.Filename
                    }
                }

                lstSeriesCollectionPerMediaGroup.Add(serSeriesPerMediaGroup);
            }

            // list Series for Server
            foreach (CSeries series in lstSeriesCollectionPerMediaGroup)
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
                    CSeries seriesClone = (CSeries)series.Clone(server);
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
        private List<CMovie> ClonePerLanguage(List<CMovie> movieCollection)
        {
            List<CMovie> movieCollectionPerLanguage = new List<CMovie>();

            foreach (CMovie movie in movieCollection)
            {
                foreach (string movieLanguage in movie.MediaLanguages)
                {
                    CMovie movieClone = (CMovie)movie.Clone();
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
        private List<CSeries> ClonePerLanguage(List<CSeries> seriesCollection)
        {
            List<CSeries> seriesCollectionPerLanguage = new List<CSeries>();

            foreach (CSeries series in seriesCollection)
            {
                foreach (string seriesLanguage in series.MediaLanguages)
                {
                    CSeries seriesClone = (CSeries)series.Clone();
                    List<string> mediaLanguagesToBeReplaced;

                    mediaLanguagesToBeReplaced = new List<string>();
                    mediaLanguagesToBeReplaced.Add(series.MediaLanguages[0]);  // series title
                    seriesClone.ClonePerLanguage(mediaLanguagesToBeReplaced, seriesLanguage);

                    foreach (CEpisode episodeClone in seriesClone.Episodes)
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

        #endregion
    }
}
