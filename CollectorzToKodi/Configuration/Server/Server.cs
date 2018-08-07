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
        /// number of server
        /// </summary>
        private readonly int number;

        /// <summary>
        /// paths to destination
        /// </summary>
        private Path path;

        /// <summary>
        /// media collection stored on this Server
        /// </summary>
        private MediaCollection media;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Server"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        /// <param name="number">Number of Server represented by object</param>
        public Server(Configuration configuration, int number)
        {
            this.configuration = configuration;
            this.number = number;
            this.path = new Path(configuration);

            string driveLetter = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][number.ToString()];
            this.Path.WindowsPathToDestination = driveLetter + ":\\";

            string localPath = this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDeviceDestinationPath][number.ToString()];
            this.Path.DevicePathToDestination = this.Path.WindowsPathToDestination.Replace(driveLetter.Trim() + ":", localPath);

            if (this.Configuration.ServerMappingType.StartsWith("UNIX"))
            {
                this.Path.DevicePathToDestination = this.Path.DevicePathToDestination.ReplaceAll("\\", "/");
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
        /// Gets number of Server
        /// </summary>
        public int Number
        {
            get { return this.number; }
        }

        /// <summary>
        /// Gets or sets path of server
        /// </summary>
        public Path Path
        {
            get { return this.Path; }
            set { this.path = value; }
        }

        /// <summary>
        /// Gets or sets media stored on this Server
        /// </summary>
        public virtual MediaCollection Media
        {
            get { return this.media; }
            set { this.media = value; }
        }

        #endregion
        #region Functions

        /// <summary>
        /// clones Server object
        /// </summary>
        /// <returns>clone of current Server object</returns>
        public virtual Server Clone()
        {
            Server serverClone = new Server(this.Configuration, this.number);
            serverClone.Configuration = this.Configuration;
            serverClone.Path = this.Path.Clone();
            serverClone.Media = this.Media.Clone();

            return serverClone;
        }

        #endregion
    }
}
