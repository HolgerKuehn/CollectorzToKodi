// <copyright file="CMovie.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Class managing movie data from MovieCollector
    /// </summary>
    public class CMovie : CVideo
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMovie"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CMovie(CConfiguration configuration)
            : base(configuration)
        {
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override void ReadMediaFiles(XmlNode xMLMedia)
        {
            int k = 0;

            // video files from Link-List
            foreach (XmlNode xMLVideodatei in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if (xMLVideodatei.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Movie" && !xMLVideodatei.XMLReadSubnode("description").XMLReadInnerText(string.Empty).Contains("Untertitel."))
                {
                    CVideoFile videoFile = new CVideoFile(this.Configuration);
                    k++;

                    videoFile.Description = xMLVideodatei.XMLReadSubnode("description").XMLReadInnerText(string.Empty);
                    videoFile.Description = videoFile.OverrideSpecial(this.OverrideVideoStreamData(videoFile.Description));
                    videoFile.Filename = this.Filename + " part " + ("0000" + k.ToString()).Substring(k.ToString().Length);
                    videoFile.URL = xMLVideodatei.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                    videoFile.Media = this;
                    videoFile.ConvertFilename();

                    this.MediaFiles.Add(videoFile);
                }
            }

            // video files from Discs and override MediaStreamData
            foreach (XmlNode xMLMovieDisc in xMLMedia.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
            {
                string discTitle = this.OverrideVideoStreamData(xMLMovieDisc.XMLReadSubnode("title").XMLReadInnerText(string.Empty));

                foreach (XmlNode xMLEpisode in xMLMovieDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                {
                    CVideoFile videoFile = new CVideoFile(this.Configuration);
                    k++;

                    videoFile.Description = xMLEpisode.XMLReadSubnode("title").XMLReadInnerText(string.Empty);
                    videoFile.OverrideSpecial(discTitle);
                    videoFile.OverrideSpecial(this.OverrideVideoStreamData(videoFile.Description));
                    videoFile.URL = xMLEpisode.XMLReadSubnode("movielink").XMLReadInnerText(string.Empty);
                    videoFile.Filename = this.Filename + " part " + ("0000" + k.ToString()).Substring(k.ToString().Length);
                    videoFile.Media = this;
                    videoFile.ConvertFilename();

                    this.MediaFiles.Add(videoFile);
                }
            }
        }

        /// <inheritdoc/>
        public override void WriteNFO()
        {
            using (StreamWriter swrNFO = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename + ".nfo", false, Encoding.UTF8, 512))
            {
                swrNFO.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                swrNFO.WriteLine("<movie>");
                swrNFO.WriteLine("    <title>" + this.Title + "</title>");
                swrNFO.WriteLine("    <sorttitle>" + this.TitleSort + "</sorttitle>");
                swrNFO.WriteLine("    <originaltitle>" + this.TitleOriginal + "</originaltitle>");
                swrNFO.WriteLine("    <set>" + this.MediaGroup + "</set>");
                swrNFO.WriteLine("    <rating>" + this.Rating + "</rating>");
                swrNFO.WriteLine("    <year>" + this.PublishingYear + "</year>");
                swrNFO.WriteLine("    <plot>" + this.Content + "</plot>");
                swrNFO.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
                swrNFO.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

                if (this.PlayDate != string.Empty)
                {
                    swrNFO.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");
                }

                swrNFO.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");

                swrNFO.WriteLine("    <aired>" + this.PublishingDate + "</aired>");
                swrNFO.WriteLine("    <premiered>" + this.PublishingDate + "</premiered>");
                swrNFO.WriteLine("    <id>" + this.IMDbId + "</id>");
                swrNFO.WriteLine("    <country>" + this.Country + "</country>");

                this.WriteGenre(swrNFO);
                this.WriteStudio(swrNFO);
                this.WriteCrew(swrNFO);
                this.WriteCast(swrNFO);
                this.WriteStreamData(swrNFO);
                this.WriteImagesToNFO(swrNFO);

                swrNFO.WriteLine("</movie>");
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

                swrSH.WriteLine("cd \"/share/XBMC/Filme/" + this.Filename + "\"");
                swrSH.WriteLine("mkdir \"extrafanart\"");

                // swrSH.WriteLine("/bin/tr -d '\rï»¿' < \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" > \"" + this.Filename + ".nfo\"");
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"" + this.Filename + ".nfo\"");

                // Videodateien
                for (int i = 0; i < this.MediaFiles.Count; i++)
                {
                    CVideoFile videoFile = (CVideoFile)this.MediaFiles.ElementAt(i);

                    swrSH.WriteLine("/bin/ln -s \"" + videoFile.URLLocalFilesystem + "\" \"" + videoFile.Filename + "\"");
                }

                // SubTitles
                for (int i = 0; i < this.SubTitleStreams.Count; i++)
                {
                    CSubTitleFile subTitleFile = this.SubTitleStreams.ElementAt(i);
                    if (subTitleFile.GetType().ToString().Contains("CSrtSubTitleFileCollection"))
                    {
                        ((CSrtSubTitleFileCollection)subTitleFile).WriteSubTitleStreamDataToSH(swrSH);
                    }
                    else
                    {
                        subTitleFile.WriteSubTitleStreamDataToSH(swrSH);
                    }
                }

                // Images
                this.WriteImagesToSH(swrSH);

                swrSH.WriteLine("cd /share/XBMC/Filme/");
            }
        }

        /// <inheritdoc/>
        public override CMedia Clone()
        {
            CMovie movieClone = new CMovie(this.Configuration);
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
            movieClone.Country = this.Country;
            movieClone.Genres = this.Genres;
            movieClone.Directors = this.Directors;
            movieClone.Studios = this.Studios;
            movieClone.Writers = this.Writers;
            movieClone.Actors = this.Actors;

            foreach (CVideoFile videoFile in this.MediaFiles)
            {
                movieClone.MediaFiles.Add((CVideoFile)videoFile.Clone());
            }

            movieClone.Filename = this.Filename;
            movieClone.Server = this.Server;
            movieClone.VideoCodec = this.VideoCodec;
            movieClone.VideoDefinition = this.VideoDefinition;
            movieClone.VideoAspectRatio = this.VideoAspectRatio;
            movieClone.AudioStreams = this.AudioStreams;
            movieClone.SubTitleStreams = this.SubTitleStreams;
            movieClone.MediaLanguages = this.MediaLanguages;

            return movieClone;
        }

        /// <inheritdoc/>
        public override CVideo Clone(int server, bool isSpecial = false)
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
                foreach (CVideoFile videoFile in this.MediaFiles)
                {
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                    {
                        hasSpecials = true;
                    }
                }
            }

            CMovie movieClone = null;

            if (cloneMovie && hasSpecials == isSpecial)
            {
                movieClone = new CMovie(this.Configuration);
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
                movieClone.Country = this.Country;
                movieClone.Genres = this.Genres;
                movieClone.Studios = this.Studios;
                movieClone.Directors = this.Directors;
                movieClone.Writers = this.Writers;
                movieClone.Actors = this.Actors;

                foreach (CVideoFile videoFile in this.MediaFiles)
                {
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                    {
                        movieClone.MediaFiles.Add((CVideoFile)videoFile.Clone());
                    }
                }

                movieClone.Filename = this.Filename;
                movieClone.AddServer(server);
                movieClone.VideoCodec = this.VideoCodec;
                movieClone.VideoDefinition = this.VideoDefinition;
                movieClone.VideoAspectRatio = this.VideoAspectRatio;
                movieClone.AudioStreams = this.AudioStreams;
                movieClone.SubTitleStreams = this.SubTitleStreams;
                movieClone.MediaLanguages = this.MediaLanguages;
            }

            return movieClone;
        }

        /// <inheritdoc/>
        public override string OverrideVideoStreamData(string title)
        {
            string returnTitle = base.OverrideVideoStreamData(title);
            this.CheckForDefaultMediaLanguages();

            return returnTitle;
        }

        /// <inheritdoc/>
        public override void AddServer(int serverList)
        {
            base.AddServer(serverList);
        }

        #endregion
    }
}
