﻿// <copyright file="Media.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Base class for media content
    /// </summary>
    public abstract class Media
    {
        #region Attributes

        /// <summary>
        /// Current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// ID of media
        /// </summary>
        private string iD;

        /// <summary>
        /// Title of media
        /// </summary>
        private string title;

        /// <summary>
        /// Title of media used for sorting
        /// </summary>
        private string titleSort;

        /// <summary>
        /// MediaPath of Media
        /// </summary>
        private MediaPath mediaPath;

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
        private List<ImageFile> images;

        /// <summary>
        /// Country of media
        /// </summary>
        private string country;

        /// <summary>
        /// List genres of media
        /// </summary>
        private GenreCollection genres;

        /// <summary>
        /// List of studios of media
        /// </summary>
        private StudioCollection studios;

        /// <summary>
        /// List of languages that are stored in different files of this media
        /// </summary>
        private MediaLanguageCollection mediaLanguages;

        /// <summary>
        /// list of associated media files
        /// </summary>
        private List<MediaFile> mediaFiles;

        /// <summary>
        /// list of servers containing parts of media
        /// </summary>
        /// <remarks>number is translated via CConfiguration.ServerListsOfServers[ListOfServerTypes]</remarks>
        private List<Server> server;

        /// <summary>
        /// NFO-File representing the actual media file
        /// </summary>
        private NfoFile nfoFile;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Media"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Media(Configuration configuration)
        {
            this.configuration = configuration;
            this.iD = string.Empty;
            this.title = string.Empty;
            this.titleSort = string.Empty;
            this.titleOriginal = string.Empty;
            this.mediaGroup = string.Empty;
            this.rating = string.Empty;
            this.publishingYear = string.Empty;
            this.publishingDate = string.Empty;
            this.content = string.Empty;
            this.runTime = string.Empty;
            this.images = new List<ImageFile>();
            this.country = string.Empty;
            this.genres = new GenreCollection(this.Configuration);
            this.studios = new StudioCollection(this.Configuration);
            this.mediaFiles = new List<MediaFile>();
            this.mediaLanguages = new MediaLanguageCollection();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets id of media
        /// </summary>
        public string ID
        {
            get { return this.iD; }
            set { this.iD = value; }
        }

        /// <summary>
        /// Gets or sets title of media
        /// </summary>
        public virtual string Title
        {
            get
            {
                return this.title;
            }

            set
            {
                this.title = value;

                this.MediaPath.Filename = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, Encoding.ASCII, System.Text.Encoding.UTF8.GetBytes(this.Title + " (" + this.PublishingYear + ")"))).Replace("?", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty).Trim();
            }
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
        public List<ImageFile> Images
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
        public GenreCollection Genres
        {
            get { return this.genres; }
            set { this.genres = value; }
        }

        /// <summary>
        /// Gets or sets list of studios of media
        /// </summary>
        public StudioCollection Studios
        {
            get { return this.studios; }
            set { this.studios = value; }
        }

        /// <summary>
        /// Gets or sets List of languages that are stored in different files of this media
        /// </summary>
        public MediaLanguageCollection MediaLanguages
        {
            get { return this.mediaLanguages; }
            set { this.mediaLanguages = value; }
        }

        /// <summary>
        /// Gets or sets list of associated media files
        /// </summary>
        public List<MediaFile> MediaFiles
        {
            get { return this.mediaFiles; }
            set { this.mediaFiles = value; }
        }

        /// <summary>
        /// Gets or sets path to Media
        /// </summary>
        public virtual List<Server> Server
        {
            get
            {
                if (this.server == null)
                {
                    this.server = new List<Server>();
                }

                return this.server;
            }

            set
            {
                this.server = value;
            }
        }

        /// <summary>
        /// Gets or sets MediaPath of Media
        /// </summary>
        public MediaPath MediaPath
        {
            get { return this.mediaPath; }
            set { this.mediaPath = value; }
        }

        /// <summary>
        /// Gets or sets NFO-File representing the actual media file
        /// </summary>
        public NfoFile NfoFile
        {
            get
            {
                if (this.nfoFile == null)
                {
                    this.nfoFile = new NfoFile(this.Configuration);
                }

                return this.nfoFile;
            }

            set
            {
                this.nfoFile = value;
            }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Reads XML-files into media collection
        /// </summary>
        /// <param name="xMLMedia">part of XML export representing Movie, Series, Episode or Music</param>
        public virtual void ReadFromXml(XmlNode xMLMedia)
        {
            // read images
            ImageFile image = null;
            ImageFile imageFileClone = null;
            int numberOfExtraBackdrop = 0;
            int numberOfExtraCover = 0;

            // Covers / Backdrops per Season
            List<List<int>> imagesPerSeason = new List<List<int>>();
            List<List<ImageFile>> imageFilesPerSeason = new List<List<ImageFile>>();

            // initialize ImagePerSeason-List with imageTypes
            for (int i = 0; i < this.Configuration.NumberOfImageTypes; i++)
            {
                imagesPerSeason.Add(new List<int>());
                imagesPerSeason[i].Add(0); // allSeasons
                imagesPerSeason[i].Add(0); // Specials
                imagesPerSeason[i].Add(0); // Season 1

                imageFilesPerSeason.Add(new List<ImageFile>());
                imageFilesPerSeason[i].Add(null); // allSeasons
                imageFilesPerSeason[i].Add(null); // Specials
                imageFilesPerSeason[i].Add(null); // Season 1
            }

            // Cover-Front-Image
            image = new ImageFile(this.Configuration);
            image.Media = this;
            image.MediaFilePath.Filename = "cover";
            image.MediaFilePath.WindowsPath = xMLMedia.XMLReadSubnode("coverfront").XMLReadInnerText(string.Empty);
            image.ImageType = Configuration.ImageType.CoverFront;

            if (image.MediaFilePath.WindowsPath != string.Empty)
            {
                image.Media.Images.Add(image);
                imagesPerSeason[(int)Configuration.ImageType.CoverFront][0]++;
                imageFilesPerSeason[(int)Configuration.ImageType.CoverFront][0] /* Cover */ = image;
            }

            // Cover-Back-Image
            image = new ImageFile(this.Configuration);
            image.Media = this;
            image.MediaFilePath.Filename = "coverback";
            image.MediaFilePath.WindowsPath = xMLMedia.XMLReadSubnode("coverback").XMLReadInnerText(string.Empty);
            image.ImageType = Configuration.ImageType.CoverBack;

            if (image.MediaFilePath.WindowsPath != string.Empty)
            {
                image.Media.Images.Add(image);
                imagesPerSeason[(int)Configuration.ImageType.CoverBack][0]++;
                imageFilesPerSeason[(int)Configuration.ImageType.CoverBack][0] /* Cover-Back */ = image;
            }

            // Poster-Image
            image = new ImageFile(this.Configuration);
            image.Media = this;
            image.MediaFilePath.Filename = "poster";
            image.MediaFilePath.WindowsPath = xMLMedia.XMLReadSubnode("poster").XMLReadInnerText(string.Empty);

            /* Estuary just displays poster instead of cover; so setting this as poster when empty */
            if (image.MediaFilePath.WindowsPath == string.Empty)
            {
                image.MediaFilePath.WindowsPath = xMLMedia.XMLReadSubnode("coverfront").XMLReadInnerText(string.Empty);
            }

            image.ImageType = Configuration.ImageType.Poster;

            if (image.MediaFilePath.WindowsPath != string.Empty)
            {
                image.Media.Images.Add(image);
                imagesPerSeason[(int)Configuration.ImageType.Poster][0]++;
                imageFilesPerSeason[(int)Configuration.ImageType.Poster][0] /* Poster */ = image;
            }

            // Backdrop-Image
            image = new ImageFile(this.Configuration);
            image.Media = this;
            image.MediaFilePath.Filename = "fanart";
            image.MediaFilePath.WindowsPath = xMLMedia.XMLReadSubnode("backdropurl").XMLReadInnerText(string.Empty);
            image.ImageType = Configuration.ImageType.Backdrop;

            if (image.MediaFilePath.WindowsPath != string.Empty)
            {
                // add Backdrop
                image.Media.Images.Add(image);
                imagesPerSeason[(int)Configuration.ImageType.Backdrop][0]++;
                imageFilesPerSeason[(int)Configuration.ImageType.Backdrop][0] /* Backdrop */ = image;

                // add Backdrop as Fanart for skins supporting it
                numberOfExtraBackdrop++;
                imageFileClone = (ImageFile)image.Clone();
                imageFileClone.ImageType = Configuration.ImageType.ExtraBackdrop;
                imageFileClone.MediaFilePath.Filename = "fanart" + ("0000" + numberOfExtraBackdrop.ToString()).Substring(numberOfExtraBackdrop.ToString().Length);
                imageFileClone.Media.Images.Add(imageFileClone);
                imagesPerSeason[(int)Configuration.ImageType.ExtraBackdrop][0]++;
                imageFilesPerSeason[(int)Configuration.ImageType.ExtraBackdrop][0] /* Backdrop */ = imageFileClone;
            }

            // add images from Links section
            foreach (XmlNode xMLImageFile in xMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                if (xMLImageFile.XMLReadSubnode("urltype").XMLReadInnerText(string.Empty) == "Image")
                {
                    ImageFile imageFile = new ImageFile(this.Configuration);
                    imageFile.Media = this;

                    imageFile.Description = imageFile.OverrideSeason(xMLImageFile.XMLReadSubnode("description").XMLReadInnerText(string.Empty));

                    // Fanart or Thumb ?
                    if (imageFile.Description.Contains("ExtraBackdrop" /* Backdrops */) ||
                        (imageFile.Description.Contains("ExtraCover") && imageFile.Media.GetType().ToString().Contains("Series")) /* or Covers for TV-Shows as no Extrathumbs are supported */)
                    {
                        numberOfExtraBackdrop++;
                        imageFile.ImageType = Configuration.ImageType.ExtraBackdrop;
                        imageFile.MediaFilePath.Filename = "fanart" + ("0000" + numberOfExtraBackdrop.ToString()).Substring(numberOfExtraBackdrop.ToString().Length);
                    }

                    // Extra-Cover only for movies
                    else if (imageFile.Description.Contains("ExtraCover") && !imageFile.Media.GetType().ToString().Contains("Series"))
                    {
                        numberOfExtraCover++;
                        imageFile.ImageType = Configuration.ImageType.ExtraCover;
                        imageFile.MediaFilePath.Filename = "thumb" + ("0000" + numberOfExtraCover.ToString()).Substring(numberOfExtraCover.ToString().Length);
                    }
                    else if (imageFile.Description.Contains("Backdrop"))
                    {
                        imageFile.ImageType = Configuration.ImageType.SeasonBackdrop;
                        imageFile.MediaFilePath.Filename = "fanart";
                    }
                    else if (imageFile.Description.Contains("Cover"))
                    {
                        imageFile.ImageType = Configuration.ImageType.SeasonCover;
                        imageFile.MediaFilePath.Filename = "cover";
                    }
                    else if (imageFile.Description.Contains("Poster"))
                    {
                        imageFile.ImageType = Configuration.ImageType.SeasonPoster;
                        imageFile.MediaFilePath.Filename = "poster";
                    }

                    if (imageFile.Season == "all")
                    {
                        imageFile.Season = "-1";
                        imageFile.MediaFilePath.Filename = imageFile.MediaFilePath.Filename + "_all";
                    }
                    else if (imageFile.Season == "spe")
                    {
                        imageFile.Season = "0";
                    }

                    imageFile.MediaFilePath.WindowsPath = xMLImageFile.XMLReadSubnode("url").XMLReadInnerText(string.Empty);

                    if (imageFile.MediaFilePath.WindowsPath != string.Empty)
                    {
                        imageFile.Media.Images.Add(imageFile);
                    }

                    // adding image data to posterPerSeason-list
                    string season = imageFile.Season;
                    if (season == string.Empty)
                    {
                        season = "0";
                    }

                    while (imagesPerSeason[(int)imageFile.ImageType].Count < int.Parse(season) + 2)
                    {
                        for (int i = 0; i < this.Configuration.NumberOfImageTypes; i++)
                        {
                            imagesPerSeason[i].Add(0);
                            imageFilesPerSeason[i].Add(null);
                        }
                    }

                    imagesPerSeason[(int)imageFile.ImageType][int.Parse(imageFile.Season) + 1]++;
                    imageFilesPerSeason[(int)imageFile.ImageType][int.Parse(imageFile.Season) + 1] = imageFile;
                }
            }

            // for all Seasons (i)
            for (int i = 0; i < imagesPerSeason[(int)Configuration.ImageType.SeasonCover].Count; i++)
            {
                // add SeasonCover if missing
                if (imagesPerSeason[(int)Configuration.ImageType.SeasonCover][i] == 0 && imageFilesPerSeason[(int)Configuration.ImageType.CoverFront][0] /* Cover */ != null)
                {
                    imageFileClone = (ImageFile)imageFilesPerSeason[(int)Configuration.ImageType.CoverFront][0].Clone() /* Cover */;
                    imageFileClone.ImageType = Configuration.ImageType.SeasonCover;
                    imageFileClone.Season = (i - 1).ToString();

                    this.Images.Add(imageFileClone);
                    imagesPerSeason[(int)Configuration.ImageType.SeasonCover][i]++;
                    imageFilesPerSeason[(int)Configuration.ImageType.SeasonCover][i] /* Season-Cover */ = imageFileClone;
                }

                // add SeasonPoster if missing
                if (imagesPerSeason[(int)Configuration.ImageType.SeasonPoster][i] == 0 && (imageFilesPerSeason[(int)Configuration.ImageType.Poster][0] /* Poster */ != null || imageFilesPerSeason[(int)Configuration.ImageType.SeasonCover][i] /* Season-Cover */ != null))
                {
                    // add Season-Cover first
                    if (imageFilesPerSeason[(int)Configuration.ImageType.SeasonCover][i] != null)
                    {
                        imageFileClone = (ImageFile)imageFilesPerSeason[(int)Configuration.ImageType.SeasonCover][i].Clone();
                    } // or add series poster instead
                    else if (imageFilesPerSeason[(int)Configuration.ImageType.Poster][0] /* Poster */ != null)
                    {
                        imageFileClone = (ImageFile)imageFilesPerSeason[(int)Configuration.ImageType.Poster][0].Clone() /* Poster */;
                    }

                    imageFileClone.ImageType = Configuration.ImageType.SeasonPoster;
                    imageFileClone.Season = (i - 1).ToString();

                    this.Images.Add(imageFileClone);
                    imagesPerSeason[(int)Configuration.ImageType.SeasonPoster][i]++;
                    imageFilesPerSeason[(int)Configuration.ImageType.SeasonPoster][i] /* SeasonPoster */ = imageFileClone;
                }

                // add SeasonBackdrop if missing
                if (imagesPerSeason[(int)Configuration.ImageType.SeasonBackdrop][i] == 0 && imageFilesPerSeason[(int)Configuration.ImageType.Backdrop][0] /* Backdrop */ != null)
                {
                    imageFileClone = (ImageFile)imageFilesPerSeason[(int)Configuration.ImageType.Backdrop][0].Clone() /* Backdrop */;
                    imageFileClone.ImageType = Configuration.ImageType.SeasonBackdrop;
                    imageFileClone.Season = (i - 1).ToString();

                    this.Images.Add(imageFileClone);
                    imagesPerSeason[(int)Configuration.ImageType.SeasonBackdrop][i]++;
                    imageFilesPerSeason[(int)Configuration.ImageType.SeasonBackdrop][i] /* SeasonBackdrop */ = imageFileClone;
                }
            }
        }

        /// <summary>
        /// delete from Library
        /// </summary>
        public abstract void DeleteFromLibrary();

        /// <summary>
        /// exports Library to Disk
        /// </summary>
        public virtual void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0].Number].StreamWriter;

            // write Images to Library
            // adding images to NfoFile
            // Cover-Thumb
            this.WriteToLibrary(Configuration.ImageType.CoverFront);
            this.WriteToLibrary(Configuration.ImageType.SeasonCover);

            // fan art
            nfoStreamWriter.WriteLine("    <fanart>");
            this.WriteToLibrary(Configuration.ImageType.Backdrop);
            this.WriteToLibrary(Configuration.ImageType.SeasonBackdrop);
            nfoStreamWriter.WriteLine("    </fanart>");

            // poster
            nfoStreamWriter.WriteLine("    <poster>");
            this.WriteToLibrary(Configuration.ImageType.Poster);
            this.WriteToLibrary(Configuration.ImageType.SeasonPoster);
            nfoStreamWriter.WriteLine("    </poster>");

            // adding images to BatchFile
            for (int i = 0; i < this.Images.Count; i++)
            {
                ImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.MediaFilePath.Filename != string.Empty && !imageFile.MediaFilePath.WindowsPath.Contains("http://") && imageFile.ImageType != Configuration.ImageType.Unknown)
                {
                    if (!imageFile.Media.GetType().ToString().Contains("Movie") && imageFile.Season != string.Empty && imageFile.Season != "-1" && imageFile.ImageType != Configuration.ImageType.ExtraBackdrop && imageFile.ImageType != Configuration.ImageType.ExtraCover)
                    {
                        bfStreamWriter.WriteLine("cd \"Season " + this.ConvertSeason(imageFile.Season) + "\"");
                    }

                    if (imageFile.ImageType == Configuration.ImageType.ExtraBackdrop)
                    {
                        bfStreamWriter.WriteLine("cd \"extrafanart\"");
                    }

                    if (imageFile.ImageType == Configuration.ImageType.ExtraCover)
                    {
                        bfStreamWriter.WriteLine("cd \"extrathumbs\"");
                    }

                    bfStreamWriter.WriteLine("/bin/cp \"" + imageFile.MediaFilePath.DevicePathForPublication + "\" \"" + imageFile.MediaFilePath.DevicePathToDestination + "\"");

                    if ((!imageFile.Media.GetType().ToString().Contains("Movie") && imageFile.Season != string.Empty && imageFile.Season != "-1") || imageFile.ImageType == Configuration.ImageType.ExtraBackdrop || imageFile.ImageType == Configuration.ImageType.ExtraCover)
                    {
                        bfStreamWriter.WriteLine("cd ..");
                    }

                    // adding season-poster and fanart in base folder
                    if (imageFile.ImageType == Configuration.ImageType.SeasonPoster || imageFile.ImageType == Configuration.ImageType.SeasonBackdrop)
                    {
                        string filename = "season";

                        if (imageFile.Season == "-1")
                        {
                            filename = filename + "-all";
                        }
                        else if (imageFile.Season == "0")
                        {
                            filename = filename + "-specials";
                        }
                        else
                        {
                            filename = filename + imageFile.Media.ConvertSeason(imageFile.Season);
                        }

                        if (imageFile.ImageType == Configuration.ImageType.SeasonPoster)
                        {
                            filename = filename + "-poster";
                        }
                        else if (imageFile.ImageType == Configuration.ImageType.SeasonBackdrop)
                        {
                            filename = filename + "-fanart";
                        }

                        filename = filename + imageFile.MediaFilePath.Extension;

                        bfStreamWriter.WriteLine("/bin/cp \"" + imageFile.MediaFilePath.DevicePathForPublication + "\" \"" + imageFile.MediaFilePath.DevicePathToDestination + "\"");
                    }
                }
            }
        }

        /// <summary>
        /// Clones media object completely
        /// </summary>
        /// <returns>new instance of media object</returns>
        public abstract Media Clone();

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
            this.MediaLanguages = new List<string>
            {
                isoCodeForReplacemant
            };

            foreach (string isoCodeToBeReplaced in isoCodesToBeReplaced)
            {
                this.Title = this.Title.ReplaceAll("(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.Title = this.Title.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                this.TitleSort = this.TitleSort.ReplaceAll("(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.TitleSort = this.TitleSort.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                this.MediaGroup = this.MediaGroup.ReplaceAll("(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.MediaGroup = this.MediaGroup.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                this.MediaPath.Filename = this.MediaPath.Filename.ReplaceAll("(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                this.MediaPath.Filename = this.MediaPath.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                foreach (MediaFile mediaFile in this.MediaFiles)
                {
                    if (!episodeContainsTargetLanguage)
                    {
                        mediaFile.MediaFilePath.Filename = string.Empty;
                    }

                    mediaFile.MediaFilePath.Filename = mediaFile.MediaFilePath.Filename.ReplaceAll("(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeToBeReplaced) + ")", "(" + this.Configuration.CovertLanguageIsoCodeToDescription(isoCodeForReplacemant) + ")");
                    mediaFile.MediaFilePath.Filename = mediaFile.MediaFilePath.Filename.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");

                    mediaFile.MediaFilePath.WindowsPath = mediaFile.MediaFilePath.WindowsPath.ReplaceAll(isoCodeToBeReplaced + mediaFile.MediaFilePath.Extension, isoCodeForReplacemant + mediaFile.MediaFilePath.Extension);
                }
            }
        }

        /// <summary>
        /// Adds reference to Server, if any part of media is stored on it
        /// </summary>
        /// <param name="serverList">Server to be added; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        public virtual void AddServer(Server serverList)
        {
            bool addServer = true;

            foreach (Server currentServerList in this.Server)
            {
                if (currentServerList.Number.Equals(serverList.Number))
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
        /// Adds reference to Server, if any part of media is stored on it
        /// </summary>
        /// <param name="serverList">Server to be added; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        public virtual void AddServer(List<Server> serverList)
        {
            foreach (Server server in serverList)
            {
                this.AddServer(server);
            }
        }

        /// <summary>
        /// converts season according to Kodi-Version
        /// </summary>
        /// <param name="season">season to be prepared for Kodi</param>
        /// <returns>string of season used in Kodi</returns>
        public string ConvertSeason(string season)
        {
            // maybe changed depending on Kodi-Version
            return ("00" + season).Substring(season.ToString().Length);
        }

        /// <summary>
        ///  Writes images to Library, when they meet to specified image type
        /// </summary>
        /// <param name="imageType">Image type, that should be added</param>
        private void WriteToLibrary(Configuration.ImageType imageType)
        {
            for (int i = 0; i < this.Images.Count; i++)
            {
                ImageFile imageFile = this.Images.ElementAt(i);

                if (imageFile.ImageType == imageType)
                {
                    imageFile.WriteToLibrary();
                }
            }
        }

        #endregion
    }
}