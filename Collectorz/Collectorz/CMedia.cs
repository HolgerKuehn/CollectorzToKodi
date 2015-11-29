/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    public abstract class CMedia
    {
        #region Attributes
        private CConfiguration configuration;
        #endregion
        #region Constructor
        public CMedia(CConfiguration configuration)
        {
            this.configuration = configuration;
        }
        #endregion
        #region Properties
        public CConfiguration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }
        #endregion
        #region Functions
        #endregion
    }
}
