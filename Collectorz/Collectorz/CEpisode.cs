using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    public class CEpisode : CVideo
    {
        #region Attributes
        private string season;
        private string episode;
        private string displaySeason;
        private string displayEpisode;
        private bool isSpecial;
        private CSeries series;
        #endregion
        #region Constructor
        public CEpisode(CConfiguration configuration)
            : base(configuration)
        {
            this.season = "";
            this.episode = "";
            this.displaySeason = "";
            this.displayEpisode = "";
            this.series = null;
            this.isSpecial = false;
        }
        #endregion
        #region Properties
        public string Season
        {
            get { return this.season; }
            set { this.season = value; }
        }
        public string DisplaySeason
        {
            get { return this.displaySeason; }
            set { this.displaySeason = value; }
        }
        public string Episode
        {
            get { return this.episode; }
            set { this.episode = value; }
        }
        public string DisplayEpisode
        {
            get { return this.displayEpisode; }
            set { this.displayEpisode = value; }
        }
        public CSeries Series
        {
            get { return this.series; }
            set { this.series = value; }
        }
        public bool IsSpecial
        {
            get { return this.isSpecial; }
            set { this.isSpecial = value; }
        }
        #endregion
        #region Functions
        public override void readVideoFiles(XmlNode XMLMedia)
        {
            CVideoFile videoFile = new CVideoFile(this.Configuration);

            videoFile.IsSpecial = this.IsSpecial;
            this.Filename = this.Series.Filename + " S" + ("0000" + this.Season).Substring(this.Season.Length) + " E" + ("0000" + this.Episode.ToString()).Substring(this.Episode.ToString().Length);
            videoFile.Filename = this.Filename;
            videoFile.Description = "EpisodeVideoFile";
            videoFile.URL = XMLMedia.XMLReadSubnode("movielink").XMLReadInnerText("");
            videoFile.Media = this;
            videoFile.convertFilename();

            this.VideoFiles.Add(videoFile);
        }
        public override void writeNFO()
        {
            using (StreamWriter swrNFO = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath +  this.Filename + ".nfo", false, Encoding.UTF8, 512))
            {
                swrNFO.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                swrNFO.WriteLine("<episodedetails>");
                swrNFO.WriteLine("    <title>" + this.Title + "</title>");
                swrNFO.WriteLine("    <season>" + this.Season + "</season>");
                swrNFO.WriteLine("    <displayseason>" + this.DisplaySeason + "</displayseason>");
                swrNFO.WriteLine("    <episode>" + this.Episode + "</episode>");
                swrNFO.WriteLine("    <displayepisode>" + this.DisplayEpisode + "</displayepisode>");
                swrNFO.WriteLine("    <aired>" + this.Airdate + "</aired>");
                swrNFO.WriteLine("    <premiered>" + this.Airdate + "</premiered>");
                swrNFO.WriteLine("    <rating>" + this.Rating + "</rating>");
                swrNFO.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");
                swrNFO.WriteLine("    <plot>" + this.Plot + "</plot>");
                swrNFO.WriteLine("    <runtime>" + this.RunTime + "</runtime>");

                if (this.PlayDate != "")
                    swrNFO.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");

                swrNFO.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");

                swrNFO.WriteLine("    <set>" + this.Set + "</set>");

                this.writeGenre(swrNFO);
                this.writeStudio(swrNFO);
                this.writeCrew(swrNFO);
                this.writeCast(swrNFO);
                this.writeStreamData(swrNFO);
                this.writeImagesToNFO(swrNFO);

                swrNFO.WriteLine("</episodedetails>");
            }
        }
        public override void writeSH(StreamWriter swrSH)
        {
            if (this.Title != "")
            {
                swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Series.Filename + "/Season " + ("00" + this.Season).Substring(this.Season.Length) + "\"");
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"" + this.Filename + ".nfo\"");

                // Videodateien
                for (int i = 0; i < this.VideoFiles.Count; i++)
                {
                    CVideoFile videoFile = this.VideoFiles.ElementAt(i);

                    if (videoFile.Filename != "")
                        swrSH.WriteLine("/bin/ln -s \"" + videoFile.URLLocalFilesystem + "\" \"" + videoFile.Filename + "\"");
                }

                // SubTitles
                for (int i = 0; i < this.SubTitleStreams.Count; i++)
                {
                    CSubTitleFile subTitleFile = this.SubTitleStreams.ElementAt(i);

                    if (subTitleFile.Filename != "")
                        swrSH.WriteLine("/bin/ln -s \"" + subTitleFile.URLLocalFilesystem + "\" \"" + subTitleFile.Filename + "\"");
                }

                this.writeImagesToSH(swrSH);
                this.writeSubTitleStreamDataToSH(swrSH);

                swrSH.WriteLine("cd /share/XBMC/Serien/");
            }
        }
        public override CVideo clone()
        {
            CEpisode episodeClone = new CEpisode(this.Configuration);
            episodeClone.Title = this.Title;
            episodeClone.TitleSort = this.TitleSort;
            episodeClone.TitleOriginal = this.TitleOriginal;
            episodeClone.Set = this.Set;
            episodeClone.Rating = this.Rating;
            episodeClone.Year = this.Year;
            episodeClone.Airdate = this.Airdate;
            episodeClone.Plot = this.Plot;
            episodeClone.RunTime = this.RunTime;
            episodeClone.Images = this.Images; // if required, still to be cloned
            episodeClone.MPAA = this.MPAA;
            episodeClone.PlayCount = this.PlayCount;
            episodeClone.PlayDate = this.PlayDate;
            episodeClone.IMDbId = this.IMDbId;
            episodeClone.Country = this.Country;
            episodeClone.Genres = this.Genres;
            episodeClone.Studios = this.Studios;
            episodeClone.Directors = this.Directors;
            episodeClone.Writers = this.Writers;
            episodeClone.Actors = this.Actors;

            foreach (CVideoFile videoFile in this.VideoFiles)
                episodeClone.VideoFiles.Add((CVideoFile)videoFile.clone());

            episodeClone.Filename = this.Filename;
            episodeClone.Server = this.Server;
            episodeClone.VideoCodec = this.VideoCodec;
            episodeClone.VideoDefinition = this.VideoDefinition;
            episodeClone.VideoAspectRatio = this.VideoAspectRatio;
            episodeClone.AudioStreams = this.AudioStreams;
            episodeClone.SubTitleStreams = this.SubTitleStreams;
            episodeClone.MediaLanguages = this.MediaLanguages;

            episodeClone.Season = this.Season;
            episodeClone.Episode = this.Episode;
            episodeClone.DisplaySeason = this.DisplaySeason;
            episodeClone.DisplayEpisode = this.DisplayEpisode;
            episodeClone.IsSpecial = this.IsSpecial;
            episodeClone.Series = this.Series;

            return (CVideo)episodeClone;
        }
        public override CVideo clone(int server, bool isSpecial = false)
        {
            throw new NotImplementedException();
        }
        public void extractSeriesData(CSeries series)
        {
            // clone only relevant Attributes
            this.MPAA = series.MPAA;
            this.VideoDefinition = series.VideoDefinition;
            this.VideoAspectRatio = series.VideoAspectRatio;
            this.Rating = series.Rating;
        }
        public void extractSeriesData(CEpisode episode)
        {
            // clone only relevant Attributes
            this.MPAA = episode.MPAA;
            this.VideoCodec = episode.VideoCodec;
            this.VideoDefinition = episode.VideoDefinition;
            this.VideoAspectRatio = episode.VideoAspectRatio;
            this.Rating = episode.Rating;

            this.IsSpecial = episode.IsSpecial;
            this.Season = episode.Season;
            this.DisplaySeason = episode.DisplaySeason;
            this.MediaLanguages = episode.MediaLanguages;
        }
        public string overrideSeason(string title, bool countEpisode = false)
        {
            this.Season = "0";

            if (this.DisplaySeason == "")
                this.DisplaySeason = "1";

            if (title.Contains("(Special)"))
                this.isSpecial = true;

            title = title.Replace("(Special)", "");

            if (title.Contains("(S"))
                this.DisplaySeason = title.RightOf("(S").LeftOf(")");

            title = title.Replace("(S" + this.DisplaySeason + ")", "");

            if (!this.IsSpecial)
                this.Season = this.DisplaySeason;

            if (countEpisode)
            {
                // neue Season einrichten, falls benötigt
                while (this.Series.NumberOfEpisodesPerSeason.Count - 1 < (int)Int32.Parse(this.Season))
                    this.Series.NumberOfEpisodesPerSeason.Add(0);


                this.Series.NumberOfEpisodes = this.Series.NumberOfEpisodes + (this.IsSpecial ? 0 : 1);
                this.Series.NumberOfSpecials = this.Series.NumberOfSpecials + (this.IsSpecial ? 1 : 0);

                this.Series.NumberOfEpisodesPerSeason[(int)Int32.Parse(this.Season)]++;

                this.Episode = (string)(this.Series.NumberOfEpisodesPerSeason.ElementAt((int)Int32.Parse(this.Season))).ToString();
                this.DisplayEpisode = (string)(this.Series.NumberOfSpecials + this.Series.NumberOfEpisodes).ToString();
            }

            return title.Trim();
        }
        public override void readImages(XmlNode XMLNode)
        {
            CImageFile image;

            // Image
            image = new CImageFile(this.Configuration);
            image.Media = this;
            image.Season = ((CEpisode)image.Media).Season;
            image.Filename = image.Media.Filename;
            image.URL = XMLNode.XMLReadSubnode("largeimage").XMLReadInnerText("");
            image.convertFilename();
            image.ImageType = CConfiguration.ImageType.EpisodeCover;

            if (image.URL != "")
                image.Media.Images.Add(image);

        }
        public override void writeImagesToNFO(StreamWriter swrNFO)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != "" && imageFile.ImageType == CConfiguration.ImageType.EpisodeCover)
                    if (!imageFile.URL.Contains("http://"))
                        swrNFO.WriteLine("    <thumb>smb://" + imageFile.Media.Server.ElementAt(0) + "/XBMC/" + (imageFile.Media.GetType().ToString().Contains("CMovie") ? "Filme" : "Serien") + "/" + ((CEpisode)imageFile.Media).Series.Filename + "/Season " + ("00" + imageFile.Season).Substring(imageFile.Season.Length) + "/" + imageFile.Filename + "</thumb>");
                    else
                        swrNFO.WriteLine("    <thumb>" + imageFile.URL + "</thumb>");
            }
        }
        public override void writeImagesToSH(StreamWriter swrSH)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != "" && !imageFile.URL.Contains("http://") && imageFile.ImageType != CConfiguration.ImageType.unknown)
                    swrSH.WriteLine("/bin/cp \"" + imageFile.URLLocalFilesystem + "\" \"" + imageFile.Filename + "\"");
            }
        }
        #endregion
    }
}
