// <copyright file="CSeries.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Class managing series from MovieCollector
    /// </summary>
    public class CSeries : CVideo
    {
        #region Attributes

        /// <summary>
        /// List of episodes in this series
        /// </summary>
        private List<CEpisode> episodes;

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
        /// Initializes a new instance of the <see cref="CSeries"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CSeries(CConfiguration configuration)
            : base(configuration)
        {
            this.episodes = new List<CEpisode>();
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
        public List<CEpisode> Episodes
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
        public override void ReadMediaFiles(XmlNode xMLMedia)
        {
            throw new NotImplementedException();
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
                swrNFO.WriteLine("    <id>" + this.IMDbId + "</id>");
                swrNFO.WriteLine("    <country>" + this.Country + "</country>");

                this.WriteGenre(swrNFO);
                this.WriteStudio(swrNFO);
                this.WriteCrew(swrNFO);
                this.WriteCast(swrNFO);
                this.WriteImagesToNFO(swrNFO);

                swrNFO.WriteLine("</tvshow>");
            }

            foreach (CEpisode episode in this.Episodes)
            {
                episode.WriteNFO();
            }
        }

        /// <inheritdoc/>
        public override void WriteSH(StreamWriter swrSH)
        {
            if (this.Title != string.Empty)
            {
                swrSH.WriteLine("if [ -d \"" + this.Filename + "\" ];");
                swrSH.WriteLine("then ");
                swrSH.WriteLine("    rm -r \"" + this.Filename + "\"");
                swrSH.WriteLine("fi;");
                swrSH.WriteLine("mkdir \"" + this.Filename + "\"");

                swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                for (int i = 0; i < this.numberOfEpisodesPerSeason.Count; i++)
                {
                    swrSH.WriteLine("mkdir \"Season " + ("00" + i.ToString()).Substring(i.ToString().Length) + "\"");
                }

                swrSH.WriteLine("mkdir \"extrafanart\"");
                swrSH.WriteLine("mkdir \"extrathumbs\"");

                swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"tvshow.nfo\"");

                // Images
                this.WriteImagesToSH(swrSH);
                this.WriteSubTitleStreamDataToSH(swrSH);

                swrSH.WriteLine("cd /share/XBMC/Serien/");

                foreach (CEpisode episode in this.Episodes)
                {
                    episode.WriteSH(swrSH);
                }
            }
        }

        /// <inheritdoc/>
        public override CMedia Clone()
        {
            CSeries seriesClone = new CSeries(this.Configuration);
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
            seriesClone.Country = this.Country;
            seriesClone.Genres = this.Genres;
            seriesClone.Studios = this.Studios;
            seriesClone.Directors = this.Directors;
            seriesClone.Writers = this.Writers;
            seriesClone.Actors = this.Actors;

            foreach (CVideoFile videoFile in this.MediaFiles)
            {
                seriesClone.MediaFiles.Add((CVideoFile)videoFile.Clone());
            }

            seriesClone.Filename = this.Filename;
            seriesClone.Server = this.Server;
            seriesClone.VideoCodec = this.VideoCodec;
            seriesClone.VideoDefinition = this.VideoDefinition;
            seriesClone.VideoAspectRatio = this.VideoAspectRatio;
            seriesClone.AudioStreams = this.AudioStreams;
            seriesClone.SubTitleStreams = this.SubTitleStreams;
            seriesClone.MediaLanguages = this.MediaLanguages;

            seriesClone.Episodes = new List<CEpisode>();
            foreach (CEpisode episode in this.Episodes)
            {
                CEpisode episodeClone = (CEpisode)episode.Clone();
                episodeClone.Series = seriesClone;

                seriesClone.Episodes.Add(episodeClone);
            }

            seriesClone.NumberOfTotalEpisodes = this.NumberOfTotalEpisodes;
            seriesClone.NumberOfEpisodes = this.NumberOfEpisodes;
            seriesClone.NumberOfSpecials = this.NumberOfSpecials;
            seriesClone.NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason;

            return seriesClone;
        }

        /// <inheritdoc/>
        public override CVideo Clone(int server, bool isSpecial = false)
        {
            bool cloneSeries = false;
            foreach (int serverList in this.Server)
            {
                if (serverList.Equals(server))
                {
                    cloneSeries = true;
                }
            }

            CSeries seriesClone = null;

            if (cloneSeries)
            {
                seriesClone = new CSeries(this.Configuration);
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
                seriesClone.Country = this.Country;
                seriesClone.Genres = this.Genres;
                seriesClone.Studios = this.Studios;
                seriesClone.Directors = this.Directors;
                seriesClone.Writers = this.Writers;
                seriesClone.Actors = this.Actors;

                foreach (CVideoFile videoFile in this.MediaFiles)
                {
                    seriesClone.MediaFiles.Add((CVideoFile)videoFile.Clone());
                }

                seriesClone.Filename = this.Filename;
                seriesClone.AddServer(server);
                seriesClone.VideoCodec = this.VideoCodec;
                seriesClone.VideoDefinition = this.VideoDefinition;
                seriesClone.VideoAspectRatio = this.VideoAspectRatio;
                seriesClone.AudioStreams = this.AudioStreams;
                seriesClone.SubTitleStreams = this.SubTitleStreams;
                seriesClone.MediaLanguages = this.MediaLanguages;

                foreach (CEpisode episode in this.Episodes)
                {
                    bool cloneEpisode = false;
                    foreach (CVideoFile videoFile in episode.MediaFiles)
                    {
                        if (videoFile.URL != string.Empty && videoFile.Server.Equals(server))
                        {
                            cloneEpisode = true;
                        }
                    }

                    if (cloneEpisode)
                    {
                        CEpisode episodeClone = (CEpisode)episode.Clone();
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

        #endregion
    }
}
