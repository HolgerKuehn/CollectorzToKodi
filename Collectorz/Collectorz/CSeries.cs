using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    public class CSeries : CVideo
    {
        #region Attributes
        private List<CEpisode> episodes;
        private int numberOfTotaLEpisodes;
        private int numberOfEpisodes;
        private int numberOfSpecials;
        private List<int> numberOfEpisodesPerSeason;
        #endregion
        #region Constructor
        public CSeries(CConfiguration configuration)
            : base(configuration)
        {
            episodes = new List<CEpisode>();
            numberOfTotaLEpisodes = 0;
            numberOfEpisodes = 0;
            numberOfSpecials = 0;
            numberOfEpisodesPerSeason = new List<int>();
            numberOfEpisodesPerSeason.Add(0); // Specials
            numberOfEpisodesPerSeason.Add(0); // Season 1
        }
        #endregion
        #region Properties
        public List<CEpisode> Episodes
        {
            get { return this.episodes; }
            set { this.episodes = value; }
        }
        public int NumberOfTotalEpisodes
        {
            get { return this.numberOfTotaLEpisodes; }
            set { this.numberOfTotaLEpisodes = value; }
        }
        public int NumberOfEpisodes
        {
            get { return this.numberOfEpisodes; }
            set { this.numberOfEpisodes = value; }
        }
        public int NumberOfSpecials
        {
            get { return this.numberOfSpecials; }
            set { this.numberOfSpecials = value; }
        }
        public List<int> NumberOfEpisodesPerSeason
        {
            get { return this.numberOfEpisodesPerSeason; }
            set { this.numberOfEpisodesPerSeason = value; }
        }
        #endregion
        #region Functions
        public override void readVideoFiles(XmlNode XMLMedia)
        {
            throw new NotImplementedException();
        }
        public override void writeNFO()
        {
            using (StreamWriter swrNFO = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename + ".nfo", false, Encoding.UTF8, 512))
            {
                swrNFO.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                swrNFO.WriteLine("<tvshow>");
                swrNFO.WriteLine("    <title>" + this.Title + "</title>");
                swrNFO.WriteLine("    <showtitle>" + this.Title + "</showtitle>");
                swrNFO.WriteLine("    <rating>" + this.Rating + "</rating>");
                swrNFO.WriteLine("    <year>" + this.Year + "</year>");
                swrNFO.WriteLine("    <plot>" + this.Plot + "</plot>");
                swrNFO.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
                swrNFO.WriteLine("    <episode>" + this.NumberOfTotalEpisodes + "</episode>");
                swrNFO.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

                if (this.PlayDate != "")
                    swrNFO.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");

                swrNFO.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");
                swrNFO.WriteLine("    <aired>" + this.Airdate + "</aired>");
                swrNFO.WriteLine("    <premiered>" + this.Airdate + "</premiered>");
                swrNFO.WriteLine("    <set>" + this.Set + "</set>");
                swrNFO.WriteLine("    <id>" + this.IMDbId + "</id>");
                swrNFO.WriteLine("    <country>" + this.Country + "</country>");

                this.writeGenre(swrNFO);
                this.writeStudio(swrNFO);
                this.writeCrew(swrNFO);
                this.writeCast(swrNFO);
                this.writeImagesToNFO(swrNFO);

                swrNFO.WriteLine("</tvshow>");
            }

            foreach (CEpisode episode in this.Episodes)
                episode.writeNFO();
        }
        public override void writeSH(StreamWriter swrSH)
        {
            if (this.Title != "")
            {
                swrSH.WriteLine("if [ -d \"" + this.Filename + "\" ];");
                swrSH.WriteLine("then ");
                swrSH.WriteLine("    rm -r \"" + this.Filename + "\"");
                swrSH.WriteLine("fi;");
                swrSH.WriteLine("mkdir \"" + this.Filename + "\"");

                swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                for (int i = 0; i < this.numberOfEpisodesPerSeason.Count; i++)
                    swrSH.WriteLine("mkdir \"Season " + ("00" + i.ToString()).Substring(i.ToString().Length) + "\"");

                swrSH.WriteLine("mkdir \"extrafanart\"");
                swrSH.WriteLine("mkdir \"extrathumbs\"");

                swrSH.WriteLine("cd \"/share/XBMC/Serien/" + this.Filename + "\"");
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"tvshow.nfo\"");

                // Images
                this.writeImagesToSH(swrSH);
                this.writeSubTitleStreamDataToSH(swrSH);

                swrSH.WriteLine("cd /share/XBMC/Serien/");

                foreach (CEpisode episode in this.Episodes)
                    episode.writeSH(swrSH);
            }
        }
        public override CVideo clone()
        {
            CSeries seriesClone = new CSeries(this.Configuration);
            seriesClone.Title = this.Title;
            seriesClone.TitleSort = this.TitleSort;
            seriesClone.TitleOriginal = this.TitleOriginal;
            seriesClone.Set = this.Set;
            seriesClone.Rating = this.Rating;
            seriesClone.Year = this.Year;
            seriesClone.Airdate = this.Airdate;
            seriesClone.Plot = this.Plot;
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

            foreach (CVideoFile videoFile in this.VideoFiles)
                seriesClone.VideoFiles.Add((CVideoFile)videoFile.clone());

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
                CEpisode episodeClone = (CEpisode)episode.clone();
                episodeClone.Series = seriesClone;

                seriesClone.Episodes.Add(episodeClone);
            }

            seriesClone.NumberOfTotalEpisodes = this.NumberOfTotalEpisodes;
            seriesClone.NumberOfEpisodes = this.NumberOfEpisodes;
            seriesClone.NumberOfSpecials = this.NumberOfSpecials;
            seriesClone.NumberOfEpisodesPerSeason = this.NumberOfEpisodesPerSeason;

            return seriesClone;
        }
        public override CVideo clone(int server, bool isSpecial = false)
        {
            bool cloneSeries = false;
            foreach (int serverList in this.Server)
                if (serverList.Equals(server))
                    cloneSeries = true;

            CSeries seriesClone = null;

            if (cloneSeries)
            {
                seriesClone = new CSeries(this.Configuration);
                seriesClone.Title = this.Title;
                seriesClone.TitleSort = this.TitleSort;
                seriesClone.TitleOriginal = this.TitleOriginal;
                seriesClone.Set = this.Set;
                seriesClone.Rating = this.Rating;
                seriesClone.Year = this.Year;
                seriesClone.Airdate = this.Airdate;
                seriesClone.Plot = this.Plot;
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

                foreach (CVideoFile videoFile in this.VideoFiles)
                    seriesClone.VideoFiles.Add((CVideoFile)videoFile.clone());

                seriesClone.Filename = this.Filename;
                seriesClone.addServer(server);
                seriesClone.VideoCodec = this.VideoCodec;
                seriesClone.VideoDefinition = this.VideoDefinition;
                seriesClone.VideoAspectRatio = this.VideoAspectRatio;
                seriesClone.AudioStreams = this.AudioStreams;
                seriesClone.SubTitleStreams = this.SubTitleStreams;
                seriesClone.MediaLanguages = this.MediaLanguages;

                foreach (CEpisode episode in this.Episodes)
                {
                    bool cloneEpisode = false;
                    foreach (CVideoFile videoFile in episode.VideoFiles)
                    {
                        if (videoFile.URL != "" && videoFile.Server.Equals(server))
                            cloneEpisode = true;
                    }

                    if (cloneEpisode)
                    {
                        CEpisode episodeClone = (CEpisode)episode.clone();
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
