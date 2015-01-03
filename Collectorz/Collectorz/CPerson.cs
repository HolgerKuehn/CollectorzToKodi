
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
            name = "";
            role = "";
            thumb = "";
        }
        #endregion
        #region Properties
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        public string Role
        {
            get { return role; }
            set { role = value; }
        }
        public string Thumb
        {
            get { return thumb; }
            set { thumb = value; }
        }
        #endregion
    }
}
