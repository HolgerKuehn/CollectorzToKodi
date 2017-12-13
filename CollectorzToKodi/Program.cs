// <copyright file="Program.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
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
            mediaCollection.ReadFromXml(configuration.MovieCollectorWindowsPathToXmlExport);

            mediaCollection.ExportLibrary();

            return 0;
        }
    }
}

// TODO
// Folder per Season
    // fanart.jpg
    // all other files accordingly

// check in Links, if Season exists