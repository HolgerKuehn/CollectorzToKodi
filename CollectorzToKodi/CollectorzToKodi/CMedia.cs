// <copyright file="CMedia.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Base class for media content
    /// </summary>
    public abstract class CMedia
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private CConfiguration configuration;

        /// <summary>
        /// Title of media
        /// </summary>
        private string title;

        /// <summary>
        /// Title of media used for sorting
        /// </summary>
        private string titleSort;

        /// <summary>
        /// Original title of media
        /// </summary>
        private string titleOriginal;

        /// <summary>
        /// Group of media; eg. album for music or sets for movies
        /// </summary>
        private string mediaGroup;

        /// <summary>
        /// Rating of media
        /// </summary>
        private string rating;

        /// <summary>
        /// Publishing year of media
        /// </summary>
        private string publishingYear;

        /// <summary>
        /// Publishing date of media
        /// </summary>
        private string publishingDate;

        /// <summary>
        /// content of media, eg. song-text for music or plot for movie
        /// </summary>
        private string content;

        /// <summary>
        /// Run time of media
        /// </summary>
        private string runTime;

        /// <summary>
        /// Images associated with media
        /// </summary>
        private List<CImageFile> images;

        /// <summary>
        /// Country of media
        /// </summary>
        private string country;

        /// <summary>
        /// List genres of media
        /// </summary>
        private List<string> genres;

        /// <summary>
        /// List of studios of media
        /// </summary>
        private List<string> studios;

        /// <summary>
        /// List of languages that are stored in different files of this media
        /// </summary>
        private List<string> mediaLanguages;

        /// <summary>
        /// list of associated media files
        /// </summary>
        private List<CMediaFile> mediaFiles;

        /// <summary>
        /// file name used to publish the media in Kodi
        /// </summary>
        /// <remarks>represents base name, will be extended by "(Specials)" or season and episode index</remarks>
        private string filename;

        /// <summary>
        /// list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        private List<int> server;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CMedia"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CMedia(CConfiguration configuration)
        {
            this.configuration = configuration;
            this.title = string.Empty;
            this.titleSort = string.Empty;
            this.titleOriginal = string.Empty;
            this.mediaGroup = string.Empty;
            this.rating = string.Empty;
            this.publishingYear = string.Empty;
            this.publishingDate = string.Empty;
            this.content = string.Empty;
            this.runTime = string.Empty;
            this.images = new List<CImageFile>();
            this.country = string.Empty;
            this.genres = new List<string>();
            this.studios = new List<string>();
            this.mediaFiles = new List<CMediaFile>();
            this.filename = string.Empty;
            this.mediaLanguages = new List<string>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets title of media
        /// </summary>
        public string Title
        {
            get { return this.title; }
            set { this.title = value; }
        }

        /// <summary>
        /// Gets or sets title of media used for sorting
        /// </summary>
        public string TitleSort
        {
            get { return this.titleSort; }
            set { this.titleSort = value; }
        }

        /// <summary>
        /// Gets or sets Original title of media
        /// </summary>
        public string TitleOriginal
        {
            get { return this.titleOriginal; }
            set { this.titleOriginal = value; }
        }

        /// <summary>
        /// Gets or sets group of media; eg. album for music or sets for movies
        /// </summary>
        public string MediaGroup
        {
            get { return this.mediaGroup; }
            set { this.mediaGroup = value; }
        }

        /// <summary>
        /// Gets or sets rating of media
        /// </summary>
        public string Rating
        {
            get { return this.rating; }
            set { this.rating = value; }
        }

        /// <summary>
        /// Gets or sets publishing year of media
        /// </summary>
        public string PublishingYear
        {
            get { return this.publishingYear; }
            set { this.publishingYear = value; }
        }

        /// <summary>
        /// Gets or sets publishing date of media
        /// </summary>
        public string PublishingDate
        {
            get { return this.publishingDate; }
            set { this.publishingDate = value; }
        }

        /// <summary>
        /// Gets or sets content of media, eg. song-text for music or plot for movie
        /// </summary>
        public string Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        /// <summary>
        /// Gets or sets run time of media
        /// </summary>
        public string RunTime
        {
            get { return this.runTime; }
            set { this.runTime = value; }
        }

        /// <summary>
        /// Gets or sets images associated with media
        /// </summary>
        public List<CImageFile> Images
        {
            get { return this.images; }
            set { this.images = value; }
        }

        /// <summary>
        /// Gets or sets country of media
        /// </summary>
        public string Country
        {
            get { return this.country; }
            set { this.country = value; }
        }

        /// <summary>
        /// Gets or sets list genres of media
        /// </summary>
        public List<string> Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }

        /// <summary>
        /// Gets or sets list of studios of media
        /// </summary>
        public List<string> Studios
        {
            get { return this.studios; }
            set { this.studios = value; }
        }

        /// <summary>
        /// Gets or sets List of languages that are stored in different files of this media
        /// </summary>
        public List<string> MediaLanguages
        {
            get { return this.mediaLanguages; }
            set { this.mediaLanguages = value; }
        }

        /// <summary>
        /// Gets or sets list of associated media files
        /// </summary>
        public List<CMediaFile> MediaFiles
        {
            get { return this.mediaFiles; }
            set { this.mediaFiles = value; }
        }

        /// <summary>
        /// Gets or sets file name used to publish the media in Kodi
        /// </summary>
        /// <remarks>represents base name, will be extended by "(Specials)" or season and episode index</remarks>
        public string Filename
        {
            get { return this.filename; }
            set { this.filename = value; }
        }

        /// <summary>
        /// Gets or sets list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        public List<int> Server
        {
            get
            {
                if (this.server == null)
                {
                    this.server = new List<int>();
                }

                return this.server;
            }

            set
            {
                this.server = value;
            }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Reads XML-files into media collection
        /// </summary>
        /// <param name="xMLMedia">part of XML export representing Movie, Series, Episode or Music</param>
        public abstract void ReadMediaFiles(XmlNode xMLMedia);

        /// <summary>
        /// Writes data to new nfo-file according to Kodi specifics for movie, series, episode or music
        /// </summary>
        public abstract void WriteNFO();

        /// <summary>
        /// appends copy strings to provided shell-script
        /// </summary>
        /// <param name="swrSH">Bash-Shell script that contains all copy statements for server</param>
        public abstract void WriteSH(StreamWriter swrSH);

        /// <summary>
        /// Clones media object completely
        /// </summary>
        /// <returns>new instance of media object</returns>
        public abstract CMedia Clone();

        /// <summary>
        /// Clones media to specified languages
        /// </summary>
        /// <param name="isoCodesToBeReplaced">list target languages</param>
        /// <param name="isoCodeForReplacemant">language used as placeholder in CollectorzToKodi programs</param>
        public virtual void ClonePerLanguage(List<string> isoCodesToBeReplaced, string isoCodeForReplacemant)
        {
            // check for target-language
            bool episodeContainsTargetLanguage = false;

            foreach (string mediaLanguage in this.MediaLanguages)
            {
                if (mediaLanguage == isoCodeForReplacemant)
                {
                    episodeContainsTargetLanguage = true;
                }
            }

            // replace all associated languages
            this.MediaLanguages = new List<string>();
            this.MediaLanguages.Add(isoCodeForReplacemant);

            foreach (string isoCodeToBeReplaced in isoCodesToBeReplaced)
            {
                this.Title = this.Title.ReplaceAll("(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.Title = this.Title.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                this.TitleSort = this.TitleSort.ReplaceAll("(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.TitleSort = this.TitleSort.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                this.Filename = this.Filename.ReplaceAll("(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.Filename = this.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                foreach (CMediaFile mediaFile in this.MediaFiles)
                {
                    if (!episodeContainsTargetLanguage)
                    {
                        mediaFile.Filename = string.Empty;
                    }

                    mediaFile.Filename = mediaFile.Filename.ReplaceAll("(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + CConfiguration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                    mediaFile.Filename = mediaFile.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                    mediaFile.URL = mediaFile.URL.ReplaceAll(isoCodeToBeReplaced + mediaFile.Extension, isoCodeForReplacemant + mediaFile.Extension);
                    mediaFile.URLLocalFilesystem = mediaFile.URLLocalFilesystem.ReplaceAll(isoCodeToBeReplaced + mediaFile.Extension, isoCodeForReplacemant + mediaFile.Extension);
                }
            }
        }

        /// <summary>
        /// Adds reference to Server, if any part of media is stored on it
        /// </summary>
        /// <param name="serverList">Server to be added; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        public void AddServer(int serverList)
        {
            bool addServer = true;
            foreach (int currentServerList in this.Server)
            {
                if (currentServerList.Equals(serverList))
                {
                    addServer = false;
                }
            }

            if (addServer)
            {
                this.Server.Add(serverList);
            }
        }

        /// <summary>
        /// Reads genre from XML-File
        /// </summary>
        /// <param name="xMLNode">Part of Collectors export, representing genres of media</param>
        public void ReadGenre(XmlNode xMLNode)
        {
            foreach (XmlNode xMLGenre in xMLNode.XMLReadSubnode("genres").XMLReadSubnodes("genre"))
            {
                this.Genres.Add(xMLGenre.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty));
            }
        }

        /// <summary>
        /// Writes genres to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the genre information should be added to</param>
        public void WriteGenre(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (string genre in this.Genres)
            {
                swrNFO.WriteLine("    <genre" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">" + genre + "</genre>");
                i++;
            }
        }

        /// <summary>
        /// Reads studios from XML-File
        /// </summary>
        /// <param name="xMLNode">Part of Collectors export, representing studios of media</param>
        public void ReadStudio(XmlNode xMLNode)
        {
            foreach (XmlNode xMLStudio in xMLNode.XMLReadSubnode("studios").XMLReadSubnodes("studio"))
            {
                this.Studios.Add(xMLStudio.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty));
            }
        }

        /// <summary>
        /// Writes studios to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the studio information should be added to</param>
        public void WriteStudio(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (string studio in this.Studios)
            {
                swrNFO.WriteLine("    <studio" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">" + studio + "</studio>");
                i++;
            }
        }

        /// <summary>
        /// Reads images from XML-File
        /// </summary>
        /// <param name="xMLNode">Part of Collectors export, representing images of media</param>
        public virtual void ReadImages(XmlNode xMLNode)
        {
            CImageFile image;

            // Cover-Front-Image
            image = new CImageFile(this.Configuration);
            image.Media = this;
            image.Filename = "cover";
            image.URL = xMLNode.XMLReadSubnode("coverfront").XMLReadInnerText(string.Empty);
            image.ConvertFilename();
            image.ImageType = CConfiguration.ImageType.CoverFront;

            if (image.URL != string.Empty)
            {
                image.Media.Images.Add(image);
            }

            // Cover-Back-Image
            image = new CImageFile(this.Configuration);
            image.Media = this;
            image.Filename = "coverback";
            image.URL = xMLNode.XMLReadSubnode("coverback").XMLReadInnerText(string.Empty);
            image.ConvertFilename();
            image.ImageType = CConfiguration.ImageType.CoverBack;

            if (image.URL != string.Empty)
            {
                image.Media.Images.Add(image);
            }

            // Poster-Image
            image = new CImageFile(this.Configuration);
            image.Media = this;
            image.Filename = "poster";
            image.URL = xMLNode.XMLReadSubnode("poster").XMLReadInnerText(string.Empty);
            image.ConvertFilename();
            image.ImageType = CConfiguration.ImageType.Poster;

            if (image.URL != string.Empty)
            {
                image.Media.Images.Add(image);
            }

            // Backdrop-Image
            image = new CImageFile(this.Configuration);
            image.Media = this;
            image.Filename = "fanart";
            image.URL = xMLNode.XMLReadSubnode("backdropurl").XMLReadInnerText(string.Empty);
            image.ConvertFilename();
            image.ImageType = CConfiguration.ImageType.Backdrop;

            if (image.URL != string.Empty)
            {
                image.Media.Images.Add(image);
            }

            // Covers / Backdrops per Season
            int numberOfExtraBackdrop = 0;
            int numberOfExtraCover = 0;
            foreach (XmlNode xMLImageFile in xMLNode.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if (xMLImageFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Image")
                {
                    CImageFile imageFile = new CImageFile(this.Configuration);
                    imageFile.Media = this;

                    imageFile.Description = imageFile.OverrideSeason(xMLImageFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty));

                    // Fanart or Thumb ?
                    if (imageFile.Description.Contains("ExtraBackdrop" /* Backdrops */) ||
                        (imageFile.Description.Contains("ExtraCover") && imageFile.Media.GetType().ToString().Contains("CSeries")) /* or Covers for TV-Shows as no Extrathumbs are supported */)
                    {
                        numberOfExtraBackdrop++;
                        imageFile.ImageType = CConfiguration.ImageType.ExtraBackdrop;
                        imageFile.Filename = "fanart" + ("0000" + numberOfExtraBackdrop.ToString()).Substring(numberOfExtraBackdrop.ToString().Length);
                    }

                    // Extra-Cover only for movies
                    else if (imageFile.Description.Contains("ExtraCover") && !imageFile.Media.GetType().ToString().Contains("CSeries"))
                    {
                        numberOfExtraCover++;
                        imageFile.ImageType = CConfiguration.ImageType.ExtraCover;
                        imageFile.Filename = "thumb" + ("0000" + numberOfExtraCover.ToString()).Substring(numberOfExtraCover.ToString().Length);
                    }
                    else if (imageFile.Description.Contains("Backdrop"))
                    {
                        imageFile.ImageType = CConfiguration.ImageType.SeasonBackdrop;
                        imageFile.Filename = "fanart";
                    }
                    else if (imageFile.Description.Contains("Cover"))
                    {
                        imageFile.ImageType = CConfiguration.ImageType.SeasonCover;
                        imageFile.Filename = "cover";
                    }

                    if (imageFile.Season == "all")
                    {
                        imageFile.Season = "-1";
                        imageFile.Filename = imageFile.Filename + "_all";
                    }
                    else if (imageFile.Season == "spe")
                    {
                        imageFile.Season = "0";
                    }

                    imageFile.URL = xMLImageFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty);
                    imageFile.ConvertFilename();

                    if (imageFile.URL != string.Empty)
                    {
                        imageFile.Media.Images.Add(imageFile);
                    }
                }
            }
        }

        /// <summary>
        /// Writes images to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the image information should be added to</param>
        public virtual void WriteImagesToNFO(StreamWriter swrNFO)
        {
            // Cover-Thumb
            this.WriteImagesToNFO(swrNFO, CConfiguration.ImageType.CoverFront);
            this.WriteImagesToNFO(swrNFO, CConfiguration.ImageType.SeasonCover);

            // fan art
            swrNFO.WriteLine("    <fanart>");
            this.WriteImagesToNFO(swrNFO, CConfiguration.ImageType.Backdrop);
            this.WriteImagesToNFO(swrNFO, CConfiguration.ImageType.SeasonBackdrop);
            swrNFO.WriteLine("    </fanart>");
        }

        /// <summary>
        /// Adds copy statements for images to provided bash-shell-script
        /// </summary>
        /// <param name="swrSH">Bash-shell-script that the image information should be added to</param>
        public virtual void WriteImagesToSH(StreamWriter swrSH)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != string.Empty && !imageFile.URL.Contains("http://") && imageFile.ImageType != CConfiguration.ImageType.Unknown)
                {
                    if (!imageFile.Media.GetType().ToString().Contains("CMovie") && imageFile.Season != string.Empty && imageFile.Season != "-1" && imageFile.ImageType != CConfiguration.ImageType.ExtraBackdrop && imageFile.ImageType != CConfiguration.ImageType.ExtraCover)
                    {
                        swrSH.WriteLine("cd \"Season " + ("00" + imageFile.Season).Substring(imageFile.Season.Length) + "\"");
                    }

                    if (imageFile.ImageType == CConfiguration.ImageType.ExtraBackdrop)
                    {
                        swrSH.WriteLine("cd \"extrafanart\"");
                    }

                    if (imageFile.ImageType == CConfiguration.ImageType.ExtraCover)
                    {
                        swrSH.WriteLine("cd \"extrathumbs\"");
                    }

                    swrSH.WriteLine("/bin/cp \"" + imageFile.URLLocalFilesystem + "\" \"" + imageFile.Filename + "\"");

                    if ((!imageFile.Media.GetType().ToString().Contains("CMovie") && imageFile.Season != string.Empty && imageFile.Season != "-1") || imageFile.ImageType == CConfiguration.ImageType.ExtraBackdrop || imageFile.ImageType == CConfiguration.ImageType.ExtraCover)
                    {
                        swrSH.WriteLine("cd ..");
                    }
                }
            }
        }

        /// <summary>
        ///  Writes images to provided NFO file, when they meet to specified image type
        /// </summary>
        /// <param name="swrNFO">NFO file that the image information should be added to</param>
        /// <param name="imageType">Image type, that should be added</param>
        private void WriteImagesToNFO(StreamWriter swrNFO, CConfiguration.ImageType imageType)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                CImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.Filename != string.Empty && imageFile.ImageType == imageType)
                {
                    if (imageFile.ImageType == CConfiguration.ImageType.CoverFront || imageFile.ImageType == CConfiguration.ImageType.Backdrop)
                    {
                        if (imageFile.ImageType == CConfiguration.ImageType.Backdrop)
                        {
                            swrNFO.Write("    ");
                        }

                        swrNFO.WriteLine("    <thumb>smb://" + imageFile.Media.Server.ElementAt(0) + "/XBMC/" + (imageFile.Media.GetType().ToString().Contains("CMovie") ? "Filme" : "Serien") + "/" + imageFile.Media.Filename + "/" + imageFile.Filename + "</thumb>");
                    }

                    if (imageFile.ImageType == CConfiguration.ImageType.SeasonCover || imageFile.ImageType == CConfiguration.ImageType.SeasonBackdrop)
                    {
                        if (imageFile.ImageType == CConfiguration.ImageType.SeasonBackdrop)
                        {
                            swrNFO.Write("    ");
                        }

                        swrNFO.WriteLine("    <thumb type=\"season\" season=\"" + imageFile.Season + "\">smb://" + imageFile.Media.Server.ElementAt(0) + "/XBMC/" + (imageFile.Media.GetType().ToString().Contains("CSeries") ? "Serien" : "Filme") + "/" + imageFile.Media.Filename + "/" + (imageFile.Season != "-1" ? "Season " + ("00" + imageFile.Season).Substring(imageFile.Season.Length) + "/" : string.Empty) + imageFile.Filename + "</thumb>");
                    }
                }
            }
        }

        #endregion
    }
}
