// <copyright file="Series.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
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
        public override void ReadMediaFiles(XmlNode xMLMedia)
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
        public override void WriteNFO()
        {
            using (StreamWriter swrNFO = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename + ".nfo", false, Encoding.UTF8, 512))
            {
                swrNFO.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                swrNFO.WriteLine("<tvshow>");
                swrNFO.WriteLine("    <title>" + this.Title + "</title>");
                swrNFO.WriteLine("    <showtitle>" + this.Title + "</showtitle>");
                swrNFO.WriteLine("    <rating>" + this.Rating + "</rating>");
                swrNFO.WriteLine("    <year>" + this.PublishingYear + "</year>");
                swrNFO.WriteLine("    <plot>" + this.Content + "</plot>");
                swrNFO.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
                swrNFO.WriteLine("    <episode>" + this.NumberOfTotalEpisodes + "</episode>");
                swrNFO.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

                if (this.PlayDate != string.Empty)
                {
                    swrNFO.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");
                }

                swrNFO.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");
                swrNFO.WriteLine("    <aired>" + this.PublishingDate + "</aired>");
                swrNFO.WriteLine("    <premiered>" + this.PublishingDate + "</premiered>");
                swrNFO.WriteLine("    <MediaGroup>" + this.MediaGroup + "</MediaGroup>");
                swrNFO.WriteLine("    <id>" + this.ID + "</id>");
                swrNFO.WriteLine("    <country>" + this.Country + "</country>");

                this.WriteGenre(swrNFO);
                this.WriteStudio(swrNFO);
                this.WriteCrew(swrNFO);
                this.WriteCast(swrNFO);
                this.WriteImagesToNFO(swrNFO);

                swrNFO.WriteLine("</tvshow>");

                if (this.IMDbId != string.Empty)
                {
                    swrNFO.WriteLine("http://www.imdb.com/title/tt" + this.IMDbId + "/");
                }

                if (this.TMDbId != string.Empty)
                {
                    swrNFO.WriteLine("http://www.themoviedb.org/" + this.TMDbType + "/" + this.TMDbId + "/");
                }
            }

            foreach (Episode episode in this.Episodes)
            {
                episode.WriteNFO();
            }
        }

        /// <inheritdoc/>
        public override void WriteSH(StreamWriter swrSH, bool createNewMedia)
        {
            if (this.Title != string.Empty)
            {
                swrSH.WriteLine("if [ -d \"" + this.Filename + "\" ];");
                swrSH.WriteLine("then ");
                swrSH.WriteLine("    rm -r \"" + this.Filename + "\"");
                swrSH.WriteLine("fi;");

                if (createNewMedia)
                {
                    swrSH.WriteLine("mkdir \"" + this.Filename + "\"");

                    swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                    for (int i = 0; i < this.numberOfEpisodesPerSeason.Count; i++)
                    {
                        swrSH.WriteLine("mkdir \"Season " + this.ConvertSeason(i.ToString()) + "\"");
                    }

                    swrSH.WriteLine("mkdir \"extrafanart\"");
                    swrSH.WriteLine("mkdir \"extrathumbs\"");

                    swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                    swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"tvshow.nfo\"");

                    // Images
                    this.WriteImagesToSH(swrSH);

                    swrSH.WriteLine("cd /share/XBMC/Serien/");

                    foreach (Episode episode in this.Episodes)
                    {
                        episode.WriteSH(swrSH, true);
                    }
                }
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
            seriesClone.Images = this.Images; // if required, still to be cloned
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

            seriesClone.Filename = this.Filename;
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
                seriesClone.Images = this.Images; // if required, still to be cloned
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

                seriesClone.Filename = this.Filename;
                seriesClone.AddServer(server);
                seriesClone.VideoCodec = this.VideoCodec;
                seriesClone.VideoDefinition = this.VideoDefinition;
                seriesClone.VideoAspectRatio = this.VideoAspectRatio;
                seriesClone.AudioStreams = this.AudioStreams;
                seriesClone.SubTitles = this.SubTitles;
                seriesClone.MediaLanguages = this.MediaLanguages;

                foreach (Episode episode in this.Episodes)
                {
                    bool cloneEpisode = false;
                    foreach (VideoFile videoFile in episode.MediaFiles)
                    {
                        if (videoFile.URL != string.Empty && videoFile.Server.Equals(server))
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

                seriesClone.NumberOfTotalEpisodes = this.NumberOfTotalEpisodes;
                seriesClone.NumberOfEpisodes = this.NumberOfEpisodes;
                seriesClone.NumberOfSpecials = this.NumberOfSpecials;
                seriesClone.NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason;
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

        /// <inheritdoc/>
        public override void SetFilename()
        {
            base.SetFilename();

            foreach (Episode epsEpisode in this.Episodes)
            {
                epsEpisode.SetFilename();
            }
        }

        #endregion
    }
}
