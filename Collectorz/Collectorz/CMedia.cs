using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;

namespace Collectorz
{
    public abstract class CMedia
    {
        #region Attributes
        private string title;
        private string titleSort;
        private string titleOriginal;
        private string set;
        private string rating;
        private string year;
        private string airdate;
        private string plot;
        private string runTime;
        private List<CImageFile> images;
        private string mPAA;
        private string playCount;
        private string playDate;
        private string iMDbId;
        private string country;
        private List<string> genres;
        private List<string> studios;
        private List<CPerson> directors;
        private List<CPerson> writers;
        private List<CPerson> actors;
        private List<CVideoFile> videoFiles;
        private string filename;
        private List<CConstants.ServerList> server;
        private CConstants.VideoCodec videoCodec;
        private CConstants.VideoDefinition videoDefinition;
        private CConstants.VideoAspectRatio videoAspectRatio;
        private List<CAudioStream> audioStreams;
        private List<CSubTitleFile> subTitleStreams;
        private List<string> mediaLanguages;
        #endregion
        #region Constructor
        public CMedia()
        {
            title = "";
            titleSort = "";
            titleOriginal = "";
            set = "";
            rating = "";
            year = "";
            airdate = "";
            plot = "";
            runTime = "";
            images = new List<CImageFile>();
            mPAA = "";
            playCount = "0";
            playDate = "";
            iMDbId = "";
            country = "";
            genres = new List<string>();
            studios = new List<string>();
            directors = new List<CPerson>();
            writers = new List<CPerson>();
            actors = new List<CPerson>();
            videoFiles = new List<CVideoFile>();
            filename = "";
            videoCodec = CConstants.VideoCodec.TV;
            videoDefinition = CConstants.VideoDefinition.SD;
            videoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_16_9;
            audioStreams = new List<CAudioStream>();
            subTitleStreams = new List<CSubTitleFile>();
            mediaLanguages = new List<string>();
        }
        #endregion
        #region Properties
        public string Title
        {
            get { return title; }
            set { title = value; }
        }
        public string TitleSort
        {
            get { return titleSort; }
            set { titleSort = value; }
        }
        public string TitleOriginal
        {
            get { return titleOriginal; }
            set { titleOriginal = value; }
        }
        public string Set
        {
            get { return set; }
            set { set = value; }
        }
        public string Rating
        {
            get { return rating; }
            set { rating = value; }
        }
        public string Year
        {
            get { return year; }
            set { year = value; }
        }
        public string Airdate
        {
            get { return airdate; }
            set { airdate = value; }
        }
        public string Plot
        {
            get { return plot; }
            set { plot = value; }
        }
        public string RunTime
        {
            get { return runTime; }
            set { runTime = value; }
        }
        public List<CImageFile> Images
        {
            get { return images; }
            set { images = value; }
        }
        public string MPAA
        {
            get { return mPAA; }
            set { mPAA = value; }
        }
        public string PlayCount
        {
            get { return playCount; }
            set { playCount = value; }
        }
        public string PlayDate
        {
            get { return playDate; }
            set { playDate = value; }
        }
        public string IMDbId
        {
            get { return iMDbId; }
            set { iMDbId = value; }
        }
        public string Country
        {
            get { return country; }
            set { country = value; }
        }
        public List<String> Genres
        {
            get { return genres; }
            set { genres = value; }
        }
        public List<string> Studios
        {
            get { return studios; }
            set { studios = value; }
        }
        public List<CPerson> Directors
        {
            get { return directors; }
            set { directors = value; }
        }
        public List<CPerson> Writers
        {
            get { return writers; }
            set { writers = value; }
        }
        public List<CPerson> Actors
        {
            get { return actors; }
            set { actors = value; }
        }
        public List<CVideoFile> VideoFiles
        {
            get { return videoFiles; }
            set { videoFiles = value; }
        }
        public string Filename
        {
            get { return filename; }
            set { filename = value; }
        }
        public List<CConstants.ServerList> Server
        {
            get
            {
                if (server == null)
                    server = new List<CConstants.ServerList>();

                return server;
            }
            set { server = value; }
        }
        public CConstants.VideoCodec VideoCodec
        {
            get { return videoCodec; }
            set { videoCodec = value; }
        }
        public CConstants.VideoDefinition VideoDefinition
        {
            get { return videoDefinition; }
            set { videoDefinition = value; }
        }
        public CConstants.VideoAspectRatio VideoAspectRatio
        {
            get { return videoAspectRatio; }
            set { videoAspectRatio = value; }
        }
        public List<CAudioStream> AudioStreams
        {
            get
            {
                if (audioStreams == null)
                    audioStreams = new List<CAudioStream>();

                return audioStreams;
            }
            set { audioStreams = value; }
        }
        public List<CSubTitleFile> SubTitleStreams
        {
            get
            {
                if (subTitleStreams == null)
                    subTitleStreams = new List<CSubTitleFile>();

                return subTitleStreams;
            }
            set { subTitleStreams = value; }
        }
        public List<string> MediaLanguages
        {
            get { return mediaLanguages; }
            set { mediaLanguages = value; }
        }
        #endregion
        #region Functions
        public abstract void readVideoFiles(XmlNode XMLMedia);
        public abstract void writeNFO(string AusgabeNFO);
        public abstract void writeSH(StreamWriter swrSH);
        public abstract CMedia clone();
        public abstract CMedia clone(CConstants.ServerList server, bool isSpecial = false);
        public void clonePerLanguage(string isoCodeToBeReplaced, string isoCodeForReplacemant)
        {
            // check for target-language
            bool episodeContainsTargetLanguage = false;

            foreach (string mediaLanguage in this.MediaLanguages)
                if (mediaLanguage == isoCodeForReplacemant)
                    episodeContainsTargetLanguage = true;
                        
            // replace all associated languages
            this.MediaLanguages = null;

            this.Title = this.Title.ReplaceAll("(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
            this.Title = this.Title.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

            this.TitleSort = this.TitleSort.ReplaceAll("(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
            this.TitleSort = this.TitleSort.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

            this.Filename = this.Filename.ReplaceAll("(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
            this.Filename = this.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

            this.IMDbId = this.IMDbId.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

            foreach (CVideoFile videoFile in this.VideoFiles)
            {
                if (!episodeContainsTargetLanguage)
                    videoFile.Filename = "";

                videoFile.Filename = videoFile.Filename.ReplaceAll("(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConstants.covertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                videoFile.Filename = videoFile.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                videoFile.URL = videoFile.URL.ReplaceAll(isoCodeToBeReplaced + videoFile.Extention, isoCodeForReplacemant + videoFile.Extention);
                videoFile.URLLocalFilesystem = videoFile.URLLocalFilesystem.ReplaceAll(isoCodeToBeReplaced + videoFile.Extention, isoCodeForReplacemant + videoFile.Extention);
            }
        }
        public virtual void readImages(XmlNode XMLNode)
        {
            //Single Video File

            //The following example "videofilename.avi" will now use the thumbnail "videofilename-poster.(jpg/png)".

            //Movies\path\videofilename-poster.(jpg/png)
            //Movies\path\videofilename.avi

            //Note that videofilename.jpg is old Eden terminology, but WILL supersede videofilename-poster.(jpg/png) which is the standard for Frodo and later versions of XBMC.

            //Multi Part (Stacked) Video Files

            //This may not be Frodo terminology, but should still work You can either use the filename of the first file in the stack or the name of the stack, so for:

            //Movies\path\moviename-CD1.avi
            //Movies\path\moviename-CD2.avi

            //Either of the below would work:

            //Movies\path\moviename-CD1.jpg
            //Movies\path\moviename.jpg

            //TV Shows

            //TV\showfolder\poster.(jpg/png)
            //or
            //TV\showfolder\banner.jpg 

            //TV Show Season Thumbnails

            //As of Frodo, XBMC can use either posters OR banners as the Season thumbnail. In the following example, the thumbnail will be used for the appropriate season in the Video Library season node. Where xx is 01, 02 etc.

            //TV\showfolder\seasonxx-poster.(jpg/png)
            //or
            //TV\showfolder\seasonxx-banner.(jpg/png)

            //TV Show Specials

            //TV\showfolder\season-specials-poster.(jpg/png)
            //or
            //TV\showfolder\season-specials-banner.(jpg/png)

            //For the all seasons item

            //TV\showfolder\season-all-poster.(jpg/png)
            //or
            //TV\showfolder\season-all-banner.(jpg/png)

            //Fanart

            //Movies in Folders:

            //Movies\path\fanart.(jpg/png) 

            //TV Series:

            //TV\showfolder\fanart.(jpg/png)

            //TV Season:

            //TV\showfolder\seasonxx-fanart.(jpg/png)

            //TV Season All:

            //TV\showfolder\season-all-fanart.(jpg/png)

            //TV Specials:

            //TV\showfolder\season-specials-fanart.(jpg/png)

            //Extra Fanart Page

            //Movies\path\extrafanart\fanart1.jpg
            //Movies\path\VIDEO_TS\extrafanart\fanart1.jpg *Special case if using DVD Video structure
            //TV\showfolder\extrafanart\fanart1.jpg

            CImageFile image;

            // Cover-Front-Image
            image = new CImageFile();
            image.Media = this;
            // image.Filename = image.Media.Filename + "-coverfront";
            image.Filename = "cover";
            image.URL = XMLNode.XMLReadSubnode("coverfront").XMLReadInnerText("");
            image.convertFilename();
            image.ImageType = CConstants.ImageType.CoverFront;

            if (image.URL != "")
                image.Media.Images.Add(image);


            // Cover-Back-Image
            image = new CImageFile();
            image.Media = this;
            //image.Filename = image.Media.Filename + "-coverback";
            image.Filename = "coverback";
            image.URL = XMLNode.XMLReadSubnode("coverback").XMLReadInnerText("");
            image.convertFilename();
            image.ImageType = CConstants.ImageType.CoverBack;

            if (image.URL != "")
                image.Media.Images.Add(image);


            // Poster-Image
            image = new CImageFile();
            image.Media = this;
            //image.Filename = image.Media.Filename + "-poster";
            image.Filename = "poster";
            image.URL = XMLNode.XMLReadSubnode("poster").XMLReadInnerText("");
            image.convertFilename();
            image.ImageType = CConstants.ImageType.Poster;

            if (image.URL != "")
                image.Media.Images.Add(image);


            // Backdrop-Image
            image = new CImageFile();
            image.Media = this;
            //image.Filename = image.Media.Filename + "-fanart";
            image.Filename = "fanart";
            image.URL = XMLNode.XMLReadSubnode("backdropurl").XMLReadInnerText("");
            image.convertFilename();
            image.ImageType = CConstants.ImageType.Backdrop;

            if (image.URL != "")
                image.Media.Images.Add(image);


            // Covers / Backdrops per Season
            int numberOfExtraBackdrop = 0;
            int numberOfExtraCover = 0;
            foreach (XmlNode XMLImageFile in XMLNode.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if ((XMLImageFile.XMLReadSubnode("urltype").XMLReadInnerText("") == "Image"))
                {
                    CImageFile imageFile = new CImageFile();
                    imageFile.Media = this;

                    imageFile.Description = imageFile.overrideSeason(XMLImageFile.XMLReadSubnode("description").XMLReadInnerText(""));

                    // Fanart or Thumb ?
                    if ( // Backdrops
                         imageFile.Description.Contains("ExtraBackdrop") ||
                        // or Covers for TV-Shows as no Extrathumbs are supported
                        (imageFile.Description.Contains("ExtraCover") && imageFile.Media.GetType().ToString().Contains("CSeries")))
                    {
                        numberOfExtraBackdrop++;
                        imageFile.ImageType = CConstants.ImageType.ExtraBackdrop;
                        imageFile.Filename = "fanart" + ("0000" + numberOfExtraBackdrop.ToString()).Substring(numberOfExtraBackdrop.ToString().Length);
                    }
                    // Extra-Cover only for movies
                    else if (imageFile.Description.Contains("ExtraCover") && !imageFile.Media.GetType().ToString().Contains("CSeries"))
                    {
                        numberOfExtraCover++;
                        imageFile.ImageType = CConstants.ImageType.ExtraCover;
                        imageFile.Filename = "thumb" + ("0000" + numberOfExtraCover.ToString()).Substring(numberOfExtraCover.ToString().Length);
                    }
                    else if (imageFile.Description.Contains("Backdrop"))
                    {
                        imageFile.ImageType = CConstants.ImageType.SeasonBackdrop;
                        imageFile.Filename = "fanart";
                    }
                    else if (imageFile.Description.Contains("Cover"))
                    {
                        imageFile.ImageType = CConstants.ImageType.SeasonCover;
                        imageFile.Filename = "cover";
                    }

                    if (imageFile.Season == "all")
                    {
                        imageFile.Season = "-1";
                        imageFile.Filename = imageFile.Filename + "_all";
                    }
                    else if (imageFile.Season == "spe")
                        imageFile.Season = "0";

                    imageFile.URL = XMLImageFile.XMLReadSubnode("url").XMLReadInnerText("");
                    imageFile.convertFilename();

                    if ((imageFile.URL != ""))
                        imageFile.Media.Images.Add(imageFile);
                }
            }
        }
        private void writeImagesToNFO(StreamWriter swrNFO, CConstants.ImageType imageType)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != "" && imageFile.ImageType == imageType)
                {
                    if (imageFile.ImageType == CConstants.ImageType.CoverFront || imageFile.ImageType == CConstants.ImageType.Backdrop)
                    {
                        if (imageFile.ImageType == CConstants.ImageType.Backdrop)
                            swrNFO.Write("    ");

                        swrNFO.WriteLine("    <thumb>smb://" + imageFile.Media.Server.ElementAt(0) + "/XBMC/" + (imageFile.Media.GetType().ToString().Contains("CMovie") ? "Filme" : "Serien") + "/" + imageFile.Media.Filename + "/" + imageFile.Filename + "</thumb>");
                    }


                    if (imageFile.ImageType == CConstants.ImageType.SeasonCover || imageFile.ImageType == CConstants.ImageType.SeasonBackdrop)
                    {
                        if (imageFile.ImageType == CConstants.ImageType.SeasonBackdrop)
                            swrNFO.Write("    ");

                        swrNFO.WriteLine("    <thumb type=\"season\" season=\"" + imageFile.Season + "\">smb://" + imageFile.Media.Server.ElementAt(0) + "/XBMC/" + (imageFile.Media.GetType().ToString().Contains("CSeries") ? "Serien" : "Filme") + "/" + imageFile.Media.Filename + "/" + (imageFile.Season != "-1" ? "Season " + ("00" + imageFile.Season).Substring(imageFile.Season.Length) + "/" : "") + imageFile.Filename + "</thumb>");
                    }
                }
            }
        }
        public virtual void writeImagesToNFO(StreamWriter swrNFO)
        {
            // Cover-Thumb
            this.writeImagesToNFO(swrNFO, CConstants.ImageType.CoverFront);
            this.writeImagesToNFO(swrNFO, CConstants.ImageType.SeasonCover);

            // Fanart
            swrNFO.WriteLine("    <fanart>");
            this.writeImagesToNFO(swrNFO, CConstants.ImageType.Backdrop);
            this.writeImagesToNFO(swrNFO, CConstants.ImageType.SeasonBackdrop);
            swrNFO.WriteLine("    </fanart>");

            //        
            //<thumb type="season" season="1">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season01-poster.jpg</thumb>
            //<thumb type="season" season="2">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season02-poster.jpg</thumb>
            //<thumb type="season" season="3">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season03-poster.jpg</thumb>
            //<thumb type="season" season="4">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season04-poster.jpg</thumb>
            //<thumb type="season" season="5">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season05-poster.jpg</thumb>
            //<thumb type="season" season="6">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season06-poster.jpg</thumb>
            //<thumb type="season" season="7">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season07-poster.jpg</thumb>
            //<thumb type="season" season="8">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season08-poster.jpg</thumb>
            //<thumb type="season" season="9">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season09-poster.jpg</thumb>
            //<thumb type="season" season="10">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season10-poster.jpg</thumb>
            //<fanart>
            //<thumb>smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season01-fanart.jpg</thumb>
            //    <thumb type="season" season="1">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season01-fanart.jpg</thumb>
            //<thumb type="season" season="2">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season02-fanart.jpg</thumb>
            //<thumb type="season" season="3">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season03-fanart.jpg</thumb>
            //<thumb type="season" season="4">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season04-fanart.jpg</thumb>
            //<thumb type="season" season="5">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season05-fanart.jpg</thumb>
            //<thumb type="season" season="6">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season06-fanart.jpg</thumb>
            //<thumb type="season" season="7">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season07-fanart.jpg</thumb>
            //<thumb type="season" season="8">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season08-fanart.jpg</thumb>
            //<thumb type="season" season="9">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season09-fanart.jpg</thumb>
            //<thumb type="season" season="10">smb://JOUSETSUSOOCHI/XBMC/Serien/Smallville (2001)/season10-fanart.jpg</thumb>
            //</fanart>
        }
        public virtual void writeImagesToSH(StreamWriter swrSH)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != "" && !imageFile.URL.Contains("http://") && imageFile.ImageType != CConstants.ImageType.unknown)
                {
                    if (!imageFile.Media.GetType().ToString().Contains("CMovie") && imageFile.Season != "" && imageFile.Season != "-1" && imageFile.ImageType != CConstants.ImageType.ExtraBackdrop && imageFile.ImageType != CConstants.ImageType.ExtraCover)
                        swrSH.WriteLine("cd \"Season " + ("00" + imageFile.Season).Substring(imageFile.Season.Length) + "\"");

                    if (imageFile.ImageType == CConstants.ImageType.ExtraBackdrop)
                        swrSH.WriteLine("cd \"extrafanart\"");

                    if (imageFile.ImageType == CConstants.ImageType.ExtraCover)
                        swrSH.WriteLine("cd \"extrathumbs\"");

                    swrSH.WriteLine("/bin/cp \"" + imageFile.URLLocalFilesystem + "\" \"" + imageFile.Filename + "\"");

                    if ((!imageFile.Media.GetType().ToString().Contains("CMovie") && imageFile.Season != "" && imageFile.Season != "-1") || imageFile.ImageType == CConstants.ImageType.ExtraBackdrop || imageFile.ImageType == CConstants.ImageType.ExtraCover)
                        swrSH.WriteLine("cd ..");
                }
            }
        }
        public void readGenre(XmlNode XMLNode)
        {
            foreach (XmlNode XMLGenre in XMLNode.XMLReadSubnode("genres").XMLReadSubnodes("genre"))
                this.Genres.Add(XMLGenre.XMLReadSubnode("displayname").XMLReadInnerText(""));
        }
        public void writeGenre(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (string genre in this.Genres)
            {
                swrNFO.WriteLine("    <genre" + (i == 0 ? " clear=\"true\"" : "") + ">" + genre + "</genre>");
                i++;
            }
        }
        public void readStudio(XmlNode XMLNode)
        {
            foreach (XmlNode XMLStudio in XMLNode.XMLReadSubnode("studios").XMLReadSubnodes("studio"))
                this.Studios.Add(XMLStudio.XMLReadSubnode("displayname").XMLReadInnerText(""));
        }
        public void writeStudio(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (string studio in this.Studios)
            {
                swrNFO.WriteLine("    <studio" + (i == 0 ? " clear=\"true\"" : "") + ">" + studio + "</studio>");
                i++;
            }
        }
        public void readCrew(XmlNode XMLNode)
        {
            foreach (XmlNode XMLCrewmember in XMLNode.XMLReadSubnode("crew").XMLReadSubnodes("crewmember"))
            {
                bool isDirector = XMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText("") == "dfDirector";
                bool isWriter = XMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText("") == "dfWriter";

                if (isDirector || isWriter)
                {
                    CPerson person = new CPerson();

                    person.Name = XMLCrewmember.XMLReadSubnode("person").XMLReadSubnode("displayname").XMLReadInnerText("");
                    person.Role = XMLCrewmember.XMLReadSubnode("character").XMLReadInnerText("");
                    person.Thumb = XMLCrewmember.XMLReadSubnode("person").XMLReadSubnode("imageurl").XMLReadInnerText("");

                    if (isDirector)
                        this.Directors.Add(person);

                    if (isWriter)
                        this.Writers.Add(person);
                }
            }
        }
        public void writeCrew(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (CPerson director in this.Directors)
            {
                swrNFO.WriteLine("    <director" + (i == 0 ? " clear=\"true\"" : "") + ">" + director.Name + "</director>");
                i++;
            }

            i = 0;
            foreach (CPerson writer in this.Writers)
            {
                swrNFO.WriteLine("    <credits" + (i == 0 ? " clear=\"true\"" : "") + ">" + writer.Name + "</credits>");
                i++;
            }
        }
        public void readCast(XmlNode XMLNode)
        {
            foreach (XmlNode XMLCast in XMLNode.XMLReadSubnode("cast").XMLReadSubnodes("star"))
            {
                if (XMLCast.XMLReadSubnode("roleid").XMLReadInnerText("") == "dfActor")
                {
                    CPerson Cast = new CPerson();

                    Cast.Name = XMLCast.XMLReadSubnode("person").XMLReadSubnode("displayname").XMLReadInnerText("");
                    Cast.Role = XMLCast.XMLReadSubnode("character").XMLReadInnerText("");
                    Cast.Thumb = XMLCast.XMLReadSubnode("person").XMLReadSubnode("imageurl").XMLReadInnerText("");

                    this.Actors.Add(Cast);
                }
            }
        }
        public void writeCast(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (CPerson actor in this.Actors)
            {
                swrNFO.WriteLine("    <actor" + (i == 0 ? " clear=\"true\"" : "") + ">");
                swrNFO.WriteLine("        <name>" + actor.Name + "</name>");
                swrNFO.WriteLine("        <role>" + actor.Role + "</role>");

                if (actor.Thumb.StartsWith("http:"))
                    swrNFO.WriteLine("        <thumb>" + actor.Thumb + "</thumb>");

                swrNFO.WriteLine("    </actor>");

                i++;
            }
        }
        public void addServer(CConstants.ServerList serverList)
        {
            bool addServer = true;
            foreach (CConstants.ServerList currentServerList in this.Server)
                if (currentServerList.Equals(serverList))
                    addServer = false;

            if (addServer)
                this.Server.Add(serverList);
        }
        public void readStreamData(XmlNode XMLMedia)
        {
            this.readVideoStreamData(XMLMedia);
            this.readAudioStreamData(XMLMedia);
            this.readSubTitleStreamData(XMLMedia);
        }
        private void readVideoStreamData(XmlNode XMLMedia)
        {
            CConstants.VideoAspectRatio videoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_16_9;
            CConstants.VideoDefinition videoDefinition = CConstants.VideoDefinition.SD;

            // VideoAspectRatio
            List<XmlNode> XMLVideoAspectRatios = XMLMedia.XMLReadSubnode("ratios").XMLReadSubnodes("ratio");
            if (XMLVideoAspectRatios.Count > 0)
            {
                XmlNode XMLVideoAspectRatio = XMLVideoAspectRatios.ElementAt(0);

                if (XMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Fullscreen (4:3)"))
                    videoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_4_3;

                if (XMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Widescreen (16:9)"))
                    videoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_16_9;

                if (XMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Theatrical Widescreen (21:9)"))
                    videoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_21_9;
            }

            // VideoDefinition
            XmlNode XMLVideoDefinition = XMLMedia.XMLReadSubnode("condition");

            if (XMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("SD"))
                videoDefinition = CConstants.VideoDefinition.SD;

            if (XMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("HD"))
                videoDefinition = CConstants.VideoDefinition.HD;


            // Werte übertragen
            this.VideoDefinition = videoDefinition;
            this.VideoAspectRatio = videoAspectRatio;
        }
        private void readAudioStreamData(XmlNode XMLMedia)
        {
            foreach (XmlNode XMLAudio in XMLMedia.XMLReadSubnode("audios").XMLReadSubnodes("audio"))
            {
                string displayname = XMLAudio.XMLReadSubnode("displayname").XMLReadInnerText("");

                CAudioStream audioStreamData = new CAudioStream();
                audioStreamData.Codec = "AC3";
                audioStreamData.Language = displayname.RightOf("[").LeftOf("]");

                if (displayname.LeftOf("[").Contains("2.0") || displayname.LeftOf("[").Contains("Stereo"))
                    audioStreamData.NumberOfChannels = "2";

                if (displayname.LeftOf("[").Contains("5.1"))
                    audioStreamData.NumberOfChannels = "6";

                this.AudioStreams.Add(audioStreamData);
            }
        }
        private void readSubTitleStreamData(XmlNode XMLMedia)
        {
            foreach (XmlNode XMLSubTitle in XMLMedia.XMLReadSubnode("subtitles").XMLReadSubnodes("subtitle"))
            {
                CSubTitleFile subTitleStream = new CSubTitleFile();
                subTitleStream.Media = this;

                string displayname = XMLSubTitle.XMLReadSubnode("displayname").XMLReadInnerText("");
                subTitleStream.Language = displayname;

                if (displayname != "")
                {
                    subTitleStream.checkForSubTitleStreamFile(XMLMedia);
                    this.SubTitleStreams.Add(subTitleStream);
                }
            }
        }
        public string overrideMediaStreamData(string title)
        {
            if (title.Contains("(TV)"))
                this.VideoCodec = CConstants.VideoCodec.TV;

            title = title.Replace("(TV)", "");

            if (title.Contains("(BluRay)"))
                this.VideoCodec = CConstants.VideoCodec.BluRay;

            title = title.Replace("(BluRay)", "");

            if (title.Contains("(H264)"))
                this.VideoCodec = CConstants.VideoCodec.H264;

            title = title.Replace("(H264)", "");

            if (title.Contains("(H265)"))
                this.VideoCodec = CConstants.VideoCodec.H265;

            title = title.Replace("(H265)", "");


            if (title.Contains("(SD)"))
                this.VideoDefinition = CConstants.VideoDefinition.SD;

            title = title.Replace("(SD)", "");

            if (title.Contains("(HD)"))
                this.VideoDefinition = CConstants.VideoDefinition.HD;

            title = title.Replace("(HD)", "");

            if (title.Contains("(4:3)"))
                this.VideoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_4_3;

            title = title.Replace("(4:3)", "");

            if (title.Contains("(16:9)"))
                this.VideoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_16_9;

            title = title.Replace("(16:9)", "");

            if (title.Contains("(21:9)"))
                this.VideoAspectRatio = CConstants.VideoAspectRatio.AspectRatio_21_9;

            title = title.Replace("(21:9)", "");

            if (title.Contains("(F"))
                this.MPAA = title.RightOf("(F").LeftOf(")");

            title = title.Replace("(F" + this.MPAA + ")", "");

            if (title.Contains("(R"))
                this.Rating = title.RightOf("(R").LeftOf(")");

            title = title.Replace("(F" + this.Rating + ")", "");

            // check for multiple Instances per Language
            if (title.Contains("(L"))
            {
                // reset list if a new definition is available
                this.mediaLanguages = new List<string>();

                string movieLanguages = title.RightOfLast("(L").LeftOf(")");
                foreach (string movieLanguage in movieLanguages._Split(" ", null, false))
                    this.MediaLanguages.Add(movieLanguage);

                title = title.Replace("(L" + movieLanguages + ")", "").Trim();
            }

            // inherit Series-data
            if (this.MediaLanguages.Count == 0 && this.GetType().ToString().Contains("CEpisode"))
                this.MediaLanguages = ((CEpisode)this).Series.MediaLanguages;

            // or set to default
            if (this.MediaLanguages.Count == 0)
                this.MediaLanguages.Add("de");

            return title.Trim();
        }
        public void writeStreamData(StreamWriter swrNFO)
        {
            this.writeVideoStreamData(swrNFO);
            this.writeAudioStreamData(swrNFO);
            this.writeSubTitleStreamData(swrNFO);
        }
        private void writeVideoStreamData(StreamWriter swrNFO)
        {
            swrNFO.WriteLine("    <fileinfo>");
            swrNFO.WriteLine("        <streamdetails>");
            swrNFO.WriteLine("            <video>");

            // VideoCodec
            if (this.VideoCodec.Equals(CConstants.VideoCodec.TV))
                swrNFO.WriteLine("                <codec>tv</codec>");

            if (this.VideoCodec.Equals(CConstants.VideoCodec.BluRay))
                swrNFO.WriteLine("                <codec>bluray</codec>");

            if (this.VideoCodec.Equals(CConstants.VideoCodec.H264))
                swrNFO.WriteLine("                <codec>h264</codec>");

            if (this.VideoCodec.Equals(CConstants.VideoCodec.H265))
                swrNFO.WriteLine("                <codec>hevc</codec>");

            // AspectRatio
            if (this.VideoAspectRatio.Equals(CConstants.VideoAspectRatio.AspectRatio_4_3))
                swrNFO.WriteLine("                <aspect>1.33</aspect>");

            if (this.VideoAspectRatio.Equals(CConstants.VideoAspectRatio.AspectRatio_16_9))
                swrNFO.WriteLine("                <aspect>1.78</aspect>");

            if (this.VideoAspectRatio.Equals(CConstants.VideoAspectRatio.AspectRatio_21_9))
                swrNFO.WriteLine("                <aspect>2.33</aspect>");

            // VideoDefinition
            if (this.VideoDefinition.Equals(CConstants.VideoDefinition.SD))
            {
                swrNFO.WriteLine("                <width>768</width>");
                swrNFO.WriteLine("                <height>576</height>");
            }

            if (this.VideoDefinition.Equals(CConstants.VideoDefinition.HD))
            {
                swrNFO.WriteLine("                <width>1920</width>");
                swrNFO.WriteLine("                <height>1080</height>");
            }

            swrNFO.WriteLine("            </video>");
        }
        private void writeAudioStreamData(StreamWriter swrNFO)
        {
            foreach (CAudioStream audioStream in this.AudioStreams)
            {
                swrNFO.WriteLine("            <audio>");
                swrNFO.WriteLine("                <codec>" + audioStream.Codec + "</codec>");
                swrNFO.WriteLine("                <language>" + audioStream.Language + "</language>");
                swrNFO.WriteLine("                <channels>" + audioStream.NumberOfChannels + "</channels>");
                swrNFO.WriteLine("            </audio>");
            }
        }
        private void writeSubTitleStreamData(StreamWriter swrNFO)
        {
            foreach (CSubTitleFile subTitleStrem in this.SubTitleStreams)
            {
                swrNFO.WriteLine("            <subtitle>");
                swrNFO.WriteLine("                <language>" + subTitleStrem.Language + "</language>");
                swrNFO.WriteLine("            </subtitle>");
            }

            swrNFO.WriteLine("        </streamdetails>");
            swrNFO.WriteLine("    </fileinfo>");
        }
        #endregion
    }
}
