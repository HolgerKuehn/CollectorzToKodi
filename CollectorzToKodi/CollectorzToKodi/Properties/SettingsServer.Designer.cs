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
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.VisualStudio.Editors.SettingsDesigner.SettingsSingleFileGenerator", "14.0.0.0")]
    internal sealed partial class SettingsServer : global::System.Configuration.ApplicationSettingsBase {
        
        private static SettingsServer defaultInstance = ((SettingsServer)(global::System.Configuration.ApplicationSettingsBase.Synchronized(new SettingsServer())));
        
        public static SettingsServer Default {
            get {
                return defaultInstance;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("4")]
        public int NumberOfServer {
            get {
                return ((int)(this["NumberOfServer"]));
            }
            set {
                this["NumberOfServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("DOCUMENTS,YOBISOOCHI,SHIRYOUSOOCHI,JOUSETSUSOOCHI")]
        public string ListOfServer {
            get {
                return ((string)(this["ListOfServer"]));
            }
            set {
                this["ListOfServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("P,U,V,X")]
        public string DriveMappingOfServer {
            get {
                return ((string)(this["DriveMappingOfServer"]));
            }
            set {
                this["DriveMappingOfServer"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("UNIX")]
        public string MappingType {
            get {
                return ((string)(this["MappingType"]));
            }
            set {
                this["MappingType"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/share/XBMC/SHIRYOUSOOCHI/Programme,/share/Video,/share/Video,/share/Video")]
        public string LocalPathOfServerForMediaStorage {
            get {
                return ((string)(this["LocalPathOfServerForMediaStorage"]));
            }
            set {
                this["LocalPathOfServerForMediaStorage"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Filme")]
        public string MovieDirectory {
            get {
                return ((string)(this["MovieDirectory"]));
            }
            set {
                this["MovieDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("Serien")]
        public string SeriesDirectory {
            get {
                return ((string)(this["SeriesDirectory"]));
            }
            set {
                this["SeriesDirectory"] = value;
            }
        }
        
        [global::System.Configuration.UserScopedSettingAttribute()]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.Configuration.DefaultSettingValueAttribute("/share/XBMC/SHIRYOUSOOCHI/Programme,/share/XBMC,/share/XBMC,/share/XBMC")]
        public string LocalPathOfServerForMediaPublication {
            get {
                return ((string)(this["LocalPathOfServerForMediaPublication"]));
            }
            set {
                this["LocalPathOfServerForMediaPublication"] = value;
            }
        }
    }
}