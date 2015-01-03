
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
            codec = "AC3";
            language = "Deutsch";
            numberOfChannels = "2";
        }
        #endregion
        #region Properties
        public string Codec
        {
            get { return codec; }
            set { codec = value; }
        }
        public string Language
        {
            get { return language; }
            set { language = value; }
        }
        public string NumberOfChannels
        {
            get { return numberOfChannels; }
            set { numberOfChannels = value; }
        }
        #endregion
        #region Functions

        #endregion
    }
}
