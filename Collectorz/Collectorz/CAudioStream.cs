
namespace Collectorz
{
    public class CAudioStream
    {
        #region Attributes
        private string codec;
        private string language;
        private string numberOfChannels;
        #endregion
        #region Constructor
        public CAudioStream()
        {
            this.codec = "AC3";
            this.language = "Deutsch";
            this.numberOfChannels = "2";
        }
        #endregion
        #region Properties
        public string Codec
        {
            get { return this.codec; }
            set { this.codec = value; }
        }
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        public string NumberOfChannels
        {
            get { return this.numberOfChannels; }
            set { this.numberOfChannels = value; }
        }
        #endregion
        #region Functions

        #endregion
    }
}
