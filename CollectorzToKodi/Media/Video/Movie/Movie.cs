// <copyright file="Movie.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    /// <summary>
    /// Class managing movie data from MovieCollector
    /// </summary>
    public class Movie : Video
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Movie"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Movie(Configuration configuration)
            : base(configuration)
        {
            this.MediaPath.WindowsPathToDestination = this.MediaPath.WindowsPathToDestination + this.Configuration.ServerMovieDirectory + "\\";
        }

        #endregion
        #region Properties

        /// <inheritdoc/>
        public override string Title
        {
            get
            {
                return this.Title;
            }

            set
            {
                string setTitle = value;

                setTitle = this.VideoStreams[0].OverrideVideoStreamData(setTitle);
                this.CheckForDefaultMediaLanguages();

                this.Title = setTitle;
            }
        }

        #endregion Properties
        #region Functions

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            base.ReadFromXml(xMLMedia);

            int k = 0;

            // video files from Link-List
            foreach (XmlNode xMLVideodatei in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if (xMLVideodatei.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie" && !xMLVideodatei.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel."))
                {
                    VideoFile videoFile = new VideoFile(this.Configuration);
                    k++;

                    videoFile.Media = this;
                    videoFile.Description = xMLVideodatei.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                    videoFile.Description = videoFile.OverrideSpecial(this.OverrideVideoStreamData(videoFile.Description));
                    videoFile.FileIndex = k;
                    videoFile.MediaFilePath.WindowsPath = xMLVideodatei.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                    videoFile.MediaFilePath.WindowsPathForPublication = videoFile.MediaFilePath.WindowsPath;

                    this.MediaFiles.Add(videoFile);
                }
            }

            // video files from Discs and override MediaStreamData
            foreach (XmlNode xMLMovieDisc in xMLMedia.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
            {
                string discTitle = this.VideoStreams[0].OverrideVideoStreamData(xMLMovieDisc.XMLReadSubnode("title").XMLReadInnerText(string.Empty));

                foreach (XmlNode xMLEpisode in xMLMovieDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                {
                    VideoFile videoFile = new VideoFile(this.Configuration);
                    k++;

                    videoFile.Media = this;
                    videoFile.Description = xMLEpisode.XMLReadSubnode("title").XMLReadInnerText(string.Empty);
                    videoFile.OverrideSpecial(discTitle);
                    videoFile.OverrideSpecial(this.VideoStreams[0].OverrideVideoStreamData(videoFile.Description));
                    videoFile.MediaFilePath.WindowsPath = xMLEpisode.XMLReadSubnode("movielink").XMLReadInnerText(string.Empty);
                    videoFile.MediaFilePath.WindowsPathForPublication = videoFile.MediaFilePath.WindowsPath;
                    videoFile.FileIndex = k;

                    this.MediaFiles.Add(videoFile);
                }
            }

            // SubTitles
            foreach (VideoFile videoFile in this.MediaFiles)
            {
                videoFile.ReadFromXml(xMLMedia);
            }
    }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            if (this.Title != string.Empty)
            {
                // write Nfo-File
                nfoStreamWriter.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                nfoStreamWriter.WriteLine("<movie>");
                nfoStreamWriter.WriteLine("    <title>" + this.Title + "</title>");
                nfoStreamWriter.WriteLine("    <sorttitle>" + this.TitleSort + "</sorttitle>");
                nfoStreamWriter.WriteLine("    <originaltitle>" + this.TitleOriginal + "</originaltitle>");
                nfoStreamWriter.WriteLine("    <set>" + this.MediaGroup + "</set>");
                nfoStreamWriter.WriteLine("    <rating>" + this.Rating + "</rating>");
                nfoStreamWriter.WriteLine("    <year>" + this.PublishingYear + "</year>");
                nfoStreamWriter.WriteLine("    <plot>" + this.Content + "</plot>");
                nfoStreamWriter.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
                nfoStreamWriter.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

                if (this.PlayDate != string.Empty)
                {
                    nfoStreamWriter.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");
                }

                nfoStreamWriter.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");

                nfoStreamWriter.WriteLine("    <aired>" + this.PublishingDate + "</aired>");
                nfoStreamWriter.WriteLine("    <premiered>" + this.PublishingDate + "</premiered>");
                nfoStreamWriter.WriteLine("    <id>" + this.ID + "</id>");
                nfoStreamWriter.WriteLine("    <country>" + this.Country + "</country>");

                this.WriteGenreToLibrary();
                this.WriteStudioToLibrary();
                this.WriteCrewToLibrary();
                this.WriteCastToLibrary();
                this.WriteStreamDataToLibrary();
                this.WriteImagesToLibrary();

                nfoStreamWriter.WriteLine("</movie>");

                if (this.IMDbId != string.Empty)
                {
                    nfoStreamWriter.WriteLine("http://www.imdb.com/title/tt" + this.IMDbId + "/");
                }

                if (this.TMDbId != string.Empty)
                {
                    nfoStreamWriter.WriteLine("http://www.themoviedb.org/" + this.TMDbType + "/" + this.TMDbId + "/");
                }

                // write BatchFile
                bfStreamWriter.WriteLine("mkdir \"" + this.Server.Filename + "\"");

                bfStreamWriter.WriteLine("cd \"/share/XBMC/Filme/" + this.Server.Filename + "\"");
                bfStreamWriter.WriteLine("mkdir \"extrafanart\"");

                // swrSH.WriteLine("/bin/tr -d '\rï»¿' < \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Server.Filename + ".nfo\" > \"" + this.Server.Filename + ".nfo\"");
                bfStreamWriter.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Server.Filename + ".nfo\" \"" + this.Server.Filename + ".nfo\"");

                // Videofiles and Images
                this.WriteVideoFilesToLibrary();
                this.WriteImagesToLibrary();

                bfStreamWriter.WriteLine("cd /share/XBMC/Filme/");
            }
        }

        /// <inheritdoc/>
        public override Media Clone()
        {
            Movie movieClone = new Movie(this.Configuration);

            movieClone.ID = this.ID;
            movieClone.Title = this.Title;
            movieClone.TitleSort = this.TitleSort;
            movieClone.TitleOriginal = this.TitleOriginal;
            movieClone.MediaGroup = this.MediaGroup;
            movieClone.Rating = this.Rating;
            movieClone.PublishingYear = this.PublishingYear;
            movieClone.PublishingDate = this.PublishingDate;
            movieClone.Content = this.Content;
            movieClone.RunTime = this.RunTime;
            movieClone.Images = this.Images; // if required, still to be cloned
            movieClone.MPAA = this.MPAA;
            movieClone.PlayCount = this.PlayCount;
            movieClone.PlayDate = this.PlayDate;
            movieClone.IMDbId = this.IMDbId;
            movieClone.TMDbType = this.TMDbType;
            movieClone.TMDbId = this.TMDbId;
            movieClone.Country = this.Country;
            movieClone.Genres = this.Genres;
            movieClone.Directors = this.Directors;
            movieClone.Studios = this.Studios;
            movieClone.Writers = this.Writers;
            movieClone.Actors = this.Actors;

            foreach (VideoFile videoFile in this.MediaFiles)
            {
                movieClone.MediaFiles.Add((VideoFile)videoFile.Clone());
            }

            movieClone.MediaPath.Filename = this.MediaPath.Filename;
            movieClone.Server = this.Server;

            foreach (VideoStream videoStream in this.VideoStreams)
            {
                movieClone.VideoStreams.Add((VideoStream)videoStream.Clone());
            }

            movieClone.AudioStreams = this.AudioStreams;
            movieClone.SubTitles = this.SubTitles;
            movieClone.MediaLanguages = this.MediaLanguages;

            return movieClone;
        }

        /// <inheritdoc/>
        public override Video Clone(int server, bool isSpecial = false)
        {
            bool cloneMovie = false;
            bool hasSpecials = false;

            // check if Movie is on requested Server
            foreach (int serverList in this.Server)
            {
                if (serverList.Equals(server))
                {
                    cloneMovie = true;
                }
            }

            // check if Movie contains Specials, if isSpecial is requested
            if (isSpecial)
            {
                foreach (VideoFile videoFile in this.MediaFiles)
                {
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                    {
                        hasSpecials = true;
                    }
                }
            }

            Movie movieClone = null;

            if (cloneMovie && hasSpecials == isSpecial)
            {
                movieClone = new Movie(this.Configuration);
                movieClone.ID = this.ID;
                movieClone.Title = this.Title;
                movieClone.TitleSort = this.TitleSort;
                movieClone.TitleOriginal = this.TitleOriginal;
                movieClone.MediaGroup = this.MediaGroup;
                movieClone.Rating = this.Rating;
                movieClone.PublishingYear = this.PublishingYear;
                movieClone.PublishingDate = this.PublishingDate;
                movieClone.Content = this.Content;
                movieClone.RunTime = this.RunTime;
                movieClone.Images = this.Images; // if required, still to be cloned
                movieClone.MPAA = this.MPAA;
                movieClone.PlayCount = this.PlayCount;
                movieClone.PlayDate = this.PlayDate;
                movieClone.IMDbId = this.IMDbId;
                movieClone.TMDbType = this.TMDbType;
                movieClone.TMDbId = this.TMDbId;
                movieClone.Country = this.Country;
                movieClone.Genres = this.Genres;
                movieClone.Studios = this.Studios;
                movieClone.Directors = this.Directors;
                movieClone.Writers = this.Writers;
                movieClone.Actors = this.Actors;

                foreach (VideoFile videoFile in this.MediaFiles)
                {
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                    {
                        movieClone.MediaFiles.Add((VideoFile)videoFile.Clone());
                    }
                }

                movieClone.MediaPath = this.MediaPath.Clone();
                movieClone.AddServer(server);

                foreach (VideoStream videoStream in this.VideoStreams)
                {
                    movieClone.VideoStreams.Add((VideoStream)videoStream.Clone());
                }

                foreach (AudioStream audioStream in this.AudioStreams)
                {
                    movieClone.AudioStreams.Add((AudioStream)audioStream.Clone());
                }

                foreach (SubTitleStream subtTitleStream in this.SubTitles)
                {
                    movieClone.SubTitles.Add((SubTitleStream)subtTitleStream.Clone());
                }

                movieClone.MediaLanguages = this.MediaLanguages;
            }

            return movieClone;
        }

        /// <inheritdoc/>
        public override void AddServer(int serverList)
        {
            base.AddServer(serverList);
        }

        #endregion
    }
}
