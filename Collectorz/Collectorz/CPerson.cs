/// <summary>
/// Namespace for managing .nfo-export from Collectorz-Programs <br/>
/// </summary>
namespace Collectorz
{
    /// <summary>
    /// class to manage persons from Collectorz-Programs<br/>
    /// </summary>
    public class CPerson
    {
        #region Attributes
        /// <summary>
        /// full name displayed in Kodi<br/>
        /// </summary>
        private string name;
        /// <summary>
        /// link to thumb of person<br/>
        /// can hold url or local-file link to thumb<br/>
        /// </summary>
        private string thumb;
        #endregion
        #region Constructor
        /// <summary>
        /// initializes person with blank values<br/>
        /// </summary>
        public CPerson()
        {
            this.name = "";
            this.thumb = "";
        }
        #endregion
        #region Properties
        /// <summary>
        /// full name displayed in Kodi<br/>
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        /// <summary>
        /// link to thumb of person<br/>
        /// can hold url or local-file link to thumb<br/>
        /// </summary>
        public string Thumb
        {
            get { return this.thumb; }
            set { this.thumb = value; }
        }
        #endregion
    }
}
