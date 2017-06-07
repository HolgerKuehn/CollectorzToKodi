// <copyright file="Configuration.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Stores all Parameters configured by settings-files.<br/>
    /// <br/>
    /// used settings-files<br/>
    /// settingsMovieCollector.settings - settings related to data storage in MovieCollector<br/>
    /// settingsKodi.settings - settings related to Kodi<br/>
    /// settingsServer.settings - settings related to your local server structure<br/>
    /// </summary>
    public class Configuration
    {
        #region Attributes

        #region Kodi

        /// <summary>
        /// contains name of configured Kodi skin<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <remarks>supported skins: Transparency!, Confluence, Estuary</remarks>
        /// <returns>name of configured Kodi skin</returns>
        private readonly string kodiSkin;

        /// <summary>
        /// sets Export for Movies to Series<br/>
        /// this way all Specials can be accessed separately
        /// </summary>
        private readonly bool kodiExportMovieAsSeries;

        #endregion
        #region MovieCollector

        /// <summary>
        /// string used to mark specials as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        private readonly string movieCollectorSpecials;

        /// <summary>
        /// string used to mark movies in series as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        private readonly string movieCollectorMovies;

        /// <summary>
        /// string to override MPAA-Rating in Disks or Episode
        /// </summary>
        private readonly string movieCollectorMPAARating;

        /// <summary>
        /// string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        private readonly string movieCollectorSeason;

        /// <summary>
        /// string used to define the available languages in this media<br/>
        /// </summary>
        /// <remarks>
        /// string is been parsed for ISO-2-country-codes<br/>
        /// "Language" needs to be a collection of valid ISO-2-country-codes<br/>
        /// the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI<br/>
        /// </remarks>
        /// <returns>string used to mark multi-language-media</returns>
        private readonly string movieCollectorLanguage;

        /// <summary>
        /// local path to CollectorzToKodi-XML-Export including Filename and extension as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export including Filename and extension</returns>
        private readonly string movieCollectorLocalPathToXMLExport;

        /// <summary>
        /// local path to CollectorzToKodi-XML-Export excluding Filename and extension as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export excluding Filename and extension</returns>
        private readonly string movieCollectorLocalPathToXMLExportPath;

        /// <summary>
        /// Filename and extension excluding path to CollectorzToKodi-XML-Export as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>Filename and extension excluding path to CollectorzToKodi-XML-Export</returns>
        private readonly string movieCollectorLocalPathToXMLExportFile;

        #endregion
        #region Server

        /// <summary>
        /// number of server used to store media<br/>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        private readonly int serverNumberOfServers;

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private readonly List<string> serverListOfServers;

        /// <summary>
        /// list of associated drive-letters for server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        private readonly List<string> serverDriveMappingOfServers;

        /// <summary>
        /// list of local paths used on the associated server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        private readonly List<string> serverLocalPathOfServerForMediaStorage;

        /// <summary>
        /// list of local paths used on the associated server names for publication<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names for publication</returns>
        /// </summary>
        private readonly List<string> serverLocalPathOfServerForMediaPublication;

        /// <summary>
        /// type of mapping used during deployment<br/>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        private readonly string serverMappingType;

        /// <summary>
        /// directory used to publish movies to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        /// </summary>
        private readonly string serverMovieDirectory;

        /// <summary>
        /// directory used to publish series to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        /// </summary>
        private readonly string serverSeriesDirectory;

        #endregion
        #region Dictionaries

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>Dictionary used to combine associated drive-letters and server names</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private readonly List<Dictionary<string, string>> serverListsOfServers;

        #endregion
        #region Configuration

        /// <summary>
        /// batch file used to execute on either UNIX or Windows
        /// </summary>
        private readonly List<BatchFile> listOfBatchFiles;

        #endregion
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// initializes configuration with settings-files
        /// </summary>
        public Configuration()
        {
            #region Kodi

            this.kodiSkin = Properties.SettingsKodi.Default.Skin;
            this.kodiExportMovieAsSeries = Properties.SettingsKodi.Default.ExportMovieAsSeries;

            #endregion
            #region MovieCollector

            this.movieCollectorSpecials = Properties.SettingsMovieCollector.Default.Specials;
            this.movieCollectorMovies = Properties.SettingsMovieCollector.Default.Movies;
            this.movieCollectorMPAARating = Properties.SettingsMovieCollector.Default.MPAARating;
            this.movieCollectorSeason = Properties.SettingsMovieCollector.Default.Season;
            this.movieCollectorLanguage = Properties.SettingsMovieCollector.Default.Language;

            this.movieCollectorLocalPathToXMLExport = Properties.SettingsMovieCollector.Default.LocalPathToXMLExport;
            this.movieCollectorLocalPathToXMLExportFile = this.movieCollectorLocalPathToXMLExport.RightOfLast("\\");
            this.movieCollectorLocalPathToXMLExportPath = this.movieCollectorLocalPathToXMLExport.LeftOfLast("\\") + "\\";

            #endregion
            #region Server

            this.serverNumberOfServers = Properties.SettingsServer.Default.NumberOfServer;
            this.serverListOfServers = Properties.SettingsServer.Default.ListOfServer.Split(",");
            this.serverDriveMappingOfServers = Properties.SettingsServer.Default.DriveMappingOfServer.Split(",");
            this.serverLocalPathOfServerForMediaStorage = Properties.SettingsServer.Default.LocalPathOfServerForMediaStorage.Split(",");
            this.serverLocalPathOfServerForMediaPublication = Properties.SettingsServer.Default.LocalPathOfServerForMediaPublication.Split(",");
            this.serverMappingType = Properties.SettingsServer.Default.MappingType;
            this.serverMovieDirectory = Properties.SettingsServer.Default.MovieDirectory;
            this.serverSeriesDirectory = Properties.SettingsServer.Default.SeriesDirectory;

            #endregion
            #region Dictionaries / Configuration

            this.serverListsOfServers = new List<Dictionary<string, string>>();
            this.listOfBatchFiles = new List<BatchFile>();

            int i = 0;
            for (i = 0; i < 6 /* number of possible conversion - defined by enum ListOfServerTypes */; i++)
            {
                this.serverListsOfServers.Add(new Dictionary<string, string>());
            }

            for (i = 0; i < this.ServerNumberOfServers; i++)
            {
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToName].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerListOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToDriveLetter].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerDriveMappingOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToLocalPathForMediaStorage].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerLocalPathOfServerForMediaStorage[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToLocalPathForMediaPublication].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerLocalPathOfServerForMediaPublication[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.DriveLetterToName].Add(this.ServerDriveMappingOfServers[i], this.ServerListOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NameToDriveLetter].Add(this.ServerListOfServers[i], this.ServerDriveMappingOfServers[i]);

                if (this.serverMappingType.Equals("UNIX"))
                {
                    this.listOfBatchFiles.Add(new ShFile(this));
                }

                /*
                else if (this.serverMappingType.Equals("Windows"))
                {
                    listOfBatchFiles.Add(new CmdFile(this));
                }
                */

                this.listOfBatchFiles[i].Server = i;
            }


            #endregion
        }

        #endregion
        #region Enums

        /// <summary>
        /// List of video codecs used for export to Kodi.
        /// </summary>
        public enum VideoCodec
        {
            /// <summary>
            /// specifies video file as TV recording
            /// </summary>
            TV,

            /// <summary>
            /// specifies video as BluRay content
            /// </summary>
            BluRay,

            /// <summary>
            /// video file with H264 codec
            /// </summary>
            H264,

            /// <summary>
            /// video file with H265 codec
            /// </summary>
            H265
        }

        /// <summary>
        /// List of server types handled by this program.
        /// </summary>
        public enum ListOfServerTypes
        {
            /// <summary>
            /// dictionary from number to name
            /// </summary>
            NumberToName,

            /// <summary>
            /// dictionary from number to associated drive letter
            /// </summary>
            NumberToDriveLetter,

            /// <summary>
            /// dictionary from number to local path for media storage in file system
            /// </summary>
            NumberToLocalPathForMediaStorage,

            /// <summary>
            /// dictionary from number to local path for media publication in file system
            /// </summary>
            NumberToLocalPathForMediaPublication,

            /// <summary>
            /// dictionary from associated drive letter to name
            /// </summary>
            DriveLetterToName,

            /// <summary>
            /// dictionary from name to associated drive letter
            /// </summary>
            NameToDriveLetter
        }

        /// <summary>
        /// List of AspectRatios handled by this program.
        /// </summary>
        public enum VideoAspectRatio
        {
            /// <summary>
            /// Fullscreen
            /// </summary>
            AspectRatio43,

            /// <summary>
            /// Widescreen
            /// </summary>
            AspectRatio169,

            /// <summary>
            /// Theatrical Widescreen
            /// </summary>
            AspectRatio219
        }

        /// <summary>
        /// List of Video definitions handled by this program.
        /// </summary>
        public enum VideoDefinition
        {
            /// <summary>
            /// SD content
            /// </summary>
            SD,

            /// <summary>
            /// (Full-) HD content
            /// </summary>
            HD
        }

        /// <summary>
        /// List of image types used for publishing in Kodi
        /// </summary>
        public enum ImageType
        {
            /// <summary>
            /// unknown image type
            /// </summary>
            Unknown,

            /// <summary>
            /// image with front cover
            /// </summary>
            CoverFront,

            /// <summary>
            /// image with back cover
            /// </summary>
            CoverBack,

            /// <summary>
            /// image of poster used for fanart
            /// </summary>
            Poster,

            /// <summary>
            /// image of backdrop used for fanart
            /// </summary>
            Backdrop,

            /// <summary>
            /// image of front cover used for seasons
            /// </summary>
            SeasonCover,

            /// <summary>
            /// image of poster used for seasons
            /// </summary>
            SeasonPoster,

            /// <summary>
            /// image of back drop used for seasons
            /// </summary>
            SeasonBackdrop,

            /// <summary>
            /// image of episode cover
            /// </summary>
            EpisodeCover,

            /// <summary>
            /// additional back drops used for fanart
            /// </summary>
            ExtraBackdrop,

            /// <summary>
            /// additional covers used for fanart
            /// </summary>
            ExtraCover
        }

        /// <summary>
        /// Schemes for SRT-files
        /// </summary>
        public enum SrtSubTitleLineType
        {
            /// <summary>
            /// first line of SRT specifications
            /// number of entry
            /// </summary>
            EntryNumber,

            /// <summary>
            /// second line of SRT specifications
            /// times, when the entry should be displayed
            /// </summary>
            Times,

            /// <summary>
            /// third and following lines of SRT specifications
            /// text displayed
            /// </summary>
            SubTitles,

            /// <summary>
            /// last line of SRT specifications
            /// empty line ending the entry
            /// </summary>
            EmptyLine
        }

        /// <summary>
        /// Gets number of ImageTypes; needs to be updated, when new ImageTypes are indroduced
        /// </summary>
        public int NumberOfImageTypes
        {
            get { return 11; }
        }

        #endregion
        #region Properties
        #region Kodi

        /// <summary>
        /// Gets name of configured Kodi skin<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <remarks>supported skins: Transparency!, Confluence, Estuary</remarks>
        /// <returns>name of configured Kodi skin</returns>
        public string KodiSkin
        {
            get { return this.kodiSkin; }
        }

        /// <summary>
        /// Gets a value indicating whether movies are exported as series or not<br/>
        /// this way all Specials can be accessed separately
        /// </summary>
        public bool KodiExportMovieAsSeries
        {
            get { return this.kodiExportMovieAsSeries; }
        }

        #endregion
        #region MovieCollector

        /// <summary>
        /// Gets string used to mark specials<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        public string MovieCollectorSpecials
        {
            get { return this.movieCollectorSpecials; }
        }

        /// <summary>
        /// Gets string used to mark movies in series<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        public string MovieCollectorMovies
        {
            get { return this.movieCollectorMovies; }
        }

        /// <summary>
        /// Gets string used to mark different MPAA-Rating from MovieData
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific MPAA-Rating<br/>
        /// "MPAARating" needs to be a valid MPAA-Rating for the intended system
        /// </remarks>
        /// <returns>string used to mark different MPAA-Rating</returns>
        public string MovieCollectorMPAARating
        {
            get { return this.movieCollectorMPAARating; }
        }

        /// <summary>
        /// Gets string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// "MPAARating" needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        public string MovieCollectorSeason
        {
            get { return this.movieCollectorSeason; }
        }

        /// <summary>
        /// Gets string used to define the available languages in this media
        /// </summary>
        /// <remarks>
        /// string is been parsed for ISO-2-country-codes<br/>
        /// "Language" needs to be a collection of valid ISO-2-country-codes<br/>
        /// the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI
        /// </remarks>
        /// <returns>string used to mark multi-language-media</returns>
        public string MovieCollectorLanguage
        {
            get { return this.movieCollectorLanguage; }
        }

        /// <summary>
        /// Gets local path to CollectorzToKodi-XML-Export including Filename and extension<br/>
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export including Filename and extension</returns>
        public string MovieCollectorLocalPathToXMLExport
        {
            get { return this.movieCollectorLocalPathToXMLExport; }
        }

        /// <summary>
        /// Gets local path to CollectorzToKodi-XML-Export excluding Filename and extension
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export excluding Filename and extension</returns>
        public string MovieCollectorLocalPathToXMLExportPath
        {
            get { return this.movieCollectorLocalPathToXMLExportPath; }
        }

        /// <summary>
        /// Gets Filename and extension excluding path to CollectorzToKodi-XML-Export<br/>
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>Filename and extension excluding path to CollectorzToKodi-XML-Export</returns>
        public string MovieCollectorLocalPathToXMLExportFile
        {
            get { return this.movieCollectorLocalPathToXMLExportFile; }
        }

        #endregion
        #region Server

        /// <summary>
        /// Gets number of server used to store media<br/>
        /// </summary>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        public int ServerNumberOfServers
        {
            get { return this.serverNumberOfServers; }
        }

        /// <summary>
        /// Gets type of mapping used during deployment<br/>
        /// </summary>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        public string ServerMappingType
        {
            get { return this.serverMappingType; }
        }

        /// <summary>
        /// Gets directory used to publish movies to Kodi<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        public string ServerMovieDirectory
        {
            get { return this.serverMovieDirectory; }
        }

        /// <summary>
        /// Gets directory used to publish series to Kodi
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        public string ServerSeriesDirectory
        {
            get { return this.serverSeriesDirectory; }
        }

        #endregion
        #region Configuration

        /// <summary>
        /// Gets batch file used to execute commands on either UNIX or Windows<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <returns>current batch file</returns>
        public List<BatchFile> ListOfBatchFiles
        {
            get { return this.listOfBatchFiles; }
        }

        #endregion
        #region Dictionaries

        /// <summary>
        /// Gets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
        public List<Dictionary<string, string>> ServerListsOfServers
        {
            get { return this.serverListsOfServers; }
        }

        /// <summary>
        /// Gets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        private List<string> ServerListOfServers
        {
            get { return this.serverListOfServers; }
        }

        /// <summary>
        /// Gets list of associated drive-letters for server names to store media
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        private List<string> ServerDriveMappingOfServers
        {
            get { return this.serverDriveMappingOfServers; }
        }

        /// <summary>
        /// Gets list of local paths used on the associated server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        private List<string> ServerLocalPathOfServerForMediaStorage
        {
            get { return this.serverLocalPathOfServerForMediaStorage; }
        }

        /// <summary>
        /// Gets list of local paths used on the associated server names for publication<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names for publication</returns>
        /// </summary>
        private List<string> ServerLocalPathOfServerForMediaPublication
        {
            get { return this.serverLocalPathOfServerForMediaPublication; }
        }

        #endregion
        #endregion
        #region Methods

        /// <summary>
        /// converts 2 letter ISO-Code to description
        /// </summary>
        /// <param name="isoCode">2 letter ISO-Code</param>
        /// <returns>language name defined by ISO-Code</returns>
        public static string CovertLanguageIsoCodeToDescription(string isoCode)
        {
            switch (isoCode)
            {
                case "de":
                default: return "deutsch";
                case "en": return "englisch";
                case "jp": return "japanisch";
            }
        }

        #endregion
    }
}
