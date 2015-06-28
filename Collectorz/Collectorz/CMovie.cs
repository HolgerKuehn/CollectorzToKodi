using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace Collectorz
{
    public class CMovie : CMedia
    {
        #region Constructor
        public CMovie(CConfiguration configuration)
            : base(configuration)
        {}
        #endregion
        #region Functions
        public override void readVideoFiles(XmlNode XMLMedia)
        {
            int k = 0;

            // videofiles from Link-List
            foreach (XmlNode XMLVideodatei in XMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if (XMLVideodatei.XMLReadSubnode("urltype").XMLReadInnerText("") == "Movie" && !XMLVideodatei.XMLReadSubnode("description").XMLReadInnerText("").Contains("Untertitel."))
                {
                    CVideoFile videoFile = new CVideoFile(this.Configuration);
                    k++;

                    videoFile.Description = XMLVideodatei.XMLReadSubnode("description").XMLReadInnerText("");
                    videoFile.Description = videoFile.overrideSpecial(this.overrideMediaStreamData(videoFile.Description));
                    videoFile.Filename = this.Filename + " part " + ("0000" + k.ToString()).Substring(k.ToString().Length);
                    videoFile.URL = XMLVideodatei.XMLReadSubnode("url").XMLReadInnerText("");
                    videoFile.Media = this;
                    videoFile.convertFilename();

                    this.VideoFiles.Add(videoFile);
                }
            }


            // Video-Files from Discs and override MediaStreamData
            foreach (XmlNode XMLMovieDisc in XMLMedia.XMLReadSubnode("discs").XMLReadSubnodes("disc"))
            {
                string discTitle = this.overrideMediaStreamData(XMLMovieDisc.XMLReadSubnode("title").XMLReadInnerText(""));

                foreach (XmlNode XMLEpisode in XMLMovieDisc.XMLReadSubnode("episodes").XMLReadSubnodes("episode"))
                {
                    CVideoFile videoFile = new CVideoFile(this.Configuration);
                    k++;

                    videoFile.Description = XMLEpisode.XMLReadSubnode("title").XMLReadInnerText("");
                    videoFile.overrideSpecial(discTitle);
                    videoFile.overrideSpecial(this.overrideMediaStreamData(videoFile.Description));
                    videoFile.URL = XMLEpisode.XMLReadSubnode("movielink").XMLReadInnerText("");
                    videoFile.Filename = this.Filename + " part " + ("0000" + k.ToString()).Substring(k.ToString().Length);
                    videoFile.Media = this;
                    videoFile.convertFilename();

                    this.VideoFiles.Add(videoFile);
                }
            }
        }
        public override void writeNFO()
        {
            using (StreamWriter swrNFO = new StreamWriter(this.Configuration.MovieCollectorLocalPathToXMLExportPath + this.Filename + ".nfo", false, Encoding.UTF8, 512))
            {
                swrNFO.WriteLine("<?xml version=\"1.0\" encoding=\"UTF-8\" standalone=\"yes\" ?>");
                swrNFO.WriteLine("<movie>");
                swrNFO.WriteLine("    <title>" + this.Title + "</title>");
                swrNFO.WriteLine("    <sorttitle>" + this.TitleSort + "</sorttitle>");
                swrNFO.WriteLine("    <originaltitle>" + this.TitleOriginal + "</originaltitle>");
                swrNFO.WriteLine("    <set>" + this.Set + "</set>");
                swrNFO.WriteLine("    <rating>" + this.Rating + "</rating>");
                swrNFO.WriteLine("    <year>" + this.Year + "</year>");
                swrNFO.WriteLine("    <plot>" + this.Plot + "</plot>");
                swrNFO.WriteLine("    <runtime>" + this.RunTime + "</runtime>");
                swrNFO.WriteLine("    <mpaa>" + this.MPAA + "</mpaa>");

                if (this.PlayDate != "")
                    swrNFO.WriteLine("    <playdate>" + this.PlayDate + "</playdate>");

                swrNFO.WriteLine("    <playcount>" + this.PlayCount + "</playcount>");

                swrNFO.WriteLine("    <aired>" + this.Airdate + "</aired>");
                swrNFO.WriteLine("    <premiered>" + this.Airdate + "</premiered>");
                swrNFO.WriteLine("    <id>" + this.IMDbId + "</id>");
                swrNFO.WriteLine("    <country>" + this.Country + "</country>");

                this.writeGenre(swrNFO);
                this.writeStudio(swrNFO);
                this.writeCrew(swrNFO);
                this.writeCast(swrNFO);
                this.writeStreamData(swrNFO);
                this.writeImagesToNFO(swrNFO);

                swrNFO.WriteLine("</movie>");
            }
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

                swrSH.WriteLine("cd \"/share/XBMC/Filme/" + this.Filename + "\"");
                swrSH.WriteLine("mkdir \"extrafanart\"");

                // swrSH.WriteLine("/bin/tr -d '\rï»¿' < \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" > \"" + this.Filename + ".nfo\"");
                swrSH.WriteLine("/bin/cp \"/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/" + this.Filename + ".nfo\" \"" + this.Filename + ".nfo\"");

                // Videodateien
                for (int i = 0; i < this.VideoFiles.Count; i++)
                {
                    CVideoFile videoFile = this.VideoFiles.ElementAt(i);

                    swrSH.WriteLine("/bin/ln -s \"" + videoFile.URLLocalFilesystem + "\" \"" + videoFile.Filename + "\"");
                }

                // SubTitles
                for (int i = 0; i < this.SubTitleStreams.Count; i++)
                {
                    CSubTitleFile subTitleFile = this.SubTitleStreams.ElementAt(i);
                    if (subTitleFile.GetType().ToString().Contains("CSrtSubTitleFile"))
                        ((CSrtSubTitleFile)subTitleFile).writeSubTitleStreamDataToSH(swrSH);
                    else
                        subTitleFile.writeSubTitleStreamDataToSH(swrSH);
                }

                // Images
                this.writeImagesToSH(swrSH);

                swrSH.WriteLine("cd /share/XBMC/Filme/");
            }
        }
        public override CMedia clone()
        {
            CMovie movieClone = new CMovie(this.Configuration);
            movieClone.Title = this.Title;
            movieClone.TitleSort = this.TitleSort;
            movieClone.TitleOriginal = this.TitleOriginal;
            movieClone.Set = this.Set;
            movieClone.Rating = this.Rating;
            movieClone.Year = this.Year;
            movieClone.Airdate = this.Airdate;
            movieClone.Plot = this.Plot;
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

            foreach (CVideoFile videoFile in this.VideoFiles)
                movieClone.VideoFiles.Add((CVideoFile)videoFile.clone());

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
        public override CMedia clone(int server, bool isSpecial = false)
        {
            bool cloneMovie = false;
            bool hasSpecials = false;

            // check if Movie is on requested Server
            foreach (int serverList in this.Server)
                if (serverList.Equals(server))
                    cloneMovie = true;

            // check if Movie contains Specials, if isSpecial is requested
            if (isSpecial)
            {
                foreach (CVideoFile videoFile in this.VideoFiles)
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                        hasSpecials = true;
            }

            CMovie movieClone = null;

            if (cloneMovie && hasSpecials == isSpecial)
            {
                movieClone = new CMovie(this.Configuration);
                movieClone.Title = this.Title;
                movieClone.TitleSort = this.TitleSort;
                movieClone.TitleOriginal = this.TitleOriginal;
                movieClone.Set = this.Set;
                movieClone.Rating = this.Rating;
                movieClone.Year = this.Year;
                movieClone.Airdate = this.Airdate;
                movieClone.Plot = this.Plot;
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

                foreach (CVideoFile videoFile in this.VideoFiles)
                    if (videoFile.Server.Equals(server) && videoFile.IsSpecial == isSpecial)
                        movieClone.VideoFiles.Add((CVideoFile)videoFile.clone());

                movieClone.Filename = this.Filename;
                movieClone.addServer(server);
                movieClone.VideoCodec = this.VideoCodec;
                movieClone.VideoDefinition = this.VideoDefinition;
                movieClone.VideoAspectRatio = this.VideoAspectRatio;
                movieClone.AudioStreams = this.AudioStreams;
                movieClone.SubTitleStreams = this.SubTitleStreams;
                movieClone.MediaLanguages = this.MediaLanguages;
            }

            return movieClone;
        }
        #endregion
    }
}
