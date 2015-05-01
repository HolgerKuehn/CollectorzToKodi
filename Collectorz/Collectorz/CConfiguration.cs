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
        /// string to override MPAA-Rating in Disks or Episode
        /// </summary>
        private string movieCollectorMPAARating;
        /// <summary>
        /// <para>string used to define the Season Episode should be shown in</para>
        /// <remarks>
        ///     <para>string is replaced in disks or titles and defines all elements with specific Seasons<para>
        ///     <para><MPAARating> needs to be a valid number</para>
        /// </remarks>string used to mark Seasons<returns>
        /// </summary>
        public string movieCollectorSeason;
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
            movieCollectorMPAARating = Properties.SettingsMovieCollector.Default.MPAARating;
            movieCollectorSeason = Properties.SettingsMovieCollector.Default.Season;
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

        #endregion
        #endregion
    }
}
