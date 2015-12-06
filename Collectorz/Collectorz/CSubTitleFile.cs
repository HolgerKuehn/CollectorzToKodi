using System.IO;
using System.Xml;

/// <summary>
/// Namespace for managing .nfo-export from Collectorz-Programs <br/>
/// </summary>
namespace Collectorz
{
    public class CSubTitleFile : CMediaFile
    {
        #region Attributes
        private string language;
        #endregion
        #region Constructor
        public CSubTitleFile(CConfiguration configuration)
            : base(configuration)
        {
            this.language = "Deutsch";
        }
        #endregion
        #region Properties
        public string Language
        {
            get { return this.language; }
            set { this.language = value; }
        }
        
        #endregion
        #region Functions
        public override CMediaFile clone()
        {
            CSubTitleFile subTitleFileClone = new CSubTitleFile(this.Configuration);
            subTitleFileClone.Language = this.Language;
            subTitleFileClone.Description = this.Description;
            subTitleFileClone.URL = this.URL;
            subTitleFileClone.URLLocalFilesystem = this.URLLocalFilesystem;
            subTitleFileClone.Filename = this.Filename;
            subTitleFileClone.Extention = this.Extention;
            subTitleFileClone.Server = this.Server;
            subTitleFileClone.Media = this.Media;

            return (CMediaFile)subTitleFileClone;
        }
        public CMediaFile checkForSubTitleStreamFile(XmlNode XMLMedia)
        {
            CSrtSubTitleFileCollection srtSubTitleFileCollection = null;
            CSubTitleFile subTitleFile = null;
            bool isSrtSubTitleFile = false;

            foreach (XmlNode XMLSubTitleStreamFile in XMLMedia.XMLReadSubnode("links").XMLReadSubnodes("link"))
            {
                // check all links for subtitle in language
                if ((XMLSubTitleStreamFile.XMLReadSubnode("urltype").XMLReadInnerText("") == "Movie") && XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("").Contains("Untertitel." + this.Language + "."))
                {
                    isSrtSubTitleFile = false;

                    // create new subtitle objects (generic Subtitle or SRT-Subtitle)
                    subTitleFile = (CSubTitleFile)this.clone();
                    
                    // name and filenames
                    subTitleFile.Description = XMLSubTitleStreamFile.XMLReadSubnode("description").XMLReadInnerText("");
                    subTitleFile.URL = XMLSubTitleStreamFile.XMLReadSubnode("url").XMLReadInnerText("");
                    subTitleFile.convertFilename();

                    // check for fileIndex
                    int completeLength = subTitleFile.Description.Length;
                    int subtitleLength = ("Untertitel." + this.Language + ".").Length;
                    int fileIndex = 1;

                    if (!int.TryParse(subTitleFile.Description.Substring(subtitleLength, completeLength - subtitleLength).LeftOf("."), out fileIndex))
                        fileIndex = 1;

                    // subtitle file name and type
                    if (subTitleFile.Extention.Contains(".srt"))
                    {
                        isSrtSubTitleFile = true;
                        if (srtSubTitleFileCollection == null)
                            srtSubTitleFileCollection = new CSrtSubTitleFileCollection(this);

                        subTitleFile.Filename = subTitleFile.Media.Filename + " part " + ("0000" + fileIndex.ToString()).Substring(fileIndex.ToString().Length) + subTitleFile.Extention;

                        while (srtSubTitleFileCollection.SubTitleFiles.Count < fileIndex)
                            srtSubTitleFileCollection.SubTitleFiles.Add(null);

                        if (srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1] == null)
                            srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1] = new CSrtSubTitleFile(subTitleFile);

                        srtSubTitleFileCollection.SubTitleFiles[fileIndex - 1].readFromSubTitleFile(subTitleFile, fileIndex);
                    }
                }
            }


            // collecting subs and return them as CMediaFiles
            if (isSrtSubTitleFile)
                return (CMediaFile)srtSubTitleFileCollection;
            else
                return (CMediaFile)subTitleFile;
        }
        public virtual void writeSubTitleStreamDataToSH(StreamWriter swrSH)
        {
            if (this.Filename != "")
                swrSH.WriteLine("/bin/ln -s \"" + this.URLLocalFilesystem + "\" \"" + this.Filename + "\"");
        }
        #endregion
    }
}
