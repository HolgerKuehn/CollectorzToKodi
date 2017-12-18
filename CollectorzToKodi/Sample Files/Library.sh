#!/bin/bash

# Link auf SHIRYOUSOOCHI

cd /share/XBMC/

if [ ! -d SHIRYOUSOOCHI ];
then
	mkdir SHIRYOUSOOCHI 
fi;

cd SHIRYOUSOOCHI

if [ ! -d Programme ];
then
	mkdir Programme 
	mount.cifs //192.168.2.252/Programme Programme -o user=<user>,pass=<pass> vers=3.0
fi;

cd /share/XBMC/

#/bin/cp "/share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/NFOJOUSETSUSOOCHIWin.sh" /share/XBMC/NFOUnx.sh
/bin/tr -d '\r﻿' < /share/XBMC/SHIRYOUSOOCHI/Programme/Collectorz.com/nfo-Konverter/nfoConverter/nfoConverter/bin/NFOYOBISOOCHIWin.sh > /share/XBMC/NFOUnx.sh

/bin/chmod 755 /share/XBMC/NFOUnx.sh

/bin/bash /share/XBMC/NFOUnx.sh

umount //192.168.2.252/Programme
cd /share/XBMC/
rmdir ./SHIRYOUSOOCHI/Programme 
rmdir ./SHIRYOUSOOCHI
