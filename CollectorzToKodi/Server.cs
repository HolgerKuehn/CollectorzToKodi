// <copyright file="Server.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// provides information about storage paths on Windows running Collectorz and the hardwareplattform storing the media
    /// </summary>
    public class Server
    {
        #region Attributes

        /// <summary>
        /// current configuration of CollectorzToKodi
        /// </summary>
        private Configuration configuration;

        /// <summary>
        /// media containing file
        /// </summary>
        private Media media;

        /// <summary>
        /// number of server
        /// </summary>
        private int server;

        /// <summary>
        /// windows path to destination
        /// </summary>
        private string windowsPathToDestination;

        /// <summary>
        /// device path to destination
        /// </summary>
        private string devicePathToDestination;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        /// <param name="server">Number of Server represented by object</param>
        public Server(Configuration configuration, int server)
        {
            this.configuration = configuration;
            this.media = null;
            this.server = server;

            string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][server.ToString()];
            this.windowsPathToDestination = driveLetter + ":\\";

            string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDeviceDestinationPath][server.ToString()];
            this.devicePathToDestination = this.windowsPathToDestination.Replace(driveLetter.Trim() + ":", localPath);

            if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
            {
                this.devicePathToDestination = this.devicePathToDestination.ReplaceAll("\\", "/");
            }
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets current configuration of CollectorzToKodi
        /// </summary>
        public Configuration Configuration
        {
            get { return this.configuration; }
            set { this.configuration = value; }
        }

        /// <summary>
        /// Gets or sets media containing file
        /// </summary>
        public virtual Media Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        /// <summary>
        /// Gets or sets windows path to destination
        /// </summary>
        public string WindowsPathToDestination
        {
            get { return this.windowsPathToDestination; }
            set { this.windowsPathToDestination = value; }
        }

        /// <summary>
        /// Gets or sets device path to destination
        /// </summary>
        public string DevicePathToDestination
        {
            get { return this.devicePathToDestination; }
            set { this.devicePathToDestination = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones Server object
        /// </summary>
        /// <returns>clone of current Server object</returns>
        public virtual Server Clone()
        {
            Server serverClone = new Server(this.Configuration, this.server);
            serverClone.Configuration = this.Configuration;
            serverClone.Media = this.Media;
            serverClone.WindowsPathToDestination = this.WindowsPathToDestination;
            serverClone.DevicePathToDestination = this.DevicePathToDestination;

            return serverClone;
        }

        #endregion
    }
}
