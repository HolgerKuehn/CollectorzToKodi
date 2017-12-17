// <copyright file="ImageFile.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Linq;
    using System.Xml;

    /// <summary>
    /// Class managing images
    /// </summary>
    public class ImageFile : MediaFile
    {
        #region Attributes

        /// <summary>
        /// type of image
        /// </summary>
        private Configuration.ImageType imageType;

        /// <summary>
        /// season the image is assigned to
        /// </summary>
        private string season;

        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFile"/> class.
        /// </summary>
        /// <param name="configuration">current configuration for CollectorzToKodi programs and Kodi</param>
        public ImageFile(Configuration configuration)
            : base(configuration)
        {
            this.imageType = Configuration.ImageType.Unknown;
            this.season = string.Empty;
        }

        #endregion
        #region Properties

        /// <summary>
        /// Gets or sets type of image
        /// </summary>
        public Configuration.ImageType ImageType
        {
            get { return this.imageType; }
            set { this.imageType = value; }
        }

        /// <summary>
        /// Gets or sets season the image is assigned to
        /// </summary>
        public string Season
        {
            get { return this.season; }
            set { this.season = value; }
        }

        #endregion
        #region Functions

        /// <inheritdoc/>
        public override MediaFile Clone()
        {
            ImageFile imageFileClone = new ImageFile(this.Configuration);

            // MediaFile
            imageFileClone.Description = this.Description;
            imageFileClone.MediaFilePath = (MediaFilePath)this.MediaFilePath.Clone();
            imageFileClone.Media = this.Media;
            imageFileClone.Server = this.Server.Clone();
            imageFileClone.FileIndex = this.FileIndex;

            // ImageFile
            imageFileClone.ImageType = this.ImageType;
            imageFileClone.Season = this.Season;

            return imageFileClone;
        }

        /// <inheritdoc/>
        public override void ReadFromXml(XmlNode xMLMedia)
        {
        }

        /// <inheritdoc/>
        public override void DeleteFromLibrary()
        {
        }

        /// <inheritdoc/>
        public override void WriteToLibrary()
        {
            StreamWriter nfoStreamWriter = this.Media.NfoFile.StreamWriter;
            StreamWriter bfStreamWriter = this.Configuration.ListOfBatchFiles[this.Server[0].Number].StreamWriter;

            if (this.MediaFilePath.Filename != string.Empty)
            {
                if (/* Fanart */ this.ImageType == Configuration.ImageType.CoverFront || this.ImageType == Configuration.ImageType.Backdrop ||
                    /* Poster */ this.ImageType == Configuration.ImageType.Poster)
                {
                    if (/* Fanart */ this.ImageType == Configuration.ImageType.Backdrop ||
                        /* Poster */ this.ImageType == Configuration.ImageType.Poster)
                    {
                        nfoStreamWriter.Write("    ");
                    }

                    nfoStreamWriter.WriteLine("    <thumb>smb://" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][this.Media.Server.ElementAt(0).Number.ToString()] + "/XBMC/" + (this.Media.GetType().ToString().Contains("Movie") ? "Filme" : "Serien") + "/" + this.Media.MediaPath + "/" + this.MediaFilePath.Filename + "</thumb>");
                }

                if (/* Fanart */ this.ImageType == Configuration.ImageType.SeasonCover || this.ImageType == Configuration.ImageType.SeasonBackdrop ||
                    /* Poster */ this.ImageType == Configuration.ImageType.SeasonPoster)
                {
                    if (/* Fanart */ this.ImageType == Configuration.ImageType.SeasonBackdrop ||
                        /* Poster*/ this.ImageType == Configuration.ImageType.SeasonPoster)
                    {
                        nfoStreamWriter.Write("    ");
                    }

                    nfoStreamWriter.WriteLine("    <thumb type=\"season\" season=\"" + this.Season + "\">smb://" + this.Configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][this.Media.Server.ElementAt(0).Number.ToString()] + "/XBMC/" + (this.Media.GetType().ToString().Contains("Movie") ? "Filme" : "Serien") + "/" + this.Media.MediaPath + "/" + (this.Season != "-1" ? "Season " + this.Media.ConvertSeason(this.Season) + "/" : string.Empty) + this.MediaFilePath.Filename + "</thumb>");
                }

                if (!this.MediaFilePath.DevicePathToDestination.Contains("http://") && this.ImageType != Configuration.ImageType.Unknown)
                {
                    bfStreamWriter.WriteLine("/bin/cp \"" + this.MediaFilePath.DevicePathForPublication + "\" \"" + this.MediaFilePath.DeviceFilenameOnDestination + "\"");
                }
            }
        }

        /// <summary>
        /// Sets season from special tags present in title
        /// </summary>
        /// <param name="title">title of image, containing tags for season</param>
        /// <returns>title of image cleaned of tags for season</returns>
        public string OverrideSeason(string title)
        {
            this.Season = "0";

            if (title.Contains("(S"))
            {
                this.Season = title.RightOf("(S").LeftOf(")");
                title = title.Replace("(S" + this.Season + ")", string.Empty);
            }

            return title.Trim();
        }

        #endregion
    }
}
