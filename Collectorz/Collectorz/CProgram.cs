using System.IO;
using System.Text;


/// <summary>
/// <para>Namespace for managing .nfo-export from Collectorz-Programs </para>
/// </summary>
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
            CConfiguration configuration = new CConfiguration();
            CMediaCollection MediaCollection = new CMediaCollection(configuration);
            MediaCollection.readXML(configuration.MovieCollectorLocalPathToXMLExport);

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