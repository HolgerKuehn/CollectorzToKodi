using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Collectorz
{
    /// <summary>
    /// <para>stores all Parameters configured by settings-files</para>
    /// <para></para>
    /// <para>used settings-files</para>
    /// <para>SettingsMovieCollector.settings - Settings related to data storage in MovieCollector</para>
    /// <para>SettingsKodi.settings - Settings related to Kodi</para>
    /// <para>SettingsServer.settings - Settings related to your local server structure</para>
    /// </summary>
    public class CConfiguration
    {
        #region Enums
        public enum VideoCodec
        {
            TV,
            BluRay,
            H264,
            H265
        }
        public enum ListOfServerTypes
        {
            NumberToName,
            NumberToDriveLetter,
            NumberToLocalPath,
            DriveLetterToName,
            NameToDriveLetter
        }
        public enum VideoAspectRatio
        {
            AspectRatio_4_3,
            AspectRatio_16_9,
            AspectRatio_21_9
        };
        public enum VideoDefinition
        {
            SD,
            HD
        };
        public enum ImageType
        {
            unknown,
            CoverFront,
            CoverBack,
            Poster,
            Backdrop,
            SeasonCover,
            SeasonBackdrop,
            EpisodeCover,
            ExtraBackdrop,
            ExtraCover
        }
        public enum SrtSubTitleLineType
        {
            entryNumber,
            times,
            subTitles,
            emptyLine
        };
        #endregion
        #region Attributes
        #region Kodi
        /// <summary>
        /// <para>contains name of configured Kodi skin</para>
        /// <para>this is used to set some attributes to skin-specific values</para>
        /// <remarks>supported skins: Transparency!, Confluence</remarks>
        /// <returns>name of configured Kodi skin</returns>
        /// </summary>
        private string kodiSkin;
        #endregion
        #region MovieCollector
        /// <summary>
        /// <para>string used to mark specials</para>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        /// </summary>
        private string movieCollectorSpecials;
        /// <summary>
        /// <para>string used to mark movies in series</para>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        /// </summary>
        private string movieCollectorMovies;
        /// <summary>
        /// string to override MPAA-Rating in Disks or Episode
        /// </summary>
        private string movieCollectorMPAARating;
        /// <summary>
        /// <para>string used to define the Season Episode should be shown in</para>
        /// <remarks>
        ///     <para>string is replaced in disks or titles and defines all elements with specific Seasons<para>
        ///     <para><Season> needs to be a valid number</para>
        /// </remarks>
        /// <returns>string used to mark Seasons<returns>
        /// </summary>
        private string movieCollectorSeason;
        /// <summary>
        /// <para>string used to define the available languages in this media</para>
        /// <remarks>
        ///     <para>string is been parsed for ISO-2-country-codes<para>
        ///     <para><Language> needs to be a collection of valid ISO-2-country-codes</para>
        ///     <para>the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI</para>
        /// </remarks>
        /// <returns>string used to mark multi-language-media<returns>
        /// </summary>
        private string movieCollectorLanguage;
        /// <summary>
        /// <para>local path to Collectorz-XML-Export including Filename and extension</para>
        /// <remarks>
        ///     <para>filename needs to end with ".xml"</para>
        /// </remarks>
        /// <returns>local path to Collectorz-XML-Export including Filename and extension<returns>
        /// </summary>
        private string movieCollectorLocalPathToXMLExport;
        /// <summary>
        /// <para>local path to Collectorz-XML-Export excluding Filename and extension</para>
        /// <remarks>
        ///     <para>is generated from movieCollectorLocalPathToXMLExport</para>
        /// </remarks>
        /// <returns>local path to Collectorz-XML-Export excluding Filename and extension<returns>
        /// </summary>
        private string movieCollectorLocalPathToXMLExportPath;
        /// <summary>
        /// <para>Filename and extension excluding path to Collectorz-XML-Export</para>
        /// <remarks>
        ///     <para>is generated from movieCollectorLocalPathToXMLExport</para>
        /// </remarks>
        /// <returns>Filename and extension excluding path to Collectorz-XML-Export<returns>
        /// </summary>
        private string movieCollectorLocalPathToXMLExportFile;
        #endregion
        #region Server
        /// <summary>
        /// <para>number of server used to store media</para>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        private int serverNumberOfServers;
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<string> serverListOfServers;
        /// <summary>
        /// <para>list of associated drive-letters for server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        private List<string> serverDriveMappingOfServers;
        /// <summary>
        /// <para>list of local paths used on the associated server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        private List<string> serverLocalPathOfServerForMediaStorage;
        /// <summary>
        /// <para>list of local paths used on the associated server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        //private List<string> serverLocalPathOfServerForMediaStorage; // MediaPublication
        /// <summary>
        /// <para>type of mapping used during deployment</para>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        private string serverMappingType;
        /// <summary>
        /// <para>directory used to publish movies to Kodi</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        /// </summary>
        private string serverMovieDirectory;
        /// <summary>
        /// <para>directory used to publish series to Kodi</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        /// </summary>
        private string serverSeriesDirectory;
        #endregion
        #region Dictionaries
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>Dictionary used to combine associated drive-letters and server names</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<Dictionary<string, string>> serverListsOfServers;
        #endregion
        #endregion
        #region Constructor
        /// <summary>
        /// initializes configuration with settings-files
        /// </summary>
        public CConfiguration()
        {
            #region Kodi
            this.kodiSkin = Properties.SettingsKodi.Default.Skin;
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
            this.serverMappingType = Properties.SettingsServer.Default.MappingType;
            this.serverMovieDirectory = Properties.SettingsServer.Default.MovieDirectory;
            this.serverSeriesDirectory = Properties.SettingsServer.Default.SeriesDirectory;
            #endregion
            #region Dictionaries
            List<Dictionary<string, string>> listOfServers = new List<Dictionary<string, string>>();
            int i = 0;
            for (i = 0; i < 5 /* number of possible conversion - defined by enum ListOfServerTypes */; i++)
                listOfServers.Add(new Dictionary<string, string>());

            for (i = 0; i < this.ServerNumberOfServers; i++)
            {
                listOfServers[(int)ListOfServerTypes.NumberToName].Add(i.ToString(), ServerListOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NumberToDriveLetter].Add(i.ToString(), ServerDriveMappingOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NumberToLocalPath].Add(i.ToString(), ServerLocalPathOfServerForMediaStorage[i]);
                listOfServers[(int)ListOfServerTypes.DriveLetterToName].Add(ServerDriveMappingOfServers[i], ServerListOfServers[i]);
                listOfServers[(int)ListOfServerTypes.NameToDriveLetter].Add(ServerListOfServers[i], ServerDriveMappingOfServers[i]);
            }

            this.serverListsOfServers = listOfServers;
            #endregion
        }
        #endregion
        #region Properties
        #region Kodi
        /// <summary>
        /// <para>contains name of configured Kodi skin</para>
        /// <para>this is used to set some attributes to skin-specific values</para>
        /// <remarks>supported skins: Transparency!, Confluence</remarks>
        /// <returns>name of configured Kodi skin</returns>
        /// </summary>
        public string KodiSkin
        {
            get { return this.kodiSkin; }
            set { }
        }
        #endregion
        #region MovieCollector
        /// <summary>
        /// <para>string used to mark specials</para>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        /// </summary>
        public string MovieCollectorSpecials
        {
            get { return this.movieCollectorSpecials; }
            set { }
        }
        /// <summary>
        /// <para>string used to mark movies in series</para>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        /// </summary>
        public string MovieCollectorMovies
        {
            get { return this.movieCollectorMovies; }
            set { }
        }
        /// <summary>
        /// <para>string used to mark different MPAA-Rating from MovieData</para>
        /// <remarks>
        ///     <para>string is replaced in disks or titles and defines all elements with specific MPAA-Rating<para>
        ///     <para><MPAARating> needs to be a valid MPAA-Rating for the intended system</para>
        /// </remarks>
        /// <returns>string used to mark different MPAA-Rating</returns>
        /// </summary>
        public string MovieCollectorMPAARating
        {
            get { return this.movieCollectorMPAARating; }
            set { }
        }
        /// <summary>
        /// <para>string used to define the Season Episode should be shown in</para>
        /// <remarks>
        ///     <para>string is replaced in disks or titles and defines all elements with specific Seasons<para>
        ///     <para><MPAARating> needs to be a valid number</para>
        /// </remarks>string used to mark Seasons<returns>
        /// </summary>
        public string MovieCollectorSeason
        {
            get { return this.movieCollectorSeason; }
            set { }
        }
        /// <summary>
        /// <para>string used to define the available languages in this media</para>
        /// <remarks>
        ///     <para>string is been parsed for ISO-2-country-codes<para>
        ///     <para><Language> needs to be a collection of valid ISO-2-country-codes</para>
        ///     <para>the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI</para>
        /// </remarks>
        /// <returns>string used to mark multi-language-media<returns>
        /// </summary>
        public string MovieCollectorLanguage
        {
            get { return this.movieCollectorLanguage; }
            set { }
        }
        /// <summary>
        /// <para>local path to Collectorz-XML-Export including Filename and extension</para>
        /// <remarks>
        ///     <para>filename needs to end with ".xml"</para>
        /// </remarks>
        /// <returns>local path to Collectorz-XML-Export including Filename and extension<returns>
        /// </summary>
        public string MovieCollectorLocalPathToXMLExport
        {
            get { return this.movieCollectorLocalPathToXMLExport; }
            set { }
        }
        /// <summary>
        /// <para>local path to Collectorz-XML-Export excluding Filename and extension</para>
        /// <remarks>
        ///     <para>is generated from movieCollectorLocalPathToXMLExport</para>
        /// </remarks>
        /// <returns>local path to Collectorz-XML-Export excluding Filename and extension<returns>
        /// </summary>
        public string MovieCollectorLocalPathToXMLExportPath
        {
            get { return this.movieCollectorLocalPathToXMLExportPath; }
            set { }
        }
        /// <summary>
        /// <para>Filename and extension excluding path to Collectorz-XML-Export</para>
        /// <remarks>
        ///     <para>is generated from movieCollectorLocalPathToXMLExport</para>
        /// </remarks>
        /// <returns>Filename and extension excluding path to Collectorz-XML-Export<returns>
        /// </summary>
        public string MovieCollectorLocalPathToXMLExportFile
        {
            get { return this.movieCollectorLocalPathToXMLExportFile; }
            set { }
        }
        #endregion
        #region Server
        /// <summary>
        /// <para>number of server used to store media</para>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        public int ServerNumberOfServers
        {
            get { return this.serverNumberOfServers; }
            set { }
        }
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<string> ServerListOfServers
        {
            get { return this.serverListOfServers; }
            set { }
        }
        /// <summary>
        /// <para>list of associated drive-letters for server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        private List<string> ServerDriveMappingOfServers
        {
            get { return this.serverDriveMappingOfServers; }
            set { }
        }
        /// <summary>
        /// <para>list of local paths used on the associated server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        private List<string> ServerLocalPathOfServerForMediaStorage
        {
            get { return this.serverLocalPathOfServerForMediaStorage; }
            set { }
        }
        /// <summary>
        /// <para>type of mapping used during deployment</para>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        public string ServerMappingType
        {
            get { return this.serverMappingType; }
            set { }
        }
        /// <summary>
        /// <para>directory used to publish movies to Kodi</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        /// </summary>
        public string ServerMovieDirectory
        {
            get { return this.serverMovieDirectory; }
            set { }
        }
        /// <summary>
        /// <para>directory used to publish series to Kodi</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        /// </summary>
        public string ServerSeriesDirectory
        {
            get { return this.serverSeriesDirectory; }
            set { }
        }
        #endregion
        #region Dictionaries
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        public List<Dictionary<string, string>> ServerListsOfServers
        {
            get { return this.serverListsOfServers; }
            set { }
        }
        #endregion
        #endregion
        #region Methods
        public static string covertLanguageIsoCodeToDescription(string isoCode)
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
