﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Dieser Code wurde von einem Tool generiert.
//     Laufzeitversion:4.0.30319.42000
//
//     Änderungen an dieser Datei können falsches Verhalten verursachen und gehen verloren, wenn
//     der Code erneut generiert wird.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CollectorzToKodi.Properties {
    
    
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "15.6.0.0")]
    internal sealed partial class SettingsMovieCollector : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsMovieCollector defaultInstance = ((SettingsMovieCollector)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsMovieCollector())));
        
        public static SettingsMovieCollector Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(Special)")]
        public string Specials {
            get {
                return ((string)(this["Specials"]));
            }
            set {
                this["Specials"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(F<MPAARating>)")]
        public string MPAARating {
            get {
                return ((string)(this["MPAARating"]));
            }
            set {
                this["MPAARating"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("(S<Season>)")]
        public string Season {
            get {
                return ((string)(this["Season"]));
            }
            set {
                this["Season"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("XBMC Movie")]
        public string Movies {
            get {
                return ((string)(this["Movies"]));
            }
            set {
                this["Movies"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("XBMC Serie")]
        public string Series {
            get {
                return ((string)(this["Series"]));
            }
            set {
                this["Series"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("de")]
        public string Language {
            get {
                return ((string)(this["Language"]));
            }
            set {
                this["Language"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Q:\\Collectorz.com\\CollectorzToKodi\\Filme.xml")]
        public string LocalPathToXMLExport {
            get {
                return ((string)(this["LocalPathToXMLExport"]));
            }
            set {
                this["LocalPathToXMLExport"] = value;
            }
        }
    }
}