// <copyright file="Video.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
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
        /// type of movie or series on TheMovieDB.org
        /// </summary>
        private string tMDbType;

        /// <summary>
        /// number of movie or series on TheMovieDB.org
        /// </summary>
        private string tMDbId;

        /// <summary>
        /// list of directors of video
        /// </summary>
        private List<Director> directors;

        /// <summary>
        /// list of writers of video
        /// </summary>
        private List<Writer> writers;

        /// <summary>
        /// list of actors of video
        /// </summary>
        private List<Actor> actors;

        /// <summary>
        /// list of video streams available in video
        /// </summary>
        private List<VideoStream> videoStreams;

        /// <summary>
        /// list of audio streams available in video
        /// </summary>
        private List<AudioStream> audioStreams;

        /// <summary>
        /// list of subtitles available in video
        /// </summary>
        private List<SubTitleStream> subTitles;

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
            this.tMDbType = string.Empty;
            this.tMDbId = string.Empty;
            this.directors = new List<Director>();
            this.writers = new List<Writer>();
            this.actors = new List<Actor>();
            this.videoIndex = 1;

            // Parameter
            this.videoStreams = new List<VideoStream>();
            this.audioStreams = new List<AudioStream>();
            this.subTitles = new List<SubTitleStream>();
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
        /// Gets or sets type of movie or series on TheMovieDB.org
        /// </summary>
        public string TMDbType
        {
            get { return this.tMDbType; }
            set { this.tMDbType = value; }
        }

        /// <summary>
        /// Gets or sets number of movie or series on TheMovieDB.org
        /// </summary>
        public string TMDbId
        {
            get { return this.tMDbId; }
            set { this.tMDbId = value; }
        }

        /// <summary>
        /// Gets or sets list of directors of video
        /// </summary>
        public List<Director> Directors
        {
            get { return this.directors; }
            set { this.directors = value; }
        }

        /// <summary>
        /// Gets or sets list of writers of video
        /// </summary>
        public List<Writer> Writers
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
        /// Gets or sets list of audio streams available in video
        /// </summary>
        public List<VideoStream> VideoStreams
        {
            get
            {
                if (this.videoStreams == null)
                {
                    this.videoStreams = new List<VideoStream>();
                }

                return this.videoStreams;
            }

            set
            {
                this.videoStreams = value;
            }
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
        public List<SubTitleStream> SubTitles
        {
            get
            {
                if (this.subTitles == null)
                {
                    this.subTitles = new List<SubTitleStream>();
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

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            // write NFO-file
            using (bfStreamWriter)
            {
                if (this.Title != string.Empty)
                {
                    bfStreamWriter.WriteLine("if [ -d \"" + this.Server.Filename + "\" ];");
                    bfStreamWriter.WriteLine("then ");
                    bfStreamWriter.WriteLine("    rm -r \"" + this.Server.Filename + "\"");
                    bfStreamWriter.WriteLine("fi;");
                    bfStreamWriter.WriteLine(string.Empty);
                }
            }
        }

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
                this.ID = this.ID.ReplaceAll("(" + isoCodeToBeReplaced + ")", "(" + isoCodeForReplacemant + ")");
            }
        }

        /// <summary>
        /// Reads crew information from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLNode">Part of XML-file representing Crew information</param>
        public void ReadCrewFromXml(XmlNode xMLNode)
        {
            foreach (XmlNode xMLCrewmember in xMLNode.XMLReadSubnode("crew").XMLReadSubnodes("crewmember"))
            {
                bool isDirector = xMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfDirector";
                bool isWriter = xMLCrewmember.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfWriter";

                if (isDirector)
                {
                    Director director = new Director(this.Configuration);
                    director.Media = this;
                    director.ReadPersonFromXml(xMLCrewmember.XMLReadSubnode("person"));
                    this.Directors.Add(director);
                }

                if (isWriter)
                {
                    Writer writer = new Writer(this.Configuration);
                    writer.Media = this;
                    writer.ReadPersonFromXml(xMLCrewmember.XMLReadSubnode("person"));
                    this.Writers.Add(writer);
                }
            }
        }

        /// <summary>
        /// Writes crew information to NFO-file of this video
        /// </summary>
        public void WriteCrewToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;

            int i = 0;
            foreach (Director director in this.Directors)
            {
                director.WritePersonToLibrary(i == 0);
                i++;
            }

            i = 0;
            foreach (Writer writer in this.Writers)
            {
                writer.WritePersonToLibrary(i == 0);
                i++;
            }
        }

        /// <summary>
        /// return new Actor or SeriesActor depending on referring class
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        /// <returns>new Actor</returns>
        public virtual Actor ActorFactory(Configuration configuration)
        {
            return new Actor(configuration);
        }

        /// <summary>
        /// Reads cast information from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLNode">Part of XML-file representing cast information</param>
        public virtual void ReadCastFromXml(XmlNode xMLNode)
        {
            foreach (XmlNode xMLCast in xMLNode.XMLReadSubnode("cast").XMLReadSubnodes("star"))
            {
                if (xMLCast.XMLReadSubnode("roleid").XMLReadInnerText(string.Empty) == "dfActor")
                {
                    Actor actor = this.ActorFactory(this.Configuration);
                    actor.Media = this;

                    actor.ReadPersonFromXml(xMLCast);

                    this.AddActor(actor);
                }
            }
        }

        /// <summary>
        /// Writes cast information to NFO-file of this video
        /// </summary>
        public void WriteCastToLibrary()
        {
            int i = 0;
            foreach (Actor actor in this.Actors)
            {
                actor.WritePersonToLibrary(i == 0);
                i++;
            }
        }

        /// <summary>
        /// Reads information about video-, audio- and subtitle-streams from MovieCollector XML-file
        /// </summary>
        /// <param name="xMLMedia">Part of XML-file representing stream information</param>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
            // read VideoStreamData
            VideoStream videoStream = new VideoStream(this.Configuration);

            videoStream.Media = this;
            videoStream.ReadFromXml(xMLMedia);
            this.VideoStreams.Add(videoStream);

            // Read AudioStreamData
            foreach (XmlNode xMLAudio in xMLMedia.XMLReadSubnode("audios").XMLReadSubnodes("audio"))
            {
                AudioStream audioStream = new AudioStream(this.Configuration);

                audioStream.Media = this;
                audioStream.ReadFromXml(xMLAudio);
                this.AudioStreams.Add(audioStream);
            }

            // Read SubTitleStreamData
            foreach (XmlNode xMLSubTitle in xMLMedia.XMLReadSubnode("subtitles").XMLReadSubnodes("subtitle"))
            {
                string displayname = xMLSubTitle.XMLReadSubnode("displayname").XMLReadInnerText(string.Empty);

                SubTitleStream subTitle = new SubTitleStream(this.Configuration);
                subTitle.Media = this;

                subTitle.Language = displayname;

                this.SubTitles.Add(subTitle);
            }
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
        /// Adds copy statements for VideoFiles to provided bash-shell-script
        /// </summary>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0]].StreamWriter;

            // copy video files to device
            foreach (VideoFile videoFile in this.MediaFiles)
            {
                if (videoFile.Server.Filename != string.Empty)
                {
                    bfStreamWriter.WriteLine("/bin/ln -s \"" + videoFile.Server.DevicePathForPublication + "\" \"" + videoFile.Server.Filename + "\"");
                    videoFile.WriteToLibrary();
                }
            }

            nfoStreamWriter.WriteLine("    <fileinfo>");
            nfoStreamWriter.WriteLine("        <streamdetails>");

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

            // add AudioStreamData to nfo File
            foreach (AudioStream audioStream in this.AudioStreams)
            {
                nfoStreamWriter.WriteLine("            <audio>");
                nfoStreamWriter.WriteLine("                <codec>" + audioStream.Codec + "</codec>");
                nfoStreamWriter.WriteLine("                <language>" + audioStream.Language + "</language>");
                nfoStreamWriter.WriteLine("                <channels>" + audioStream.NumberOfChannels + "</channels>");
                nfoStreamWriter.WriteLine("            </audio>");
            }

            // add SubTitleStreamData to nfo File
            foreach (SubTitleStream subTitleStream in this.SubTitles)
            {
                subTitleStream.WriteToLibrary();
            }

            nfoStreamWriter.WriteLine("        </streamdetails>");
            nfoStreamWriter.WriteLine("    </fileinfo>");
        }

        /// <summary>
        /// Adds director to Video
        /// </summary>
        /// <param name="director">Person, to be added as director</param>
        public void AddDirector(Director director)
        {
            bool addDirector = true;
            foreach (Director currentDirector in this.Directors)
            {
                if (currentDirector.Name == director.Name)
                {
                    addDirector = false;
                }
            }

            if (addDirector)
            {
                this.Directors.Add(director);
            }
        }

        /// <summary>
        /// Adds director to Video
        /// </summary>
        /// <param name="directors">List of Persons, to be added as directors</param>
        public void AddDirector(List<Director> directors)
        {
            foreach (Director director in directors)
            {
                this.AddDirector(director);
            }
        }

        /// <summary>
        /// Adds writer to Video
        /// </summary>
        /// <param name="writer">Person, to be added as writer</param>
        public void AddWriter(Writer writer)
        {
            bool addWriter = true;
            foreach (Writer currentWriter in this.Writers)
            {
                if (currentWriter.Name == writer.Name)
                {
                    addWriter = false;
                }
            }

            if (addWriter)
            {
                this.Writers.Add(writer);
            }
        }

        /// <summary>
        /// Adds writer to Video
        /// </summary>
        /// <param name="writers">List of Persons, to be added as writers</param>
        public void AddWriter(List<Writer> writers)
        {
            foreach (Writer writer in writers)
            {
                this.AddWriter(writer);
            }
        }

        /// <summary>
        /// Adds actor to Video
        /// </summary>
        /// <param name="actor">Person, to be added as actor</param>
        public void AddActor(Actor actor)
        {
            bool addActor = true;
            foreach (Actor currentActor in this.Actors)
            {
                // needs to be added: check for Season
                if (currentActor.Name == actor.Name && currentActor.Role == actor.Role)
                {
                    addActor = false;
                }
            }

            if (addActor)
            {
                this.Actors.Add(actor);
            }
        }

        /// <summary>
        /// Adds actor to Video
        /// </summary>
        /// <param name="actors">List of Persons, to be added as actors</param>
        public virtual void AddActor(List<Actor> actors)
        {
            foreach (Actor actor in actors)
            {
                this.AddActor(actor);
            }
        }

        #endregion
    }
}
