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
        private DirectorCollection directors;

        /// <summary>
        /// list of writers of video
        /// </summary>
        private WriterCollection writers;

        /// <summary>
        /// list of actors of video
        /// </summary>
        private ActorCollection actors;

        /// <summary>
        /// list of video streams available in video
        /// </summary>
        private VideoStreamCollection videoStreams;

        /// <summary>
        /// list of audio streams available in video
        /// </summary>
        private AudioStreamCollection audioStreams;

        /// <summary>
        /// list of subtitles available in video
        /// </summary>
        private SubTitleStreamCollection subTitleStreams;

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
            this.subTitleStreams = new List<SubTitleStream>();

            this.MediaPath.WindowsPathToDestination = string.Empty;
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
        public DirectorCollection Directors
        {
            get { return this.directors; }
            set { this.directors = value; }
        }

        /// <summary>
        /// Gets or sets list of writers of video
        /// </summary>
        public WriterCollection Writers
        {
            get { return this.writers; }
            set { this.writers = value; }
        }

        /// <summary>
        /// Gets or sets list of actors of video
        /// </summary>
        public ActorCollection Actors
        {
            get { return this.actors; }
            set { this.actors = value; }
        }

        /// <summary>
        /// Gets or sets list of audio streams available in video
        /// </summary>
        public VideoStreamCollection VideoStreams
        {
            get
            {
                if (this.videoStreams == null)
                {
                    this.videoStreams = new VideoStreamCollection(this.Configuration);
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
                if (this.subTitleStreams == null)
                {
                    this.subTitleStreams = new List<SubTitleStream>();
                }

                return this.subTitleStreams;
            }

            set
            {
                this.subTitleStreams = value;
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
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0].Number].StreamWriter;

            // write NFO-file
            using (bfStreamWriter)
            {
                if (this.Title != string.Empty)
                {
                    bfStreamWriter.WriteLine("if [ -d \"" + this.MediaPath.Filename + "\" ];");
                    bfStreamWriter.WriteLine("then ");
                    bfStreamWriter.WriteLine("    rm -r \"" + this.MediaPath.Filename + "\"");
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
            base.ReadFromXml(xMLMedia);

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
        /// checks, if MediaLanguages is still empty and add default Language "de" if necessary
        /// </summary>
        public void CheckForDefaultMediaLanguages()
        {
            // or MediaGroup to default
            if (this.MediaLanguages.MediaLanguages.Count == 0)
            {
                MediaLanguage mediaLanguage = new MediaLanguage(this.Configuration);
                mediaLanguage.Media = this;
                mediaLanguage.Name = "de";

                this.MediaLanguages.MediaLanguages.Add(mediaLanguage);
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
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0].Number].StreamWriter;

            // copy video files to device
            foreach (VideoFile videoFile in this.MediaFiles)
            {
                videoFile.WriteToLibrary();
            }

            nfoStreamWriter.WriteLine("    <fileinfo>");
            nfoStreamWriter.WriteLine("        <streamdetails>");

            // add VideoStream to nfo File
            foreach (VideoStream viedeoStream in this.VideoStreams)
            {
                viedeoStream.WriteToLibrary();
            }

            // add AudioStreamData to nfo File
            foreach (AudioStream audioStream in this.AudioStreams)
            {
                audioStream.WriteToLibrary();
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
