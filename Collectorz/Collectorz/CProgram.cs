using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Configuration;


namespace Collectorz
{
    /// <summary>
    /// <para>Main class of Collectorz converter, managing main program flow</para>
    /// </summary>
    public class CProgram
    {
        /// <summary>
        /// <para>Entry-point for command-line-tool</para>
        /// </summary>
        /// <returns>Error Code 1, if no valid file was assigned</returns>
        static int Main()
        {
            string[] Arguments = Environment.GetCommandLineArgs();
            if (Arguments.Length != 2)
            {
                Console.Write("Dateiname wurde nicht übergeben.");
                return 1;
            }
            
            CConfiguration configuration = new CConfiguration();
            CMediaCollection MediaCollection = new CMediaCollection(configuration);
            MediaCollection.readXML(Arguments[1]);

            for (int i = 0; i < configuration.ServerNumberOfServers; i++)
            {
                int server = (int)i;

                using (StreamWriter swrSH = new StreamWriter(configuration.MovieCollectorLocalPathToXMLExportPath + "NFO" + configuration.ServerListsOfServers[(int)CConfiguration.ListOfServerTypes.NumberToName][server.ToString()] + "Win.sh", false, Encoding.UTF8, 512))
                {
                    // SH-Dateiheader
                    swrSH.WriteLine("#!/bin/bash");
                    swrSH.WriteLine("");
                    
                    // Movie
                    swrSH.WriteLine("cd /share/XBMC/Filme/");

                    foreach (CMovie movie in MediaCollection.listMovieCollectionPerServer(server))
                    {
                        movie.writeNFO();
                        movie.writeSH(swrSH);
                    }

                    // SH-Dateiheader
                    swrSH.WriteLine("");
                    swrSH.WriteLine("cd /share/XBMC/Serien/");

                    foreach (CSeries series in MediaCollection.listSeriesCollectionPerServer(server))
                    {
                        series.writeNFO();
                        series.writeSH(swrSH);
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