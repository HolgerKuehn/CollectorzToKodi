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
        private List<string> serverDriveMappingOfServer;
        /// <summary>
        /// <para>type of mapping used during deployment</para>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        private string serverMappingType;
        #endregion
        #region Dictionaries
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>Dictionary used to combine associated drive-letters and server names</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private List<Dictionary<string, string>> listOfServers;
        #endregion
        #endregion
        #region Constructor
        /// <summary>
        /// initializes configuration with settings-files
        /// </summary>
        public CConfiguration()
        {
            #region Kodi
            kodiSkin = Properties.SettingsKodi.Default.Skin;
            #endregion
            #region MovieCollector
            movieCollectorSpecials = Properties.SettingsMovieCollector.Default.Specials;
            movieCollectorMovies = Properties.SettingsMovieCollector.Default.Movies;
            movieCollectorMPAARating = Properties.SettingsMovieCollector.Default.MPAARating;
            movieCollectorSeason = Properties.SettingsMovieCollector.Default.Season;
            movieCollectorLanguage = Properties.SettingsMovieCollector.Default.Language;
            #endregion
            #region Server
            serverNumberOfServers = Properties.SettingsServer.Default.NumberOfServer;
            serverListOfServers = Properties.SettingsServer.Default.ListOfServer._Split(",");
            serverDriveMappingOfServer = Properties.SettingsServer.Default.DriveMappingOfServer._Split(",");
            serverMappingType = Properties.SettingsServer.Default.MappingType;
            #endregion
            #region Dictionaries
            listOfServers = new List<Dictionary<string, string>>();
            listOfServers.Add(new Dictionary<string, string>()); // conversion number       to name
            listOfServers.Add(new Dictionary<string, string>()); // conversion number       to drive-letter
            listOfServers.Add(new Dictionary<string, string>()); // conversion drive-letter to name
            listOfServers.Add(new Dictionary<string, string>()); // conversion name         to drive-letter
            
            for (int i = 0; i < ServerNumberOfServer; i++)
            {
                listOfServers[0].Add(i.ToString(), ServerListOfServers[i]);                  // conversion number       to name
                listOfServers[1].Add(i.ToString(), ServerDriveMappingOfServer[i]);           // conversion number       to drive-letter
                listOfServers[2].Add(ServerDriveMappingOfServer[i], ServerListOfServers[i]); // conversion drive-letter to name
                listOfServers[3].Add(ServerListOfServers[i], ServerDriveMappingOfServer[i]); // conversion name         to drive-letter
            }

            ListOfServers = listOfServers;
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
            get { return kodiSkin; }
            set { kodiSkin = Properties.SettingsKodi.Default.Skin; }
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
            get { return movieCollectorSpecials; }
            set { movieCollectorSpecials = Properties.SettingsMovieCollector.Default.Specials; }
        }
        /// <summary>
        /// <para>string used to mark movies in series</para>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        /// </summary>
        public string MovieCollectorMovies
        {
            get { return movieCollectorMovies; }
            set { movieCollectorMovies = Properties.SettingsMovieCollector.Default.Movies; }
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
            get { return movieCollectorMPAARating; }
            set { movieCollectorMPAARating = Properties.SettingsMovieCollector.Default.MPAARating; }
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
            get { return movieCollectorSeason; }
            set { movieCollectorSeason = Properties.SettingsMovieCollector.Default.Season; }
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
            get { return movieCollectorLanguage; }
            set { movieCollectorLanguage = Properties.SettingsMovieCollector.Default.Movies; }
        }
        #endregion
        #region Server
        /// <summary>
        /// <para>number of server used to store media</para>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        public int ServerNumberOfServer
        {
            get { return serverNumberOfServers; }
            set { serverNumberOfServers = Properties.SettingsServer.Default.NumberOfServer; }
        }
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        public List<string> ServerListOfServers
        {
            get { return serverListOfServers; }
            set { serverListOfServers = Properties.SettingsServer.Default.ListOfServer._Split(","); }
        }
        /// <summary>
        /// <para>list of associated drive-letters for server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        public List<string> ServerDriveMappingOfServer
        {
            get { return serverDriveMappingOfServer; }
            set { serverDriveMappingOfServer = Properties.SettingsServer.Default.DriveMappingOfServer._Split(","); }
        }
        /// <summary>
        /// <para>type of mapping used during deployment</para>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        public string ServerMappingType
        {
            get { return serverMappingType; }
            set { serverMappingType = Properties.SettingsServer.Default.MappingType; }
        }
        #endregion
        #region Dictionaries
        /// <summary>
        /// <para>list of server names to store media</para>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        public List<Dictionary<string, string>> ListOfServers
        {
            get { return listOfServers; }
            set { listOfServers = value; }
        }
        #endregion
        #endregion
    }
}
