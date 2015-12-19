// <copyright file="CBaseClassExtention.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2016 Holger Kühn. All rights reserved.
// </copyright>

namespace Collectorz
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Security;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Xml;

    /// <summary>
    /// extension class for base types
    /// </summary>
    [global::System.Runtime.CompilerServices.CompilerGenerated]
    public static class CBaseClassExtention
    {
        #region variables

        // for performance reasons these classes are not created each time accessed
        private static MD5CryptoServiceProvider mD5HashProvider = null;
        private static TripleDESCryptoServiceProvider cryptoProvider = null;

        #endregion
        #region enums

        /// <summary>
        /// Methods used for "_Encode" or "_Decode" - extensions
        /// </summary>
        public enum EncodingMethods
        {
            /// <summary>
            ///   codes string to Base64<br/>
            ///   &#160;&#160;&#160;result uses only: A–Z, a–z, 0–9, +, / and =<br/>
            /// </summary>
            Base64,

            /// <summary>
            ///     replaces special characters with in XML files<br/>
            ///     &#160;&#160;&#160;"&lt;" > &amp;lt;<br/>
            ///     &#160;&#160;&#160;">" > &amp;gt;<br/>
            ///     &#160;&#160;&#160;"'" > &amp;apos;<br/>
            ///     &#160;&#160;&#160;""" > &amp;quot;<br/>
            ///     &#160;&#160;&#160;"&amp;" > &amp;amp;<br/>
            /// </summary>
            XML,

            /// <summary>
            ///     In SQL Servers procedure "execute master..xp_cmdshell" it is necessary to mask spaces in a special way.<br/>
            ///     &#160;&#160;&#160;eg. D:\Projekt\Aktualisierung\"SSMW Aufträge verwalten.exe"<br/>
            ///     &#160;&#160;&#160;eg. C:\"Program Files (x86)"\"Internet Explorer"\iexplore.exe<br/>
            /// </summary>
            SQL_Server_Path,

            /// <summary>
            ///     In HTML e-mail-addresses can be encrypted in special way, to avoid recognition by spambots:<br/>
            ///     &lt;a href="mailto:webmaster@google.de">Webmaster Google&lt;/a><br/>
            ///     &lt;a href="&#38;#109;&#38;#097;&#38;#105;&#38;#108;&#38;#116;&#38;#111;&#38;#058;&#38;#119;&#38;#101;&#38;#098;&#38;#109;&#38;#097;&#38;#115;&#38;#116;&#38;#101;&#38;#114;&#38;#064;&#38;#103;&#38;#111;&#38;#111;&#38;#103;&#38;#108;&#38;#101;&#38;#046;&#38;#100;&#38;#101;">Webmaster Google&lt;/a><br/>
            /// </summary>
            EncryptedHTML,

            /// <summary>
            ///     HTML URLs must not contain spaces:<br/>
            ///     &#160;&#160;&#160;eg. abc def?<br/>
            ///     &#160;&#160;&#160;eg. abc%20def%3f<br/>
            /// </summary>
            HTMLUrl
        }

        #endregion
        #region XML-Files

        /// <summary>
        /// reads root node of XML file
        /// </summary>
        /// <param name="filename">Name of XML file to be read</param>
        /// <param name="rootnodename">Name of root node</param>
        /// <returns>XmlNode containing root node</returns>
        public static XmlNode XMLReadFile(string filename, string rootnodename)
        {
            if (!File.Exists(filename))
            {
                return null;
            }

            XmlDocument xmlInputFile = new XmlDocument();

            try
            {
                xmlInputFile.Load(filename);
            }
            catch (Exception ex)
            {
                throw new Exception("The file \"" + filename + "\" does not contain valid XML.\n\n" + ex.ToString());
            }

            return xmlInputFile.SelectSingleNode(rootnodename);
        }

        /// <summary>
        /// select first node matching subnodename
        /// </summary>
        /// <param name="node">XmlNode, that contains subnodes</param>
        /// <param name="subnodename">name of subsequent XML-node</param>
        /// <returns>first node matching subnodename</returns>
        public static XmlNode XMLReadSubnode(this XmlNode node, string subnodename)
        {
            if (node == null)
            {
                return null;
            }

            return node.SelectSingleNode(subnodename);
        }

        /// <summary>
        /// selects many XML-nodes matching subnodename
        /// </summary>
        /// <param name="node">XmlNode, that contains subnodes</param>
        /// <param name="subnodename">name of subsequent XML-node</param>
        /// <param name="uniqueAttribute">name of unique attribute used to identify node</param>
        /// <returns>List of XmlNodes identified by subnodename</returns>
        public static List<XmlNode> XMLReadSubnodes(this XmlNode node, string subnodename, string uniqueAttribute = null)
        {
            if (node == null)
            {
                return new List<XmlNode>();
            }

            List<XmlNode> result = new List<XmlNode>();

            foreach (XmlNode subnode in node.SelectNodes(subnodename))
            {
                result.Add(subnode);
            }

            if (uniqueAttribute != null)
            {
                List<string> usedAttributes = new List<string>();

                foreach (XmlNode entry in result)
                {
                    string value = entry.XMLReadAttribute(uniqueAttribute, null);

                    if (value == null || usedAttributes.Contains(value))
                    {
                        throw new Exception("The node \"" + node.Name + "\" contains two different nodes with identical \"UniqueAttribute\" \"" + uniqueAttribute + "\".");
                    }

                    usedAttributes.Add(value);
                }
            }

            return result;
        }

        /// <summary>
        /// reads attribute from XML-node
        /// </summary>
        /// <param name="node">XmlNode, that contains attribute</param>
        /// <param name="attributename">name of attribute</param>
        /// <param name="defaultvalue">default value used, when specified attribute does not exist or empty</param>
        /// <returns>value of attribute</returns>
        public static string XMLReadAttribute(this XmlNode node, string attributename, string defaultvalue)
        {
            if (node == null || node.Attributes[attributename] == null)
            {
                return defaultvalue;
            }

            return node.Attributes[attributename].Value;
        }

        /// <summary>
        /// reads inner Text of XML-node
        /// </summary>
        /// <param name="node">XmlNode, that contains inner text</param>
        /// <param name="defaultvalue">default value used, when specified inner text does not exist or empty</param>
        /// <returns>value of inner text</returns>
        public static string XMLReadInnerText(this XmlNode node, string defaultvalue)
        {
            if (node == null)
            {
                return defaultvalue;
            }

            return node.InnerText;
        }

        /// <summary>
        /// writes root node to xMLOutputFile
        /// </summary>
        /// <param name="xMLOutputFile">XmlOutputFile used for writing</param>
        /// <param name="rootnodename">name of root node</param>
        /// <returns>top level XmlNode, that had been written to XmlOutputFile</returns>
        public static XmlNode XMLWriteRootnode(XmlDocument xMLOutputFile, string rootnodename)
        {
            if (xMLOutputFile != null)
            {
                throw new Exception("A root node had already been generated, but not written to disk.");
            }

            xMLOutputFile = new XmlDocument();
            XmlNode rootnode = xMLOutputFile.CreateElement(rootnodename);
            xMLOutputFile.AppendChild(rootnode);

            return rootnode;
        }

        /// <summary>
        /// writes subnode to xMLOutputFile
        /// </summary>
        /// <param name="node">XML-node, that is containing the new node specified by subnodename</param>
        /// <param name="xMLOutputFile">XmlOutputFile used for writing</param>
        /// <param name="subnodename">name of subnode</param>
        /// <returns>sub level XmlNode, that had been written to XmlOutputFile</returns>
        public static XmlNode XMLWriteSubnode(this XmlNode node, XmlDocument xMLOutputFile, string subnodename)
        {
            if (xMLOutputFile == null)
            {
                throw new Exception("Root node had not been written to XML file, the sub node can not be added.");
            }

            XmlNode subnode = xMLOutputFile.CreateElement(subnodename);
            node.AppendChild(subnode);

            return subnode;
        }

        /// <summary>
        /// creates XML-Node
        /// </summary>
        /// <param name="xMLOutputFile">XmlOutputFile used for writing</param>
        /// <param name="nodename">name of subnode</param>
        /// <returns>created XmlNode</returns>
        public static XmlNode XMLCreateNode(XmlDocument xMLOutputFile, string nodename)
        {
            if (xMLOutputFile == null)
            {
                throw new Exception("Root node had not been written to XML file, the XML node can not be created.");
            }

            XmlNode node = xMLOutputFile.CreateElement(nodename);

            return node;
        }

        /// <summary>
        /// writes attribute in XMl node
        /// </summary>
        /// <param name="node">XML-node, that is used for adding the attribute</param>
        /// <param name="xMLOutputFile">XmlOutputFile used for writing</param>
        /// <param name="attributename">name of attribute</param>
        /// <param name="value">value of attribute to be written</param>
        /// <returns>XmlNode with added attribute</returns>
        public static XmlNode XMLWriteAttribute(this XmlNode node, XmlDocument xMLOutputFile, string attributename, string value)
        {
            if (xMLOutputFile == null)
            {
                throw new Exception("Root node had not been written to XML file, the attribute can not be created.");
            }

            XmlAttribute attribute = xMLOutputFile.CreateAttribute(attributename);

            attribute.Value = value;
            node.Attributes.Append(attribute);

            return node;
        }

        /// <summary>
        /// write inner Text to XML-node
        /// </summary>
        /// <param name="node">XML-node, that is used for adding inner Text</param>
        /// <param name="value">value of inner text</param>
        /// <returns>XML node with inner Text</returns>
        public static XmlNode XMLWriteInnerText(this XmlNode node, string value)
        {
            node.InnerText = value;
            return node;
        }

        /// <summary>
        /// writes root node to XML file
        /// </summary>
        /// <param name="filename">name of file that is to be written</param>
        /// <param name="xMLOutputFile">XmlOutputFile used for writing</param>
        public static void XMLWriteFile(string filename, XmlDocument xMLOutputFile)
        {
            try
            {
                xMLOutputFile.Save(filename);
            }
            catch (Exception ex)
            {
                throw new Exception("The file \"" + filename + "\" could not be written.\n\n" + ex.ToString());
            }

            xMLOutputFile = null;
        }
        #endregion
        #region low-level extensions

        /// <summary>
        /// text left of the first search string
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text left of searched text; if this is not present the whole test is returned</returns>
        public static string LeftOf(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.IndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return text;
            }

            return text.Substring(0, pos);
        }

        /// <summary>
        /// text that is left of last match of searched text
        /// </summary>
        /// <param name="text">text to be tested</param>
        /// <param name="searchtext">text that is been searched</param>
        /// <param name="caseSensitive">lower and uppercase are been distinguished (default: "true")</param>
        /// <returns>text that is left of last match of searched text; if this is not present the whole test is returned</returns>
        public static string LeftOfLast(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.LastIndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return string.Empty;
            }

            return text.Substring(0, pos);
        }

        /// <summary>
        ///     den Text rechts vom ersten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird ein Leerstring zurückgegeben)
        /// </summary>
        /// <param name="text">zu durchsuchender Text</param>
        /// <param name="searchtext">zu suchender Text</param>
        /// <param name="caseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        /// <returns>text that is right of first match of searched text; if this is not present an empty text is returned</returns>
        public static string RightOf(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.IndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return string.Empty;
            }

            return text.Substring(pos + searchtext.Length);
        }

        /// <summary>
        ///     den Text rechts vom letzten Vorkommen eines Suchtextes bestimmen (wird der Suchtext nicht gefunden, wird der ganze Text zurückgegeben)
        /// </summary>
        /// <param name="text">zu durchsuchender Text</param>
        /// <param name="searchtext">zu suchender Text</param>
        /// <param name="caseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        /// <returns>text that is right of last match of searched text; if this is not present the whole test is returned</returns>
        public static string RightOfLast(this string text, string searchtext, bool caseSensitive = true)
        {
            if (text == null || searchtext == null)
            {
                return null;
            }

            int pos = text.LastIndexOf(searchtext, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);

            if (pos == -1)
            {
                return text;
            }

            return text.Substring(pos + searchtext.Length);
        }

        /// <summary>
        ///     den Text zwischen zwei Vorkommen eines Suchtextes bestimmen (der Index gibt an welche Vorkommen verwendet werden)
        /// </summary>
        /// <param name="text">zu durchsuchender Text</param>
        /// <param name="searchtext">zu suchender Text</param>
        /// <param name="index">Position des zu suchenden Textes (Standardwert 1)</param>
        /// <param name="caseSensitive">Groß- und Kleinschreibung bei der Suche beachten? (Standardwert: "true")</param>
        /// <returns>IndexOf</returns>
        public static string IndexOf(this string text, string searchtext, int index = 1, bool caseSensitive = true)
        {
            if (index < 1 || text == null || searchtext == null)
            {
                return null;
            }

            int pos1 = 0, pos2 = 0;

            while (1 == 1)
            {
                pos1 = text.IndexOf(searchtext, pos2, caseSensitive ? StringComparison.CurrentCulture : StringComparison.CurrentCultureIgnoreCase);
                if (--index == 0)
                {
                    break;
                }

                if (pos1 == -1)
                {
                    return null;
                }

                pos2 = pos1 + searchtext.Length;
            }

            if (pos1 == -1)
            {
                pos1 = text.Length;
            }

            return text.Substring(pos2, pos1 - pos2);
        }

        /// <summary>
        ///     die ersten "n" Zeichen eines Textes bestimmen
        /// </summary>
        /// <param name="text">ursprünglicher Text</param>
        /// <param name="length">Anzahl der zu bestimmenden Zeichen (Standardwert: "1")</param>
        /// <returns>return the first length characters of the text</returns>
        public static string Left(this string text, int length = 1)
        {
            if (text == null)
            {
                return null;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (length >= text.Length)
            {
                return text;
            }

            return text.Substring(0, length);
        }

        /// <summary>
        ///     die letzten "n" Zeichen eines Textes bestimmen
        /// </summary>
        /// <param name="text">ursprünglicher Text</param>
        /// <param name="length">Anzahl der zu bestimmenden Zeichen (Standardwert: "1")</param>
        /// <returns>returns the last length characters of the text</returns>
        public static string Right(this string text, int length = 1)
        {
            if (text == null)
            {
                return null;
            }

            if (length <= 0)
            {
                return string.Empty;
            }

            if (length >= text.Length)
            {
                return text;
            }

            return text.Substring(text.Length - length);
        }

        /// <summary>
        ///     die ersten "n" Zeichen eines Textes entfernen
        /// </summary>
        /// <param name="text">ursprünglicher Text</param>
        /// <param name="length">Anzahl der zu entfernenden Zeichen (Standardwert: "1")</param>
        /// <returns>removes the first length characters from text</returns>
        public static string RemoveLeft(this string text, int length = 1)
        {
            if (text == null)
            {
                return null;
            }

            if (length <= 0)
            {
                return text;
            }

            if (length >= text.Length)
            {
                return string.Empty;
            }

            return text.Substring(length);
        }

        /// <summary>
        ///     die letzten "n" Zeichen eines Textes entfernen
        /// </summary>
        /// <param name="text">ursprünglicher Text</param>
        /// <param name="length">Anzahl der zu entfernenden Zeichen (Standardwert: "1")</param>
        /// <returns>returns the last length characters fro text</returns>
        public static string RemoveRight(this string text, int length = 1)
        {
            if (text == null)
            {
                return null;
            }

            if (length <= 0)
            {
                return text;
            }

            if (length >= text.Length)
            {
                return string.Empty;
            }

            return text.Substring(0, text.Length - length);
        }

        /// <summary>
        ///     dupliziert einen Text beliebig oft
        /// </summary>
        /// <param name="text">der zu duplizierende Text</param>
        /// <param name="count">die Anzahl der zu erstellenden Duplikate</param>
        /// <returns>replicates text count times</returns>
        public static string Replicate(this string text, int count)
        {
            return new StringBuilder().Insert(0, text, count).ToString();
        }

        /// <summary>
        ///     dreht den Text um, so dass der letzte Buchstabe am Anfang steht
        /// </summary>
        /// <param name="text">ursprünglicher Text</param>
        /// <returns>reversed text</returns>
        public static string Reverse(this string text)
        {
            if (text == null)
            {
                return null;
            }

            char[] chars = text.ToCharArray();
            Array.Reverse(chars);

            return new string(chars);
        }

        /// <summary>
        ///     prüft ob ein Text leer oder null ist
        /// </summary>
        /// <param name="text">der zu überprüfende Text</param>
        /// <returns>checks if text is null or empty</returns>
        public static bool IsNullOrEmpty(this string text)
        {
            return string.IsNullOrEmpty(text);
        }

        #endregion
        #region high-level extensions

        /// <summary>
        ///     durch ein Trennzeichen getrennte Bestandteile eines Textes in eine Liste einfügen
        /// </summary>
        /// <param name="text">der aufzuteilende Text</param>
        /// <param name="delimeter">Trennzeichen (Standardwert: "\n")</param>
        /// <param name="omitCharacters">am Anfang und Ende eines Bestandteiles zu ignorierende Zeichen (Standardwert: "null")</param>
        /// <param name="addEmptyEntries">fügt Bestandteile auch dann hinzu, wenn diese leer sind (Standardwert: "true")</param>
        /// <returns>list with text parts delemited by delimeter</returns>
        public static List<string> Split(this string text, string delimeter = "\n", string omitCharacters = null, bool addEmptyEntries = true)
        {
            if (text == null || delimeter == null)
            {
                return null;
            }

            string[] resultArray = text.Split(new string[] { delimeter }, StringSplitOptions.None);
            List<string> result = new List<string>();

            for (int i = 0; i < resultArray.Length; i++)
            {
                string entry = null;
                if (omitCharacters == null)
                {
                    entry = resultArray[i];
                }
                else
                {
                    entry = resultArray[i].Trim(omitCharacters.ToCharArray());
                }

                if (addEmptyEntries || !string.IsNullOrEmpty(entry))
                {
                    result.Add(entry);
                }
            }

            return result;
        }

        /// <summary>
        ///     einzelne Elemente einer Liste mit einem Trennzeichen zusammenfügen
        /// </summary>
        /// <param name="source">Liste deren Elemente verbunden werden sollen - Die Elemente werden jeweils über die Methode "ToString()" bestimmt</param>
        /// <param name="delimeter">Trennzeichen zum Verbinden der Elemente (Standardwert: ", ")</param>
        /// <param name="omitCharacters">am Anfang und Ende eines Listeneintrags zu ignorierende Zeichen (Standardwert: "null")</param>
        /// <param name="addEmptyEntries">fügt Listeneinträge auch dann hinzu, wenn diese leer sind (Standardwert: "true")</param>
        /// <returns>returns string joined from list</returns>
        /// <typeparam name="T">different types of Lists</typeparam>
        public static string Join<T>(this List<T> source, string delimeter = ", ", string omitCharacters = null, bool addEmptyEntries = true)
        {
            if (source == null || delimeter == null)
            {
                return null;
            }

            StringBuilder result = new StringBuilder();

            for (int i = 0; i < source.Count; i++)
            {
                string entry = null;

                if (omitCharacters == null)
                {
                    entry = source[i].ToString();
                }
                else
                {
                    entry = source[i].ToString().Trim(omitCharacters.ToCharArray());
                }

                if (addEmptyEntries || !string.IsNullOrEmpty(entry))
                {
                    if (i != 0)
                    {
                        result.Append(delimeter);
                    }

                    result.Append(entry);
                }
            }

            return result.ToString();
        }

        /// <summary>
        ///     in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen
        /// </summary>
        /// <param name="text">der aufzuteilende Text</param>
        /// <param name="replaceEntries">Angabe von Texttupeln: jeweils der zu ersetzende mit dem zugehörigen neuen Text</param>
        /// <returns>returns text with replacements</returns>
        public static string Replace(this string text, params string[] replaceEntries)
        {
            if (text == null || replaceEntries.Length == 0)
            {
                return null;
            }

            if (replaceEntries.Length % 2 == 1)
            {
                throw new Exception("M_String.Replace: ungültige Parameteranzahl");
            }

            StringBuilder result = new StringBuilder(text);

            for (int i = 0; i < replaceEntries.Length; i += 2)
            {
                if (replaceEntries[i] == null || replaceEntries[i + 1] == null)
                {
                    return null;
                }

                result.Replace(replaceEntries[i], replaceEntries[i + 1]);
            }

            return result.ToString();
        }

        /// <summary>
        ///     in einem Text werden mehrere Ersetzungsvorgänge gleichzeitig vorgenommen (so oft wie möglich)
        /// </summary>
        /// <param name="text">der aufzuteilende Text</param>
        /// <param name="replaceEntries">Angabe von Texttupeln: jeweils der zu ersetzende mit dem zugehörigen neuen Text</param>
        /// <returns>returns text with replacements</returns>
        public static string ReplaceAll(this string text, params string[] replaceEntries)
        {
            if (text == null || replaceEntries.Length == 0)
            {
                return null;
            }

            if (replaceEntries.Length % 2 == 1)
            {
                throw new Exception("M_String._Replace: ungültige Parameteranzahl");
            }

            StringBuilder result = new StringBuilder(text);

            for (int i = 0; i < replaceEntries.Length; i += 2)
            {
                if (replaceEntries[i] == null || replaceEntries[i + 1] == null)
                {
                    return null;
                }

                // es werden nur dann Ersetzungen mehrfach durchgeführt, wenn die Zeichenkette dadurch kürzer wird (Rekursionsgefahr)
                int length = int.MaxValue;

                while (length > result.Length)
                {
                    length = result.Length;
                    result.Replace(replaceEntries[i], replaceEntries[i + 1]);
                }
            }

            return result.ToString();
        }

        /// <summary>
        ///     prüft ob der reguläre Ausdruck im Text vorhanden ist
        /// </summary>
        /// <param name="text">zu prüfender Text</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>returns whether the RegEx is matching or not</returns>
        public static bool RegEx_IsMatch(this string text, string pattern, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null)
            {
                return false;
            }

            return Regex.IsMatch(text, pattern, options);
        }

        /// <summary>
        ///     sucht das erste Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="text">zu prüfender Text</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="group">
        ///     Standardmäßig wird das Matching des vollständigen regulären Ausdruckes zurückgegeben<br/>
        ///     Dieser Parameter bestimmt, welcher Teilausdruck (in runden Klammern) alternativ für die Rückgabe verwendet werden soll<br/>
        ///     Beispiel: @"^(-|\+)?[0-9]+(,[0-9]+)?$"<br/>
        ///     &#160;&#160;&#160;Gruppe 0: @"^(-|\+)?[0-9]+(,[0-9]+)?$"<br/>
        ///     &#160;&#160;&#160;Gruppe 1: @"(-|\+)?"<br/>
        ///     &#160;&#160;&#160;Gruppe 2: @"(,[0-9]+)?"<br/>
        /// </param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>first match of RegEx in Text</returns>
        public static string RegEx_Match(this string text, string pattern, int group, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null)
            {
                return null;
            }

            Match matchResult = Regex.Match(text, pattern, options);

            if (!matchResult.Success)
            {
                return null;
            }

            if (group >= 0 && group < matchResult.Groups.Count)
            {
                return matchResult.Groups[group].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        ///     sucht das erste Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="text">zu prüfender Text</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>List of matches found in text</returns>
        public static List<string> RegEx_Match(this string text, string pattern, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null)
            {
                return null;
            }

            Match matchResult = Regex.Match(text, pattern, options);

            if (!matchResult.Success)
            {
                return new List<string>();
            }

            List<string> result = new List<string>();

            foreach (Group entry in matchResult.Groups)
            {
                result.Add(entry.Value);
            }

            return result;
        }

        /// <summary>
        ///     sucht alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="text">zu prüfender Text</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="group">
        ///     Standardmäßig wird das Matching des vollständigen regulären Ausdruckes zurückgegeben<br/>
        ///     Dieser Parameter bestimmt, welcher Teilausdruck (in runden Klammern) alternativ für die Rückgabe verwendet werden soll<br/>
        ///     Beispiel: @"^(-|\+)?[0-9]+(,[0-9]+)?$"<br/>
        ///     &#160;&#160;&#160;Gruppe 0: @"^(-|\+)?[0-9]+(,[0-9]+)?$"<br/>
        ///     &#160;&#160;&#160;Gruppe 1: @"(-|\+)?"<br/>
        ///     &#160;&#160;&#160;Gruppe 2: @"(,[0-9]+)?"<br/>
        /// </param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>List of matching parts</returns>
        public static List<string> RegEx_Matches(this string text, string pattern, int group, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null)
            {
                return null;
            }

            MatchCollection matchResults = Regex.Matches(text, pattern, options);

            if (matchResults.Count == 0)
            {
                return new List<string>();
            }

            List<string> result = new List<string>();

            foreach (Match matchResult in matchResults)
            {
                if (group >= 0 && group < matchResult.Groups.Count)
                {
                    result.Add(matchResult.Groups[group].Value);
                }
                else
                {
                    return null;
                }
            }

            return result;
        }

        /// <summary>
        ///     sucht alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="text">zu prüfender Text</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>List of matching parts</returns>
        public static List<List<string>> RegEx_Matches(this string text, string pattern, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null)
            {
                return null;
            }

            MatchCollection matchResults = Regex.Matches(text, pattern, options);

            if (matchResults.Count == 0)
            {
                return new List<List<string>>();
            }

            List<List<string>> result = new List<List<string>>();

            foreach (Match matchResult in matchResults)
            {
                result.Add(new List<string>());

                foreach (Group entry in matchResult.Groups)
                {
                    result[result.Count - 1].Add(entry.Value);
                }
            }

            return result;
        }

        /// <summary>
        ///     ersetzt alle Vorkommen des regulären Ausdrucks im Text
        /// </summary>
        /// <param name="text">Text in dem ersetzt werden soll</param>
        /// <param name="pattern">
        ///     verwendeter regulärer Ausdruck<br/>
        ///     &#160;<br/>
        ///     Zeichenklassen:<br/>
        ///     &#160;&#160;&#160;"^" und "$" entsprechen dem Anfang und dem Ende des Textes / einer Zeile<br/>
        ///     &#160;&#160;&#160;"[a-zA-ZäöüßÄÖÜ]" entspricht einem beliebigen Zeichen innerhalb der eckigen Klammern<br/>
        ///     &#160;&#160;&#160;"[^0-9]" entspricht einem beliebigen Zeichen das nicht innerhalb der eckigen Klammern vorkommt<br/>
        ///     &#160;&#160;&#160;"\b" entspricht der leeren Zeichenkette am Anfang oder Ende eines Wortes ("\B" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\d" entspricht [0-9] ("\D" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\w" entspricht [a-zA-Z_0-9] + Umlaute ("\W" entspricht dem Gegenteil davon)<br/>
        ///     &#160;&#160;&#160;"\s" entspricht Leerzeichen, Tabs, etc. ("\S" entspricht dem Gegenteil davon)<br/>
        ///     &#160;<br/>
        ///     Ausdrücke zusammensetzen:<br/>
        ///     &#160;&#160;&#160;"S(ams|onn)tag" entspricht entweder "Samstag" oder "Sonntag"<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)?" der Ausdruck ist optional<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*" der Ausdruck darf beliebig oft vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)+" der Ausdruck darf beliebig oft, aber mindestens einmal vorkommen<br/>
        ///     &#160;&#160;&#160;"(Ausdruck)*?" oder "(Ausdruck)+?" aktiviert genügsames Verhalten<br/>
        ///     &#160;&#160;&#160;"\\", "\(", "\[", "\*", "\?", "\." Escapeausdrücke für Sonderzeichen<br/>
        /// </param>
        /// <param name="replaceText">der neue Text</param>
        /// <param name="options">
        ///     für die Suche des regulären Ausdrucks zu verwendende Optionen (Standardwert: "RegexOptions.Singleline | RegexOptions.Multiline")<br/>
        ///     &#160;<br/>
        ///     wichtige Werte:<br/>
        ///     &#160;&#160;&#160;RegexOptions.IgnoreCase - Groß- und Kleinschreibung wird bei der Suche ignoriert<br/>
        ///     &#160;&#160;&#160;RegexOptions.Multiline - "^" und "$" entspricht nicht nur Anfang und Ende des Textes, sondern auch Anfang und Ende der einzelnen Zeilen<br/>
        ///     &#160;&#160;&#160;RegexOptions.Singleline - ein "." entspricht auch dem "\n" Zeichen<br/>
        /// </param>
        /// <returns>List of matching parts</returns>
        public static string RegEx_Replace(this string text, string pattern, string replaceText, RegexOptions options = RegexOptions.Singleline | RegexOptions.Multiline)
        {
            if (text == null || pattern == null || replaceText == null)
            {
                return null;
            }

            return Regex.Replace(text, pattern, replaceText, options);
        }

        /// <summary>
        ///     bestimmt den MD5 Hash eines Textes
        /// </summary>
        /// <param name="text">Text für den ein MD5 Hash bestimmt werden soll</param>
        /// <returns>MD5 hash of text</returns>
        public static string MD5Hash(this string text)
        {
            if (text == null)
            {
                return null;
            }

            UTF8Encoding uTF8 = new UTF8Encoding();

            if (mD5HashProvider == null)
            {
                mD5HashProvider = new MD5CryptoServiceProvider();
            }

            return BitConverter.ToString(mD5HashProvider.ComputeHash(uTF8.GetBytes(text))).Replace("-", string.Empty).ToLower();
        }

        /// <summary>
        ///     kodiert einen Text mit einer bestimmten Kodierungsmethode
        /// </summary>
        /// <param name="text">Text der kodiert werden soll</param>
        /// <param name="encodingMethod">zu verwendende Kodierungsmethode</param>
        /// <returns>encoded text</returns>
        public static string Encode(this string text, EncodingMethods encodingMethod)
        {
            string result;
            int unicode;

            if (text == null)
            {
                return null;
            }

            if (encodingMethod == EncodingMethods.Base64)
            {
                UTF8Encoding uTF8 = new UTF8Encoding();
                return Convert.ToBase64String(uTF8.GetBytes(text));
            }
            else if (encodingMethod == EncodingMethods.XML)
            {
                return SecurityElement.Escape(text);
            }
            else if (encodingMethod == EncodingMethods.SQL_Server_Path)
            {
                return text.Replace("\"", string.Empty).Replace(@"\", "\"\\\"").Replace(":\"\\", ":\\") + "\""; // C:\"Program Files (x86)"\"Internet Explorer"\"iexplore.exe"
            }
            else if (encodingMethod == EncodingMethods.EncryptedHTML)
            {
                result = string.Empty;

                foreach (char c in text)
                {
                    unicode = c;
                    result += "&#" + unicode + ";";
                }

                return result;

                // <a href="mailto:webmaster@google.de">Webmaster von Google</a><br/>
                // <a href="&#109;&#097;&#105;&#108;&#116;&#111;&#058;&#119;&#101;&#098;&#109;&#097;&#115;&#116;&#101;&#114;&#064;&#103;&#111;&#111;&#103;&#108;&#101;&#046;&#100;&#101;">Webmaster von Google</a>
            }
            else if (encodingMethod == EncodingMethods.HTMLUrl)
            {
                result = string.Empty;

                foreach (char c in text)
                {
                    if (c == ' ')
                    {
                        result += '+';
                    }
                    else if (c.ToString().RegEx_IsMatch("^[a-zA-Z0-9]$"))
                    {
                        result += c;
                    }
                    else
                    {
                        unicode = c;

                        if (unicode <= 255)
                        {
                            result += Uri.HexEscape(c);
                        }
                        else
                        {
                            result += Uri.EscapeDataString(c.ToString());
                        }
                    }
                }

                return result;

                // return Uri.EscapeDataString(Text);
                // return HttpUtility.UrlEncode(Text);
            }
            else
            {
                throw new NotImplementedException("M_String._Encode: das gesuchte Element \"" + encodingMethod.ToString() + "\" wurde nicht implementiert.");
            }
        }

        /// <summary>
        ///     dekodiert einen Text mit einer bestimmten Kodierungsmethode
        /// </summary>
        /// <param name="encodedText">Text der dekodiert werden soll</param>
        /// <param name="encodingMethod">zu verwendende Kodierungsmethode</param>
        /// <returns>decoded text</returns>
        public static string Decode(this string encodedText, EncodingMethods encodingMethod)
        {
            if (encodedText == null)
            {
                return null;
            }

            if (encodingMethod == EncodingMethods.Base64)
            {
                UTF8Encoding uTF8 = new UTF8Encoding();
                return uTF8.GetString(System.Convert.FromBase64String(encodedText));
            }
            else if (encodingMethod == EncodingMethods.XML)
            {
                return encodedText.Replace("&lt;", "<", "&gt;", ">", "&apos;", "'", "&quot;", "\"", "&amp;", "&");
            }
            else if (encodingMethod == EncodingMethods.SQL_Server_Path)
            {
                return "\"" + encodedText.Replace("\"", string.Empty) + "\""; // "C:\Program Files (x86)\Internet Explorer\iexplore.exe"
            }
            else
            {
                throw new NotImplementedException("M_String._Decode: das gesuchte Element \"" + encodingMethod.ToString() + "\" wurde nicht implementiert.");
            }
        }

        /// <summary>
        ///     verschlüsselt einen Text mit einem Passwort
        /// </summary>
        /// <param name="text">Text der verschlüsselt werden soll</param>
        /// <param name="password">Passwort mit dem verschlüsselt werden soll</param>
        /// <returns>encrypted text</returns>
        public static string Encrypt(this string text, string password)
        {
            if (text == null || password == null)
            {
                return null;
            }

            UTF8Encoding uTF8 = new UTF8Encoding();

            if (mD5HashProvider == null)
            {
                mD5HashProvider = new MD5CryptoServiceProvider();
            }

            if (cryptoProvider == null)
            {
                cryptoProvider = new TripleDESCryptoServiceProvider();
            }

            cryptoProvider.Key = mD5HashProvider.ComputeHash(uTF8.GetBytes(password)); // Passwort hashen
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] textData = uTF8.GetBytes(text);
            byte[] encryptedTextData = cryptoProvider.CreateEncryptor().TransformFinalBlock(textData, 0, textData.Length); // Text verschlüsseln

            return Convert.ToBase64String(encryptedTextData);
        }

        /// <summary>
        ///     entschlüsselt einen Text mit einem Passwort
        /// </summary>
        /// <param name="encryptedText">Text der entschlüsselt werden soll</param>
        /// <param name="password">Passwort mit dem entschlüsselt werden soll</param>
        /// <returns>decrypted text</returns>
        public static string Decrypt(this string encryptedText, string password)
        {
            if (encryptedText == null || password == null)
            {
                return null;
            }

            UTF8Encoding uTF8 = new UTF8Encoding();

            if (mD5HashProvider == null)
            {
                mD5HashProvider = new MD5CryptoServiceProvider();
            }

            if (cryptoProvider == null)
            {
                cryptoProvider = new TripleDESCryptoServiceProvider();
            }

            cryptoProvider.Key = mD5HashProvider.ComputeHash(uTF8.GetBytes(password)); // Passwort hashen
            cryptoProvider.Mode = CipherMode.ECB;
            cryptoProvider.Padding = PaddingMode.PKCS7;

            byte[] textData;
            try
            {
                byte[] encryptedTextData = Convert.FromBase64String(encryptedText);
                textData = cryptoProvider.CreateDecryptor().TransformFinalBlock(encryptedTextData, 0, encryptedTextData.Length); // Text entschlüsseln
            }
            catch
            {
                return null;
            }

            return uTF8.GetString(textData);
        }

        #endregion
    }
}
