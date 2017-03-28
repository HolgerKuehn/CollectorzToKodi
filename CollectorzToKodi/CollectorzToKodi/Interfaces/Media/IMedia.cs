// <copyright file="IMedia.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.IO;
    using System.Xml;

    public interface IMedia
    {
        Configuration Configuration { get; set; }

        string Content { get; set; }

        string Country { get; set; }

        string Filename { get; set; }

        List<string> Genres { get; set; }

        List<ImageFile> Images { get; set; }

        List<MediaFile> MediaFiles { get; set; }

        string MediaGroup { get; set; }

        List<string> MediaLanguages { get; set; }

        string PublishingDate { get; set; }

        string PublishingYear { get; set; }

        string Rating { get; set; }

        string RunTime { get; set; }

        List<int> Server { get; set; }

        List<string> Studios { get; set; }

        string Title { get; set; }

        string TitleOriginal { get; set; }

        string TitleSort { get; set; }

        void AddServer(List<int> serverList);

        void AddServer(int serverList);

        Media Clone();

        void ClonePerLanguage(List<string> isoCodesToBeReplaced, string isoCodeForReplacemant);

        string ConvertSeason(string season);

        void ReadGenre(XmlNode xMLNode);

        void ReadImages(XmlNode xMLNode);

        void ReadMediaFiles(XmlNode xMLMedia);

        void ReadStudio(XmlNode xMLNode);

        void WriteGenre(StreamWriter swrNFO);

        void WriteImagesToNFO(StreamWriter swrNFO);

        void WriteImagesToSH(StreamWriter swrSH);

        void WriteNFO();

        void WriteSH(StreamWriter swrSH, bool createNewMedia = true);

        void WriteStudio(StreamWriter swrNFO);
    }
}