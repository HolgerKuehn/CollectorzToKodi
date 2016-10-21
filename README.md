# CollectorzToKodi
This project has been created to get my movie collection from Collextorz' Movie Collector into Kodi.

Both are individual trademarks and more information about them can be found under the following links:

[http://www.collectorz.com/](http://www.collectorz.com/ "http://www.collectorz.com/")  
[https://kodi.tv/](https://kodi.tv/ "https://kodi.tv/")

I've used Movie Collector May 2012 to organize my movie collection and started to use Kodi Helix in June 2014. So I had a lot of content allready catalogued and did not want to do it again.

So I started to create this project to get the following to work:

* Transfer as much content from Movie Collector to Kodi by 
    1. Export existing movie collection to xml, created directly by Movie Collector
    2. Create folder and file structure supported by Kodi and provide additional content via nfo-files used by Kodis "local information only" scaper

As I am using a QNAP-NAS to store all movie data this project creates a bash shell script that can be run directly on the NAS to create the folder and file structure needed.

* In Basic configuration the following parts a exported for movies and series
	1. Plot
	2. Cast and Crew
	3. Publishing dates
	4. Cover and Background images
	
* While using these with different skins on Kodi the following extentions had been added
    1. Cover and Background per season
    2. Extra Cover and Background used in skins (e.g Transparency!)
    3. SRT-Subtitles

* Parts that could be extended, if someone is interested to use them on other platforms
    1. support for network shares stored on Windows machines (not unix)
