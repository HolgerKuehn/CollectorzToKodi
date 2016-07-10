// <copyright file="Video.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml;

    /// <summary>
    /// Class managing Video-Data from CollectorzToKodi MovieCollector
    /// </summary>
    public abstract class Video : Media
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
        private List<Person> directors;

        /// <summary>
        /// list of writers of video
        /// </summary>
        private List<Person> writers;

        /// <summary>
        /// list of actors of video
        /// </summary>
        private List<Actor> actors;

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

        /// <summary>
        /// list of audio streams available in video
        /// </summary>
        private List<AudioStream> audioStreams;

        /// <summary>
        /// list of subtitles available in video
        /// </summary>
        private List<SubTitle> subTitles;

        /// <summary>
        /// index of represented Video in "MovieCollector's Episodes and Features tab"
        /// </summary>
        private int videoIndex;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Video"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public Video(Configuration configuration)
            : base(configuration)
        {
            this.mPAA = string.Empty;
            this.playCount = "0";
            this.playDate = string.Empty;
            this.iMDbId = string.Empty;
            this.directors = new List<Person>();
            this.writers = new List<Person>();
            this.actors = new List<Actor>();
            this.videoIndex = 1;

            // Parameter
            this.videoCodec = Configuration.VideoCodec.H265;
            this.videoDefinition = Configuration.VideoDefinition.SD;
            this.videoAspectRatio = Configuration.VideoAspectRatio.AspectRatio169;
            this.audioStreams = new List<AudioStream>();
            this.subTitles = new List<SubTitle>();
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
        public List<Person> Directors
        {
            get { return this.directors; }
            set { this.directors = value; }
        }

        /// <summary>
        /// Gets or sets list of writers of video
        /// </summary>
        public List<Person> Writers
        {
            get { return this.writers; }
            set { this.writers = value; }
        }

        /// <summary>
        /// Gets or sets list of actors of video
        /// </summary>
        public List<Actor> Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

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

        /// <summary>
        /// Gets or sets list of audio streams available in video
        /// </summary>
        public List<AudioStream> AudioStreams
        {
            get
            {
                if (this.audioStreams == null)
                {
                    this.audioStreams = new List<AudioStream>();
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
        public List<SubTitle> SubTitles
        {
            get
            {
                if (this.subTitles == null)
                {
                    this.subTitles = new List<SubTitle>();
                }

                return this.subTitles;
            }

            set
            {
                this.subTitles = value;
            }
        }

        /// <summary>
        /// Gets or sets index of represented Video in "MovieCollector's Episodes and Features tab"
        /// </summary>
        public int VideoIndex
        {
            get { return this.videoIndex; }
            set { this.videoIndex = value; }
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
            ((Media)this).ClonePerLanguage(isoCodesToBeReplaced, isoCodeForReplacemant);

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
                Person person = new Person();

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
            foreach (Person director in this.Directors)
            {
                swrNFO.WriteLine("    <director" + (i == 0 ? " clear=\"true\"" : string.Empty) + ">" + director.Name + "</director>");
                i++;
            }

            i = 0;
            foreach (Person writer in this.Writers)
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
                    Actor actor = new Actor();

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
            foreach (Actor actor in this.Actors)
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

            return title.Trim();
        }

        /// <summary>
        /// checks, if MediaLanguages is still empty and add default Language "de" if necessary
        /// </summary>
        public void CheckForDefaultMediaLanguages()
        {
            // or MediaGroup to default
            if (this.MediaLanguages.Count == 0)
            {
                this.MediaLanguages.Add("de");
            }
        }

        /// <summary>
        /// Writes stream data to provided NFO file for this video
        /// </summary>
        /// <param name="swrNFO">NFO-file the stream data should be added to</param>
        public void WriteStreamData(StreamWriter swrNFO)
        {
            this.WriteVideoStreamData(swrNFO);
            this.WriteAudioStreamData(swrNFO);
            this.WriteSubTitleStreamDataToNFO(swrNFO);
        }

        /// <summary>
        /// Returns value indicating whether the video has specials or not
        /// </summary>
        /// <returns>value indicating whether the video has specials or not</returns>
        public bool HasSpecials()
        {
            bool hasSpecials = false;

            foreach (VideoFile videoFile in this.MediaFiles)
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
        public abstract Video Clone(int server, bool isSpecial = false);

        /// <summary>
        /// Reads stream data representing subtitle information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        public virtual void ReadSubTitleStreamData(XmlNode xMLMedia)
        {
            foreach (XmlNode xMLSubTitle in xMLMedia.XMLReadSubnode("subtitles").XMLReadSubnodes("subtitle"))
            {
                SubTitle subTitle = new SubTitle(this.Configuration);
                subTitle.Video = this;

                string displayname = xMLSubTitle.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);
                subTitle.Language = displayname;

                this.SubTitles.Add(subTitle);
            }
        }

        /// <summary>
        /// Adds copy statements for VideoFiles to provided bash-shell-script
        /// </summary>
        /// <param name="swrSH">Bash-shell-script that the image information should be added to</param>
        public virtual void WriteVideoFilesToSH(StreamWriter swrSH)
        {
            foreach (VideoFile videoFile in this.MediaFiles)
            {
                if (videoFile.Filename != string.Empty)
                {
                    swrSH.WriteLine("/bin/ln -s \"" + videoFile.URLLocalFilesystem + "\" \"" + videoFile.Filename + "\"");
                    videoFile.WriteSubTitleToSH(swrSH);
                }
            }
        }

        /// <summary>
        /// sets Filenames according to media-type for all corresponding files
        /// </summary>
        public virtual void SetFilename()
        {
            this.Filename = System.Text.Encoding.ASCII.GetString(System.Text.Encoding.Convert(System.Text.Encoding.UTF8, Encoding.ASCII, System.Text.Encoding.UTF8.GetBytes(this.Title + " (" + this.PublishingYear + ")"))).Replace("?", string.Empty).Replace("-", string.Empty).Replace(":", string.Empty).Trim();
        }

        /// <summary>
        /// Reads stream data representing video information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        private void ReadVideoStreamData(XmlNode xMLMedia)
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

        /// <summary>
        /// Reads stream data representing audio information
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        private void ReadAudioStreamData(XmlNode xMLMedia)
        {
            foreach (XmlNode xMLAudio in xMLMedia.XMLReadSubnode("audios").XMLReadSubnodes("audio"))
            {
                string displayname = xMLAudio.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);

                AudioStream audioStreamData = new AudioStream();
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
        /// Writes video-stream data to provided NFO file
        /// </summary>
        /// <param name="swrNFO">NFO file that the stream information should be added to</param>
        private void WriteVideoStreamData(StreamWriter swrNFO)
        {
            swrNFO.WriteLine("    <fileinfo>");
            swrNFO.WriteLine("        <streamdetails>");
            swrNFO.WriteLine("            <video>");

            // VideoCodec
            if (this.VideoCodec.Equals(Configuration.VideoCodec.TV))
            {
                swrNFO.WriteLine("                <codec>tv</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.BluRay))
            {
                swrNFO.WriteLine("                <codec>bluray</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.H264))
            {
                swrNFO.WriteLine("                <codec>h264</codec>");
            }
            else if (this.VideoCodec.Equals(Configuration.VideoCodec.H265))
            {
                swrNFO.WriteLine("                <codec>hevc</codec>");
            }

            // AspectRatio
            if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio43))
            {
                swrNFO.WriteLine("                <aspect>1.33</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio169))
            {
                swrNFO.WriteLine("                <aspect>1.78</aspect>");
            }
            else if (this.VideoAspectRatio.Equals(Configuration.VideoAspectRatio.AspectRatio219))
            {
                swrNFO.WriteLine("                <aspect>2.33</aspect>");
            }

            // VideoDefinition
            if (this.VideoDefinition.Equals(Configuration.VideoDefinition.SD))
            {
                swrNFO.WriteLine("                <width>768</width>");
                swrNFO.WriteLine("                <height>576</height>");
            }
            else if (this.VideoDefinition.Equals(Configuration.VideoDefinition.HD))
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
            foreach (AudioStream audioStream in this.AudioStreams)
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
        private void WriteSubTitleStreamDataToNFO(StreamWriter swrNFO)
        {
            foreach (SubTitle subTitleStream in this.SubTitles)
            {
                subTitleStream.WriteSubTitleStreamDataToNFO(swrNFO);
            }

            swrNFO.WriteLine("        </streamdetails>");
            swrNFO.WriteLine("    </fileinfo>");
        }

        #endregion
    }
}
