/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
namespace Collectorz
{
    /// <summary>
    /// <para>class to mange Actors</para>
    /// </summary>
    public class CActor : CPerson
    {
        #region Attributes
        /// <summary>
        /// <para>role played by actor</para>
        /// </summary>
        private string role;
        #endregion
        #region Constructor
        /// <summary>
        /// <para>initializes actor with blank values</para>
        /// </summary>
        public CActor() : base()
        {
            this.role = "";
        }
        #endregion
        #region Properties
        /// <summary>
        /// <para>role played by actor</para>
        /// </summary>
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }
        #endregion
    }
}