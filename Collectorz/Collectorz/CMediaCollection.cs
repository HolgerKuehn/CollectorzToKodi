using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Collectorz
{
    /// <summary>
    /// provides methods to handle the complete media-collection (e.g. movies and series) from MovieCollector
    /// </summary>
    public class CMediaCollection
    {
        #region Attributes
        /// <summary>
        /// stores configuration for MediaCollection
        /// </summary>
        private CConfiguration configuration;
        /// <summary>
        /// stores MediaCollection for movies
        /// </summary>
        private List<CMovie> movieCollection;
        /// <summary>
        /// stores MediaCollection for series
        /// </summary>
        private List<CSeries> seriesCollection;
        #endregion
        #region Constructor
        /// <summary>
        /// initializes MediaCollection and Configuration
        /// </summary>
        public CMediaCollection(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.movieCollection = new List<CMovie>();
            this.seriesCollection = new List<CSeries>();
        }
        #endregion
        #region Properties
        /// <summary>
        /// <returns>current configuration used by MediaCollection</returns>
        /// </summary>
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { }
        }
        /// <summary>
        /// <returns>current MovieCollection used by MediaCollection</returns>
        /// </summary>
        public List<CMovie> MovieCollection
        {
            get { return this.movieCollection; }
            set { }
        }
        /// <summary>
        /// <returns>current SeriesCollection used by MediaCollection</returns>
        /// </summary>
        public List<CSeries> SeriesCollection
        {
            get { return this.seriesCollection; }
            set { }
        }
        #endregion
        #region Functions
        /// <summary>
        /// reads the MovieCollectoz XML export into MediaCollection
        /// </summary>
        /// <remarks>
        ///     <para>requires user defined field:</para>
        ///     <para>"XBMC Movie" - true if entry is to be interpreted as a movie</para>
        ///     <para>"XBMC Series" - true if entry is to be interpreted as a series</para>
        /// </remarks>
        /// <param name="EingabeXML">File that was used while export from MovieCollector - complete local path</param>
        public void readXML(string EingabeXML)
        {
            bool XMLMovieIsMovie = false;
            bool XMLMovieIsSeries = false;
            CMedia media = null;
            foreach (XmlNode XMLMovie in CXML.XMLReadFile(EingabeXML, "movieinfo").XMLReadSubnode("movielist").XMLReadSubnodes("movie"))
            {
                #region evaluate Type and create media-object
                foreach (XmlNode XMLUserDefinedValue in XMLMovie.XMLReadSubnode("userdefinedvalues").XMLReadSubnodes("userdefinedvalue"))
                {
                    if (XMLUserDefinedValue.XMLReadSubnode("userdefinedfield").XMLReadSubnode("label").XMLReadInnerText("") == "XBMC Movie")
                        XMLMovieIsMovie = XMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText("") == "Yes" || XMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText("") == "Ja";

                    if (XMLUserDefinedValue.XMLReadSubnode("userdefinedfield").XMLReadSubnode("label").XMLReadInnerText("") == "XBMC Serie")
                        XMLMovieIsSeries = XMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText("") == "Yes" || XMLUserDefinedValue.XMLReadSubnode("value").XMLReadInnerText("") == "Ja";
                }

                if (XMLMovieIsMovie)
                    media = new CMovie(this.Configuration);

                if (XMLMovieIsSeries)
                    media = new CSeries(this.Configuration);

                if (media == null)
                    continue; // TODO: ### write error log

                #endregion
                #region Media (Movie & Series)
                media.Title = XMLMovie.XMLReadSubnode("title").XMLReadInnerText("");
                media.Title = media.overrideMediaStreamData(media.Title);
                media.Year = XMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("year").XMLReadSubnode("displayname").XMLReadInnerText("");
                media.Filename = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.Convert(Encoding.UTF8, Encoding.ASCII, System.Text.Encoding.UTF8.GetBytes(media.Title + " (" + media.Year + ")"))).Replace("?", "").Replace("-", "").Replace(":", "").Trim();
                media.TitleSort = XMLMovie.XMLReadSubnode("titlesort").XMLReadInnerText("");
                media.TitleOriginal = XMLMovie.XMLReadSubnode("originaltitle").XMLReadInnerText("");
                media.Set = XMLMovie.XMLReadSubnode("series").XMLReadSubnode("displayname").XMLReadInnerText("");
                media.Rating = XMLMovie.XMLReadSubnode("imdbrating").XMLReadInnerText("");
                media.Plot = XMLMovie.XMLReadSubnode("plot").XMLReadInnerText("");
                media.RunTime = XMLMovie.XMLReadSubnode("runtime").XMLReadInnerText("");
                media.MPAA = XMLMovie.XMLReadSubnode("mpaarating").XMLReadSubnode("displayname").XMLReadInnerText("");
                media.IMDbId = "tt" + XMLMovie.XMLReadSubnode("imdbnum").XMLReadInnerText("");
                media.Country = XMLMovie.XMLReadSubnode("country").XMLReadSubnode("displayname").XMLReadInnerText("");
                media.readImages(XMLMovie);
                media.readGenre(XMLMovie);
                media.readStudio(XMLMovie);
                media.readCrew(XMLMovie);
                media.readCast(XMLMovie);
                media.readStreamData(XMLMovie);
                #endregion
                #region Movie
                if (XMLMovieIsMovie)
                {
                    ((CMovie)media).readVideoFiles(XMLMovie);
                    media.Airdate = XMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.Year);
                    media.PlayCount = (XMLMovie.XMLReadSubnode("seenit").XMLReadInnerText("") == "Yes" || XMLMovie.XMLReadSubnode("seenit").XMLReadInnerText("") == "Ja" ? "1" : "0");
                    media.PlayDate = XMLMovie.XMLReadSubnode("viewingdate").XMLReadSubnode("date").XMLReadInnerText("");

                    if (media.hasSpecials() && media.Set == "")
                    {
                        media.Set = media.Title;
                    }
                    this.MovieCollection.Add(((CMovie)media));
                }
                #endregion
                #region Series
                if (XMLMovieIsSeries)
                {
                    // releasedate and number episodes in series
                    media.Airdate = XMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.Year);
                    ((CSeries)media).NumberOfTotalEpisodes = (int)Int32.Parse(XMLMovie.XMLReadSubnode("chapters").XMLReadInnerText(""));

                    // episodes
                    foreach (XmlNode XMLSeriesDisc in XMLMovie.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
                    {
                        CEpisode seriesDiscEpisode = new CEpisode(this.Configuration);
                        seriesDiscEpisode.Series = ((CSeries)media);
                        seriesDiscEpisode.extractSeriesData((CSeries)media);
                        seriesDiscEpisode.overrideSeason(seriesDiscEpisode.overrideMediaStreamData(XMLSeriesDisc.XMLReadSubnode("title").XMLReadInnerText("")), false);

                        foreach (XmlNode XMLEpisode in XMLSeriesDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                        {
                            CEpisode episode = new CEpisode(this.Configuration);

                            // reset Episode-Attributes to Disc-Attributes
                            episode.extractSeriesData(seriesDiscEpisode);

                            // read Episode-Details
                            episode.Series = ((CSeries)media);
                            episode.Title = episode.overrideSeason(episode.overrideMediaStreamData(XMLEpisode.XMLReadSubnode("title").XMLReadInnerText("")), true);
                            episode.TitleSort = episode.Title;
                            episode.TitleOriginal = episode.Title;
                            episode.Set = episode.Series.Set;
                            episode.Plot = XMLEpisode.XMLReadSubnode("plot").XMLReadInnerText("");
                            episode.RunTime = XMLEpisode.XMLReadSubnode("runtimeminutes").XMLReadInnerText("");
                            episode.Airdate = XMLEpisode.XMLReadSubnode("firstairdate").XMLReadSubnode("date").XMLReadInnerText(episode.Series.Airdate);
                            episode.PlayCount = (XMLEpisode.XMLReadSubnode("seenit").XMLReadInnerText("") == "Yes" || XMLEpisode.XMLReadSubnode("seenit").XMLReadInnerText("") == "Ja" ? "1" : "0");
                            episode.PlayDate = XMLEpisode.XMLReadSubnode("viewingdate").XMLReadSubnode("date").XMLReadInnerText("");
                            episode.Genres = episode.Series.Genres;
                            episode.Studios = episode.Series.Studios;
                            episode.AudioStreams = episode.Series.AudioStreams;
                            episode.SubTitleStreams = episode.Series.SubTitleStreams;
                            episode.readCrew(XMLEpisode);
                            episode.readCast(XMLEpisode);
                            episode.readVideoFiles(XMLEpisode);
                            episode.readImages(XMLEpisode);

                            ((CSeries)media).Episodes.Add(episode);
                        }
                    }

                    SeriesCollection.Add(((CSeries)media));
                }
                #endregion
            };
        }
        private List<CMovie> clonePerLanguage(List<CMovie> movieCollection)
        {
            List<CMovie> movieCollectionPerLanguage = new List<CMovie>();

            foreach (CMovie movie in movieCollection)
                foreach (string movieLanguage in movie.MediaLanguages)
                {
                    CMovie movieClone = (CMovie)movie.clone();
                    List<string> mediaLanguagesToBeReplaced = new List<string>();
                    mediaLanguagesToBeReplaced.Add(movie.MediaLanguages[0]);
                    movieClone.clonePerLanguage(mediaLanguagesToBeReplaced, movieLanguage);
                    movieCollectionPerLanguage.Add(movieClone);
                }

            return movieCollectionPerLanguage;
        }
        private List<CSeries> clonePerLanguage(List<CSeries> seriesCollection)
        {
            List<CSeries> seriesCollectionPerLanguage = new List<CSeries>();

            foreach (CSeries series in seriesCollection)
                foreach (string seriesLanguage in series.MediaLanguages)
                {
                    CSeries seriesClone = (CSeries)series.clone();
                    List<string> mediaLanguagesToBeReplaced;

                    mediaLanguagesToBeReplaced = new List<string>();
                    mediaLanguagesToBeReplaced.Add(series.MediaLanguages[0]);  // series title
                    seriesClone.clonePerLanguage(mediaLanguagesToBeReplaced, seriesLanguage);


                    foreach (CEpisode episodeClone in seriesClone.Episodes)
                    {
                        mediaLanguagesToBeReplaced = new List<string>();
                        mediaLanguagesToBeReplaced.Add(series.MediaLanguages[0]);  // series title
                        mediaLanguagesToBeReplaced.Add(episodeClone.MediaLanguages[0]); // and episode data, when available languages change during series

                        episodeClone.clonePerLanguage(mediaLanguagesToBeReplaced, seriesLanguage); 
                    }
                    seriesCollectionPerLanguage.Add(seriesClone);
                }

            return seriesCollectionPerLanguage;
        }
        public List<CMovie> listMovieCollectionPerServer(int server)
        {
            List<CMovie> movieCollectionPerServer = new List<CMovie>();
            
            foreach (CMovie movie in listMovieCollectionPerServer(server, false))
            {
                if (movie != null)
                    movieCollectionPerServer.Add(movie);
            }
            
            foreach (CMovie movie in listMovieCollectionPerServer(server, true))
            {
                if (movie != null) { 
                    movie.Title = movie.Title + " (Specials)";
                    movie.TitleSort = movie.TitleSort + " (Specials)";
                    movie.TitleOriginal = movie.TitleOriginal + " (Specials)";
                    movie.Filename = movie.Filename + " (Specials)";
                    movieCollectionPerServer.Add(movie);
                }
            }

            return movieCollectionPerServer;
        }
        public List<CMovie> listMovieCollectionPerServer(int server, bool isSpecial)
        {
            List<CMovie> movieCollectionPerServer = new List<CMovie>();
            foreach (CMovie movie in this.clonePerLanguage(this.MovieCollection))
            {
                bool addMovie = false;
                foreach (int serverList in movie.Server)
                    if (serverList.Equals(server))
                        addMovie = true;

                if (movie.VideoFiles.Count > 0 && addMovie)
                {
                    CMovie movieClone = (CMovie)movie.clone(server, isSpecial);
                    movieCollectionPerServer.Add(movieClone);
                }
            }

            return movieCollectionPerServer;
        }
        public List<CSeries> listSeriesCollectionPerServer(int server)
        {
            List<CSeries> seriesCollectionPerServer = new List<CSeries>();

            foreach (CSeries series in this.clonePerLanguage(this.SeriesCollection))
            {
                bool addSeries = false;
                foreach (int serverList in series.Server)
                    if (serverList.Equals(server))
                        addSeries = true;

                if (addSeries)
                {
                    CSeries seriesClone = (CSeries)series.clone(server);
                    seriesCollectionPerServer.Add(seriesClone);
                }
            }

            return seriesCollectionPerServer;
        }
        #endregion
    }
}
