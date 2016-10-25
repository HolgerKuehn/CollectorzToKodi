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

        /// <summary>
        /// stores non-grouped Collection grouped by MediaGroup, which is used to remove old MediaFiles inside publishing directory, when the Media was published previously outside of one or a different MediaGroup
        /// </summary>
        private List<Series> seriesCollectionWithoutMediaGroup;

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
            this.seriesCollectionWithoutMediaGroup = new List<Series>();
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

        /// <summary>
        /// Gets or sets stored Collection non-grouped by MediaGroup
        /// </summary>
        private List<Series> SeriesCollectionWithoutMediaGroup
        {
            get { return this.seriesCollectionWithoutMediaGroup; }
            set { this.seriesCollectionWithoutMediaGroup = value; }
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
        /// list of series stored at the specified server, that need to be removed as all Series are stored within a new Media Group
        /// </summary>
        /// <param name="server">Server containing the Movie; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <returns>new list of series stored at the specified server</returns>
        public List<Series> ListSeriesCollectionWithoutMediaGroupPerServer(int server)
        {
            List<Series> lstSeriesCollectionWithoutMediaGroupPerServer = new List<Series>();

            // list Series for Server
            foreach (Series series in this.SeriesCollectionWithoutMediaGroup)
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
                    lstSeriesCollectionWithoutMediaGroupPerServer.Add(seriesClone);
                }
            }

            return lstSeriesCollectionWithoutMediaGroupPerServer;
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
            List<Series> seriesCollection = this.ClonePerLanguage(this.SeriesCollection); /* original collection, cloned per language */
            List<Series> seriesCollectionPerMediaGroup = new List<Series>(); /* new collection of grouped series */
            List<Series> seriesCollectionWithoutMediaGroup = new List<Series>(); /* original series, used to remove old folders  */

            #region creating seriesListsPerMediaGroup
            string activeMediaGroup = string.Empty;
            List<Series> seriesListPerMediaGroup = new List<Series>();
            List<List<Series>> seriesListsPerMediaGroup = new List<List<Series>>();

            // creates new collection of lists of series, each latter one belongs to the same MediaGroup
            foreach (Series series in seriesCollection.OrderBy(o => o.MediaGroup).ThenBy(o => o.TitleSort).ToList())
            {
                seriesCollectionWithoutMediaGroup.Add(series);

                // create new Series, when a different MediaGroup is present
                if (series.MediaGroup != activeMediaGroup)
                {
                    // when new MediaGroup is found
                    // set new MediaGroup
                    activeMediaGroup = series.MediaGroup;

                    // create new series
                    seriesListPerMediaGroup = new List<Series>();
                    seriesListPerMediaGroup.Add(series);

                    // add list to new collection
                    seriesListsPerMediaGroup.Add(seriesListPerMediaGroup);
                }
                else
                {
                    // while finding series of same MediaGroup, adding those to seriesPerMediaGroup
                    seriesListPerMediaGroup.Add(series);
                }
            }
            #endregion

            // each first-level-List of seriesListsPerMediaGroup contains a series
            #region creating new Series with appropriate settings

            foreach (List<Series> seriesList in seriesListsPerMediaGroup)
            {
                if (seriesList.Count == 1)
                {
                    // adding directly, if only one series in MediaGroup
                    seriesCollectionPerMediaGroup.Add(seriesList[0]);
                }
                else
                {
                    // create new series per node in seriesListsPerMediaGroup
                    Series seriesPerMediaGroup = new Series(this.Configuration);
                    bool imagesContainsOneWithSeasonOne = false;
                    List<bool> seriesPerMediagroupContainsImageWithSeasonSpecial = new List<bool>();
                    List<bool> seriesPerMediagroupContainsImageWithSeasonAll = new List<bool>();

                    for (int i = 0; i < this.Configuration.NumberOfImageTypes; i++)
                    {
                        seriesPerMediagroupContainsImageWithSeasonSpecial.Add(false);
                        seriesPerMediagroupContainsImageWithSeasonAll.Add(false);
                    }

                    // set basic parameters for new Series, according to first member of MediaGroup
                    Series seriesBasicMember = seriesList[0];

                    seriesPerMediaGroup.Title = seriesBasicMember.MediaGroup;
                    seriesPerMediaGroup.TitleSort = seriesBasicMember.MediaGroup;
                    seriesPerMediaGroup.TitleOriginal = seriesBasicMember.MediaGroup;
                    seriesPerMediaGroup.MediaGroup = seriesBasicMember.MediaGroup;
                    seriesPerMediaGroup.Rating = seriesBasicMember.Rating;
                    seriesPerMediaGroup.PublishingYear = seriesBasicMember.PublishingYear;
                    seriesPerMediaGroup.PublishingDate = seriesBasicMember.PublishingDate;
                    seriesPerMediaGroup.RunTime = seriesBasicMember.RunTime;
                    seriesPerMediaGroup.MPAA = seriesBasicMember.MPAA;
                    seriesPerMediaGroup.PlayCount = seriesBasicMember.PlayCount;
                    seriesPerMediaGroup.PlayDate = seriesBasicMember.PlayDate;
                    seriesPerMediaGroup.IMDbId = seriesBasicMember.IMDbId;
                    seriesPerMediaGroup.Country = seriesBasicMember.Country;
                    seriesPerMediaGroup.Genres = seriesBasicMember.Genres;
                    seriesPerMediaGroup.Studios = seriesBasicMember.Studios;
                    seriesPerMediaGroup.VideoCodec = seriesBasicMember.VideoCodec;
                    seriesPerMediaGroup.VideoDefinition = seriesBasicMember.VideoDefinition;
                    seriesPerMediaGroup.VideoAspectRatio = seriesBasicMember.VideoAspectRatio;
                    seriesPerMediaGroup.AudioStreams = seriesBasicMember.AudioStreams;
                    seriesPerMediaGroup.SubTitles = seriesBasicMember.SubTitles;
                    seriesPerMediaGroup.MediaLanguages = seriesBasicMember.MediaLanguages;
                    seriesPerMediaGroup.NumberOfEpisodesPerSeason = new List<int>();
                    seriesPerMediaGroup.NumberOfEpisodesPerSeason.Add(0);

                    // adding basic images (without season) from seriesBasicMember
                    foreach (ImageFile imageFile in seriesBasicMember.Images)
                    {
                        if (imageFile.ImageType == Configuration.ImageType.CoverFront ||
                            imageFile.ImageType == Configuration.ImageType.CoverBack ||
                            imageFile.ImageType == Configuration.ImageType.Backdrop ||
                            imageFile.ImageType == Configuration.ImageType.Poster)
                        {
                            ImageFile imageFilePerMediaGroup = (ImageFile)imageFile.Clone();
                            imageFilePerMediaGroup.Media = seriesPerMediaGroup;

                            seriesPerMediaGroup.Images.Add(imageFilePerMediaGroup);
                        }
                    }

                    seriesPerMediaGroup.Content = "MediaCollection for " + seriesBasicMember.MediaGroup;

                    // adding each series to seriesPerMediaGroup
                    foreach (Series series in seriesList)
                    {
                        int fistSeasonInSeries = seriesPerMediaGroup.NumberOfEpisodesPerSeason.Count;

                        // series based attributes
                        // checking if images of series contains one with Season 1
                        imagesContainsOneWithSeasonOne = false;
                        foreach (ImageFile imageContainsOneWithSeasonOne in series.Images)
                        {
                            if (imageContainsOneWithSeasonOne.Season == "1")
                            {
                                imagesContainsOneWithSeasonOne = true;
                                break;
                            }
                        }

                        foreach (ImageFile imageFile in series.Images)
                        {
                            ImageFile imageFilePerMediaGroup = (ImageFile)imageFile.Clone();
                            imageFilePerMediaGroup.Media = seriesPerMediaGroup;

                            // change type from basic image to season image
                            if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.Backdrop)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonBackdrop;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.CoverFront)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonCover;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.ExtraBackdrop)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.ExtraBackdrop;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.ExtraCover)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.ExtraCover;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.Poster)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonPoster;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.SeasonBackdrop)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonBackdrop;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.SeasonCover)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonCover;
                            }
                            else if (imageFilePerMediaGroup.ImageType == Configuration.ImageType.SeasonPoster)
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.SeasonPoster;
                            }
                            else
                            {
                                imageFilePerMediaGroup.ImageType = Configuration.ImageType.Unknown;
                            }

                            // exclude multiple special images for complete Series and Specials
                            if (imageFilePerMediaGroup.Season == string.Empty && !imagesContainsOneWithSeasonOne /* default images overruled by image per season */)
                            {
                                // setting Image without Season for first season
                                imageFilePerMediaGroup.Season = "1";
                            }
                            else if (imageFilePerMediaGroup.Season == "-1" && !seriesPerMediagroupContainsImageWithSeasonAll[(int)imageFilePerMediaGroup.ImageType] /* image for AllSeasons already added */)
                            {
                                seriesPerMediagroupContainsImageWithSeasonAll[(int)imageFilePerMediaGroup.ImageType] = true;
                            }
                            else if (imageFilePerMediaGroup.Season == "-1")
                            {
                                imageFilePerMediaGroup.Season = string.Empty;
                            }
                            else if (imageFilePerMediaGroup.Season == "0" && !seriesPerMediagroupContainsImageWithSeasonSpecial[(int)imageFilePerMediaGroup.ImageType] /* image for specials already added */)
                            {
                                seriesPerMediagroupContainsImageWithSeasonSpecial[(int)imageFilePerMediaGroup.ImageType] = true;
                            }
                            else if (imageFilePerMediaGroup.Season == "0")
                            {
                                imageFilePerMediaGroup.Season = string.Empty;
                            }

                            if (imageFilePerMediaGroup.Season == string.Empty)
                            {
                                // exclude multiple images for seasons
                                continue;
                            }
                            else if (imageFilePerMediaGroup.Season != "-1" && imageFilePerMediaGroup.Season != "0")
                            {
                                // Season for Specials ("0") and AllSeasons ("-1") stays unchanged
                                imageFilePerMediaGroup.Season = (int.Parse(imageFilePerMediaGroup.Season) + fistSeasonInSeries - 1).ToString();
                            }

                            if (imageFilePerMediaGroup.ImageType != Configuration.ImageType.Unknown)
                            {
                                seriesPerMediaGroup.Images.Add(imageFilePerMediaGroup);
                            }
                        }

                        // change Season for Actors accordingly
                        foreach (SeriesActor seriesActor in series.Actors)
                        {
                            for (int i = 0; i < seriesActor.Seasons.Count; i++)
                            {
                                if (seriesActor.Seasons[i] != string.Empty)
                                {
                                    seriesActor.Seasons[i] = (int.Parse(seriesActor.Seasons[i]) + fistSeasonInSeries - 1).ToString();
                                }
                            }
                        }

                        // add server to new series
                        seriesPerMediaGroup.AddServer(series.Server);

                        // episodes
                        foreach (Episode episode in series.Episodes)
                        {
                            Episode episodePerMediaGroup = (Episode)episode.Clone();
                            episodePerMediaGroup.Series = seriesPerMediaGroup;

                            // set new season and prepare NumberOfEpisodesPerSeason
                            episodePerMediaGroup.ActualSeason = episode.ActualSeason == "0" ? "0" : (int.Parse(episode.ActualSeason) + fistSeasonInSeries - 1).ToString();
                            episodePerMediaGroup.DisplaySeason = (int.Parse(episode.DisplaySeason) + fistSeasonInSeries - 1).ToString();
                            while (seriesPerMediaGroup.NumberOfEpisodesPerSeason.Count - 1 < (int)int.Parse(episodePerMediaGroup.ActualSeason))
                            {
                                seriesPerMediaGroup.NumberOfEpisodesPerSeason.Add(0);
                            }

                            seriesPerMediaGroup.NumberOfEpisodes = seriesPerMediaGroup.NumberOfEpisodes + (episode.IsSpecial ? 0 : 1);
                            seriesPerMediaGroup.NumberOfSpecials = seriesPerMediaGroup.NumberOfSpecials + (episode.IsSpecial ? 1 : 0);
                            seriesPerMediaGroup.NumberOfEpisodesPerSeason[(int)int.Parse(episodePerMediaGroup.ActualSeason)]++;

                            episodePerMediaGroup.ActualEpisode = (string)seriesPerMediaGroup.NumberOfEpisodesPerSeason.ElementAt((int)int.Parse(episodePerMediaGroup.ActualSeason)).ToString();
                            episodePerMediaGroup.DisplayEpisode = (string)(seriesPerMediaGroup.NumberOfSpecials + seriesPerMediaGroup.NumberOfEpisodes).ToString();

                            // set plot - each content episode gets content from series, if empty
                            if (!episodePerMediaGroup.IsSpecial && episodePerMediaGroup.Content == string.Empty)
                            {
                                episodePerMediaGroup.Content = series.Content;
                            }

                            // add series director, writer and actors to episodes within
                            episodePerMediaGroup.AddDirector(series.Directors);
                            episodePerMediaGroup.AddWriter(series.Writers);
                            episodePerMediaGroup.AddActor(series.Actors);

                            seriesPerMediaGroup.Episodes.Add(episodePerMediaGroup);
                        }
                    }

                    seriesPerMediaGroup.SetFilename();

                    seriesCollectionPerMediaGroup.Add(seriesPerMediaGroup);
                }
            }

            #endregion

            this.SeriesCollectionGroupedByMediaGroup = seriesCollectionPerMediaGroup;
            this.SeriesCollectionWithoutMediaGroup = seriesCollectionWithoutMediaGroup;
        }
    }
    #endregion
}