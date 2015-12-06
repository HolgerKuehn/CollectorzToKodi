// <copyright file="CConfiguration.cs" company="Holger Kühn">
// Copyright (c) Holger Kühn. All rights reserved.
// </copyright>

namespace Collectorz
{
    using System.Collections.Generic;

    /// <summary>
    /// Stores all Parameters configured by settings-files.<br/>
    /// <br/>
    /// used settings-files<br/>
    /// settingsMovieCollector.settings - settings related to data storage in MovieCollector<br/>
    /// settingsKodi.settings - settings related to Kodi<br/>
    /// settingsServer.settings - settings related to your local server structure<br/>
    /// </summary>
    public class CConfiguration
    {
        #region Attributes

        #region Kodi

        /// <summary>
        /// contains name of configured Kodi skin<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <remarks>supported skins: Transparency!, Confluence</remarks>
        /// <returns>name of configured Kodi skin</returns>
        private string kodiSkin;

        #endregion
        #region MovieCollector

        /// <summary>
        /// string used to mark specials<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        private string movieCollectorSpecials;

        /// <summary>
        /// string used to mark movies in series<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        private string movieCollectorMovies;

        /// <summary>
        /// string to override MPAA-Rating in Disks or Episode
        /// </summary>
        private string movieCollectorMPAARating;

        /// <summary>
        /// string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        private string movieCollectorSeason;

        /// <summary>
        /// string used to define the available languages in this media<br/>
        /// </summary>
        /// <remarks>
        /// string is been parsed for ISO-2-country-codes<br/>
        /// "Language" needs to be a collection of valid ISO-2-country-codes<br/>
        /// the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI<br/>
        /// </remarks>
        /// <returns>string used to mark multi-language-media</returns>
        private string movieCollectorLanguage;

        /// <summary>
        /// local path to Collectorz-XML-Export including Filename and extension<br/>
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to Collectorz-XML-Export including Filename and extension</returns>
        private string movieCollectorLocalPathToXMLExport;

        /// <summary>
        /// local path to Collectorz-XML-Export excluding Filename and extension<br/>
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>local path to Collectorz-XML-Export excluding Filename and extension</returns>
        private string movieCollectorLocalPathToXMLExportPath;

        /// <summary>
        /// Filename and extension excluding path to Collectorz-XML-Export
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>Filename and extension excluding path to Collectorz-XML-Export</returns>
        private string movieCollectorLocalPathToXMLExportFile;

        #endregion
        #region Server

        /// <summary>
        /// number of server used to store media<br/>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        private int serverNumberOfServers;

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<string> serverListOfServers;

        /// <summary>
        /// list of associated drive-letters for server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        private List<string> serverDriveMappingOfServers;

        /// <summary>
        /// list of local paths used on the associated server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        private List<string> serverLocalPathOfServerForMediaStorage;

        /// <summary>
        /// type of mapping used during deployment<br/>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        private string serverMappingType;

        /// <summary>
        /// directory used to publish movies to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        /// </summary>
        private string serverMovieDirectory;

        /// <summary>
        /// directory used to publish series to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        /// </summary>
        private string serverSeriesDirectory;

        #endregion
        #region Dictionaries

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>Dictionary used to combine associated drive-letters and server names</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<Dictionary<string, string>> serverListsOfServers;

        #endregion

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CConfiguration"/> class.
        /// initializes configuration with settings-files
        /// </summary>
        public CConfiguration()
        {
            #region Kodi

            this.kodiSkin = Properties.settingsKodi.Default.Skin;

            #endregion
            #region MovieCollector

            this.movieCollectorSpecials = Properties.settingsMovieCollector.Default.Specials;
            this.movieCollectorMovies = Properties.settingsMovieCollector.Default.Movies;
            this.movieCollectorMPAARating = Properties.settingsMovieCollector.Default.MPAARating;
            this.movieCollectorSeason = Properties.settingsMovieCollector.Default.Season;
            this.movieCollectorLanguage = Properties.settingsMovieCollector.Default.Language;

            this.movieCollectorLocalPathToXMLExport = Properties.settingsMovieCollector.Default.LocalPathToXMLExport;
            this.movieCollectorLocalPathToXMLExportFile = this.movieCollectorLocalPathToXMLExport.RightOfLast("\\");
            this.movieCollectorLocalPathToXMLExportPath = this.movieCollectorLocalPathToXMLExport.LeftOfLast("\\") + "\\";

            #endregion
            #region Server

            this.serverNumberOfServers = Properties.settingsServer.Default.NumberOfServer;
            this.serverListOfServers = Properties.settingsServer.Default.ListOfServer.Split(",");
            this.serverDriveMappingOfServers = Properties.settingsServer.Default.DriveMappingOfServer.Split(",");
            this.serverLocalPathOfServerForMediaStorage = Properties.settingsServer.Default.LocalPathOfServerForMediaStorage.Split(",");
            this.serverMappingType = Properties.settingsServer.Default.MappingType;
            this.serverMovieDirectory = Properties.settingsServer.Default.MovieDirectory;
            this.serverSeriesDirectory = Properties.settingsServer.Default.SeriesDirectory;

            #endregion
            #region Dictionaries

            List<Dictionary<string, string>> listOfServers = new List<Dictionary<string, string>>();
            int i = 0;
            for (i = 0; i < 5 /* number of possible conversion - defined by enum ListOfServerTypes */; i++)
            {
                listOfServers.Add(new Dictionary<string, string>());
            }

            for (i = 0; i < this.ServerNumberOfServers; i++)
            {
                listOfServers[(int)ListOfServerTypes.NumberToName].Add(i.ToString(), this.ServerListOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NumberToDriveLetter].Add(i.ToString(), this.ServerDriveMappingOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NumberToLocalPath].Add(i.ToString(), this.ServerLocalPathOfServerForMediaStorage[i]);
                listOfServers[(int)ListOfServerTypes.DriveLetterToName].Add(this.ServerDriveMappingOfServers[i], this.ServerListOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NameToDriveLetter].Add(this.ServerListOfServers[i], this.ServerDriveMappingOfServers[i]);
            }

            this.serverListsOfServers = listOfServers;

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
            /// dictionary from number to local path in filesystem
            /// </summary>
            NumberToLocalPath,

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
            AspectRatio_4_3,

            /// <summary>
            /// Widescreen
            /// </summary>
            AspectRatio_16_9,

            /// <summary>
            /// Theatrical Widescreen
            /// </summary>
            AspectRatio_21_9
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

        #endregion
        #region Properties

        #region Kodi

        /// <summary>
        /// Gets or sets name of configured Kodi skin<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <remarks>supported skins: Transparency!, Confluence</remarks>
        /// <returns>name of configured Kodi skin</returns>
        public string KodiSkin
        {
            get { return this.kodiSkin; }
            set { }
        }

        #endregion
        #region MovieCollector

        /// <summary>
        /// Gets or sets string used to mark specials<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        public string MovieCollectorSpecials
        {
            get { return this.movieCollectorSpecials; }
            set { }
        }

        /// <summary>
        /// Gets or sets string used to mark movies in series<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        public string MovieCollectorMovies
        {
            get { return this.movieCollectorMovies; }
            set { }
        }

        /// <summary>
        /// Gets or sets string used to mark different MPAA-Rating from MovieData
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific MPAA-Rating<br/>
        /// "MPAARating" needs to be a valid MPAA-Rating for the intended system
        /// </remarks>
        /// <returns>string used to mark different MPAA-Rating</returns>
        public string MovieCollectorMPAARating
        {
            get { return this.movieCollectorMPAARating; }
            set { }
        }

        /// <summary>
        /// Gets or sets string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// "MPAARating" needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        public string MovieCollectorSeason
        {
            get { return this.movieCollectorSeason; }
            set { }
        }

        /// <summary>
        /// Gets or sets string used to define the available languages in this media
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
            set { }
        }

        /// <summary>
        /// Gets or sets local path to Collectorz-XML-Export including Filename and extension<br/>
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to Collectorz-XML-Export including Filename and extension</returns>
        public string MovieCollectorLocalPathToXMLExport
        {
            get { return this.movieCollectorLocalPathToXMLExport; }
            set { }
        }

        /// <summary>
        /// Gets or sets local path to Collectorz-XML-Export excluding Filename and extension
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>local path to Collectorz-XML-Export excluding Filename and extension</returns>
        public string MovieCollectorLocalPathToXMLExportPath
        {
            get { return this.movieCollectorLocalPathToXMLExportPath; }
            set { }
        }

        /// <summary>
        /// Gets or sets Filename and extension excluding path to Collectorz-XML-Export<br/>
        /// </summary>
        /// <remarks>is generated from movieCollectorLocalPathToXMLExport</remarks>
        /// <returns>Filename and extension excluding path to Collectorz-XML-Export</returns>
        public string MovieCollectorLocalPathToXMLExportFile
        {
            get { return this.movieCollectorLocalPathToXMLExportFile; }
            set { }
        }

        #endregion
        #region Server

        /// <summary>
        /// Gets or sets number of server used to store media<br/>
        /// </summary>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        public int ServerNumberOfServers
        {
            get { return this.serverNumberOfServers; }
            set { }
        }

        /// <summary>
        /// Gets or sets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        private List<string> ServerListOfServers
        {
            get { return this.serverListOfServers; }
            set { }
        }

        /// <summary>
        /// Gets or sets list of associated drive-letters for server names to store media
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        private List<string> ServerDriveMappingOfServers
        {
            get { return this.serverDriveMappingOfServers; }
            set { }
        }

        /// <summary>
        /// Gets or sets list of local paths used on the associated server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        private List<string> ServerLocalPathOfServerForMediaStorage
        {
            get { return this.serverLocalPathOfServerForMediaStorage; }
            set { }
        }

        #pragma warning disable SA1202 // ElementsMustBeOrderedByAccess
        /// <summary>
        /// Gets or sets type of mapping used during deployment<br/>
        /// </summary>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        public string ServerMappingType
        {
            get { return this.serverMappingType; }
            set { }
        }

        /// <summary>
        /// Gets or sets directory used to publish movies to Kodi<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        public string ServerMovieDirectory
        {
            get { return this.serverMovieDirectory; }
            set { }
        }

        /// <summary>
        /// Gets or sets directory used to publish series to Kodi
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        public string ServerSeriesDirectory
        {
            get { return this.serverSeriesDirectory; }
            set { }
        }

        #endregion
        #region Dictionaries

        /// <summary>
        /// Gets or sets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
        public List<Dictionary<string, string>> ServerListsOfServers
        {
            get { return this.serverListsOfServers; }
            set { }
        }
        #pragma warning restore  SA1202 // ElementsMustBeOrderedByAccess

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
                case "de": return "deutsch";
                case "en": return "englisch";
                case "jp": return "japanisch";
                default: return "deutsch";
            }
        }

        #endregion
    }
}
