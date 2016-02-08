// <copyright file="CVideo.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Class managing Video-Data from CollectorzToKodi MovieCollector
    /// </summary>
    public abstract class CVideo : CMedia
    {
        #region Attributes

        /// <summary>
        /// MPAA rating of video
        /// </summary>
        private string mPAA;

        /// <summary>
        /// times the video had been played
        /// </summary>
        private string playCount;

        /// <summary>
        /// last date the video had been played
        /// </summary>
        private string playDate;

        /// <summary>
        /// number of movie or series on IMDB.com
        /// </summary>
        private string iMDbId;

        /// <summary>
        /// list of directors of video
        /// </summary>
        private List<CPerson> directors;

        /// <summary>
        /// list of writers of video
        /// </summary>
        private List<CPerson> writers;

        /// <summary>
        /// list of actors of video
        /// </summary>
        private List<CActor> actors;

        /// <summary>
        /// codec used for this video
        /// </summary>
        private CConfiguration.VideoCodec videoCodec;

        /// <summary>
        /// definition of video
        /// </summary>
        private CConfiguration.VideoDefinition videoDefinition;

        /// <summary>
        /// aspect ratio of video
        /// </summary>
        private CConfiguration.VideoAspectRatio videoAspectRatio;

        /// <summary>
        /// list of audio streams available in video
        /// </summary>
        private List<CAudioStream> audioStreams;

        /// <summary>
        /// list of subtitles available in video
        /// </summary>
        private List<CSubTitleFile> subTitleStreams;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CVideo"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public CVideo(CConfiguration configuration)
            : base(configuration)
        {
            this.mPAA = string.Empty;
            this.playCount = "0";
            this.playDate = string.Empty;
            this.iMDbId = string.Empty;
            this.directors = new List<CPerson>();
            this.writers = new List<CPerson>();
            this.actors = new List<CActor>();

            // Parameter
            this.videoCodec = CConfiguration.VideoCodec.H265;
            this.videoDefinition = CConfiguration.VideoDefinition.SD;
            this.videoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio169;
            this.audioStreams = new List<CAudioStream>();
            this.subTitleStreams = new List<CSubTitleFile>();
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets MPAA rating of video
        /// </summary>
        public string MPAA
        {
            get { return this.mPAA; }
            set { this.mPAA = value; }
        }

        /// <summary>
        /// Gets or sets times the video had been played
        /// </summary>
        public string PlayCount
        {
            get { return this.playCount; }
            set { this.playCount = value; }
        }

        /// <summary>
        /// Gets or sets last date the video had been played
        /// </summary>
        public string PlayDate
        {
            get { return this.playDate; }
            set { this.playDate = value; }
        }

        /// <summary>
        /// Gets or sets number of movie or series on IMDB.com
        /// </summary>
        public string IMDbId
        {
            get { return this.iMDbId; }
            set { this.iMDbId = value; }
        }

        /// <summary>
        /// Gets or sets list of directors of video
        /// </summary>
        public List<CPerson> Directors
        {
            get { return this.directors; }
            set { this.directors = value; }
        }

        /// <summary>
        /// Gets or sets list of writers of video
        /// </summary>
        public List<CPerson> Writers
        {
            get { return this.writers; }
            set { this.writers = value; }
        }

        /// <summary>
        /// Gets or sets list of actors of video
        /// </summary>
        public List<CActor> Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

        /// <summary>
        /// Gets or sets codec used for this video
        /// </summary>
        public CConfiguration.VideoCodec VideoCodec
        {
            get { return this.videoCodec; }
            set { this.videoCodec = value; }
        }

        /// <summary>
        /// Gets or sets definition of video
        /// </summary>
        public CConfiguration.VideoDefinition VideoDefinition
        {
            get { return this.videoDefinition; }
            set { this.videoDefinition = value; }
        }

        /// <summary>
        /// Gets or sets aspect ratio of video
        /// </summary>
        public CConfiguration.VideoAspectRatio VideoAspectRatio
        {
            get { return this.videoAspectRatio; }
            set { this.videoAspectRatio = value; }
        }

        /// <summary>
        /// Gets or sets list of audio streams available in video
        /// </summary>
        public List<CAudioStream> AudioStreams
        {
            get
            {
                if (this.audioStreams == null)
                {
                    this.audioStreams = new List<CAudioStream>();
                }

                return this.audioStreams;
            }

            set
            {
                this.audioStreams = value;
            }
        }

        /// <summary>
        /// Gets or sets list of subtitles available in video
        /// </summary>
        public List<CSubTitleFile> SubTitleStreams
        {
            get
            {
                if (this.subTitleStreams == null)
                {
                    this.subTitleStreams = new List<CSubTitleFile>();
                }

                return this.subTitleStreams;
            }

            set
            {
                this.subTitleStreams = value;
            }
        }

        #endregion
        #region Functions

        /// <summary>
        /// Clones video to specified languages
        /// </summary>
        /// <remarks>extends base transformations from CMedia.ClonePerLanguage with IMDbID-Transformation</remarks>
        /// <param name="isoCodesToBeReplaced">list target languages</param>
        /// <param name="isoCodeForReplacemant">language used as placeholder in CollectorzToKodi programs</param>
        public new void ClonePerLanguage(List<string> isoCodesToBeReplaced, string isoCodeForReplacemant)
        {
            ((CMedia)this).ClonePerLanguage(isoCodesToBeReplaced, isoCodeForReplacemant);

            foreach (string isoCodeToBeReplaced in isoCodesToBeReplaced)
            {
                this.IMDbId = this.IMDbId.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");
            }
        }

        /// <summary>
        /// Reads crew information from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLNode">Part of XML-file representing Crew information</param>
        public void ReadCrew(XmlNode xMLNode)
        {
            foreach (XmlNode xMLCrewmember in xMLNode.XMLReadSubnode("crew").XMLReadSubnodes("crewmember"))
            {
                CPerson person = new CPerson();

                bool isDirector = xMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfDirector";
                bool isWriter = xMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfWriter";

                person.Name = xMLCrewmember.XMLReadSubnode("person").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                person.Thumb = xMLCrewmember.XMLReadSubnode("person").XMLReadSubnode("imageurl").XMLReadInnerText(string.Empty);

                if (isDirector || isWriter)
                {
                    if (isDirector)
                    {
                        this.Directors.Add(person);
                    }

                    if (isWriter)
                    {
                        this.Writers.Add(person);
                    }
                }
            }
        }

        /// <summary>
        /// Writes crew information to NFO-file of this video
        /// </summary>
        /// <param name="swrNFO">NFO file, the crew information should be added to</param>
        public void WriteCrew(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (CPerson director in this.Directors)
            {
                swrNFO.WriteLine("    <director" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">" + director.Name + "</director>");
                i++;
            }

            i = 0;
            foreach (CPerson writer in this.Writers)
            {
                swrNFO.WriteLine("    <credits" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">" + writer.Name + "</credits>");
                i++;
            }
        }

        /// <summary>
        /// Reads cast information from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLNode">Part of XML-file representing cast information</param>
        public void ReadCast(XmlNode xMLNode)
        {
            foreach (XmlNode xMLCast in xMLNode.XMLReadSubnode("cast").XMLReadSubnodes("star"))
            {
                if (xMLCast.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfActor")
                {
                    CActor actor = new CActor();

                    actor.Name = xMLCast.XMLReadSubnode("person").XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                    actor.Role = xMLCast.XMLReadSubnode("character").XMLReadInnerText(string.Empty);
                    actor.Thumb = xMLCast.XMLReadSubnode("person").XMLReadSubnode("imageurl").XMLReadInnerText(string.Empty);

                    this.Actors.Add(actor);
                }
            }
        }

        /// <summary>
        /// Writes cast information to NFO-file of this video
        /// </summary>
        /// <param name="swrNFO">NFO file, the cast information should be added to</param>
        public void WriteCast(StreamWriter swrNFO)
        {
            int i = 0;
            foreach (CActor actor in this.Actors)
            {
                swrNFO.WriteLine("    <actor" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">");
                swrNFO.WriteLine("        <name>" + actor.Name + "</name>");
                swrNFO.WriteLine("        <role>" + actor.Role + "</role>");

                if (actor.Thumb.StartsWith("http:"))
                {
                    swrNFO.WriteLine("        <thumb>" + actor.Thumb + "</thumb>");
                }

                swrNFO.WriteLine("    </actor>");

                i++;
            }
        }

        /// <summary>
        /// Reads information about video-, audio- and subtitle-streams from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        public void ReadStreamData(XmlNode xMLMedia)
        {
            this.ReadVideoStreamData(xMLMedia);
            this.ReadAudioStreamData(xMLMedia);
            this.ReadSubTitleStreamData(xMLMedia);
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
        public string OverrideVideoStreamData(string title)
        {
            if (title.Contains("(TV)"))
            {
                if (this.Configuration.KodiSkin == "Transparency!")
                {
                    this.VideoCodec = CConfiguration.VideoCodec.H264; // BluRay Standard-Codec
                }

                if (this.Configuration.KodiSkin != "Transparency!")
                {
                    this.VideoCodec = CConfiguration.VideoCodec.TV;
                }

                title = title.Replace("(TV)", string.Empty);
            }

            if (title.Contains("(BluRay)"))
            {
                if (this.Configuration.KodiSkin == "Transparency!")
                {
                    this.VideoCodec = CConfiguration.VideoCodec.H264; // BluRay Standard-Codec
                }

                if (this.Configuration.KodiSkin != "Transparency!")
                {
                    this.VideoCodec = CConfiguration.VideoCodec.BluRay;
                }

                title = title.Replace("(BluRay)", string.Empty);
            }

            if (title.Contains("(H264)"))
            {
                this.VideoCodec = CConfiguration.VideoCodec.H264;
            }

            title = title.Replace("(H264)", string.Empty);

            if (title.Contains("(H265)"))
            {
                this.VideoCodec = CConfiguration.VideoCodec.H265;
                title = title.Replace("(H265)", string.Empty);
            }

            if (title.Contains("(SD)"))
            {
                this.VideoDefinition = CConfiguration.VideoDefinition.SD;
                title = title.Replace("(SD)", string.Empty);
            }

            if (title.Contains("(HD)"))
            {
                this.VideoDefinition = CConfiguration.VideoDefinition.HD;
                title = title.Replace("(HD)", string.Empty);
            }

            if (title.Contains("(4:3)"))
            {
                this.VideoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio43;
                title = title.Replace("(4:3)", string.Empty);
            }

            if (title.Contains("(16:9)"))
            {
                this.VideoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio169;
                title = title.Replace("(16:9)", string.Empty);
            }

            if (title.Contains("(21:9)"))
            {
                this.VideoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio219;
                title = title.Replace("(21:9)", string.Empty);
            }

            if (title.Contains("(F"))
            {
                this.MPAA = title.RightOf("(F").LeftOf(")");
                title = title.Replace("(F" + this.MPAA + ")", string.Empty);
            }

            if (title.Contains("(R"))
            {
                this.Rating = title.RightOf("(R").LeftOf(")");
                title = title.Replace("(R" + this.Rating + ")", string.Empty);
            }

            // check for multiple Instances per Language
            if (title.Contains("(L"))
            {
                // reset list if a new definition is available
                this.MediaLanguages = new List<string>();

                string movieLanguages = title.RightOfLast("(L").LeftOf(")");
                foreach (string movieLanguage in movieLanguages.Split(" ", null, false))
                {
                    this.MediaLanguages.Add(movieLanguage);
                }

                title = title.Replace("(L" + movieLanguages + ")", string.Empty).Trim();
            }

            // inherit Series-data
            if (this.MediaLanguages.Count == 0 && this.GetType().ToString().Contains("CEpisode"))
            {
                this.MediaLanguages = ((CEpisode)this).Series.MediaLanguages;
            }

            // or MediaGroup to default
            if (this.MediaLanguages.Count == 0)
            {
                this.MediaLanguages.Add("de");
            }

            return title.Trim();
        }

        /// <summary>
        /// Writes stream data to provided NFO file for this video
        /// </summary>
        /// <param name="swrNFO">NFO-file the stream data should be added to</param>
        public void WriteStreamData(StreamWriter swrNFO)
        {
            this.WriteVideoStreamData(swrNFO);
            this.WriteAudioStreamData(swrNFO);
            this.WriteSubTitleStreamData(swrNFO);
        }

        /// <summary>
        /// Adds copy statements for subtitles to Bash-shell-script
        /// </summary>
        /// <param name="swrSH">Bash-shell-script that the subtitles information should be added to</param>
        public virtual void WriteSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            foreach (CSubTitleFile subTitleStream in this.SubTitleStreams)
            {
                if (subTitleStream.GetType().ToString().Contains("CSrtSubTitleFile"))
                {
                    swrSH.WriteLine("/bin/cp \"" + this.Configuration.MovieCollectorLocalPathToXMLExportPath + subTitleStream.Filename + "\" \"" + subTitleStream.Filename + "\"");
                }
            }
        }

        /// <summary>
        /// Returns value indicating whether the video has specials or not
        /// </summary>
        /// <returns>value indicating whether the video has specials or not</returns>
        public bool HasSpecials()
        {
            bool hasSpecials = false;

            foreach (CVideoFile videoFile in this.MediaFiles)
            {
                hasSpecials = hasSpecials || videoFile.IsSpecial;
            }

            return hasSpecials;
        }

        /// <summary>
        /// Clones video object completely, when it meets specifications to server or specials
        /// </summary>
        /// <param name="server">Server containing the video; Server is resolved via CConfiguration.ServerListsOfServers[ListOfServerTypes]</param>
        /// <param name="isSpecial">value indicating whether the new CVideo object should contain specials or not</param>
        /// <returns>new instance of video object meeting specification to server and specials</returns>
        public abstract CVideo Clone(int server, bool isSpecial = false);

        /// <summary>
        /// Reads stream data representing video information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        private void ReadVideoStreamData(XmlNode xMLMedia)
        {
            CConfiguration.VideoAspectRatio videoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio169;
            CConfiguration.VideoDefinition videoDefinition = CConfiguration.VideoDefinition.SD;

            // VideoAspectRatio
            List<XmlNode> xMLVideoAspectRatios = xMLMedia.XMLReadSubnode("ratios").XMLReadSubnodes("ratio");
            if (xMLVideoAspectRatios.Count > 0)
            {
                XmlNode xMLVideoAspectRatio = xMLVideoAspectRatios.ElementAt(0);

                if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Fullscreen (4:3)"))
                {
                    videoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio43;
                }
                else if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Widescreen (16:9)"))
                {
                    videoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio169;
                }
                else if (xMLVideoAspectRatio.XMLReadSubnode("displayname").XMLReadInnerText("Widescreen (16:9)").Equals("Theatrical Widescreen (21:9)"))
                {
                    videoAspectRatio = CConfiguration.VideoAspectRatio.AspectRatio219;
                }
            }

            // VideoDefinition
            XmlNode xMLVideoDefinition = xMLMedia.XMLReadSubnode("condition");

            if (xMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("SD"))
            {
                videoDefinition = CConfiguration.VideoDefinition.SD;
            }
            else if (xMLVideoDefinition.XMLReadSubnode("displayname").XMLReadInnerText("SD").Equals("HD"))
            {
                videoDefinition = CConfiguration.VideoDefinition.HD;
            }

            // Werte übertragen
            this.VideoDefinition = videoDefinition;
            this.VideoAspectRatio = videoAspectRatio;
        }

        /// <summary>
        /// Reads stream data representing audio information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        private void ReadAudioStreamData(XmlNode xMLMedia)
        {
            foreach (XmlNode xMLAudio in xMLMedia.XMLReadSubnode("audios").XMLReadSubnodes("audio"))
            {
                string displayname = xMLAudio.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);

                CAudioStream audioStreamData = new CAudioStream();
                audioStreamData.Codec = "AC3";
                audioStreamData.Language = displayname.RightOf("[").LeftOf("]");

                if (displayname.LeftOf("[").Contains("2.0") || displayname.LeftOf("[").Contains("Stereo"))
                {
                    audioStreamData.NumberOfChannels = "2";
                }

                if (displayname.LeftOf("[").Contains("5.1"))
                {
                    audioStreamData.NumberOfChannels = "6";
                }

                this.AudioStreams.Add(audioStreamData);
            }
        }

        /// <summary>
        /// Reads stream data representing subtitle information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        private void ReadSubTitleStreamData(XmlNode xMLMedia)
        {
            foreach (XmlNode xMLSubTitle in xMLMedia.XMLReadSubnode("subtitles").XMLReadSubnodes("subtitle"))
            {
                CSubTitleFile subTitleStream = new CSubTitleFile(this.Configuration);
                subTitleStream.Media = this;

                string displayname = xMLSubTitle.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                subTitleStream.Language = displayname;

                if (displayname != string.Empty)
                {
                    CSubTitleFile checkedSubTitleStream = (CSubTitleFile)subTitleStream.CheckForSubTitleStreamFile(xMLMedia);
                    if (checkedSubTitleStream != null)
                    {
                        this.SubTitleStreams.Add(checkedSubTitleStream);
                    }
                    else
                    {
                        this.SubTitleStreams.Add(subTitleStream);
                    }
                }
            }
        }

        /// <summary>
        /// Writes video-stream data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        private void WriteVideoStreamData(StreamWriter swrNFO)
        {
            swrNFO.WriteLine("    <fileinfo>");
            swrNFO.WriteLine("        <streamdetails>");
            swrNFO.WriteLine("            <video>");

            // VideoCodec
            if (this.VideoCodec.Equals(CConfiguration.VideoCodec.TV))
            {
                swrNFO.WriteLine("                <codec>tv</codec>");
            }
            else if (this.VideoCodec.Equals(CConfiguration.VideoCodec.BluRay))
            {
                swrNFO.WriteLine("                <codec>bluray</codec>");
            }
            else if (this.VideoCodec.Equals(CConfiguration.VideoCodec.H264))
            {
                swrNFO.WriteLine("                <codec>h264</codec>");
            }
            else if (this.VideoCodec.Equals(CConfiguration.VideoCodec.H265))
            {
                swrNFO.WriteLine("                <codec>hevc</codec>");
            }

            // AspectRatio
            if (this.VideoAspectRatio.Equals(CConfiguration.VideoAspectRatio.AspectRatio43))
            {
                swrNFO.WriteLine("                <aspect>1.33</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(CConfiguration.VideoAspectRatio.AspectRatio169))
            {
                swrNFO.WriteLine("                <aspect>1.78</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(CConfiguration.VideoAspectRatio.AspectRatio219))
            {
                swrNFO.WriteLine("                <aspect>2.33</aspect>");
            }

            // VideoDefinition
            if (this.VideoDefinition.Equals(CConfiguration.VideoDefinition.SD))
            {
                swrNFO.WriteLine("                <width>768</width>");
                swrNFO.WriteLine("                <height>576</height>");
            }
            else if (this.VideoDefinition.Equals(CConfiguration.VideoDefinition.HD))
            {
                swrNFO.WriteLine("                <width>1920</width>");
                swrNFO.WriteLine("                <height>1080</height>");
            }

            swrNFO.WriteLine("            </video>");
        }

        /// <summary>
        /// Writes audio-stream data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        private void WriteAudioStreamData(StreamWriter swrNFO)
        {
            foreach (CAudioStream audioStream in this.AudioStreams)
            {
                swrNFO.WriteLine("            <audio>");
                swrNFO.WriteLine("                <codec>" + audioStream.Codec + "</codec>");
                swrNFO.WriteLine("                <language>" + audioStream.Language + "</language>");
                swrNFO.WriteLine("                <channels>" + audioStream.NumberOfChannels + "</channels>");
                swrNFO.WriteLine("            </audio>");
            }
        }

        /// <summary>
        /// Writes subtitle data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        /// <remarks>If video contains SRT-subtitles, the SRT-files are created as well</remarks>
        private void WriteSubTitleStreamData(StreamWriter swrNFO)
        {
            foreach (CSubTitleFile subTitleStream in this.SubTitleStreams)
            {
                swrNFO.WriteLine("            <subtitle>");
                swrNFO.WriteLine("                <language>" + subTitleStream.Language + "</language>");
                swrNFO.WriteLine("            </subtitle>");

                if (subTitleStream.GetType().ToString().Contains("CSrtSubTitleFileCollection"))
                {
                    ((CSrtSubTitleFileCollection)subTitleStream).WriteSrtSubTitleStreamDataToSRT();
                }
            }

            swrNFO.WriteLine("        </streamdetails>");
            swrNFO.WriteLine("    </fileinfo>");
        }

        #endregion
    }
}
