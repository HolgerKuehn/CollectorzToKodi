﻿// <copyright file="Configuration.cs" company="Holger Kühn">
// Copyright (c) 2014 - 2018 Holger Kühn. All rights reserved.
// </copyright>

namespace CollectorzToKodi
{
    using System.Collections.Generic;
    using System.Globalization;

    /// <summary>
    /// Stores all Parameters configured by settings-files.<br/>
    /// <br/>
    /// used settings-files<br/>
    /// settingsMovieCollector.settings - settings related to data storage in MovieCollector<br/>
    /// settingsKodi.settings - settings related to Kodi<br/>
    /// settingsServer.settings - settings related to your local server structure<br/>
    /// </summary>
    public class Configuration
    {
        #region Enums Part 1

        /// <summary>
        /// list of MimeTypes from file extension
        /// <remarks>List from github://cymen/ApacheMimeTypesToDotNet</remarks>
        /// </summary>
        private readonly Dictionary<string, string> extensionToMimeTypes = new Dictionary<string, string>
        {
            { "123", "application/vnd.lotus-1-2-3" },
            { "3dml", "text/vnd.in3d.3dml" },
            { "3g2", "video/3gpp2" },
            { "3gp", "video/3gpp" },
            { "7z", "application/x-7z-compressed" },
            { "aab", "application/x-authorware-bin" },
            { "aac", "audio/x-aac" },
            { "aam", "application/x-authorware-map" },
            { "aas", "application/x-authorware-seg" },
            { "abw", "application/x-abiword" },
            { "ac", "application/pkix-attr-cert" },
            { "acc", "application/vnd.americandynamics.acc" },
            { "ace", "application/x-ace-compressed" },
            { "acu", "application/vnd.acucobol" },
            { "acutc", "application/vnd.acucorp" },
            { "adp", "audio/adpcm" },
            { "aep", "application/vnd.audiograph" },
            { "afm", "application/x-font-type1" },
            { "afp", "application/vnd.ibm.modcap" },
            { "ahead", "application/vnd.ahead.space" },
            { "ai", "application/postscript" },
            { "aif", "audio/x-aiff" },
            { "aifc", "audio/x-aiff" },
            { "aiff", "audio/x-aiff" },
            { "air", "application/vnd.adobe.air-application-installer-package+zip" },
            { "ait", "application/vnd.dvb.ait" },
            { "ami", "application/vnd.amiga.ami" },
            { "apk", "application/vnd.android.package-archive" },
            { "application", "application/x-ms-application" },
            { "apr", "application/vnd.lotus-approach" },
            { "asc", "application/pgp-signature" },
            { "asf", "video/x-ms-asf" },
            { "asm", "text/x-asm" },
            { "aso", "application/vnd.accpac.simply.aso" },
            { "asx", "video/x-ms-asf" },
            { "atc", "application/vnd.acucorp" },
            { "atom", "application/atom+xml" },
            { "atomcat", "application/atomcat+xml" },
            { "atomsvc", "application/atomsvc+xml" },
            { "atx", "application/vnd.antix.game-component" },
            { "au", "audio/basic" },
            { "avi", "video/x-msvideo" },
            { "aw", "application/applixware" },
            { "azf", "application/vnd.airzip.filesecure.azf" },
            { "azs", "application/vnd.airzip.filesecure.azs" },
            { "azw", "application/vnd.amazon.ebook" },
            { "bat", "application/x-msdownload" },
            { "bcpio", "application/x-bcpio" },
            { "bdf", "application/x-font-bdf" },
            { "bdm", "application/vnd.syncml.dm+wbxml" },
            { "bed", "application/vnd.realvnc.bed" },
            { "bh2", "application/vnd.fujitsu.oasysprs" },
            { "bin", "application/octet-stream" },
            { "bmi", "application/vnd.bmi" },
            { "bmp", "image/bmp" },
            { "book", "application/vnd.framemaker" },
            { "box", "application/vnd.previewsystems.box" },
            { "boz", "application/x-bzip2" },
            { "bpk", "application/octet-stream" },
            { "btif", "image/prs.btif" },
            { "bz", "application/x-bzip" },
            { "bz2", "application/x-bzip2" },
            { "c", "text/x-c" },
            { "c11amc", "application/vnd.cluetrust.cartomobile-config" },
            { "c11amz", "application/vnd.cluetrust.cartomobile-config-pkg" },
            { "c4d", "application/vnd.clonk.c4group" },
            { "c4f", "application/vnd.clonk.c4group" },
            { "c4g", "application/vnd.clonk.c4group" },
            { "c4p", "application/vnd.clonk.c4group" },
            { "c4u", "application/vnd.clonk.c4group" },
            { "cab", "application/vnd.ms-cab-compressed" },
            { "car", "application/vnd.curl.car" },
            { "cat", "application/vnd.ms-pki.seccat" },
            { "cc", "text/x-c" },
            { "cct", "application/x-director" },
            { "ccxml", "application/ccxml+xml" },
            { "cdbcmsg", "application/vnd.contact.cmsg" },
            { "cdf", "application/x-netcdf" },
            { "cdkey", "application/vnd.mediastation.cdkey" },
            { "cdmia", "application/cdmi-capability" },
            { "cdmic", "application/cdmi-container" },
            { "cdmid", "application/cdmi-domain" },
            { "cdmio", "application/cdmi-object" },
            { "cdmiq", "application/cdmi-queue" },
            { "cdx", "chemical/x-cdx" },
            { "cdxml", "application/vnd.chemdraw+xml" },
            { "cdy", "application/vnd.cinderella" },
            { "cer", "application/pkix-cert" },
            { "cgm", "image/cgm" },
            { "chat", "application/x-chat" },
            { "chm", "application/vnd.ms-htmlhelp" },
            { "chrt", "application/vnd.kde.kchart" },
            { "cif", "chemical/x-cif" },
            { "cii", "application/vnd.anser-web-certificate-issue-initiation" },
            { "cil", "application/vnd.ms-artgalry" },
            { "cla", "application/vnd.claymore" },
            { "class", "application/java-vm" },
            { "clkk", "application/vnd.crick.clicker.keyboard" },
            { "clkp", "application/vnd.crick.clicker.palette" },
            { "clkt", "application/vnd.crick.clicker.template" },
            { "clkw", "application/vnd.crick.clicker.wordbank" },
            { "clkx", "application/vnd.crick.clicker" },
            { "clp", "application/x-msclip" },
            { "cmc", "application/vnd.cosmocaller" },
            { "cmdf", "chemical/x-cmdf" },
            { "cml", "chemical/x-cml" },
            { "cmp", "application/vnd.yellowriver-custom-menu" },
            { "cmx", "image/x-cmx" },
            { "cod", "application/vnd.rim.cod" },
            { "com", "application/x-msdownload" },
            { "conf", "text/plain" },
            { "cpio", "application/x-cpio" },
            { "cpp", "text/x-c" },
            { "cpt", "application/mac-compactpro" },
            { "crd", "application/x-mscardfile" },
            { "crl", "application/pkix-crl" },
            { "crt", "application/x-x509-ca-cert" },
            { "cryptonote", "application/vnd.rig.cryptonote" },
            { "csh", "application/x-csh" },
            { "csml", "chemical/x-csml" },
            { "csp", "application/vnd.commonspace" },
            { "css", "text/css" },
            { "cst", "application/x-director" },
            { "csv", "text/csv" },
            { "cu", "application/cu-seeme" },
            { "curl", "text/vnd.curl" },
            { "cww", "application/prs.cww" },
            { "cxt", "application/x-director" },
            { "cxx", "text/x-c" },
            { "dae", "model/vnd.collada+xml" },
            { "daf", "application/vnd.mobius.daf" },
            { "dataless", "application/vnd.fdsn.seed" },
            { "davmount", "application/davmount+xml" },
            { "dcr", "application/x-director" },
            { "dcurl", "text/vnd.curl.dcurl" },
            { "dd2", "application/vnd.oma.dd2+xml" },
            { "ddd", "application/vnd.fujixerox.ddd" },
            { "deb", "application/x-debian-package" },
            { "def", "text/plain" },
            { "deploy", "application/octet-stream" },
            { "der", "application/x-x509-ca-cert" },
            { "dfac", "application/vnd.dreamfactory" },
            { "dic", "text/x-c" },
            { "dir", "application/x-director" },
            { "dis", "application/vnd.mobius.dis" },
            { "dist", "application/octet-stream" },
            { "distz", "application/octet-stream" },
            { "djv", "image/vnd.djvu" },
            { "djvu", "image/vnd.djvu" },
            { "dll", "application/x-msdownload" },
            { "dmg", "application/octet-stream" },
            { "dms", "application/octet-stream" },
            { "dna", "application/vnd.dna" },
            { "doc", "application/msword" },
            { "docm", "application/vnd.ms-word.document.macroenabled.12" },
            { "docx", "application/vnd.openxmlformats-officedocument.wordprocessingml.document" },
            { "dot", "application/msword" },
            { "dotm", "application/vnd.ms-word.template.macroenabled.12" },
            { "dotx", "application/vnd.openxmlformats-officedocument.wordprocessingml.template" },
            { "dp", "application/vnd.osgi.dp" },
            { "dpg", "application/vnd.dpgraph" },
            { "dra", "audio/vnd.dra" },
            { "dsc", "text/prs.lines.tag" },
            { "dssc", "application/dssc+der" },
            { "dtb", "application/x-dtbook+xml" },
            { "dtd", "application/xml-dtd" },
            { "dts", "audio/vnd.dts" },
            { "dtshd", "audio/vnd.dts.hd" },
            { "dump", "application/octet-stream" },
            { "dvi", "application/x-dvi" },
            { "dwf", "model/vnd.dwf" },
            { "dwg", "image/vnd.dwg" },
            { "dxf", "image/vnd.dxf" },
            { "dxp", "application/vnd.spotfire.dxp" },
            { "dxr", "application/x-director" },
            { "ecelp4800", "audio/vnd.nuera.ecelp4800" },
            { "ecelp7470", "audio/vnd.nuera.ecelp7470" },
            { "ecelp9600", "audio/vnd.nuera.ecelp9600" },
            { "ecma", "application/ecmascript" },
            { "edm", "application/vnd.novadigm.edm" },
            { "edx", "application/vnd.novadigm.edx" },
            { "efif", "application/vnd.picsel" },
            { "ei6", "application/vnd.pg.osasli" },
            { "elc", "application/octet-stream" },
            { "eml", "message/rfc822" },
            { "emma", "application/emma+xml" },
            { "eol", "audio/vnd.digital-winds" },
            { "eot", "application/vnd.ms-fontobject" },
            { "eps", "application/postscript" },
            { "epub", "application/epub+zip" },
            { "es3", "application/vnd.eszigno3+xml" },
            { "esf", "application/vnd.epson.esf" },
            { "et3", "application/vnd.eszigno3+xml" },
            { "etx", "text/x-setext" },
            { "exe", "application/x-msdownload" },
            { "exi", "application/exi" },
            { "ext", "application/vnd.novadigm.ext" },
            { "ez", "application/andrew-inset" },
            { "ez2", "application/vnd.ezpix-album" },
            { "ez3", "application/vnd.ezpix-package" },
            { "f", "text/x-fortran" },
            { "f4v", "video/x-f4v" },
            { "f77", "text/x-fortran" },
            { "f90", "text/x-fortran" },
            { "fbs", "image/vnd.fastbidsheet" },
            { "fcs", "application/vnd.isac.fcs" },
            { "fdf", "application/vnd.fdf" },
            { "fe_launch", "application/vnd.denovo.fcselayout-link" },
            { "fg5", "application/vnd.fujitsu.oasysgp" },
            { "fgd", "application/x-director" },
            { "fh", "image/x-freehand" },
            { "fh4", "image/x-freehand" },
            { "fh5", "image/x-freehand" },
            { "fh7", "image/x-freehand" },
            { "fhc", "image/x-freehand" },
            { "fig", "application/x-xfig" },
            { "fli", "video/x-fli" },
            { "flo", "application/vnd.micrografx.flo" },
            { "flv", "video/x-flv" },
            { "flw", "application/vnd.kde.kivio" },
            { "flx", "text/vnd.fmi.flexstor" },
            { "fly", "text/vnd.fly" },
            { "fm", "application/vnd.framemaker" },
            { "fnc", "application/vnd.frogans.fnc" },
            { "for", "text/x-fortran" },
            { "fpx", "image/vnd.fpx" },
            { "frame", "application/vnd.framemaker" },
            { "fsc", "application/vnd.fsc.weblaunch" },
            { "fst", "image/vnd.fst" },
            { "ftc", "application/vnd.fluxtime.clip" },
            { "fti", "application/vnd.anser-web-funds-transfer-initiation" },
            { "fvt", "video/vnd.fvt" },
            { "fxp", "application/vnd.adobe.fxp" },
            { "fxpl", "application/vnd.adobe.fxp" },
            { "fzs", "application/vnd.fuzzysheet" },
            { "g2w", "application/vnd.geoplan" },
            { "g3", "image/g3fax" },
            { "g3w", "application/vnd.geospace" },
            { "gac", "application/vnd.groove-account" },
            { "gdl", "model/vnd.gdl" },
            { "geo", "application/vnd.dynageo" },
            { "gex", "application/vnd.geometry-explorer" },
            { "ggb", "application/vnd.geogebra.file" },
            { "ggt", "application/vnd.geogebra.tool" },
            { "ghf", "application/vnd.groove-help" },
            { "gif", "image/gif" },
            { "gim", "application/vnd.groove-identity-message" },
            { "gmx", "application/vnd.gmx" },
            { "gnumeric", "application/x-gnumeric" },
            { "gph", "application/vnd.flographit" },
            { "gqf", "application/vnd.grafeq" },
            { "gqs", "application/vnd.grafeq" },
            { "gram", "application/srgs" },
            { "gre", "application/vnd.geometry-explorer" },
            { "grv", "application/vnd.groove-injector" },
            { "grxml", "application/srgs+xml" },
            { "gsf", "application/x-font-ghostscript" },
            { "gtar", "application/x-gtar" },
            { "gtm", "application/vnd.groove-tool-message" },
            { "gtw", "model/vnd.gtw" },
            { "gv", "text/vnd.graphviz" },
            { "gxt", "application/vnd.geonext" },
            { "h", "text/x-c" },
            { "h261", "video/h261" },
            { "h263", "video/h263" },
            { "h264", "video/h264" },
            { "hal", "application/vnd.hal+xml" },
            { "hbci", "application/vnd.hbci" },
            { "hdf", "application/x-hdf" },
            { "hh", "text/x-c" },
            { "hlp", "application/winhlp" },
            { "hpgl", "application/vnd.hp-hpgl" },
            { "hpid", "application/vnd.hp-hpid" },
            { "hps", "application/vnd.hp-hps" },
            { "hqx", "application/mac-binhex40" },
            { "htke", "application/vnd.kenameaapp" },
            { "htm", "text/html" },
            { "html", "text/html" },
            { "hvd", "application/vnd.yamaha.hv-dic" },
            { "hvp", "application/vnd.yamaha.hv-voice" },
            { "hvs", "application/vnd.yamaha.hv-script" },
            { "i2g", "application/vnd.intergeo" },
            { "icc", "application/vnd.iccprofile" },
            { "ice", "x-conference/x-cooltalk" },
            { "icm", "application/vnd.iccprofile" },
            { "ico", "image/x-icon" },
            { "ics", "text/calendar" },
            { "ief", "image/ief" },
            { "ifb", "text/calendar" },
            { "ifm", "application/vnd.shana.informed.formdata" },
            { "iges", "model/iges" },
            { "igl", "application/vnd.igloader" },
            { "igm", "application/vnd.insors.igm" },
            { "igs", "model/iges" },
            { "igx", "application/vnd.micrografx.igx" },
            { "iif", "application/vnd.shana.informed.interchange" },
            { "imp", "application/vnd.accpac.simply.imp" },
            { "ims", "application/vnd.ms-ims" },
            { "in", "text/plain" },
            { "ipfix", "application/ipfix" },
            { "ipk", "application/vnd.shana.informed.package" },
            { "irm", "application/vnd.ibm.rights-management" },
            { "irp", "application/vnd.irepository.package+xml" },
            { "iso", "application/octet-stream" },
            { "itp", "application/vnd.shana.informed.formtemplate" },
            { "ivp", "application/vnd.immervision-ivp" },
            { "ivu", "application/vnd.immervision-ivu" },
            { "jad", "text/vnd.sun.j2me.app-descriptor" },
            { "jam", "application/vnd.jam" },
            { "jar", "application/java-archive" },
            { "java", "text/x-java-source" },
            { "jisp", "application/vnd.jisp" },
            { "jlt", "application/vnd.hp-jlyt" },
            { "jnlp", "application/x-java-jnlp-file" },
            { "joda", "application/vnd.joost.joda-archive" },
            { "jpe", "image/jpeg" },
            { "jpeg", "image/jpeg" },
            { "jpg", "image/jpeg" },
            { "jpgm", "video/jpm" },
            { "jpgv", "video/jpeg" },
            { "jpm", "video/jpm" },
            { "js", "application/javascript" },
            { "json", "application/json" },
            { "kar", "audio/midi" },
            { "karbon", "application/vnd.kde.karbon" },
            { "kfo", "application/vnd.kde.kformula" },
            { "kia", "application/vnd.kidspiration" },
            { "kml", "application/vnd.google-earth.kml+xml" },
            { "kmz", "application/vnd.google-earth.kmz" },
            { "kne", "application/vnd.kinar" },
            { "knp", "application/vnd.kinar" },
            { "kon", "application/vnd.kde.kontour" },
            { "kpr", "application/vnd.kde.kpresenter" },
            { "kpt", "application/vnd.kde.kpresenter" },
            { "ksp", "application/vnd.kde.kspread" },
            { "ktr", "application/vnd.kahootz" },
            { "ktx", "image/ktx" },
            { "ktz", "application/vnd.kahootz" },
            { "kwd", "application/vnd.kde.kword" },
            { "kwt", "application/vnd.kde.kword" },
            { "lasxml", "application/vnd.las.las+xml" },
            { "latex", "application/x-latex" },
            { "lbd", "application/vnd.llamagraphics.life-balance.desktop" },
            { "lbe", "application/vnd.llamagraphics.life-balance.exchange+xml" },
            { "les", "application/vnd.hhe.lesson-player" },
            { "lha", "application/octet-stream" },
            { "link66", "application/vnd.route66.link66+xml" },
            { "list", "text/plain" },
            { "list3820", "application/vnd.ibm.modcap" },
            { "listafp", "application/vnd.ibm.modcap" },
            { "log", "text/plain" },
            { "lostxml", "application/lost+xml" },
            { "lrf", "application/octet-stream" },
            { "lrm", "application/vnd.ms-lrm" },
            { "ltf", "application/vnd.frogans.ltf" },
            { "lvp", "audio/vnd.lucent.voice" },
            { "lwp", "application/vnd.lotus-wordpro" },
            { "lzh", "application/octet-stream" },
            { "m13", "application/x-msmediaview" },
            { "m14", "application/x-msmediaview" },
            { "m1v", "video/mpeg" },
            { "m21", "application/mp21" },
            { "m2a", "audio/mpeg" },
            { "m2v", "video/mpeg" },
            { "m3a", "audio/mpeg" },
            { "m3u", "audio/x-mpegurl" },
            { "m3u8", "application/vnd.apple.mpegurl" },
            { "m4u", "video/vnd.mpegurl" },
            { "m4v", "video/x-m4v" },
            { "ma", "application/mathematica" },
            { "mads", "application/mads+xml" },
            { "mag", "application/vnd.ecowin.chart" },
            { "maker", "application/vnd.framemaker" },
            { "man", "text/troff" },
            { "mathml", "application/mathml+xml" },
            { "mb", "application/mathematica" },
            { "mbk", "application/vnd.mobius.mbk" },
            { "mbox", "application/mbox" },
            { "mc1", "application/vnd.medcalcdata" },
            { "mcd", "application/vnd.mcd" },
            { "mcurl", "text/vnd.curl.mcurl" },
            { "mdb", "application/x-msaccess" },
            { "mdi", "image/vnd.ms-modi" },
            { "me", "text/troff" },
            { "mesh", "model/mesh" },
            { "meta4", "application/metalink4+xml" },
            { "mets", "application/mets+xml" },
            { "mfm", "application/vnd.mfmp" },
            { "mgp", "application/vnd.osgeo.mapguide.package" },
            { "mgz", "application/vnd.proteus.magazine" },
            { "mid", "audio/midi" },
            { "midi", "audio/midi" },
            { "mif", "application/vnd.mif" },
            { "mime", "message/rfc822" },
            { "mj2", "video/mj2" },
            { "mjp2", "video/mj2" },
            { "mlp", "application/vnd.dolby.mlp" },
            { "mmd", "application/vnd.chipnuts.karaoke-mmd" },
            { "mmf", "application/vnd.smaf" },
            { "mmr", "image/vnd.fujixerox.edmics-mmr" },
            { "mny", "application/x-msmoney" },
            { "mobi", "application/x-mobipocket-ebook" },
            { "mods", "application/mods+xml" },
            { "mov", "video/quicktime" },
            { "movie", "video/x-sgi-movie" },
            { "mp2", "audio/mpeg" },
            { "mp21", "application/mp21" },
            { "mp2a", "audio/mpeg" },
            { "mp3", "audio/mpeg" },
            { "mp4", "video/mp4" },
            { "mp4a", "audio/mp4" },
            { "mp4s", "application/mp4" },
            { "mp4v", "video/mp4" },
            { "mpc", "application/vnd.mophun.certificate" },
            { "mpe", "video/mpeg" },
            { "mpeg", "video/mpeg" },
            { "mpg", "video/mpeg" },
            { "mpg4", "video/mp4" },
            { "mpga", "audio/mpeg" },
            { "mpkg", "application/vnd.apple.installer+xml" },
            { "mpm", "application/vnd.blueice.multipass" },
            { "mpn", "application/vnd.mophun.application" },
            { "mpp", "application/vnd.ms-project" },
            { "mpt", "application/vnd.ms-project" },
            { "mpy", "application/vnd.ibm.minipay" },
            { "mqy", "application/vnd.mobius.mqy" },
            { "mrc", "application/marc" },
            { "mrcx", "application/marcxml+xml" },
            { "ms", "text/troff" },
            { "mscml", "application/mediaservercontrol+xml" },
            { "mseed", "application/vnd.fdsn.mseed" },
            { "mseq", "application/vnd.mseq" },
            { "msf", "application/vnd.epson.msf" },
            { "msh", "model/mesh" },
            { "msi", "application/x-msdownload" },
            { "msl", "application/vnd.mobius.msl" },
            { "msty", "application/vnd.muvee.style" },
            { "mts", "model/vnd.mts" },
            { "mus", "application/vnd.musician" },
            { "musicxml", "application/vnd.recordare.musicxml+xml" },
            { "mvb", "application/x-msmediaview" },
            { "mwf", "application/vnd.mfer" },
            { "mxf", "application/mxf" },
            { "mxl", "application/vnd.recordare.musicxml" },
            { "mxml", "application/xv+xml" },
            { "mxs", "application/vnd.triscape.mxs" },
            { "mxu", "video/vnd.mpegurl" },
            { "n3", "text/n3" },
            { "nb", "application/mathematica" },
            { "nbp", "application/vnd.wolfram.player" },
            { "nc", "application/x-netcdf" },
            { "ncx", "application/x-dtbncx+xml" },
            { "n-gage", "application/vnd.nokia.n-gage.symbian.install" },
            { "ngdat", "application/vnd.nokia.n-gage.data" },
            { "nlu", "application/vnd.neurolanguage.nlu" },
            { "nml", "application/vnd.enliven" },
            { "nnd", "application/vnd.noblenet-directory" },
            { "nns", "application/vnd.noblenet-sealer" },
            { "nnw", "application/vnd.noblenet-web" },
            { "npx", "image/vnd.net-fpx" },
            { "nsf", "application/vnd.lotus-notes" },
            { "oa2", "application/vnd.fujitsu.oasys2" },
            { "oa3", "application/vnd.fujitsu.oasys3" },
            { "oas", "application/vnd.fujitsu.oasys" },
            { "obd", "application/x-msbinder" },
            { "oda", "application/oda" },
            { "odb", "application/vnd.oasis.opendocument.database" },
            { "odc", "application/vnd.oasis.opendocument.chart" },
            { "odf", "application/vnd.oasis.opendocument.formula" },
            { "odft", "application/vnd.oasis.opendocument.formula-template" },
            { "odg", "application/vnd.oasis.opendocument.graphics" },
            { "odi", "application/vnd.oasis.opendocument.image" },
            { "odm", "application/vnd.oasis.opendocument.text-master" },
            { "odp", "application/vnd.oasis.opendocument.presentation" },
            { "ods", "application/vnd.oasis.opendocument.spreadsheet" },
            { "odt", "application/vnd.oasis.opendocument.text" },
            { "oga", "audio/ogg" },
            { "ogg", "audio/ogg" },
            { "ogv", "video/ogg" },
            { "ogx", "application/ogg" },
            { "onepkg", "application/onenote" },
            { "onetmp", "application/onenote" },
            { "onetoc", "application/onenote" },
            { "onetoc2", "application/onenote" },
            { "opf", "application/oebps-package+xml" },
            { "oprc", "application/vnd.palm" },
            { "org", "application/vnd.lotus-organizer" },
            { "osf", "application/vnd.yamaha.openscoreformat" },
            { "osfpvg", "application/vnd.yamaha.openscoreformat.osfpvg+xml" },
            { "otc", "application/vnd.oasis.opendocument.chart-template" },
            { "otf", "application/x-font-otf" },
            { "otg", "application/vnd.oasis.opendocument.graphics-template" },
            { "oth", "application/vnd.oasis.opendocument.text-web" },
            { "oti", "application/vnd.oasis.opendocument.image-template" },
            { "otp", "application/vnd.oasis.opendocument.presentation-template" },
            { "ots", "application/vnd.oasis.opendocument.spreadsheet-template" },
            { "ott", "application/vnd.oasis.opendocument.text-template" },
            { "oxt", "application/vnd.openofficeorg.extension" },
            { "p", "text/x-pascal" },
            { "p10", "application/pkcs10" },
            { "p12", "application/x-pkcs12" },
            { "p7b", "application/x-pkcs7-certificates" },
            { "p7c", "application/pkcs7-mime" },
            { "p7m", "application/pkcs7-mime" },
            { "p7r", "application/x-pkcs7-certreqresp" },
            { "p7s", "application/pkcs7-signature" },
            { "p8", "application/pkcs8" },
            { "pas", "text/x-pascal" },
            { "paw", "application/vnd.pawaafile" },
            { "pbd", "application/vnd.powerbuilder6" },
            { "pbm", "image/x-portable-bitmap" },
            { "pcf", "application/x-font-pcf" },
            { "pcl", "application/vnd.hp-pcl" },
            { "pclxl", "application/vnd.hp-pclxl" },
            { "pct", "image/x-pict" },
            { "pcurl", "application/vnd.curl.pcurl" },
            { "pcx", "image/x-pcx" },
            { "pdb", "application/vnd.palm" },
            { "pdf", "application/pdf" },
            { "pfa", "application/x-font-type1" },
            { "pfb", "application/x-font-type1" },
            { "pfm", "application/x-font-type1" },
            { "pfr", "application/font-tdpfr" },
            { "pfx", "application/x-pkcs12" },
            { "pgm", "image/x-portable-graymap" },
            { "pgn", "application/x-chess-pgn" },
            { "pgp", "application/pgp-encrypted" },
            { "pic", "image/x-pict" },
            { "pkg", "application/octet-stream" },
            { "pki", "application/pkixcmp" },
            { "pkipath", "application/pkix-pkipath" },
            { "plb", "application/vnd.3gpp.pic-bw-large" },
            { "plc", "application/vnd.mobius.plc" },
            { "plf", "application/vnd.pocketlearn" },
            { "pls", "application/pls+xml" },
            { "pml", "application/vnd.ctc-posml" },
            { "png", "image/png" },
            { "pnm", "image/x-portable-anymap" },
            { "portpkg", "application/vnd.macports.portpkg" },
            { "pot", "application/vnd.ms-powerpoint" },
            { "potm", "application/vnd.ms-powerpoint.template.macroenabled.12" },
            { "potx", "application/vnd.openxmlformats-officedocument.presentationml.template" },
            { "ppam", "application/vnd.ms-powerpoint.addin.macroenabled.12" },
            { "ppd", "application/vnd.cups-ppd" },
            { "ppm", "image/x-portable-pixmap" },
            { "pps", "application/vnd.ms-powerpoint" },
            { "ppsm", "application/vnd.ms-powerpoint.slideshow.macroenabled.12" },
            { "ppsx", "application/vnd.openxmlformats-officedocument.presentationml.slideshow" },
            { "ppt", "application/vnd.ms-powerpoint" },
            { "pptm", "application/vnd.ms-powerpoint.presentation.macroenabled.12" },
            { "pptx", "application/vnd.openxmlformats-officedocument.presentationml.presentation" },
            { "pqa", "application/vnd.palm" },
            { "prc", "application/x-mobipocket-ebook" },
            { "pre", "application/vnd.lotus-freelance" },
            { "prf", "application/pics-rules" },
            { "ps", "application/postscript" },
            { "psb", "application/vnd.3gpp.pic-bw-small" },
            { "psd", "image/vnd.adobe.photoshop" },
            { "psf", "application/x-font-linux-psf" },
            { "pskcxml", "application/pskc+xml" },
            { "ptid", "application/vnd.pvi.ptid1" },
            { "pub", "application/x-mspublisher" },
            { "pvb", "application/vnd.3gpp.pic-bw-var" },
            { "pwn", "application/vnd.3m.post-it-notes" },
            { "pya", "audio/vnd.ms-playready.media.pya" },
            { "pyv", "video/vnd.ms-playready.media.pyv" },
            { "qam", "application/vnd.epson.quickanime" },
            { "qbo", "application/vnd.intu.qbo" },
            { "qfx", "application/vnd.intu.qfx" },
            { "qps", "application/vnd.publishare-delta-tree" },
            { "qt", "video/quicktime" },
            { "qwd", "application/vnd.quark.quarkxpress" },
            { "qwt", "application/vnd.quark.quarkxpress" },
            { "qxb", "application/vnd.quark.quarkxpress" },
            { "qxd", "application/vnd.quark.quarkxpress" },
            { "qxl", "application/vnd.quark.quarkxpress" },
            { "qxt", "application/vnd.quark.quarkxpress" },
            { "ra", "audio/x-pn-realaudio" },
            { "ram", "audio/x-pn-realaudio" },
            { "rar", "application/x-rar-compressed" },
            { "ras", "image/x-cmu-raster" },
            { "rcprofile", "application/vnd.ipunplugged.rcprofile" },
            { "rdf", "application/rdf+xml" },
            { "rdz", "application/vnd.data-vision.rdz" },
            { "rep", "application/vnd.businessobjects" },
            { "res", "application/x-dtbresource+xml" },
            { "rgb", "image/x-rgb" },
            { "rif", "application/reginfo+xml" },
            { "rip", "audio/vnd.rip" },
            { "rl", "application/resource-lists+xml" },
            { "rlc", "image/vnd.fujixerox.edmics-rlc" },
            { "rld", "application/resource-lists-diff+xml" },
            { "rm", "application/vnd.rn-realmedia" },
            { "rmi", "audio/midi" },
            { "rmp", "audio/x-pn-realaudio-plugin" },
            { "rms", "application/vnd.jcp.javame.midlet-rms" },
            { "rnc", "application/relax-ng-compact-syntax" },
            { "roff", "text/troff" },
            { "rp9", "application/vnd.cloanto.rp9" },
            { "rpss", "application/vnd.nokia.radio-presets" },
            { "rpst", "application/vnd.nokia.radio-preset" },
            { "rq", "application/sparql-query" },
            { "rs", "application/rls-services+xml" },
            { "rsd", "application/rsd+xml" },
            { "rss", "application/rss+xml" },
            { "rtf", "application/rtf" },
            { "rtx", "text/richtext" },
            { "s", "text/x-asm" },
            { "saf", "application/vnd.yamaha.smaf-audio" },
            { "sbml", "application/sbml+xml" },
            { "sc", "application/vnd.ibm.secure-container" },
            { "scd", "application/x-msschedule" },
            { "scm", "application/vnd.lotus-screencam" },
            { "scq", "application/scvp-cv-request" },
            { "scs", "application/scvp-cv-response" },
            { "scurl", "text/vnd.curl.scurl" },
            { "sda", "application/vnd.stardivision.draw" },
            { "sdc", "application/vnd.stardivision.calc" },
            { "sdd", "application/vnd.stardivision.impress" },
            { "sdkd", "application/vnd.solent.sdkm+xml" },
            { "sdkm", "application/vnd.solent.sdkm+xml" },
            { "sdp", "application/sdp" },
            { "sdw", "application/vnd.stardivision.writer" },
            { "see", "application/vnd.seemail" },
            { "seed", "application/vnd.fdsn.seed" },
            { "sema", "application/vnd.sema" },
            { "semd", "application/vnd.semd" },
            { "semf", "application/vnd.semf" },
            { "ser", "application/java-serialized-object" },
            { "setpay", "application/set-payment-initiation" },
            { "setreg", "application/set-registration-initiation" },
            { "sfd-hdstx", "application/vnd.hydrostatix.sof-data" },
            { "sfs", "application/vnd.spotfire.sfs" },
            { "sgl", "application/vnd.stardivision.writer-global" },
            { "sgm", "text/sgml" },
            { "sgml", "text/sgml" },
            { "sh", "application/x-sh" },
            { "shar", "application/x-shar" },
            { "shf", "application/shf+xml" },
            { "sig", "application/pgp-signature" },
            { "silo", "model/mesh" },
            { "sis", "application/vnd.symbian.install" },
            { "sisx", "application/vnd.symbian.install" },
            { "sit", "application/x-stuffit" },
            { "sitx", "application/x-stuffitx" },
            { "skd", "application/vnd.koan" },
            { "skm", "application/vnd.koan" },
            { "skp", "application/vnd.koan" },
            { "skt", "application/vnd.koan" },
            { "sldm", "application/vnd.ms-powerpoint.slide.macroenabled.12" },
            { "sldx", "application/vnd.openxmlformats-officedocument.presentationml.slide" },
            { "slt", "application/vnd.epson.salt" },
            { "sm", "application/vnd.stepmania.stepchart" },
            { "smf", "application/vnd.stardivision.math" },
            { "smi", "application/smil+xml" },
            { "smil", "application/smil+xml" },
            { "snd", "audio/basic" },
            { "snf", "application/x-font-snf" },
            { "so", "application/octet-stream" },
            { "spc", "application/x-pkcs7-certificates" },
            { "spf", "application/vnd.yamaha.smaf-phrase" },
            { "spl", "application/x-futuresplash" },
            { "spot", "text/vnd.in3d.spot" },
            { "spp", "application/scvp-vp-response" },
            { "spq", "application/scvp-vp-request" },
            { "spx", "audio/ogg" },
            { "src", "application/x-wais-source" },
            { "srt", "text/srt-subtitle" },
            { "sru", "application/sru+xml" },
            { "srx", "application/sparql-results+xml" },
            { "sse", "application/vnd.kodak-descriptor" },
            { "ssf", "application/vnd.epson.ssf" },
            { "ssml", "application/ssml+xml" },
            { "st", "application/vnd.sailingtracker.track" },
            { "stc", "application/vnd.sun.xml.calc.template" },
            { "std", "application/vnd.sun.xml.draw.template" },
            { "stf", "application/vnd.wt.stf" },
            { "sti", "application/vnd.sun.xml.impress.template" },
            { "stk", "application/hyperstudio" },
            { "stl", "application/vnd.ms-pki.stl" },
            { "str", "application/vnd.pg.format" },
            { "stw", "application/vnd.sun.xml.writer.template" },
            { "sub", "image/vnd.dvb.subtitle" },
            { "sus", "application/vnd.sus-calendar" },
            { "susp", "application/vnd.sus-calendar" },
            { "sv4cpio", "application/x-sv4cpio" },
            { "sv4crc", "application/x-sv4crc" },
            { "svc", "application/vnd.dvb.service" },
            { "svd", "application/vnd.svd" },
            { "svg", "image/svg+xml" },
            { "svgz", "image/svg+xml" },
            { "swa", "application/x-director" },
            { "swf", "application/x-shockwave-flash" },
            { "swi", "application/vnd.aristanetworks.swi" },
            { "sxc", "application/vnd.sun.xml.calc" },
            { "sxd", "application/vnd.sun.xml.draw" },
            { "sxg", "application/vnd.sun.xml.writer.global" },
            { "sxi", "application/vnd.sun.xml.impress" },
            { "sxm", "application/vnd.sun.xml.math" },
            { "sxw", "application/vnd.sun.xml.writer" },
            { "t", "text/troff" },
            { "tao", "application/vnd.tao.intent-module-archive" },
            { "tar", "application/x-tar" },
            { "tcap", "application/vnd.3gpp2.tcap" },
            { "tcl", "application/x-tcl" },
            { "teacher", "application/vnd.smart.teacher" },
            { "tei", "application/tei+xml" },
            { "teicorpus", "application/tei+xml" },
            { "tex", "application/x-tex" },
            { "texi", "application/x-texinfo" },
            { "texinfo", "application/x-texinfo" },
            { "text", "text/plain" },
            { "tfi", "application/thraud+xml" },
            { "tfm", "application/x-tex-tfm" },
            { "thmx", "application/vnd.ms-officetheme" },
            { "tif", "image/tiff" },
            { "tiff", "image/tiff" },
            { "tmo", "application/vnd.tmobile-livetv" },
            { "torrent", "application/x-bittorrent" },
            { "tpl", "application/vnd.groove-tool-template" },
            { "tpt", "application/vnd.trid.tpt" },
            { "tr", "text/troff" },
            { "tra", "application/vnd.trueapp" },
            { "trm", "application/x-msterminal" },
            { "tsd", "application/timestamped-data" },
            { "tsv", "text/tab-separated-values" },
            { "ttc", "application/x-font-ttf" },
            { "ttf", "application/x-font-ttf" },
            { "ttl", "text/turtle" },
            { "twd", "application/vnd.simtech-mindmapper" },
            { "twds", "application/vnd.simtech-mindmapper" },
            { "txd", "application/vnd.genomatix.tuxedo" },
            { "txf", "application/vnd.mobius.txf" },
            { "txt", "text/plain" },
            { "u32", "application/x-authorware-bin" },
            { "udeb", "application/x-debian-package" },
            { "ufd", "application/vnd.ufdl" },
            { "ufdl", "application/vnd.ufdl" },
            { "umj", "application/vnd.umajin" },
            { "unityweb", "application/vnd.unity" },
            { "uoml", "application/vnd.uoml+xml" },
            { "uri", "text/uri-list" },
            { "uris", "text/uri-list" },
            { "urls", "text/uri-list" },
            { "ustar", "application/x-ustar" },
            { "utz", "application/vnd.uiq.theme" },
            { "uu", "text/x-uuencode" },
            { "uva", "audio/vnd.dece.audio" },
            { "uvd", "application/vnd.dece.data" },
            { "uvf", "application/vnd.dece.data" },
            { "uvg", "image/vnd.dece.graphic" },
            { "uvh", "video/vnd.dece.hd" },
            { "uvi", "image/vnd.dece.graphic" },
            { "uvm", "video/vnd.dece.mobile" },
            { "uvp", "video/vnd.dece.pd" },
            { "uvs", "video/vnd.dece.sd" },
            { "uvt", "application/vnd.dece.ttml+xml" },
            { "uvu", "video/vnd.uvvu.mp4" },
            { "uvv", "video/vnd.dece.video" },
            { "uvva", "audio/vnd.dece.audio" },
            { "uvvd", "application/vnd.dece.data" },
            { "uvvf", "application/vnd.dece.data" },
            { "uvvg", "image/vnd.dece.graphic" },
            { "uvvh", "video/vnd.dece.hd" },
            { "uvvi", "image/vnd.dece.graphic" },
            { "uvvm", "video/vnd.dece.mobile" },
            { "uvvp", "video/vnd.dece.pd" },
            { "uvvs", "video/vnd.dece.sd" },
            { "uvvt", "application/vnd.dece.ttml+xml" },
            { "uvvu", "video/vnd.uvvu.mp4" },
            { "uvvv", "video/vnd.dece.video" },
            { "uvvx", "application/vnd.dece.unspecified" },
            { "uvx", "application/vnd.dece.unspecified" },
            { "vcd", "application/x-cdlink" },
            { "vcf", "text/x-vcard" },
            { "vcg", "application/vnd.groove-vcard" },
            { "vcs", "text/x-vcalendar" },
            { "vcx", "application/vnd.vcx" },
            { "vis", "application/vnd.visionary" },
            { "viv", "video/vnd.vivo" },
            { "vor", "application/vnd.stardivision.writer" },
            { "vox", "application/x-authorware-bin" },
            { "vrml", "model/vrml" },
            { "vsd", "application/vnd.visio" },
            { "vsf", "application/vnd.vsf" },
            { "vss", "application/vnd.visio" },
            { "vst", "application/vnd.visio" },
            { "vsw", "application/vnd.visio" },
            { "vtu", "model/vnd.vtu" },
            { "vxml", "application/voicexml+xml" },
            { "w3d", "application/x-director" },
            { "wad", "application/x-doom" },
            { "wav", "audio/x-wav" },
            { "wax", "audio/x-ms-wax" },
            { "wbmp", "image/vnd.wap.wbmp" },
            { "wbs", "application/vnd.criticaltools.wbs+xml" },
            { "wbxml", "application/vnd.wap.wbxml" },
            { "wcm", "application/vnd.ms-works" },
            { "wdb", "application/vnd.ms-works" },
            { "weba", "audio/webm" },
            { "webm", "video/webm" },
            { "webp", "image/webp" },
            { "wg", "application/vnd.pmi.widget" },
            { "wgt", "application/widget" },
            { "wks", "application/vnd.ms-works" },
            { "wm", "video/x-ms-wm" },
            { "wma", "audio/x-ms-wma" },
            { "wmd", "application/x-ms-wmd" },
            { "wmf", "application/x-msmetafile" },
            { "wml", "text/vnd.wap.wml" },
            { "wmlc", "application/vnd.wap.wmlc" },
            { "wmls", "text/vnd.wap.wmlscript" },
            { "wmlsc", "application/vnd.wap.wmlscriptc" },
            { "wmv", "video/x-ms-wmv" },
            { "wmx", "video/x-ms-wmx" },
            { "wmz", "application/x-ms-wmz" },
            { "woff", "application/x-font-woff" },
            { "wpd", "application/vnd.wordperfect" },
            { "wpl", "application/vnd.ms-wpl" },
            { "wps", "application/vnd.ms-works" },
            { "wqd", "application/vnd.wqd" },
            { "wri", "application/x-mswrite" },
            { "wrl", "model/vrml" },
            { "wsdl", "application/wsdl+xml" },
            { "wspolicy", "application/wspolicy+xml" },
            { "wtb", "application/vnd.webturbo" },
            { "wvx", "video/x-ms-wvx" },
            { "x32", "application/x-authorware-bin" },
            { "x3d", "application/vnd.hzn-3d-crossword" },
            { "xap", "application/x-silverlight-app" },
            { "xar", "application/vnd.xara" },
            { "xbap", "application/x-ms-xbap" },
            { "xbd", "application/vnd.fujixerox.docuworks.binder" },
            { "xbm", "image/x-xbitmap" },
            { "xdf", "application/xcap-diff+xml" },
            { "xdm", "application/vnd.syncml.dm+xml" },
            { "xdp", "application/vnd.adobe.xdp+xml" },
            { "xdssc", "application/dssc+xml" },
            { "xdw", "application/vnd.fujixerox.docuworks" },
            { "xenc", "application/xenc+xml" },
            { "xer", "application/patch-ops-error+xml" },
            { "xfdf", "application/vnd.adobe.xfdf" },
            { "xfdl", "application/vnd.xfdl" },
            { "xht", "application/xhtml+xml" },
            { "xhtml", "application/xhtml+xml" },
            { "xhvml", "application/xv+xml" },
            { "xif", "image/vnd.xiff" },
            { "xla", "application/vnd.ms-excel" },
            { "xlam", "application/vnd.ms-excel.addin.macroenabled.12" },
            { "xlc", "application/vnd.ms-excel" },
            { "xlm", "application/vnd.ms-excel" },
            { "xls", "application/vnd.ms-excel" },
            { "xlsb", "application/vnd.ms-excel.sheet.binary.macroenabled.12" },
            { "xlsm", "application/vnd.ms-excel.sheet.macroenabled.12" },
            { "xlsx", "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" },
            { "xlt", "application/vnd.ms-excel" },
            { "xltm", "application/vnd.ms-excel.template.macroenabled.12" },
            { "xltx", "application/vnd.openxmlformats-officedocument.spreadsheetml.template" },
            { "xlw", "application/vnd.ms-excel" },
            { "xml", "application/xml" },
            { "xo", "application/vnd.olpc-sugar" },
            { "xop", "application/xop+xml" },
            { "xpi", "application/x-xpinstall" },
            { "xpm", "image/x-xpixmap" },
            { "xpr", "application/vnd.is-xpr" },
            { "xps", "application/vnd.ms-xpsdocument" },
            { "xpw", "application/vnd.intercon.formnet" },
            { "xpx", "application/vnd.intercon.formnet" },
            { "xsl", "application/xml" },
            { "xslt", "application/xslt+xml" },
            { "xsm", "application/vnd.syncml+xml" },
            { "xspf", "application/xspf+xml" },
            { "xul", "application/vnd.mozilla.xul+xml" },
            { "xvm", "application/xv+xml" },
            { "xvml", "application/xv+xml" },
            { "xwd", "image/x-xwindowdump" },
            { "xyz", "chemical/x-xyz" },
            { "yang", "application/yang" },
            { "yin", "application/yin+xml" },
            { "zaz", "application/vnd.zzazz.deck+xml" },
            { "zip", "application/zip" },
            { "zir", "application/vnd.zul" },
            { "zirz", "application/vnd.zul" },
            { "zmm", "application/vnd.handheld-entertainment+xml" },
        };

        private readonly Dictionary<string, string> languageIsoCodeToDescription = new Dictionary<string, string>
        {
            { "de", "deutsch" },
            { "en", "englisch" },
            { "en", "japanisch" }
        };

        #endregion
        #region Attributes

        #region Kodi

            /// <summary>
            /// contains name of configured Kodi skin<br/>
            /// this is used to MediaGroup some attributes to skin-specific values
            /// </summary>
            /// <remarks>supported skins: Transparency!, Confluence, Estuary</remarks>
            /// <returns>name of configured Kodi skin</returns>
        private readonly string kodiSkin;

        /// <summary>
        /// sets Export for Movies to Series<br/>
        /// this way all Specials can be accessed separately
        /// </summary>
        private readonly bool kodiExportMovieAsSeries;

        #endregion
        #region MovieCollector

        /// <summary>
        /// string used to mark specials as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        private readonly string movieCollectorSpecials;

        /// <summary>
        /// string used to mark movies in series as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        private readonly string movieCollectorMovies;

        /// <summary>
        /// string to override MPAA-Rating in Disks or Episode
        /// </summary>
        private readonly string movieCollectorMPAARating;

        /// <summary>
        /// string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        private readonly string movieCollectorSeason;

        /// <summary>
        /// string used to define the available languages in this media<br/>
        /// </summary>
        /// <remarks>
        /// string is been parsed for ISO-2-country-codes<br/>
        /// "Language" needs to be a collection of valid ISO-2-country-codes<br/>
        /// the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI<br/>
        /// </remarks>
        /// <returns>string used to mark multi-language-media</returns>
        private readonly string movieCollectorLanguage;

        /// <summary>
        /// local path to CollectorzToKodi-XML-Export including Filename and extension as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export including Filename and extension</returns>
        private readonly string movieCollectorUrlForXMLExport;

        /// <summary>
        /// local path to CollectorzToKodi-XML-Export excluding Filename and extension as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export excluding Filename and extension</returns>
        private readonly string movieCollectorUrlForXMLExportPath;

        /// <summary>
        /// Filename and extension excluding path to CollectorzToKodi-XML-Export as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>Filename and extension excluding path to CollectorzToKodi-XML-Export</returns>
        private readonly string movieCollectorUrlForXMLExportFile;

        /// <summary>
        /// Filename and extension excluding path to CollectorzToKodi-XML-Export as specified in "SettingsMovieCollector.settings"
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>Path to CollectorzToKodi-XML-Export</returns>
        private readonly string movieCollectorUrlForXMLExportPathLocalFilesystem;

        #endregion
        #region Server

        /// <summary>
        /// number of server used to store media<br/>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        /// </summary>
        private readonly int serverNumberOfServers;

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private readonly List<string> serverListOfServers;

        /// <summary>
        /// list of associated drive-letters for server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        /// </summary>
        private readonly List<string> serverDriveMappingOfServers;

        /// <summary>
        /// list of local paths used on the associated server names to store media<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        /// </summary>
        private readonly List<string> serverUrlForMediaStorageLocalFilesystem;

        /// <summary>
        /// list of local paths used on the associated server names for publication<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names for publication</returns>
        /// </summary>
        private readonly List<string> serverUrlForMediaPublicationLocalFilesystem;

        /// <summary>
        /// type of mapping used during deployment<br/>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        /// </summary>
        private readonly string serverMappingType;

        /// <summary>
        /// directory used to publish movies to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        /// </summary>
        private readonly string serverMovieDirectory;

        /// <summary>
        /// directory used to publish series to Kodi<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        /// </summary>
        private readonly string serverSeriesDirectory;

        #endregion
        #region Dictionaries

        /// <summary>
        /// list of server names to store media<br/>
        /// <remarks>Dictionary used to combine associated drive-letters and server names</remarks>
        /// <returns>list of server names</returns>
        /// </summary>
        private readonly List<Dictionary<string, string>> serverListsOfServers;

        #endregion
        #region Configuration

        /// <summary>
        /// batch file used to execute on either UNIX or Windows
        /// </summary>
        private readonly List<BatchFile> listOfBatchFiles;

        #endregion
        #endregion
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Configuration"/> class.
        /// initializes configuration with settings-files
        /// </summary>
        public Configuration()
        {
            #region Kodi

            this.kodiSkin = Properties.SettingsKodi.Default.Skin;
            this.kodiExportMovieAsSeries = Properties.SettingsKodi.Default.ExportMovieAsSeries;

            #endregion
            #region MovieCollector

            this.movieCollectorSpecials = Properties.SettingsMovieCollector.Default.Specials;
            this.movieCollectorMovies = Properties.SettingsMovieCollector.Default.Movies;
            this.movieCollectorMPAARating = Properties.SettingsMovieCollector.Default.MPAARating;
            this.movieCollectorSeason = Properties.SettingsMovieCollector.Default.Season;
            this.movieCollectorLanguage = Properties.SettingsMovieCollector.Default.Language;

            this.movieCollectorUrlForXMLExport = Properties.SettingsMovieCollector.Default.UrlForXMLExport;
            this.movieCollectorUrlForXMLExportFile = this.movieCollectorUrlForXMLExport.RightOfLast("\\");
            this.movieCollectorUrlForXMLExportPath = this.movieCollectorUrlForXMLExport.LeftOfLast("\\") + "\\";
            this.movieCollectorUrlForXMLExportPathLocalFilesystem = this.movieCollectorUrlForXMLExportPath;

            #endregion
            #region Server

            this.serverNumberOfServers = Properties.SettingsServer.Default.NumberOfServer;
            this.serverListOfServers = Properties.SettingsServer.Default.ListOfServer.Split(",");
            this.serverDriveMappingOfServers = Properties.SettingsServer.Default.DriveMappingOfServer.Split(",");
            this.serverUrlForMediaStorageLocalFilesystem = Properties.SettingsServer.Default.UrlForMediaStorageLocalFilesystem.Split(",");
            this.serverUrlForMediaPublicationLocalFilesystem = Properties.SettingsServer.Default.UrlForMediaPublicationLocalFilesystem.Split(",");
            this.serverMappingType = Properties.SettingsServer.Default.MappingType;
            this.serverMovieDirectory = Properties.SettingsServer.Default.MovieDirectory + (this.serverMappingType == "UNIX" ? "/" : "\\");
            this.serverSeriesDirectory = Properties.SettingsServer.Default.SeriesDirectory + (this.serverMappingType == "UNIX" ? "/" : "\\");

            #endregion
            #region Dictionaries / Configuration

            this.serverListsOfServers = new List<Dictionary<string, string>>();
            this.listOfBatchFiles = new List<BatchFile>();

            int i = 0;
            for (i = 0; i < 6 /* number of possible conversion - defined by enum ListOfServerTypes */; i++)
            {
                this.serverListsOfServers.Add(new Dictionary<string, string>());
            }

            for (i = 0; i < this.ServerNumberOfServers; i++)
            {
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToName].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerListOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToDriveLetter].Add(i.ToString(CultureInfo.InvariantCulture), this.ServerDriveMappingOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToLocalPathForMediaStorage].Add(i.ToString(CultureInfo.InvariantCulture), this.UrlForMediaStorage[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NumberToLocalPathForMediaPublication].Add(i.ToString(CultureInfo.InvariantCulture), this.UrlForMediaPublicationLocalFilesystem[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.DriveLetterToName].Add(this.ServerDriveMappingOfServers[i], this.ServerListOfServers[i]);
                this.serverListsOfServers[(int)ListOfServerTypes.NameToDriveLetter].Add(this.ServerListOfServers[i], this.ServerDriveMappingOfServers[i]);

                if (this.serverMappingType.Equals("UNIX"))
                {
                    this.listOfBatchFiles.Add(new ShFile(this));
                }

                /*
                else if (this.serverMappingType.Equals("Windows"))
                {
                    listOfBatchFiles.Add(new CmdFile(this));
                }
                */

                this.listOfBatchFiles[i].Server = i;

                // determine movieCollectorUrlForXMLExportPathLocalFilesystem
                string driveLetter = this.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToDriveLetter][i.ToString()];
                string localPath = this.ServerListsOfServers[(int)Configuration.ListOfServerTypes.NumberToLocalPathForMediaStorage][i.ToString()];

                // determine used servers from assigned driveLetters
                if (this.movieCollectorUrlForXMLExportPathLocalFilesystem.StartsWith(driveLetter.Trim() + ":", true, System.Globalization.CultureInfo.CurrentCulture))
                {
                    this.movieCollectorUrlForXMLExportPathLocalFilesystem = this.movieCollectorUrlForXMLExportPathLocalFilesystem.Replace(driveLetter.Trim() + ":", localPath);
                }
            }

            #endregion
        }

        #endregion
        #region Enums Part 2

        /// <summary>
        /// List of video codecs used for export to Kodi.
        /// </summary>
        public enum VideoCodec
        {
            /// <summary>
            /// specifies video file as TV recording
            /// </summary>
            TV,

            /// <summary>
            /// specifies video as BluRay content
            /// </summary>
            BluRay,

            /// <summary>
            /// video file with H264 codec
            /// </summary>
            H264,

            /// <summary>
            /// video file with H265 codec
            /// </summary>
            H265
        }

        /// <summary>
        /// List of server types handled by this program.
        /// </summary>
        public enum ListOfServerTypes
        {
            /// <summary>
            /// dictionary from number to name
            /// </summary>
            NumberToName,

            /// <summary>
            /// dictionary from number to associated drive letter
            /// </summary>
            NumberToDriveLetter,

            /// <summary>
            /// dictionary from number to local path for media storage in file system
            /// </summary>
            NumberToLocalPathForMediaStorage,

            /// <summary>
            /// dictionary from number to local path for media publication in file system
            /// </summary>
            NumberToLocalPathForMediaPublication,

            /// <summary>
            /// dictionary from associated drive letter to name
            /// </summary>
            DriveLetterToName,

            /// <summary>
            /// dictionary from name to associated drive letter
            /// </summary>
            NameToDriveLetter
        }

        /// <summary>
        /// List of AspectRatios handled by this program.
        /// </summary>
        public enum VideoAspectRatio
        {
            /// <summary>
            /// Fullscreen
            /// </summary>
            AspectRatio43,

            /// <summary>
            /// Widescreen
            /// </summary>
            AspectRatio169,

            /// <summary>
            /// Theatrical Widescreen
            /// </summary>
            AspectRatio219
        }

        /// <summary>
        /// List of Video definitions handled by this program.
        /// </summary>
        public enum VideoDefinition
        {
            /// <summary>
            /// SD content
            /// </summary>
            SD,

            /// <summary>
            /// (Full-) HD content
            /// </summary>
            HD
        }

        /// <summary>
        /// List of image types used for publishing in Kodi
        /// </summary>
        public enum ImageType
        {
            /// <summary>
            /// unknown image type
            /// </summary>
            Unknown,

            /// <summary>
            /// image with front cover
            /// </summary>
            CoverFront,

            /// <summary>
            /// image with back cover
            /// </summary>
            CoverBack,

            /// <summary>
            /// image of poster used for fanart
            /// </summary>
            Poster,

            /// <summary>
            /// image of backdrop used for fanart
            /// </summary>
            Backdrop,

            /// <summary>
            /// image of front cover used for seasons
            /// </summary>
            SeasonCover,

            /// <summary>
            /// image of poster used for seasons
            /// </summary>
            SeasonPoster,

            /// <summary>
            /// image of back drop used for seasons
            /// </summary>
            SeasonBackdrop,

            /// <summary>
            /// image of episode cover
            /// </summary>
            EpisodeCover,

            /// <summary>
            /// additional back drops used for fanart
            /// </summary>
            ExtraBackdrop,

            /// <summary>
            /// additional covers used for fanart
            /// </summary>
            ExtraCover
        }

        /// <summary>
        /// Schemes for SRT-files
        /// </summary>
        public enum SrtSubTitleLineType
        {
            /// <summary>
            /// first line of SRT specifications
            /// number of entry
            /// </summary>
            EntryNumber,

            /// <summary>
            /// second line of SRT specifications
            /// times, when the entry should be displayed
            /// </summary>
            Times,

            /// <summary>
            /// third and following lines of SRT specifications
            /// text displayed
            /// </summary>
            SubTitles,

            /// <summary>
            /// last line of SRT specifications
            /// empty line ending the entry
            /// </summary>
            EmptyLine
        }

        /// <summary>
        /// Gets number of ImageTypes; needs to be updated, when new ImageTypes are indroduced
        /// </summary>
        public int NumberOfImageTypes
        {
            get { return 11; }
        }

        #endregion
        #region Properties
        #region Kodi

        /// <summary>
        /// Gets name of configured Kodi skin<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <remarks>supported skins: Transparency!, Confluence, Estuary</remarks>
        /// <returns>name of configured Kodi skin</returns>
        public string KodiSkin
        {
            get { return this.kodiSkin; }
        }

        /// <summary>
        /// Gets a value indicating whether movies are exported as series or not<br/>
        /// this way all Specials can be accessed separately
        /// </summary>
        public bool KodiExportMovieAsSeries
        {
            get { return this.kodiExportMovieAsSeries; }
        }

        #endregion
        #region MovieCollector

        /// <summary>
        /// Gets string used to mark specials<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as specials</remarks>
        /// <returns>string used to mark specials</returns>
        public string MovieCollectorSpecials
        {
            get { return this.movieCollectorSpecials; }
        }

        /// <summary>
        /// Gets string used to mark movies in series<br/>
        /// </summary>
        /// <remarks>string is replaced in disks or titles and defines all elements as movies</remarks>
        /// <returns>string used to mark movies in series</returns>
        public string MovieCollectorMovies
        {
            get { return this.movieCollectorMovies; }
        }

        /// <summary>
        /// Gets string used to mark different MPAA-Rating from MovieData
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific MPAA-Rating<br/>
        /// "MPAARating" needs to be a valid MPAA-Rating for the intended system
        /// </remarks>
        /// <returns>string used to mark different MPAA-Rating</returns>
        public string MovieCollectorMPAARating
        {
            get { return this.movieCollectorMPAARating; }
        }

        /// <summary>
        /// Gets string used to define the Season Episode should be shown in<br/>
        /// </summary>
        /// <remarks>
        /// string is replaced in disks or titles and defines all elements with specific Seasons<br/>
        /// "MPAARating" needs to be a valid number<br/>
        /// </remarks>
        /// <returns>string used to mark Seasons</returns>
        public string MovieCollectorSeason
        {
            get { return this.movieCollectorSeason; }
        }

        /// <summary>
        /// Gets string used to define the available languages in this media
        /// </summary>
        /// <remarks>
        /// string is been parsed for ISO-2-country-codes<br/>
        /// "Language" needs to be a collection of valid ISO-2-country-codes<br/>
        /// the codes must be present in the IMDB-ID, movie- or episode-title, to be displayed in KODI
        /// </remarks>
        /// <returns>string used to mark multi-language-media</returns>
        public string MovieCollectorLanguage
        {
            get { return this.movieCollectorLanguage; }
        }

        /// <summary>
        /// Gets local path to CollectorzToKodi-XML-Export including Filename and extension<br/>
        /// </summary>
        /// <remarks>filename needs to end with ".xml"</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export including Filename and extension</returns>
        public string MovieCollectorUrlForXMLExport
        {
            get { return this.movieCollectorUrlForXMLExport; }
        }

        /// <summary>
        /// Gets local path to CollectorzToKodi-XML-Export excluding Filename and extension
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>local path to CollectorzToKodi-XML-Export excluding Filename and extension</returns>
        public string MovieCollectorUrlForXMLExportPath
        {
            get { return this.movieCollectorUrlForXMLExportPath; }
        }

        /// <summary>
        /// Gets Filename and extension excluding path to CollectorzToKodi-XML-Export<br/>
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>Filename and extension excluding path to CollectorzToKodi-XML-Export</returns>
        public string MovieCollectorUrlForXMLExportFile
        {
            get { return this.movieCollectorUrlForXMLExportFile; }
        }

        /// <summary>
        /// Gets path to CollectorzToKodi-XML-Export<br/>
        /// </summary>
        /// <remarks>is generated from movieCollectorUrlForXMLExport</remarks>
        /// <returns>Path to CollectorzToKodi-XML-Export on local filesystem</returns>
        public string MovieCollectorUrlForXMLExportPathLocalFilesystem
        {
            get { return this.movieCollectorUrlForXMLExportPathLocalFilesystem; }
        }

        #endregion
        #region Server

        /// <summary>
        /// Gets number of server used to store media<br/>
        /// </summary>
        /// <remarks>needs to be a valid number equal or above 1</remarks>
        /// <returns>number of server</returns>
        public int ServerNumberOfServers
        {
            get { return this.serverNumberOfServers; }
        }

        /// <summary>
        /// Gets type of mapping used during deployment<br/>
        /// </summary>
        /// <remarks>supported values: UNIX (Windows will be added later)</remarks>
        /// <returns>type of mapping used during deployment</returns>
        public string ServerMappingType
        {
            get { return this.serverMappingType; }
        }

        /// <summary>
        /// Gets directory used to publish movies to Kodi<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish movies to Kodi</returns>
        public string ServerMovieDirectory
        {
            get { return this.serverMovieDirectory; }
        }

        /// <summary>
        /// Gets directory used to publish series to Kodi
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>directory used to publish series to Kodi</returns>
        public string ServerSeriesDirectory
        {
            get { return this.serverSeriesDirectory; }
        }

        #endregion
        #region Configuration

        /// <summary>
        /// Gets batch file used to execute commands on either UNIX or Windows<br/>
        /// this is used to MediaGroup some attributes to skin-specific values
        /// </summary>
        /// <returns>current batch file</returns>
        public List<BatchFile> ListOfBatchFiles
        {
            get { return this.listOfBatchFiles; }
        }

        #endregion
        #region Dictionaries

        /// <summary>
        /// Gets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        /// [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1202:ElementsMustBeOrderedByAccess", Justification = "Reviewed.")]
        public List<Dictionary<string, string>> ServerListsOfServers
        {
            get { return this.serverListsOfServers; }
        }

        /// <summary>
        /// Gets list of server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of server names</returns>
        private List<string> ServerListOfServers
        {
            get { return this.serverListOfServers; }
        }

        /// <summary>
        /// Gets list of associated drive-letters for server names to store media
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of associated drive-letters used for server names</returns>
        private List<string> ServerDriveMappingOfServers
        {
            get { return this.serverDriveMappingOfServers; }
        }

        /// <summary>
        /// Gets list of local paths used on the associated server names to store media<br/>
        /// </summary>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names to store media</returns>
        private List<string> UrlForMediaStorage
        {
            get { return this.serverUrlForMediaStorageLocalFilesystem; }
        }

        /// <summary>
        /// Gets list of local paths used on the associated server names for publication<br/>
        /// <remarks>values separated by colon</remarks>
        /// <returns>list of local paths used on the associated server names for publication</returns>
        /// </summary>
        private List<string> UrlForMediaPublicationLocalFilesystem
        {
            get { return this.serverUrlForMediaPublicationLocalFilesystem; }
        }

        #endregion
        #endregion
        #region Methods

        /// <summary>
        /// converts 2 letter ISO-Code to description
        /// </summary>
        /// <param name="isoCode">2 letter ISO-Code</param>
        /// <returns>language name defined by ISO-Code</returns>
        public string CovertLanguageIsoCodeToDescription(string isoCode)
        {
            string languageDescription = "deutsch";

            if (this.languageIsoCodeToDescription.ContainsKey(isoCode))
            {
                languageDescription = this.languageIsoCodeToDescription[isoCode];
            }

            return languageDescription;
        }

        /// <summary>
        /// converts file-extension to MimeType
        /// </summary>
        /// <param name="extension">file-extension</param>
        /// <returns>supposed Mime-Type of file</returns>
        public string CovertExtensionToMimeType(string extension)
        {
            string mimeType = "unknown";

            if (this.extensionToMimeTypes.ContainsKey(extension))
            {
                mimeType = this.extensionToMimeTypes[extension];
            }

            return mimeType;
        }

        #endregion
    }
}
