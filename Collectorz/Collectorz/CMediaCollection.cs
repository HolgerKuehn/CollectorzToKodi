using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace Collectorz
{
    public class CMediaCollection
    {
        #region Attributes
        private List<CMovie> movieCollection;
        private List<CSeries> seriesCollection;
        #endregion
        #region Constructor
        public CMediaCollection()
        {
            movieCollection = this.MovieCollection;
            seriesCollection = this.SeriesCollection;
        }
        #endregion
        #region Properties (Singleton)
        public List<CMovie> MovieCollection
        {
            get
            {
                if (movieCollection == null)
                    movieCollection = new List<CMovie>();

                return movieCollection;
            }
            set { }
        }
        public List<CSeries> SeriesCollection
        {
            get
            {
                if (seriesCollection == null)
                    seriesCollection = new List<CSeries>();

                return seriesCollection;
            }
            set { }
        }
        #endregion
        #region Functions
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
                    media = new CMovie();

                if (XMLMovieIsSeries)
                    media = new CSeries();

                if (media == null)
                    throw new NotImplementedException("Media not defined as Movie nor Series.");

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
                    
                    this.MovieCollection.Add(((CMovie)media));
                }
                #endregion
                #region Series
                if (XMLMovieIsSeries)
                {
                    // releasdate and number episodes in series
                    media.Airdate = XMLMovie.XMLReadSubnode("releasedate").XMLReadSubnode("date").XMLReadInnerText(media.Year);
                    ((CSeries)media).NumberOfTotalEpisodes = (int)Int32.Parse(XMLMovie.XMLReadSubnode("chapters").XMLReadInnerText(""));

                    // episodes
                    foreach (XmlNode XMLSeriesDisc in XMLMovie.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
                    {
                        CEpisode seriesDiscEpisode = new CEpisode();
                        seriesDiscEpisode.Series = ((CSeries)media);
                        seriesDiscEpisode.extractSeriesData((CSeries)media);
                        seriesDiscEpisode.overrideSeason(seriesDiscEpisode.overrideMediaStreamData(XMLSeriesDisc.XMLReadSubnode("title").XMLReadInnerText("")), false);

                        foreach (XmlNode XMLEpisode in XMLSeriesDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                        {
                            CEpisode episode = new CEpisode();

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
                    movieClone.clonePerLanguage(movie.MediaLanguages[0], movieLanguage);
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
                    seriesClone.clonePerLanguage(series.MediaLanguages[0], seriesLanguage);

                    foreach (CEpisode episodeClone in seriesClone.Episodes)
                        episodeClone.clonePerLanguage(episodeClone.MediaLanguages[0], seriesLanguage);

                    seriesCollectionPerLanguage.Add(seriesClone);
                }

            return seriesCollectionPerLanguage;
        }
        public List<CMovie> listMovieCollectionPerServer(CConstants.ServerList server, bool isSpecial = false)
        {
            List<CMovie> movieCollectionPerServer = new List<CMovie>();
            foreach (CMovie movie in this.clonePerLanguage(this.MovieCollection))
            {
                bool addMovie = false;
                foreach (CConstants.ServerList serverList in movie.Server)
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
        public List<CSeries> listSeriesCollectionPerServer(CConstants.ServerList server)
        {
            List<CSeries> seriesCollectionPerServer = new List<CSeries>();

            foreach (CSeries series in this.clonePerLanguage(this.SeriesCollection))
            {
                bool addSeries = false;
                foreach (CConstants.ServerList serverList in series.Server)
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
