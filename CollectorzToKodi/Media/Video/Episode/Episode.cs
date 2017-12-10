﻿// <copyright file="Episode.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Class managing episodes
    /// </summary>
    public class Episode : Video
    {
        #region Attributes

        /// <summary>
        /// number of season the episode is placed
        /// </summary>
        /// <remarks>0 = specials; other = number of actual season</remarks>
        private string actualSeason;

        /// <summary>
        /// number of episode in complete series
        /// </summary>
        private string actualEpisode;

        /// <summary>
        /// number of season the episode is placed as displayed in Kodi
        /// </summary>
        /// <remarks>specials are using the actual season-number they are to be displayed in</remarks>
        private string displaySeason;

        /// <summary>
        /// number of episode in display season
        /// </summary>
        private string displayEpisode;

        /// <summary>
        /// marks specials or regular episodes
        /// </summary>
        private bool isSpecial;

        /// <summary>
        /// reference to series containing the episode
        /// </summary>
        private Series series;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectorzToKodi.Episode"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Episode(Configuration configuration)
            : base(configuration)
        {
            this.actualSeason = string.Empty;
            this.actualEpisode = string.Empty;
            this.displaySeason = string.Empty;
            this.displayEpisode = string.Empty;
            this.series = null;
            this.isSpecial = false;
        }

        #endregion
        #region Properties

        /// <inheritdoc/>
        public override string Filename
        {
            get
            {
                return base.Filename;
            }

            set
            {
                base.Filename = this.Series.Filename + " S" + ("0000" + this.actualSeason).Substring(this.actualSeason.Length) + " E" + ("0000" + this.actualEpisode.ToString()).Substring(this.actualEpisode.ToString().Length);

                foreach (VideoFile videoFile in this.MediaFiles)
                {
                    videoFile.Filename = this.Filename;
                }

                foreach (ImageFile imageFile in this.Images)
                {
                    imageFile.Filename = this.Filename + "-thumb";
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

                this.UrlForMediaPublicationLocalFilesystem = this.Series.UrlForMediaPublicationLocalFilesystem + "Season " + this.ConvertSeason(this.actualSeason) + (this.Configuration.ServerMappingType == "UNIX" ? "/" : "\\");
            }
        }

        /// <summary>
        /// Gets or sets number of season the episode is placed
        /// </summary>
        /// <remarks>0 = specials; other = number of actual season</remarks>
        public string ActualSeason
        {
            get { return this.actualSeason; }
            set { this.actualSeason = value; }
        }

        /// <summary>
        /// Gets or sets number of season the episode is placed as displayed in Kodi
        /// </summary>
        /// <remarks>specials are using the actual season-number they are to be displayed in</remarks>
        public string DisplaySeason
        {
            get { return this.displaySeason; }
            set { this.displaySeason = value; }
        }

        /// <summary>
        /// Gets or sets number of episode in display season
        /// </summary>
        public string ActualEpisode
        {
            get { return this.actualEpisode; }
            set { this.actualEpisode = value; }
        }

        /// <summary>
        /// Gets or sets number of episode in display season
        /// </summary>
        public string DisplayEpisode
        {
            get { return this.displayEpisode; }
            set { this.displayEpisode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the episode is a specials or regular episode
        /// </summary>
        public bool IsSpecial
        {
            get { return this.isSpecial; }
            set { this.isSpecial = value; }
        }

        /// <summary>
        /// Gets or sets reference to series containing the episode
        /// </summary>
        public Series Series
        {
            get { return this.series; }
            set { this.series = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override void ReadMediaFilesFromXml(XmlNode xMLMedia)
        {
            VideoFile videoFile = new VideoFile(this.Configuration);

            videoFile.IsSpecial = this.IsSpecial;
            videoFile.Description = "EpisodeVideoFile";
            videoFile.UrlForMediaStorage = xMLMedia.XMLReadSubnode("movielink").XMLReadInnerText(string.Empty);
            videoFile.Media = this;
            videoFile.FileIndex = this.VideoIndex;

            this.MediaFiles.Add(videoFile);
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            nfoStreamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
            nfoStreamWriter.WriteLine("<episodedetails>");
            nfoStreamWriter.WriteLine("    <title>" + this.Title + "</title>");
            nfoStreamWriter.WriteLine("    <season>" + this.actualSeason + "</season>");
            nfoStreamWriter.WriteLine("    <displayseason>" + this.DisplaySeason + "</displayseason>");
            nfoStreamWriter.WriteLine("    <episode>" + this.actualEpisode + "</episode>");
            nfoStreamWriter.WriteLine("    <displayepisode>" + this.DisplayEpisode + "</displayepisode>");
            nfoStreamWriter.WriteLine("    <aired>" + this.PublishingDate + "</aired>");
            nfoStreamWriter.WriteLine("    <premiered>" + this.PublishingDate + "</premiered>");
            nfoStreamWriter.WriteLine("    <rating>" + this.Rating + "</rating>");
            nfoStreamWriter.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");
            nfoStreamWriter.WriteLine("    <plot>" + this.Content + "</plot>");
            nfoStreamWriter.WriteLine("    <runtime>" + this.RunTime + "</runtime>");

            if (this.PlayDate != string.Empty)
            {
                nfoStreamWriter.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");
            }

            nfoStreamWriter.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");

            nfoStreamWriter.WriteLine("    <set>" + this.MediaGroup + "</set>");

            this.WriteGenreToLibrary();
            this.WriteStudioToLibrary();
            this.WriteCrewToLibrary();
            this.WriteCastToLibrary();
            this.WriteStreamDataToLibraryToLibrary();
            this.WriteImagesToLibrary();

            nfoStreamWriter.WriteLine("</episodedetails>");

            // createNewMedia is only checked for series; episodes are not changed, as not generated
            if (this.Title != string.Empty)
            {
                bfStreamWriter.WriteLine("cd \"" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaPublication][this.Server[0].ToString()] + "/" + this.Configuration.ServerSeriesDirectory + "/" + this.Series.Filename + "/Season " + this.ConvertSeason(this.actualSeason) + "\"");

                bfStreamWriter.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"" + this.Filename + ".nfo\"");

                // video files
                this.WriteVideoFilesToLibrary();
                this.WriteImagesToLibrary();

                bfStreamWriter.WriteLine("cd /share/XBMC/Serien/");
            }
        }

        /// <inheritdoc/>
        public override Media Clone()
        {
            Episode episodeClone = new Episode(this.Configuration);
            episodeClone.ID = this.ID;
            episodeClone.Title = this.Title;
            episodeClone.TitleSort = this.TitleSort;
            episodeClone.TitleOriginal = this.TitleOriginal;
            episodeClone.MediaGroup = this.MediaGroup;
            episodeClone.Rating = this.Rating;
            episodeClone.PublishingYear = this.PublishingYear;
            episodeClone.PublishingDate = this.PublishingDate;
            episodeClone.Content = this.Content;
            episodeClone.RunTime = this.RunTime;
            episodeClone.Images = this.Images; // if required, still to be cloned
            episodeClone.MPAA = this.MPAA;
            episodeClone.PlayCount = this.PlayCount;
            episodeClone.PlayDate = this.PlayDate;
            episodeClone.IMDbId = this.IMDbId;
            episodeClone.TMDbType = this.TMDbType;
            episodeClone.TMDbId = this.TMDbId;
            episodeClone.Country = this.Country;
            episodeClone.Genres = this.Genres;
            episodeClone.Studios = this.Studios;
            episodeClone.Directors = this.Directors;
            episodeClone.Writers = this.Writers;
            episodeClone.Actors = this.Actors;

            foreach (VideoFile mediaFile in this.MediaFiles)
            {
                episodeClone.MediaFiles.Add((VideoFile)mediaFile.Clone());
            }

            episodeClone.Filename = this.Filename;
            episodeClone.Server = this.Server;
            episodeClone.VideoCodec = this.VideoCodec;
            episodeClone.VideoDefinition = this.VideoDefinition;
            episodeClone.VideoAspectRatio = this.VideoAspectRatio;
            episodeClone.AudioStreams = this.AudioStreams;
            episodeClone.SubTitles = this.SubTitles;
            episodeClone.MediaLanguages = this.MediaLanguages;

            episodeClone.actualSeason = this.actualSeason;
            episodeClone.actualEpisode = this.actualEpisode;
            episodeClone.DisplaySeason = this.DisplaySeason;
            episodeClone.DisplayEpisode = this.DisplayEpisode;
            episodeClone.IsSpecial = this.IsSpecial;
            episodeClone.Series = this.Series;

            return (Episode)episodeClone;
        }

        /// <inheritdoc/>
        public override Video Clone(int server, bool isSpecial = false)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Sets the following attributes for a single episode according to the complete series<br />
        /// <br />
        /// MPAA-rating<br />
        /// Video-Definition<br />
        /// Video-Aspect-Ration<br />
        /// Rating<br />
        /// </summary>
        /// <param name="series">Series that contains this episode and the attribute should be cloned from</param>
        public void ExtractSeriesData(Series series)
        {
            // clone only relevant Attributes
            this.MPAA = series.MPAA;
            this.VideoDefinition = series.VideoDefinition;
            this.VideoAspectRatio = series.VideoAspectRatio;
            this.Rating = series.Rating;
        }

        /// <summary>
        /// Sets the following attributes for a single episode according to different episode<br />
        /// This is used to extract meta data from the Disc-object in MovieCollector.
        /// <br />
        /// MPAA-rating<br />
        /// Video-Definition<br />
        /// Video-Aspect-Ration<br />
        /// Rating<br />
        /// IsSpecial<br />
        /// Season<br />
        /// DisplaySeason<br />
        /// MediaLanguages<br />
        /// </summary>
        /// <param name="episode">Episode that contains the attributes to be cloned</param>
        public void ExtractSeriesData(Episode episode)
        {
            // clone only relevant Attributes
            this.MPAA = episode.MPAA;
            this.VideoCodec = episode.VideoCodec;
            this.VideoDefinition = episode.VideoDefinition;
            this.VideoAspectRatio = episode.VideoAspectRatio;
            this.Rating = episode.Rating;

            this.IsSpecial = episode.IsSpecial;
            this.actualSeason = episode.ActualSeason;
            this.DisplaySeason = episode.DisplaySeason;
            this.MediaLanguages = episode.MediaLanguages;
        }

        /// <summary>
        /// Sets Season and DisplaySeason for episode, as specified in disc- or episode-title
        /// </summary>
        /// <param name="title">Disc- or episode-title containing meta-data for season</param>
        /// <param name="countEpisode">Value indicating whether the episodes should be counted or not</param>
        /// <returns>Title without special tags containing meta-data for season</returns>
        /// <remarks>Meta-Tag available are:<br />
        /// (Special) - represents special
        /// (S###) - defines season the episode should be displayed in
        /// </remarks>
        public string OverrideSeason(string title, bool countEpisode = false)
        {
            this.actualSeason = "0";

            if (this.DisplaySeason == string.Empty)
            {
                this.DisplaySeason = "1";
            }

            if (title.Contains(this.Configuration.MovieCollectorSpecials))
            {
                this.isSpecial = true;
                title = title.Replace(this.Configuration.MovieCollectorSpecials, string.Empty);
            }

            if (title.Contains("(S"))
            {
                this.DisplaySeason = title.RightOf("(S").LeftOf(")");
                title = title.Replace("(S" + this.DisplaySeason + ")", string.Empty);
            }

            if (!this.IsSpecial)
            {
                this.actualSeason = this.DisplaySeason;
            }

            if (countEpisode)
            {
                // generate new season if necessary
                while (this.Series.NumberOfEpisodesPerSeason.Count - 1 < (int)int.Parse(this.actualSeason))
                {
                    this.Series.NumberOfEpisodesPerSeason.Add(0);
                }

                this.Series.NumberOfEpisodes = this.Series.NumberOfEpisodes + (this.IsSpecial ? 0 : 1);
                this.Series.NumberOfSpecials = this.Series.NumberOfSpecials + (this.IsSpecial ? 1 : 0);

                this.Series.NumberOfEpisodesPerSeason[(int)int.Parse(this.actualSeason)]++;

                this.actualEpisode = (string)this.Series.NumberOfEpisodesPerSeason.ElementAt((int)int.Parse(this.actualSeason)).ToString();
                this.DisplayEpisode = (string)(this.Series.NumberOfSpecials + this.Series.NumberOfEpisodes).ToString();
            }

            return title.Trim();
        }

        /// <inheritdoc/>
        public override void ReadImagesFromXml(XmlNode xMLNode)
        {
            ImageFile image;

            // Image
            image = new ImageFile(this.Configuration);
            image.Media = this;
            image.Season = this.ActualSeason;
            image.Filename = this.Filename;
            image.UrlForMediaStorage = xMLNode.XMLReadSubnode("largeimage").XMLReadInnerText(string.Empty);
            image.ImageType = Configuration.ImageType.EpisodeCover;

            if (image.UrlForMediaStorage != string.Empty)
            {
                image.Media.Images.Add(image);
            }
        }

        /// <inheritdoc/>
        public override void WriteImagesToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            // write to NFO-File
            for (int i = 0; i < this.Images.Count; i++)
            {
                ImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != string.Empty && imageFile.ImageType == Configuration.ImageType.EpisodeCover)
                {
                    if (!imageFile.UrlForMediaStorage.Contains("http://"))
                    {
                        nfoStreamWriter.WriteLine("    <thumb>smb://" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][this.Server.ElementAt(0).ToString()] + "/XBMC/Serien/" + this.Series.Filename + "/Season " + imageFile.Season + "/" + imageFile.Filename + "</thumb>");
                    }
                    else
                    {
                        nfoStreamWriter.WriteLine("    <thumb>" + imageFile.UrlForMediaStorage + "</thumb>");
                    }
                }
            }

            // write to BatchFile
            for (int i = 0; i < this.Images.Count; i++)
            {
                ImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != string.Empty && !imageFile.UrlForMediaStorage.Contains("http://") && imageFile.ImageType != Configuration.ImageType.Unknown)
                {
                    bfStreamWriter.WriteLine("/bin/cp \"" + imageFile.UrlForMediaStorageLocalFilesystem + "\" \"" + imageFile.Filename + "\"");
                }
            }
        }

        /// <inheritdoc/>
        public override string OverrideVideoStreamData(string title)
        {
            string returnTitle = base.OverrideVideoStreamData(title);

            // inherit Series data
            if (this.MediaLanguages.Count == 0)
            {
                this.MediaLanguages = this.Series.MediaLanguages;
            }

            this.CheckForDefaultMediaLanguages();

            return returnTitle;
        }

        /// <inheritdoc/>
        public override void AddServer(int serverList)
        {
            base.AddServer(serverList);
            this.Series.AddServer(serverList);
        }

        /// <summary>
        /// Adds SeriesActor to Episode
        /// </summary>
        /// <param name="seriesActors">List of Persons, to be added as actors</param>
        public override void AddActor(List<Actor> seriesActors)
        {
            foreach (SeriesActor seriesActor in seriesActors)
            {
                if (seriesActor.Seasons.Contains(this.DisplaySeason) || (seriesActor.Seasons.Count > 0 && seriesActor.Seasons[0] == string.Empty))
                {
                    this.AddActor(seriesActor);
                }
            }
        }

        /// <inheritdoc/>
        public override Actor ActorFactory(Configuration configuration)
        {
            return new SeriesActor(configuration);
        }

        #endregion
    }
}
