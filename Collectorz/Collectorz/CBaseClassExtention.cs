using System;
using System.Collections.Generic;
using System.IO;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Collectorz
{
    /// <summary>
    /// extension class for XML-type-variables 
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static class CXML
    {
        #region XML-Files
        // liest den Wurzelknoten aus einer XML-Datei
        #region public static XmlNode XMLReadFile(string Filename, string Rootnodename)
        /// <summary>
        /// liest den Wurzelknoten aus einer XML-Datei
        /// </summary>
        /// <param name="Filename">Name der XML-Datei, aus der gelesen werden soll</param>
        /// <param name="Rootnodename">Name des Wurzelknotens</param>
        public static XmlNode XMLReadFile(string Filename, string Rootnodename)
        {
            if (!File.Exists(Filename)) return null;
            XmlDocument XMLInputFile = new XmlDocument();
            try { XMLInputFile.Load(Filename); }
            catch (Exception ex) { throw new Exception("Die Datei \"" + Filename + "\" enthält kein valides XML.\n\n" + ex.ToString()); }
            return XMLInputFile.SelectSingleNode(Rootnodename);
        }
        #endregion

        // bestimmt einen untergeordneten XML-Knoten
        #region public static XmlNode XMLReadSubnode(this XmlNode Node, string Subnodename)
        /// <summary>
        /// bestimmt einen untergeordneten XML-Knoten
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, aus dem gelesen werden soll</param>
        /// <param name="Subnodename">Name des untergeordneten XML-Knotens</param>
        public static XmlNode XMLReadSubnode(this XmlNode Node, string Subnodename)
        {
            if (Node == null) return null;
            return Node.SelectSingleNode(Subnodename);
        }
        #endregion

        // bestimmt mehrere untergeordnete XML-Knoten
        #region public static List<XmlNode> XMLReadSubnodes(this XmlNode Node, string Subnodename, string UniqueAttribute = null)
        /// <summary>
        /// bestimmt mehrere untergeordnete XML-Knoten
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, aus dem gelesen werden soll</param>
        /// <param name="Subnodename">Name der untergeordneten XML-Knoten</param>
        /// <param name="UniqueAttribute"></param>
        public static List<XmlNode> XMLReadSubnodes(this XmlNode Node, string Subnodename, string UniqueAttribute = null)
        {
            if (Node == null) return new List<XmlNode>();
            List<XmlNode> Result = new List<XmlNode>();
            foreach (XmlNode Subnode in Node.SelectNodes(Subnodename)) Result.Add(Subnode);
            if (UniqueAttribute != null)
            {
                List<string> UsedAttributes = new List<string>();
                foreach (XmlNode Entry in Result)
                {
                    string Value = Entry.XMLReadAttribute(UniqueAttribute, null);
                    if (Value == null || UsedAttributes.Contains(Value)) throw new Exception("Unter dem Knoten \"" + Node.Name + "\" befinden sich 2 untergeordnete Knoten mit identischem Schlüsselattribut \"" + UniqueAttribute + "\".");
                    UsedAttributes.Add(Value);
                }
            }
            return Result;
        }
        #endregion

        // liest ein Attribut aus einem XML-Knoten aus
        #region public static string XMLReadAttribute(this XmlNode Node, string Attributename, string Defaultvalue)
        /// <summary>
        /// liest ein Attribut aus einem XML-Knoten aus
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, aus dem gelesen werden soll</param>
        /// <param name="Attributename">Name des Attributes</param>
        /// <param name="Defaultvalue">Standardwert der verwendet wird, wenn das Attribut nicht existiert</param>
        public static string XMLReadAttribute(this XmlNode Node, string Attributename, string Defaultvalue)
        {
            if (Node == null || Node.Attributes[Attributename] == null) return Defaultvalue;
            return Node.Attributes[Attributename].Value;
        }
        #endregion

        // liest den inneren Text aus einem XML-Knoten aus
        #region public static string XMLReadInnerText(this XmlNode Node, string Defaultvalue)
        /// <summary>
        /// liest den inneren Text aus einem XML-Knoten aus
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, aus dem gelesen werden soll</param>
        /// <param name="Defaultvalue">Standardwert der verwendet wird, wenn der XML-Knoten nicht existiert</param>
        public static string XMLReadInnerText(this XmlNode Node, string Defaultvalue)
        {
            if (Node == null) return Defaultvalue;
            return Node.InnerText;
        }
        #endregion

        // erstellt den Wurzelknoten für eine XML-Datei
        #region public static XmlNode XMLWriteRootnode(string Rootnodename)
        /// <summary>
        /// erstellt den Wurzelknoten für eine XML-Datei
        /// </summary>
        /// <param name="Rootnodename">Name des Wurzelknotens</param>
        public static XmlNode XMLWriteRootnode(XmlDocument XMLOutputFile, string Rootnodename)
        {
            if (XMLOutputFile != null) throw new Exception("Es wurde bereits ein Wurzelknoten für eine XML-Datei erstellt. Dieser wurde noch nicht in eine Datei geschrieben.");
            XMLOutputFile = new XmlDocument();
            XmlNode Rootnode = XMLOutputFile.CreateElement(Rootnodename);
            XMLOutputFile.AppendChild(Rootnode);
            return Rootnode;
        }
        #endregion

        // erstellt einen untergeordneten XML-Knoten
        #region public static XmlNode XMLWriteSubnode(this XmlNode Node, string Subnodename)
        /// <summary>
        /// erstellt einen untergeordneten XML-Knoten
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, dem der untergeordnete XML-Knoten hinzugefügt werden soll</param>
        /// <param name="Subnodename">Name des untergeordneten XML-Knotens</param>
        public static XmlNode XMLWriteSubnode(this XmlNode Node, XmlDocument XMLOutputFile, string Subnodename)
        {
            if (XMLOutputFile == null) throw new Exception("Es wurde noch kein Wurzelknoten für die XML-Datei erstellt. Der untergeordnete XML-Knoten kann nicht hinzugefügt werden.");
            XmlNode Subnode = XMLOutputFile.CreateElement(Subnodename);
            Node.AppendChild(Subnode);
            return Subnode;
        }
        #endregion

        // erstellt einen XML-Knoten
        #region public static XmlNode XMLCreateNode(string Nodename)
        /// <summary>
        /// erstellt einen XML-Knoten
        /// </summary>
        /// <param name="Nodename">Name des untergeordneten XML-Knotens</param>
        public static XmlNode XMLCreateNode(XmlDocument XMLOutputFile, string Nodename)
        {
            if (XMLOutputFile == null) throw new Exception("Es wurde noch kein Wurzelknoten für die XML-Datei erstellt. Der XML-Knoten kann nicht erstellt werden.");
            XmlNode Node = XMLOutputFile.CreateElement(Nodename);
            return Node;
        }
        #endregion

        // schreibt ein Attribut in einem XML-Knoten
        #region public static XmlNode XMLWriteAttribute(this XmlNode Node, string Attributename, string Value)
        /// <summary>
        /// schreibt ein Attribut in einem XML-Knoten
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, dem das Attribut hinzugefügt werden soll</param>
        /// <param name="Attributename">Name des Attributes</param>
        /// <param name="Value">Wert der geschrieben werden soll</param>
        public static XmlNode XMLWriteAttribute(this XmlNode Node, XmlDocument XMLOutputFile, string Attributename, string Value)
        {
            if (XMLOutputFile == null) throw new Exception("Es wurde noch kein Wurzelknoten für die XML-Datei erstellt. Das Attribut kann nicht hinzugefügt werden.");
            XmlAttribute Attribute = XMLOutputFile.CreateAttribute(Attributename);
            Attribute.Value = Value;
            Node.Attributes.Append(Attribute);
            return Node;
        }
        #endregion

        // schreibt einen inneren Text in einem XML-Knoten
        #region public static XmlNode XMLWriteInnerText(this XmlNode Node, string Value)
        /// <summary>
        /// schreibt einen inneren Text in einem XML-Knoten
        /// </summary>
        /// <param name="Node">Name des XML-Knoten, dem der innere Text hinzugefügt werden soll</param>
        /// <param name="Value">Wert der geschrieben werden soll</param>
        public static XmlNode XMLWriteInnerText(this XmlNode Node, XmlDocument XMLOutputFile, string Value)
        {
            if (XMLOutputFile == null) throw new Exception("Es wurde noch kein Wurzelknoten für die XML-Datei erstellt. Der innere Text kann nicht hinzugefügt werden.");
            Node.InnerText = Value;
            return Node;
        }
        #endregion

        // schreibt den Wurzelknoten in eine XML-Datei
        #region public static void XMLWriteFile(string Filename, string BackupLocation = null)
        /// <summary>
        /// schreibt den Wurzelknoten in eine XML-Datei
        /// </summary>
        /// <param name="Filename">Name der XML-Datei, in die geschrieben werden soll</param>
        /// <param name="BackupLocation">Backupordner in dem eine Sicherheitskopie der Datei abgelegt wird</param>
        public static void XMLWriteFile(string Filename, XmlDocument XMLOutputFile, string BackupLocation = null)
        {
            //if (XMLOutputFile == null) throw new Exception("Es wurde noch kein Wurzelknoten für die XML-Datei erstellt. Die Datei kann nicht geschrieben werden.");
            //M_File.DirectoryCreate(Path.GetDirectoryName(Filename));

            //if (BackupLocation != null) FileBackup(Filename, BackupLocation); // ggf. Backup anlegen
            //FileDelete(Filename);

            try { XMLOutputFile.Save(Filename); }
            catch (Exception ex) { throw new Exception("Die Datei \"" + Filename + "\" konnte nicht geschrieben werden.\n\n" + ex.ToString()); }
            XMLOutputFile = null;
        }
        #endregion
        #endregion
    }

    /// <summary>
    /// extension class for String-type-variables 
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static class CString
    {
        #region Variablen
        // aus Performanzgründen werden diese Klassen nicht jedes mal neu erstellt
        private static MD5CryptoServiceProvider MD5HashProvider = null;
        private static TripleDESCryptoServiceProvider CryptoProvider = null;
        #endregion
        #region Aufzählungen
        /// <summary>
        ///     Kodierungsmethode für die Erweiterungen "_Encode" und "_Decode"
        /// </summary>
        public enum EncodingMethods
        {
            /// <summary>
            ///     <para>konvertiert die Zeichenkette in die Base64 Kodierung</para>
            ///     <para>&#160;&#160;&#160;Es werden dabei nur folgende Zeichen verwendet: A–Z, a–z, 0–9, +, /, =</para>
            /// </summary>
            Base64,
            /// <summary>
            ///     <para>ersetzt Sonderzeichen für den Einsatz in XML Dateien</para>
            ///     <para>&#160;&#160;&#160;"&lt;" > &amp;lt;</para>
            ///     <para>&#160;&#160;&#160;">" > &amp;gt;</para>
            ///     <para>&#160;&#160;&#160;"'" > &amp;apos;</para>
            ///     <para>&#160;&#160;&#160;""" > &amp;quot;</para>
            ///     <para>&#160;&#160;&#160;"&amp;" > &amp;amp;</para>
            /// </summary>
            XML,
            /// <summary>
            ///     <para>Im SQL Server muss die ausführbare Datei beim Aufruf durch "execute master..xp_cmdshell" besonders maskiert werden, wenn diese Leerzeichen enthält (andere Parameter sind nicht betroffen)</para>
            ///     <para>&#160;&#160;&#160;Beispiel: D:\Projekt\Aktualisierung\"SSMW Aufträge verwalten.exe"</para>
            ///     <para>&#160;&#160;&#160;Beispiel: C:\"Program Files (x86)"\"Internet Explorer"\iexplore.exe</para>
            /// </summary>
            SQL_Server_Path,
            /// <summary>
            ///     <para>In HTML können E-Mailadressen verschlüsselt werden, damit diese durch Spambots nicht direkt ausgelesen werden können:</para>
            ///     <para>&lt;a href="mailto:webmaster@google.de">Webmaster von Google&lt;/a></para>
            ///     <para>&lt;a href="&#38;#109;&#38;#097;&#38;#105;&#38;#108;&#38;#116;&#38;#111;&#38;#058;&#38;#119;&#38;#101;&#38;#098;&#38;#109;&#38;#097;&#38;#115;&#38;#116;&#38;#101;&#38;#114;&#38;#064;&#38;#103;&#38;#111;&#38;#111;&#38;#103;&#38;#108;&#38;#101;&#38;#046;&#38;#100;&#38;#101;">Webmaster von Google&lt;/a></para>
            /// </summary>
            EncryptedHTML,
            /// <summary>
            ///     <para>HTML URLs durfen keine Leerzeichen enthalten:</para>
            ///     <para>&#160;&#160;&#160;Beispiel: abc def?</para>
            ///     <para>&#160;&#160;&#160;Beispiel: abc%20def%3f</para>
            /// </summary>
            HTMLUrl
        };
        #endregion
        #region Low-Level Erweiterungen
        // Erweiterung: den Text links vom ersten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird der ganze Text zurückgegeben)
        #region public static string LeftOf(this string Text, string Searchtext, bool CaseSensitive = true)
        /// <summary>
        ///     den Text links vom ersten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird der ganze Text zurückgegeben)
        /// </summary>
        /// <param name="Text">zu durchsuchender Text</param>
        /// <param name="Searchtext">zu suchender Text</param>
        /// <param name="CaseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        public static string LeftOf(this string Text, string Searchtext, bool CaseSensitive = true)
        {
            if (Text == null || Searchtext == null) return null;
            int pos = Text.IndexOf(Searchtext, CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            if (pos == -1) return Text;
            return Text.Substring(0, pos);
        }
        #endregion

        // Erweiterung: den Text links vom letzten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird ein Leerstring zurückgegeben)
        #region public static string LeftOfLast(this string Text, string Searchtext, bool CaseSensitive = true)
        /// <summary>
        ///     den Text links vom letzten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird ein Leerstring zurückgegeben)
        /// </summary>
        /// <param name="Text">zu durchsuchender Text</param>
        /// <param name="Searchtext">zu suchender Text</param>
        /// <param name="CaseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        public static string LeftOfLast(this string Text, string Searchtext, bool CaseSensitive = true)
        {
            if (Text == null || Searchtext == null) return null;
            int pos = Text.LastIndexOf(Searchtext, CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            if (pos == -1) return "";
            return Text.Substring(0, pos);
        }
        #endregion

        // Erweiterung: den Text rechts vom ersten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird ein Leerstring zurückgegeben)
        #region public static string RightOf(this string Text, string Searchtext, bool CaseSensitive = true)
        /// <summary>
        ///     den Text rechts vom ersten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird ein Leerstring zurückgegeben)
        /// </summary>
        /// <param name="Text">zu durchsuchender Text</param>
        /// <param name="Searchtext">zu suchender Text</param>
        /// <param name="CaseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        public static string RightOf(this string Text, string Searchtext, bool CaseSensitive = true)
        {
            if (Text == null || Searchtext == null) return null;
            int pos = Text.IndexOf(Searchtext, CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            if (pos == -1) return "";
            return Text.Substring(pos + Searchtext.Length);
        }
        #endregion

        // Erweiterung: den Text rechts vom letzten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird der ganze Text zurückgegeben)
        #region public static string RightOfLast(this string Text, string Searchtext, bool CaseSensitive = true)
        /// <summary>
        ///     den Text rechts vom letzten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird der ganze Text zurückgegeben)
        /// </summary>
        /// <param name="Text">zu durchsuchender Text</param>
        /// <param name="Searchtext">zu suchender Text</param>
        /// <param name="CaseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        public static string RightOfLast(this string Text, string Searchtext, bool CaseSensitive = true)
        {
            if (Text == null || Searchtext == null) return null;
            int pos = Text.LastIndexOf(Searchtext, CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
            if (pos == -1) return Text;
            return Text.Substring(pos + Searchtext.Length);
        }
        #endregion

        // Erweiterung: den Text zwischen zwei Vorkommen eines Suchtextes bestimmen (der Index gibt an welche Vorkommen verwendet werden)
        #region public static string IndexOf(this string Text, string Searchtext, int Index = 1, bool CaseSensitive = true)
        /// <summary>
        ///     den Text zwischen zwei Vorkommen eines Suchtextes bestimmen (der Index gibt an welche Vorkommen verwendet werden)
        /// </summary>
        /// <param name="Text">zu durchsuchender Text</param>
        /// <param name="Searchtext">zu suchender Text</param>
        /// <param name="Index">Position des zu suchenden Textes (Standardwert 1)</param>
        /// <param name="CaseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        public static string IndexOf(this string Text, string Searchtext, int Index = 1, bool CaseSensitive = true)
        {
            if (Index < 1 || Text == null || Searchtext == null) return null;
            int pos1 = 0, pos2 = 0;

            while (1 == 1)
            {
                pos1 = Text.IndexOf(Searchtext, pos2, CaseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
                if (--Index == 0) break;
                if (pos1 == -1) return null;
                pos2 = pos1 + Searchtext.Length;
            }
            if (pos1 == -1) pos1 = Text.Length;

            return Text.Substring(pos2, pos1 - pos2);
        }
        #endregion

        // Erweiterung: die ersten "n" Zeichen eines Textes bestimmen
        #region public static string Left(this string Text, int Length = 1)
        /// <summary>
        ///     die ersten "n" Zeichen eines Textes bestimmen
        /// </summary>
        /// <param name="Text">ursprünglicher Text</param>
        /// <param name="Length">Anzahl der zu bestimmenden Zeichen (Standardwert: "1")</param>
        public static string Left(this string Text, int Length = 1)
        {
            if (Text == null) return null;
            if (Length <= 0) return "";
            if (Length >= Text.Length) return Text;
            return Text.Substring(0, Length);
        }
        #endregion

        // Erweiterung: die letzten "n" Zeichen eines Textes bestimmen
        #region public static string Right(this string Text, int Length = 1)
        /// <summary>
        ///     die letzten "n" Zeichen eines Textes bestimmen
        /// </summary>
        /// <param name="Text">ursprünglicher Text</param>
        /// <param name="Length">Anzahl der zu bestimmenden Zeichen (Standardwert: "1")</param>
        public static string Right(this string Text, int Length = 1)
        {
            if (Text == null) return null;
            if (Length <= 0) return "";
            if (Length >= Text.Length) return Text;
            return Text.Substring(Text.Length - Length);
        }
        #endregion

        // Erweiterung: die ersten "n" Zeichen eines Textes entfernen
        #region public static string RemoveLeft(this string Text, int Length = 1)
        /// <summary>
        ///     die ersten "n" Zeichen eines Textes entfernen
        /// </summary>
        /// <param name="Text">ursprünglicher Text</param>
        /// <param name="Length">Anzahl der zu entfernenden Zeichen (Standardwert: "1")</param>
        public static string RemoveLeft(this string Text, int Length = 1)
        {
            if (Text == null) return null;
            if (Length <= 0) return Text;
            if (Length >= Text.Length) return "";
            return Text.Substring(Length);
        }
        #endregion

        // Erweiterung: die letzten "n" Zeichen eines Textes entfernen
        #region public static string RemoveRight(this string Text, int Length = 1)
        /// <summary>
        ///     die letzten "n" Zeichen eines Textes entfernen
        /// </summary>
        /// <param name="Text">ursprünglicher Text</param>
        /// <param name="Length">Anzahl der zu entfernenden Zeichen (Standardwert: "1")</param>
        public static string RemoveRight(this string Text, int Length = 1)
        {
            if (Text == null) return null;
            if (Length <= 0) return Text;
            if (Length >= Text.Length) return "";
            return Text.Substring(0, Text.Length - Length);
        }
        #endregion

        // Erweiterung: dupliziert einen Text beliebig oft
        #region public static string Replicate(this string Text, int Count)
        /// <summary>
        ///     dupliziert einen Text beliebig oft
        /// </summary>
        /// <param name="Text">der zu duplizierende Text</param>
        /// <param name="Count">die Anzahl der zu erstellenden Duplikate</param>
        public static string Replicate(this string Text, int Count)
        {
            return new StringBuilder().Insert(0, Text, Count).ToString();
        }
        #endregion

        // Erweiterung: dreht den Text um, so dass der letzte Buchstabe am Anfang steht
        #region public static string Reverse(this string Text)
        /// <summary>
        ///     dreht den Text um, so dass der letzte Buchstabe am Anfang steht
        /// </summary>
        /// <param name="Text">ursprünglicher Text</param>
        public static string Reverse(this string Text)
        {
            if (Text == null) return null;
            char[] Chars = Text.ToCharArray();
            Array.Reverse(Chars);
            return new String(Chars);
        }
        #endregion

        // Erweiterung: prüft ob ein Text leer oder null ist
        #region public static bool _IsNullOrEmpty(this string Text)
        /// <summary>
        ///     prüft ob ein Text leer oder null ist
        /// </summary>
        /// <param name="Text">der zu überprüfende Text</param>
        public static bool _IsNullOrEmpty(this string Text)
        {
            return string.IsNullOrEmpty(Text);
        }
        #endregion
        #endregion
        #region High-Level Erweiterungen
        // Erweiterung: durch ein Trennzeichen getrennte Bestandteile eines Textes in eine Liste einfügen
        #region public static List<string> _Split(this string Text, string Delimeter = "\n", string OmitCharacters = null, bool AddEmptyEntries = true)
        /// <summary>
        ///     durch ein Trennzeichen getrennte Bestandteile eines Textes in eine Liste einfügen
        /// </summary>
        /// <param name="Text">der aufzuteilende Text</param>
        /// <param name="Delimeter">Trennzeichen (Standardwert: "\n")</param>
        /// <param name="OmitCharacters">am Anfang und Ende eines Bestandteiles zu ignorierende Zeichen (Standardwert: "null")</param>
        /// <param name="AddEmptyEntries">fügt Bestandteile auch dann hinzu, wenn diese leer sind (Standardwert: "true")</param>
        public static List<string> _Split(this string Text, string Delimeter = "\n", string OmitCharacters = null, bool AddEmptyEntries = true)
        {
            if (Text == null || Delimeter == null) return null;
            string[] ResultArray = Text.Split(new string[] { Delimeter }, StringSplitOptions.None);
            List<string> Result = new List<string>();
            for (int i = 0; i < ResultArray.Length; i++)
            {
                string Entry = null;
                if (OmitCharacters == null) Entry = ResultArray[i];
                else Entry = ResultArray[i].Trim(OmitCharacters.ToCharArray());
                if (AddEmptyEntries || !string.IsNullOrEmpty(Entry)) Result.Add(Entry);
            }
            return Result;
        }
        #endregion

        // Erweiterung: einzelne Elemente einer Liste mit einem Trennzeichen zusammenfügen
        #region public static string Join<T>(this List<T> Source, string Delimeter = ", ", string OmitCharacters = null, bool AddEmptyEntries = true)
        /// <summary>
        ///     einzelne Elemente einer Liste mit einem Trennzeichen zusammenfügen
        /// </summary>
        /// <param name="Source">Liste deren Elemente verbunden werden sollen - Die Elemente werden jeweils über die Methode "ToString()" bestimmt</param>
        /// <param name="Delimeter">Trennzeichen zum Verbinden der Elemente (Standardwert: ", ")</param>
        /// <param name="OmitCharacters">am Anfang und Ende eines Listeneintrags zu ignorierende Zeichen (Standardwert: "null")</param>
        /// <param name="AddEmptyEntries">fügt Listeneinträge auch dann hinzu, wenn diese leer sind (Standardwert: "true")</param>
        public static string Join<T>(this List<T> Source, string Delimeter = ", ", string OmitCharacters = null, bool AddEmptyEntries = true)
        {
            if (Source == null || Delimeter == null) return null;
            StringBuilder Result = new StringBuilder();
            for (int i = 0; i < Source.Count; i++)
            {
                string Entry = null;
                if (OmitCharacters == null) Entry = Source[i].ToString();
                else Entry = Source[i].ToString().Trim(OmitCharacters.ToCharArray());
                if (AddEmptyEntries || !string.IsNullOrEmpty(Entry))
                {
                    if (i != 0) Result.Append(Delimeter);
                    Result.Append(Entry);
                }
            }
            return Result.ToString();
        }
        #endregion

        // Erweiterung: in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen
        #region public static string Replace(this string Text, params string[] ReplaceEntries)
        /// <summary>
        ///     in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen
        /// </summary>
        /// <param name="Text">der aufzuteilende Text</param>
        /// <param name="ReplaceEntries">Angabe von Texttupeln: jeweils der zu ersetzende mit dem zugehörigen neuen Text</param>
        public static string Replace(this string Text, params string[] ReplaceEntries)
        {
            if (Text == null || ReplaceEntries.Length == 0) return null;
            if (ReplaceEntries.Length % 2 == 1) throw new Exception("M_String._Replace: ungültige Parameteranzahl");

            StringBuilder Result = new StringBuilder(Text);
            for (int i = 0; i < ReplaceEntries.Length; i += 2)
            {
                if (ReplaceEntries[i] == null || ReplaceEntries[i + 1] == null) return null;
                Result.Replace(ReplaceEntries[i], ReplaceEntries[i + 1]);
            }
            return Result.ToString();
        }
        #endregion

        // Erweiterung: in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen (so oft wie möglich)
        #region public static string ReplaceAll(this string Text, params string[] ReplaceEntries)
        /// <summary>
        ///     in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen (so oft wie möglich)
        /// </summary>
        /// <param name="Text">der aufzuteilende Text</param>
        /// <param name="ReplaceEntries">Angabe von Texttupeln: jeweils der zu ersetzende mit dem zugehörigen neuen Text</param>
        public static string ReplaceAll(this string Text, params string[] ReplaceEntries)
        {
            if (Text == null || ReplaceEntries.Length == 0) return null;
            if (ReplaceEntries.Length % 2 == 1) throw new Exception("M_String._Replace: ungültige Parameteranzahl");

            StringBuilder Result = new StringBuilder(Text);
            for (int i = 0; i < ReplaceEntries.Length; i += 2)
            {
                if (ReplaceEntries[i] == null || ReplaceEntries[i + 1] == null) return null;

                // es werden nur dann Ersetzungen mehrfach durchgeführt, wenn die Zeichenkette dadurch kürzer wird (Rekursionsgefahr)
                int Length = int.MaxValue;
                while (Length > Result.Length)
                {
                    Length = Result.Length;
                    Result.Replace(ReplaceEntries[i], ReplaceEntries[i + 1]);
                }
            }
            return Result.ToString();
        }
        #endregion

        // Erweiterung: prüft ob der reguläre Ausdruck im Text vorhanden ist
        #region public static bool _RegEx_IsMatch(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     prüft ob der reguläre Ausdruck im Text vorhanden ist
        /// </summary>
        /// <param name="Text">zu prüfender Text</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static bool _RegEx_IsMatch(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null) return false;
            return Regex.IsMatch(Text, Pattern, Options);
        }
        #endregion

        // Erweiterung: sucht das erste Vorkommen des regulären Ausdrucks im Text
        #region public static string RegEx_Match(this string Text, string Pattern, int Group, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     sucht das erste Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="Text">zu prüfender Text</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="Group">
        ///     <para>Standardmäßig wird das Matching des vollständigen regulären Ausdruckes zurückgegeben</para>
        ///     <para>Dieser Parameter bestimmt, welcher Teilausdruck (in runden Klammern) alternativ für die Rückgabe verwendet werden soll</para>
        ///     <para>Beispiel: @"^(-|\+)?[0-9]+(,[0-9]+)?$"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 0: @"^(-|\+)?[0-9]+(,[0-9]+)?$"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 1: @"(-|\+)?"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 2: @"(,[0-9]+)?"</para>
        /// </param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static string RegEx_Match(this string Text, string Pattern, int Group, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null) return null;
            Match MatchResult = Regex.Match(Text, Pattern, Options);
            if (!MatchResult.Success) return null;
            if (Group >= 0 && Group < MatchResult.Groups.Count) return MatchResult.Groups[Group].Value;
            else return null;
        }
        #endregion

        // Erweiterung: sucht das erste Vorkommen des regulären Ausdrucks im Text
        #region public static List<string> _RegEx_Match(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     sucht das erste Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="Text">zu prüfender Text</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static List<string> _RegEx_Match(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null) return null;
            Match MatchResult = Regex.Match(Text, Pattern, Options);
            if (!MatchResult.Success) return new List<string>();

            List<string> Result = new List<string>();
            foreach (Group Entry in MatchResult.Groups) Result.Add(Entry.Value);
            return Result;
        }
        #endregion

        // Erweiterung: sucht alle Vorkommen des regulären Ausdrucks im Text
        #region public static List<string> _RegEx_Matches(this string Text, string Pattern, int Group, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     sucht alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="Text">zu prüfender Text</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="Group">
        ///     <para>Standardmäßig wird das Matching des vollständigen regulären Ausdruckes zurückgegeben</para>
        ///     <para>Dieser Parameter bestimmt, welcher Teilausdruck (in runden Klammern) alternativ für die Rückgabe verwendet werden soll</para>
        ///     <para>Beispiel: @"^(-|\+)?[0-9]+(,[0-9]+)?$"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 0: @"^(-|\+)?[0-9]+(,[0-9]+)?$"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 1: @"(-|\+)?"</para>
        ///     <para>&#160;&#160;&#160;Gruppe 2: @"(,[0-9]+)?"</para>
        /// </param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static List<string> _RegEx_Matches(this string Text, string Pattern, int Group, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null) return null;
            MatchCollection MatchResults = Regex.Matches(Text, Pattern, Options);
            if (MatchResults.Count == 0) return new List<string>();
            List<string> Result = new List<string>();
            foreach (Match MatchResult in MatchResults)
            {
                if (Group >= 0 && Group < MatchResult.Groups.Count) Result.Add(MatchResult.Groups[Group].Value);
                else return null;
            }
            return Result;
        }
        #endregion

        // Erweiterung: sucht alle Vorkommen des regulären Ausdrucks im Text
        #region public static List<List<string>> _RegEx_Matches(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     sucht alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="Text">zu prüfender Text</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static List<List<string>> _RegEx_Matches(this string Text, string Pattern, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null) return null;
            MatchCollection MatchResults = Regex.Matches(Text, Pattern, Options);
            if (MatchResults.Count == 0) return new List<List<string>>();
            List<List<string>> Result = new List<List<string>>();
            foreach (Match MatchResult in MatchResults)
            {
                Result.Add(new List<string>());
                foreach (Group Entry in MatchResult.Groups) Result[Result.Count - 1].Add(Entry.Value);
            }
            return Result;
        }
        #endregion

        // Erweiterung: ersetzt alle Vorkommen des regulären Ausdrucks im Text
        #region public static string RegEx_Replace(this string Text, string Pattern, string ReplaceText, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        /// <summary>
        ///     ersetzt alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="Text">Text in dem ersetzt werden soll</param>
        /// <param name="Pattern">
        ///     <para>verwendeter regulärer Ausdruck</para>
        ///     <para>&#160;</para>
        ///     <para>Zeichenklassen:</para>
        ///     <para>&#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile</para>
        ///     <para>&#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern</para>
        ///     <para>&#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt</para>
        ///     <para>&#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)</para>
        ///     <para>&#160;</para>
        ///     <para>Ausdrücke zusammensetzen:</para>
        ///     <para>&#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen</para>
        ///     <para>&#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten</para>
        ///     <para>&#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen</para>
        /// </param>
        /// <param name="ReplaceText">der neue Text</param>
        /// <param name="Options">
        ///     <para>für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")</para>
        ///     <para>&#160;</para>
        ///     <para>wichtige Werte:</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen</para>
        ///     <para>&#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen</para>
        /// </param>
        public static string RegEx_Replace(this string Text, string Pattern, string ReplaceText, RegexOptions Options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (Text == null || Pattern == null || ReplaceText == null) return null;
            return Regex.Replace(Text, Pattern, ReplaceText, Options);
        }
        #endregion

        // Erweiterung: bestimmt den MD5 Hash eines Textes
        #region public static string MD5Hash(this string Text)
        /// <summary>
        ///     bestimmt den MD5 Hash eines Textes
        /// </summary>
        /// <param name="Text">Text für den ein MD5 Hash bestimmt werden soll</param>
        public static string MD5Hash(this string Text)
        {
            if (Text == null) return null;
            UTF8Encoding UTF8 = new UTF8Encoding();
            if (MD5HashProvider == null) MD5HashProvider = new MD5CryptoServiceProvider();
            return BitConverter.ToString(MD5HashProvider.ComputeHash(UTF8.GetBytes(Text))).Replace("-", "").ToLower();
        }
        #endregion

        // Erweiterung: kodiert einen Text mit einer bestimmten Kodierungsmethode
        #region public static string Encode(this string Text)
        /// <summary>
        ///     kodiert einen Text mit einer bestimmten Kodierungsmethode
        /// </summary>
        /// <param name="Text">Text der kodiert werden soll</param>
        /// <param name="EncodingMethod">zu verwendende Kodierungsmethode</param>
        public static string Encode(this string Text, EncodingMethods EncodingMethod)
        {
            if (Text == null) return null;
            if (EncodingMethod == EncodingMethods.Base64)
            {
                UTF8Encoding UTF8 = new UTF8Encoding();
                return Convert.ToBase64String(UTF8.GetBytes(Text));
            }
            else if (EncodingMethod == EncodingMethods.XML)
            {
                return SecurityElement.Escape(Text);
            }
            else if (EncodingMethod == EncodingMethods.SQL_Server_Path)
            {
                return Text.Replace("\"", "").Replace(@"\", "\"\\\"").Replace(":\"\\", ":\\") + "\""; // C:\"Program Files (x86)"\"Internet Explorer"\"iexplore.exe"
            }
            else if (EncodingMethod == EncodingMethods.EncryptedHTML)
            {
                string Result = "";
                foreach (char c in Text)
                {
                    int Unicode = c;
                    Result += "&#" + Unicode + ";";
                }
                return Result;
                // <a href="mailto:webmaster@google.de">Webmaster von Google</a></para>
                // <a href="&#109;&#097;&#105;&#108;&#116;&#111;&#058;&#119;&#101;&#098;&#109;&#097;&#115;&#116;&#101;&#114;&#064;&#103;&#111;&#111;&#103;&#108;&#101;&#046;&#100;&#101;">Webmaster von Google</a>
            }
            else if (EncodingMethod == EncodingMethods.HTMLUrl)
            {
                string Result = "";
                foreach (char c in Text)
                {
                    if (c == ' ')
                    {
                        Result += '+';
                    }
                    else if (c.ToString()._RegEx_IsMatch("^[a-zA-Z0-9]$"))
                    {
                        Result += c;
                    }
                    else
                    {
                        int Unicode = c;
                        if (Unicode <= 255) Result += Uri.HexEscape(c);
                        else Result += Uri.EscapeDataString(c.ToString());
                    }
                }
                return Result;

                //return Uri.EscapeDataString(Text);
                //return HttpUtility.UrlEncode(Text);
            }
            else throw new NotImplementedException("M_String._Encode: das gesuchte Element \"" + EncodingMethod.ToString() + "\" wurde nicht implementiert.");
        }
        #endregion

        // Erweiterung: dekodiert einen Text mit einer bestimmten Kodierungsmethode
        #region public static string Decode(this string Text)
        /// <summary>
        ///     dekodiert einen Text mit einer bestimmten Kodierungsmethode
        /// </summary>
        /// <param name="EncodedText">Text der dekodiert werden soll</param>
        /// <param name="EncodingMethod">zu verwendende Kodierungsmethode</param>
        public static string Decode(this string EncodedText, EncodingMethods EncodingMethod)
        {
            if (EncodedText == null) return null;
            if (EncodingMethod == EncodingMethods.Base64)
            {
                UTF8Encoding UTF8 = new UTF8Encoding();
                return UTF8.GetString(System.Convert.FromBase64String(EncodedText));
            }
            else if (EncodingMethod == EncodingMethods.XML)
            {
                return EncodedText.Replace("&lt;", "<", "&gt;", ">", "&apos;", "'", "&quot;", "\"", "&amp;", "&");
            }
            else if (EncodingMethod == EncodingMethods.SQL_Server_Path)
            {
                return "\"" + EncodedText.Replace("\"", "") + "\""; // "C:\Program Files (x86)\Internet Explorer\iexplore.exe"
            }
            else throw new NotImplementedException("M_String._Decode: das gesuchte Element \"" + EncodingMethod.ToString() + "\" wurde nicht implementiert.");
        }
        #endregion

        // Erweiterung: verschlüsselt einen Text mit einem Passwort
        #region public static string Encrypt(this string Text, string Password)
        /// <summary>
        ///     verschlüsselt einen Text mit einem Passwort
        /// </summary>
        /// <param name="Text">Text der verschlüsselt werden soll</param>
        /// <param name="Password">Passwort mit dem verschlüsselt werden soll</param>
        public static string Encrypt(this string Text, string Password)
        {
            if (Text == null || Password == null) return null;
            UTF8Encoding UTF8 = new UTF8Encoding();
            if (MD5HashProvider == null) MD5HashProvider = new MD5CryptoServiceProvider();
            if (CryptoProvider == null) CryptoProvider = new TripleDESCryptoServiceProvider();
            CryptoProvider.Key = MD5HashProvider.ComputeHash(UTF8.GetBytes(Password)); // Passwort hashen
            CryptoProvider.Mode = CipherMode.ECB;
            CryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] TextData = UTF8.GetBytes(Text);
            byte[] EncryptedTextData = CryptoProvider.CreateEncryptor().TransformFinalBlock(TextData, 0, TextData.Length); // Text verschlüsseln
            return Convert.ToBase64String(EncryptedTextData);
        }
        #endregion

        // Erweiterung: entschlüsselt einen Text mit einem Passwort
        #region public static string Decrypt(this string EncryptedText, string Password)
        /// <summary>
        ///     entschlüsselt einen Text mit einem Passwort
        /// </summary>
        /// <param name="EncryptedText">Text der entschlüsselt werden soll</param>
        /// <param name="Password">Passwort mit dem entschlüsselt werden soll</param>
        public static string Decrypt(this string EncryptedText, string Password)
        {
            if (EncryptedText == null || Password == null) return null;
            UTF8Encoding UTF8 = new UTF8Encoding();
            if (MD5HashProvider == null) MD5HashProvider = new MD5CryptoServiceProvider();
            if (CryptoProvider == null) CryptoProvider = new TripleDESCryptoServiceProvider();
            CryptoProvider.Key = MD5HashProvider.ComputeHash(UTF8.GetBytes(Password)); // Passwort hashen
            CryptoProvider.Mode = CipherMode.ECB;
            CryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] TextData;
            try
            {
                byte[] EncryptedTextData = Convert.FromBase64String(EncryptedText);
                TextData = CryptoProvider.CreateDecryptor().TransformFinalBlock(EncryptedTextData, 0, EncryptedTextData.Length); // Text entschlüsseln
            }
            catch
            {
                return null;
            }
            return UTF8.GetString(TextData);
        }
        #endregion
        #endregion
    }    
}
