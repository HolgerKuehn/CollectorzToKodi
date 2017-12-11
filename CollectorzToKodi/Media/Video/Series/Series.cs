// <copyright file="Series.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing series from MovieCollector
    /// </summary>
    public class Series : Video
    {
        #region Attributes

        /// <summary>
        /// List of episodes in this series
        /// </summary>
        private List<Episode> episodes;

        /// <summary>
        /// number of episodes, whether specials or not, that are part of this series
        /// </summary>
        private int numberOfTotalEpisodes;

        /// <summary>
        /// number of episodes, not including specials, that are part of this series
        /// </summary>
        private int numberOfEpisodes;

        /// <summary>
        /// number of specials, that are part of this series
        /// </summary>
        private int numberOfSpecials;

        /// <summary>
        /// list of numbers of episodes in each season
        /// </summary>
        private List<int> numberOfEpisodesPerSeason;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Series"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Series(Configuration configuration)
            : base(configuration)
        {
            this.episodes = new List<Episode>();
            this.numberOfTotalEpisodes = 0;
            this.numberOfEpisodes = 0;
            this.numberOfSpecials = 0;
            this.numberOfEpisodesPerSeason = new List<int>();
            this.numberOfEpisodesPerSeason.Add(0); // Specials
            this.numberOfEpisodesPerSeason.Add(0); // Season 1
        }

        #endregion
        #region Properties

        /// <inheritdoc/>
        public override MediaPath MediaPath
        {
            get
            {
                return base.MediaPath;
            }

            set
            {
                base.MediaPath = value;

                base.MediaPath.WindowsPathToDestination = base.MediaPath.WindowsPathToDestination + "\\" + this.Configuration.ServerSeriesDirectory + "\\" + this.MediaPath.Filename;

                foreach (Episode episode in this.Episodes)
                {
                    episode.MediaPath = this.MediaPath;
                }
            }
        }

        /// <inheritdoc/>
        public override List<int> Server
        {
            get
            {
                return base.Server;
            }

            set
            {
                base.Server = value;

                this.DeviceDestinationPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaPublication][this.Server[0].ToString()] + this.Configuration.ServerSeriesDirectory;
            }
        }

        /// <summary>
        /// Gets or sets list of episodes in this series
        /// </summary>
        public List<Episode> Episodes
        {
            get { return this.episodes; }
            set { this.episodes = value; }
        }

        /// <summary>
        /// Gets or sets number of episodes, whether specials or not, that are part of this series
        /// </summary>
        public int NumberOfTotalEpisodes
        {
            get { return this.numberOfTotalEpisodes; }
            set { this.numberOfTotalEpisodes = value; }
        }

        /// <summary>
        /// Gets or sets number of episodes, not including specials, that are part of this series
        /// </summary>
        public int NumberOfEpisodes
        {
            get { return this.numberOfEpisodes; }
            set { this.numberOfEpisodes = value; }
        }

        /// <summary>
        /// Gets or sets number of specials, that are part of this series
        /// </summary>
        public int NumberOfSpecials
        {
            get { return this.numberOfSpecials; }
            set { this.numberOfSpecials = value; }
        }

        /// <summary>
        /// Gets or sets list of numbers of episodes in each season
        /// </summary>
        public List<int> NumberOfEpisodesPerSeason
        {
            get { return this.numberOfEpisodesPerSeason; }
            set { this.numberOfEpisodesPerSeason = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override Actor ActorFactory(Configuration configuration)
        {
            return new SeriesActor(configuration);
        }

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            // read SubTitles for each Episode
            foreach (Episode episode in this.Episodes)
            {
                foreach (VideoFile videoFileSubtitle in episode.MediaFiles)
                {
                    videoFileSubtitle.ReadSubTitleFile(xMLMedia);
                }
            }
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter shStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            // write NFO-file for Series
            nfoStreamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
            nfoStreamWriter.WriteLine("<tvshow>");
            nfoStreamWriter.WriteLine("    <title>" + this.Title + "</title>");
            nfoStreamWriter.WriteLine("    <showtitle>" + this.Title + "</showtitle>");
            nfoStreamWriter.WriteLine("    <rating>" + this.Rating + "</rating>");
            nfoStreamWriter.WriteLine("    <year>" + this.PublishingYear + "</year>");
            nfoStreamWriter.WriteLine("    <plot>" + this.Content + "</plot>");
            nfoStreamWriter.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
            nfoStreamWriter.WriteLine("    <episode>" + this.NumberOfTotalEpisodes + "</episode>");
            nfoStreamWriter.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

            if (this.PlayDate != string.Empty)
            {
                nfoStreamWriter.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");
            }

            nfoStreamWriter.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");
            nfoStreamWriter.WriteLine("    <aired>" + this.PublishingDate + "</aired>");
            nfoStreamWriter.WriteLine("    <premiered>" + this.PublishingDate + "</premiered>");
            nfoStreamWriter.WriteLine("    <MediaGroup>" + this.MediaGroup + "</MediaGroup>");
            nfoStreamWriter.WriteLine("    <id>" + this.ID + "</id>");
            nfoStreamWriter.WriteLine("    <country>" + this.Country + "</country>");

            this.WriteGenreToLibrary();
            this.WriteStudioToLibrary();
            this.WriteCrewToLibrary();
            this.WriteCastToLibrary();
            this.WriteImagesToLibrary();

            nfoStreamWriter.WriteLine("</tvshow>");

            if (this.IMDbId != string.Empty)
            {
                nfoStreamWriter.WriteLine("http://www.imdb.com/title/tt" + this.IMDbId + "/");
            }

            if (this.TMDbId != string.Empty)
            {
                nfoStreamWriter.WriteLine("http://www.themoviedb.org/" + this.TMDbType + "/" + this.TMDbId + "/");
            }

            // write Batch-File for Series
            if (this.Title != string.Empty)
            {
                // ######
                shStreamWriter.WriteLine("mkdir \"" + this.MediaPath.Filename + "\"");

                shStreamWriter.WriteLine("cd \"/share/XBMC/Serien/" + this.MediaPath.Filename + "\"");
                for (int i = 0; i < this.numberOfEpisodesPerSeason.Count; i++)
                {
                    shStreamWriter.WriteLine("mkdir \"Season " + this.ConvertSeason(i.ToString()) + "\"");
                }

                shStreamWriter.WriteLine("mkdir \"extrafanart\"");
                shStreamWriter.WriteLine("mkdir \"extrathumbs\"");

                shStreamWriter.WriteLine("cd \"/share/XBMC/Serien/" + this.MediaPath.Filename + "\"");
                shStreamWriter.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.MediaPath.Filename + ".nfo\" \"tvshow.nfo\"");

                // Images
                this.WriteImagesToLibrary();
            }

            // create NFO-files and Batch-file for each Episode
            foreach (Episode episode in this.Episodes)
            {
                episode.WriteToLibrary();
            }
        }

        /// <inheritdoc/>
        public override Media Clone()
        {
            Series seriesClone = new Series(this.Configuration);
            seriesClone.ID = this.ID;
            seriesClone.Title = this.Title;
            seriesClone.TitleSort = this.TitleSort;
            seriesClone.TitleOriginal = this.TitleOriginal;
            seriesClone.MediaGroup = this.MediaGroup;
            seriesClone.Rating = this.Rating;
            seriesClone.PublishingYear = this.PublishingYear;
            seriesClone.PublishingDate = this.PublishingDate;
            seriesClone.Content = this.Content;
            seriesClone.RunTime = this.RunTime;
            seriesClone.Images = this.Images; // if required; still to be cloned
            seriesClone.MPAA = this.MPAA;
            seriesClone.PlayCount = this.PlayCount;
            seriesClone.PlayDate = this.PlayDate;
            seriesClone.IMDbId = this.IMDbId;
            seriesClone.TMDbType = this.TMDbType;
            seriesClone.TMDbId = this.TMDbId;
            seriesClone.Country = this.Country;
            seriesClone.Genres = this.Genres;
            seriesClone.Studios = this.Studios;
            seriesClone.Directors = this.Directors;
            seriesClone.Writers = this.Writers;
            seriesClone.Actors = this.Actors;

            foreach (VideoFile videoFile in this.MediaFiles)
            {
                seriesClone.MediaFiles.Add((VideoFile)videoFile.Clone());
            }

            seriesClone.MediaPath.Filename = this.MediaPath.Filename;
            seriesClone.Server = this.Server;
            seriesClone.VideoCodec = this.VideoCodec;
            seriesClone.VideoDefinition = this.VideoDefinition;
            seriesClone.VideoAspectRatio = this.VideoAspectRatio;
            seriesClone.AudioStreams = this.AudioStreams;
            seriesClone.SubTitles = this.SubTitles;
            seriesClone.MediaLanguages = this.MediaLanguages;

            seriesClone.Episodes = new List<Episode>();
            foreach (Episode episode in this.Episodes)
            {
                Episode episodeClone = (Episode)episode.Clone();
                episodeClone.Series = seriesClone;

                seriesClone.Episodes.Add(episodeClone);
            }

            seriesClone.NumberOfTotalEpisodes = this.NumberOfTotalEpisodes;
            seriesClone.NumberOfEpisodes = this.NumberOfEpisodes;
            seriesClone.NumberOfSpecials = this.NumberOfSpecials;
            seriesClone.NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason;

            return (Series)seriesClone;
        }

        /// <inheritdoc/>
        public override Video Clone(int server, bool isSpecial = false)
        {
            bool cloneSeries = false;
            foreach (int serverList in this.Server)
            {
                if (serverList.Equals(server))
                {
                    cloneSeries = true;
                }
            }

            Series seriesClone = null;

            if (cloneSeries)
            {
                seriesClone = new Series(this.Configuration);
                seriesClone.ID = this.ID;
                seriesClone.Title = this.Title;
                seriesClone.TitleSort = this.TitleSort;
                seriesClone.TitleOriginal = this.TitleOriginal;
                seriesClone.MediaGroup = this.MediaGroup;
                seriesClone.Rating = this.Rating;
                seriesClone.PublishingYear = this.PublishingYear;
                seriesClone.PublishingDate = this.PublishingDate;
                seriesClone.Content = this.Content;
                seriesClone.RunTime = this.RunTime;
                seriesClone.Images = this.Images; // if required; still to be cloned
                seriesClone.MPAA = this.MPAA;
                seriesClone.PlayCount = this.PlayCount;
                seriesClone.PlayDate = this.PlayDate;
                seriesClone.IMDbId = this.IMDbId;
                seriesClone.TMDbType = this.TMDbType;
                seriesClone.TMDbId = this.TMDbId;
                seriesClone.Country = this.Country;
                seriesClone.Genres = this.Genres;
                seriesClone.Studios = this.Studios;
                seriesClone.Directors = this.Directors;
                seriesClone.Writers = this.Writers;
                seriesClone.Actors = this.Actors;
                seriesClone.MediaPath.Filename = this.MediaPath.Filename;
                seriesClone.VideoCodec = this.VideoCodec;
                seriesClone.VideoDefinition = this.VideoDefinition;
                seriesClone.VideoAspectRatio = this.VideoAspectRatio;
                seriesClone.AudioStreams = this.AudioStreams;
                seriesClone.SubTitles = this.SubTitles;
                seriesClone.MediaLanguages = this.MediaLanguages;

                seriesClone.NumberOfTotalEpisodes = this.NumberOfTotalEpisodes;
                seriesClone.NumberOfEpisodes = this.NumberOfEpisodes;
                seriesClone.NumberOfSpecials = this.NumberOfSpecials;
                seriesClone.NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason;

                foreach (VideoFile videoFile in this.MediaFiles)
                {
                    seriesClone.MediaFiles.Add((VideoFile)videoFile.Clone());
                }

                seriesClone.AddServer(server);

                foreach (Episode episode in this.Episodes)
                {
                    bool cloneEpisode = false;
                    foreach (VideoFile videoFile in episode.MediaFiles)
                    {
                        if (videoFile.UrlForMediaStorage != string.Empty && videoFile.Server.Equals(server))
                        {
                            cloneEpisode = true;
                        }
                    }

                    if (cloneEpisode)
                    {
                        Episode episodeClone = (Episode)episode.Clone();
                        episodeClone.Series = seriesClone;
                        seriesClone.Episodes.Add(episodeClone);
                    }
                }
            }

            return seriesClone;
        }

        /// <inheritdoc/>
        public override string OverrideVideoStreamData(string title)
        {
            string returnTitle = base.OverrideVideoStreamData(title);
            this.CheckForDefaultMediaLanguages();

            return returnTitle;
        }

        #endregion
    }
}
