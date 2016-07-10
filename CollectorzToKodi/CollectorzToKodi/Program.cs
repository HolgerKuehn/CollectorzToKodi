// <copyright file="Program.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.IO;
    using System.Text;

    /// <summary>
    /// Main class of CollectorzToKodi converter, managing main program flow<br/>
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Entry-point for command-line-tool<br/>
        /// </summary>
        /// <returns>Error Code 1, if no valid file was assigned</returns>
        public static int Main()
        {
            Configuration configuration = new Configuration();
            MediaCollection mediaCollection = new MediaCollection(configuration);
            mediaCollection.ReadXML(configuration.MovieCollectorLocalPathToXMLExport);

            for (int i = 0; i < configuration.ServerNumberOfServers; i++)
            {
                int server = (int)i;

                using (StreamWriter swrSH = new StreamWriter(configuration.MovieCollectorLocalPathToXMLExportPath + "NFO" + configuration.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToName][server.ToString()] + "Win.sh", false, Encoding.UTF8, 512))
                {
                    // SH file header
                    swrSH.WriteLine("#!/bin/bash");
                    swrSH.WriteLine(string.Empty);

                    // Movie
                    swrSH.WriteLine("cd /share/XBMC/Filme/");

                    foreach (Movie movie in mediaCollection.ListMovieCollectionPerServer(server))
                    {
                        movie.WriteNFO();
                        movie.WriteSH(swrSH);
                    }

                    // series
                    swrSH.WriteLine(string.Empty);
                    swrSH.WriteLine("cd /share/XBMC/Serien/");

                    foreach (Series series in mediaCollection.ListSeriesCollectionPerServer(server))
                    {
                        series.WriteNFO();
                        series.WriteSH(swrSH);
                    }
                }
            }

            return 0;
        }
    }
}

// TODO
// Folder per Season
    // fanart.jpg
    // all other files accordingly

// check in Links, if Season exists