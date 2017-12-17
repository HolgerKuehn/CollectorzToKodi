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
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
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
        public override MediaStream Clone()
        {
            VideoStream videoStreamClone = new VideoStream(this.Configuration);

            // MediaStream
            videoStreamClone.Media = this.Media;

            // VideoStream
            videoStreamClone.VideoCodec = this.VideoCodec;
            videoStreamClone.VideoDefinition = this.VideoDefinition;
            videoStreamClone.VideoAspectRatio = this.VideoAspectRatio;

            return videoStreamClone;
        }

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
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;

            // add VideoStreamData to nfo File
            nfoStreamWriter.WriteLine("            <video>");

            // VideoCodec
            if (this.VideoCodec.Equals(Configuration.VideoCodec.TV))
            {
                nfoStreamWriter.WriteLine("                <codec>tv</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.BluRay))
            {
                nfoStreamWriter.WriteLine("                <codec>bluray</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.H264))
            {
                nfoStreamWriter.WriteLine("                <codec>h264</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.H265))
            {
                nfoStreamWriter.WriteLine("                <codec>hevc</codec>");
            }

            // AspectRatio
            if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio43))
            {
                nfoStreamWriter.WriteLine("                <aspect>1.33</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio169))
            {
                nfoStreamWriter.WriteLine("                <aspect>1.78</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio219))
            {
                nfoStreamWriter.WriteLine("                <aspect>2.33</aspect>");
            }

            // VideoDefinition
            if (this.VideoDefinition.Equals(Configuration.VideoDefinition.SD))
            {
                nfoStreamWriter.WriteLine("                <width>768</width>");
                nfoStreamWriter.WriteLine("                <height>576</height>");
            }
            else if (this.VideoDefinition.Equals(Configuration.VideoDefinition.HD))
            {
                nfoStreamWriter.WriteLine("                <width>1920</width>");
                nfoStreamWriter.WriteLine("                <height>1080</height>");
            }

            nfoStreamWriter.WriteLine("            </video>");
        }

        /// <summary>
        /// Overrides specific stream data from XML-file, thats stored in titles
        /// </summary>
        /// <param name="title">String containing additional information for stream data</param>
        /// <returns>cleared string without special tags representing additional information for stream data</returns>
        /// <remarks>
        /// available modifiers are:<br />
        /// (TV) - represents video as recording from TV program<br />
        /// (BluRay) - represents video as part of BluRay<br />
        /// (H264) - represents video using the H264 codec<br />
        /// (H265) - represents video using the H265 codec<br />
        /// (SD) - represents video with SD resolution<br />
        /// (HD) - represents video with HD resolution<br />
        /// (4:3) - represents video as Full frame<br />
        /// (16:9) - represents video as Widescreen<br />
        /// (21:9) - represents video as Theatrical Widescreen<br />
        /// (F###) - represent different MPAA Rating<br />
        /// (R###) - represent different Rating<br />
        /// (L## ##) - represents languages (2 digit ISO code) that are stored in different files for this video
        /// </remarks>
        public virtual string OverrideVideoStreamData(string title)
        {
            if (title.Contains("(TV)"))
            {
                if (this.Configuration.KodiSkin == "Transparency!")
                {
                    this.VideoCodec = Configuration.VideoCodec.H264; // BluRay Standard-Codec
                }

                if (this.Configuration.KodiSkin != "Transparency!")
                {
                    this.VideoCodec = Configuration.VideoCodec.TV;
                }

                title = title.Replace("(TV)", string.Empty);
            }

            if (title.Contains("(BluRay)"))
            {
                if (this.Configuration.KodiSkin == "Transparency!")
                {
                    this.VideoCodec = Configuration.VideoCodec.H264; // BluRay Standard-Codec
                }

                if (this.Configuration.KodiSkin != "Transparency!")
                {
                    this.VideoCodec = Configuration.VideoCodec.BluRay;
                }

                title = title.Replace("(BluRay)", string.Empty);
            }

            if (title.Contains("(H264)"))
            {
                this.VideoCodec = Configuration.VideoCodec.H264;
            }

            title = title.Replace("(H264)", string.Empty);

            if (title.Contains("(H265)"))
            {
                this.VideoCodec = Configuration.VideoCodec.H265;
                title = title.Replace("(H265)", string.Empty);
            }

            if (title.Contains("(SD)"))
            {
                this.VideoDefinition = Configuration.VideoDefinition.SD;
                title = title.Replace("(SD)", string.Empty);
            }

            if (title.Contains("(HD)"))
            {
                this.VideoDefinition = Configuration.VideoDefinition.HD;
                title = title.Replace("(HD)", string.Empty);
            }

            if (title.Contains("(4:3)"))
            {
                this.VideoAspectRatio = Configuration.VideoAspectRatio.AspectRatio43;
                title = title.Replace("(4:3)", string.Empty);
            }

            if (title.Contains("(16:9)"))
            {
                this.VideoAspectRatio = Configuration.VideoAspectRatio.AspectRatio169;
                title = title.Replace("(16:9)", string.Empty);
            }

            if (title.Contains("(21:9)"))
            {
                this.VideoAspectRatio = Configuration.VideoAspectRatio.AspectRatio219;
                title = title.Replace("(21:9)", string.Empty);
            }

            if (title.Contains("(F"))
            {
                ((Video)this.Media).MPAA = title.RightOf("(F").LeftOf(")");
                title = title.Replace("(F" + ((Video)this.Media).MPAA + ")", string.Empty);
            }

            if (title.Contains("(R"))
            {
                ((Video)this.Media).Rating = title.RightOf("(R").LeftOf(")");
                title = title.Replace("(R" + ((Video)this.Media).Rating + ")", string.Empty);
            }

            // check for multiple Instances per Language
            if (title.Contains("(L"))
            {
                // reset list if a new definition is available
                ((Video)this.Media).MediaLanguages = new List<string>();

                string movieLanguages = title.RightOfLast("(L").LeftOf(")");
                foreach (string movieLanguage in movieLanguages.Split(" ", null, false))
                {
                    ((Video)this.Media).MediaLanguages.Add(movieLanguage);
                }

                title = title.Replace("(L" + movieLanguages + ")", string.Empty).Trim();
            }

            return title.Trim();
        }

        #endregion
    }
}
