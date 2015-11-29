/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    /// <summary>
    /// <para>class to manage persons from Collectorz-Programs</para>
    /// </summary>
    public class CPerson
    {
        #region Attributes
        /// <summary>
        /// <para>full name displayed in Kodi</para>
        /// </summary>
        private string name;
        /// <summary>
        /// <para>link to thumb of person</para>
        /// <para>can hold url or local-file link to thumb</para>
        /// </summary>
        private string thumb;
        #endregion
        #region Constructor
        /// <summary>
        /// <para>initializes person with blank values</para>
        /// </summary>
        public CPerson()
        {
            this.name = "";
            this.thumb = "";
        }
        #endregion
        #region Properties
        /// <summary>
        /// <para>full name displayed in Kodi</para>
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        /// <summary>
        /// <para>link to thumb of person</para>
        /// <para>can hold url or local-file link to thumb</para>
        /// </summary>
        public string Thumb
        {
            get { return this.thumb; }
            set { this.thumb = value; }
        }
        #endregion
    }
}
