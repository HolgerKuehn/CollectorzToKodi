#!/bin/bash

# Link auf SHIRYOUSOOCHI

cd /share/Kodi/

if [ ! -d SHIRYOUSOOCHI ];
then
	mkdir SHIRYOUSOOCHI 
fi;

cd SHIRYOUSOOCHI

if [ ! -d Programme ];
then
	mkdir Programme 
	mount.cifs //192.168.2.252/Programme Programme -o user=<user>,pass=<pass>
fi;

cd /share/Kodi/

#/bin/cp "/share/Kodi/SHIRYOUSOOCHI/Programme/Collectorz.com/CollectorzToKodi/NFOJOUSETSUSOOCHIWin.sh" /share/Kodi/NFOUnx.sh
/bin/tr -d '\r﻿' < /share/Kodi/SHIRYOUSOOCHI/Programme/Collectorz.com/CollectorzToKodi/NFOYOBISOOCHIWin.sh > /share/Kodi/NFOUnx.sh

/bin/chmod 755 /share/Kodi/NFOUnx.sh

/bin/bash /share/Kodi/NFOUnx.sh

umount //192.168.2.252/Programme
cd /share/Kodi/
rmdir ./SHIRYOUSOOCHI/Programme 
rmdir ./SHIRYOUSOOCHI
