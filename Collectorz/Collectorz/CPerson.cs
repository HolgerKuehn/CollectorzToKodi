
namespace Collectorz
{
    public class CPerson
    {
        #region Attributes
        private string name;
        private string role;
        private string thumb;
        #endregion
        #region Constructor
        public CPerson()
        {
            this.name = "";
            this.role = "";
            this.thumb = "";
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }
        public string Role
        {
            get { return this.role; }
            set { this.role = value; }
        }
        public string Thumb
        {
            get { return this.thumb; }
            set { this.thumb = value; }
        }
        #endregion
    }
}
