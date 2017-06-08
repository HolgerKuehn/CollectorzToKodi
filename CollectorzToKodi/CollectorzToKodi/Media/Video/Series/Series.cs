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
            this.numberOfEpisodesPerSeason = new List<int>
            {
                0, // Specials
                0 // Season 1
            };
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
        public override void DeleteFromLibrary()
        {
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            // write NFO-file
            using (bfStreamWriter)
            {
                if (this.Title != string.Empty)
                {
                    bfStreamWriter.WriteLine("if [ -d \"" + this.Filename + "\" ];");
                    bfStreamWriter.WriteLine("then ");
                    bfStreamWriter.WriteLine("    rm -r \"" + this.Filename + "\"");
                    bfStreamWriter.WriteLine("fi;");
                    bfStreamWriter.WriteLine("");
                }
            }
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            // create NFO-file and nfoStreamWriter
            if (this.NfoFile == null)
            {
                this.NfoFile = new NfoFile(this.Configuration)
                {
                    Media = this
                };
            }

            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;

            // write NFO-file for Series
            using (nfoStreamWriter)
            {
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

                this.WriteGenre(nfoStreamWriter);
                this.WriteStudio(nfoStreamWriter);
                this.WriteCrew(nfoStreamWriter);
                this.WriteCast(nfoStreamWriter);
                this.WriteImagesToNFO(nfoStreamWriter);

                nfoStreamWriter.WriteLine("</tvshow>");

                if (this.IMDbId != string.Empty)
                {
                    nfoStreamWriter.WriteLine("http://www.imdb.com/title/tt" + this.IMDbId + "/");
                }

                if (this.TMDbId != string.Empty)
                {
                    nfoStreamWriter.WriteLine("http://www.themoviedb.org/" + this.TMDbType + "/" + this.TMDbId + "/");
                }
            }

            // write Batch-File for Series
            if (this.Title != string.Empty)
            {
                // ######
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
            Series seriesClone = new Series(this.Configuration)
            {
                ID = this.ID,
                Title = this.Title,
                TitleSort = this.TitleSort,
                TitleOriginal = this.TitleOriginal,
                MediaGroup = this.MediaGroup,
                Rating = this.Rating,
                PublishingYear = this.PublishingYear,
                PublishingDate = this.PublishingDate,
                Content = this.Content,
                RunTime = this.RunTime,
                Images = this.Images, // if required, still to be cloned
                MPAA = this.MPAA,
                PlayCount = this.PlayCount,
                PlayDate = this.PlayDate,
                IMDbId = this.IMDbId,
                TMDbType = this.TMDbType,
                TMDbId = this.TMDbId,
                Country = this.Country,
                Genres = this.Genres,
                Studios = this.Studios,
                Directors = this.Directors,
                Writers = this.Writers,
                Actors = this.Actors
            };
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
                seriesClone = new Series(this.Configuration)
                {
                    ID = this.ID,
                    Title = this.Title,
                    TitleSort = this.TitleSort,
                    TitleOriginal = this.TitleOriginal,
                    MediaGroup = this.MediaGroup,
                    Rating = this.Rating,
                    PublishingYear = this.PublishingYear,
                    PublishingDate = this.PublishingDate,
                    Content = this.Content,
                    RunTime = this.RunTime,
                    Images = this.Images, // if required, still to be cloned
                    MPAA = this.MPAA,
                    PlayCount = this.PlayCount,
                    PlayDate = this.PlayDate,
                    IMDbId = this.IMDbId,
                    TMDbType = this.TMDbType,
                    TMDbId = this.TMDbId,
                    Country = this.Country,
                    Genres = this.Genres,
                    Studios = this.Studios,
                    Directors = this.Directors,
                    Writers = this.Writers,
                    Actors = this.Actors,
                    Filename = this.Filename,
                    VideoCodec = this.VideoCodec,
                    VideoDefinition = this.VideoDefinition,
                    VideoAspectRatio = this.VideoAspectRatio,
                    AudioStreams = this.AudioStreams,
                    SubTitles = this.SubTitles,
                    MediaLanguages = this.MediaLanguages,

                    NumberOfTotalEpisodes = this.NumberOfTotalEpisodes,
                    NumberOfEpisodes = this.NumberOfEpisodes,
                    NumberOfSpecials = this.NumberOfSpecials,
                    NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason
                };

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
