// <copyright file="VideoStream.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

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

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            Configuration.VideoAspectRatio videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio169;
            Configuration.VideoDefinition videoDefinition = Configuration.VideoDefinition.SD;

            // VideoAspectRatio
            List<XmlNode> xMLVideoAspectRatios = xMLMedia.XMLReadSubnode("ratios").XMLReadSubnodes("ratio");
            if (xMLVideoAspectRatios.Count > 0)
            {
                XmlNode xMLVideoAspectRatio = xMLVideoAspectRatios.ElementAt(0);

                if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Fullscreen (4:3)"))
                {
                    videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio43;
                }
                else if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Widescreen (16:9)"))
                {
                    videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio169;
                }
                else if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Theatrical Widescreen (21:9)"))
                {
                    videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio219;
                }
            }

            // VideoDefinition
            XmlNode xMLVideoDefinition = xMLMedia.XMLReadSubnode("condition");

            if (xMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("SD"))
            {
                videoDefinition = Configuration.VideoDefinition.SD;
            }
            else if (xMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("HD"))
            {
                videoDefinition = Configuration.VideoDefinition.HD;
            }

            // Werte übertragen
            this.VideoDefinition = videoDefinition;
            this.VideoAspectRatio = videoAspectRatio;
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
        }

        #endregion
    }
}
