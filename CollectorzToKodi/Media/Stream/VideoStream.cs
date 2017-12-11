// <copyright file="VideoStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    /// <summary>
    /// class to manage VideoStreams
    /// </summary>
    public class VideoStream : MediaStream
    {
        #region Attributes

        /// <summary>
        /// codec used for this video
        /// </summary>
        private Configuration.VideoCodec videoCodec;

        /// <summary>
        /// definition of video
        /// </summary>
        private Configuration.VideoDefinition videoDefinition;

        /// <summary>
        /// aspect ratio of video
        /// </summary>
        private Configuration.VideoAspectRatio videoAspectRatio;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="VideoStream"/> class.
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        /// </summary>
        public VideoStream(Configuration configuration)
            : base(configuration)
        {
            this.videoCodec = Configuration.VideoCodec.H265;
            this.videoDefinition = Configuration.VideoDefinition.SD;
            this.videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio169;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets codec used for this video
        /// </summary>
        public Configuration.VideoCodec VideoCodec
        {
            get { return this.videoCodec; }
            set { this.videoCodec = value; }
        }

        /// <summary>
        /// Gets or sets definition of video
        /// </summary>
        public Configuration.VideoDefinition VideoDefinition
        {
            get { return this.videoDefinition; }
            set { this.videoDefinition = value; }
        }

        /// <summary>
        /// Gets or sets aspect ratio of video
        /// </summary>
        public Configuration.VideoAspectRatio VideoAspectRatio
        {
            get { return this.videoAspectRatio; }
            set { this.videoAspectRatio = value; }
        }

        #endregion
        #region Functions

        #endregion
    }
}
