
namespace Collectorz
{
    public static class CConstants
    {
        public enum ServerList
        {
            SHIRYOUSOOCHI,
            YOBISOOCHI,
            EIZOUSOOCHI,
            JOUSETSUSOOCHI
        };
        public enum VideoCodec
        {
            TV,
            BluRay,
            H264,
            H265
        }
        public enum VideoAspectRatio
        {
            AspectRatio_4_3,
            AspectRatio_16_9,
            AspectRatio_21_9
        };
        public enum VideoDefinition
        {
            SD,
            HD
        };
        public enum ImageType
        {
            unknown,
            CoverFront,
            CoverBack,
            Poster,
            Backdrop,
            SeasonCover,
            SeasonBackdrop,
            EpisodeCover,
            ExtraBackdrop,
            ExtraCover
        }
        public static int NumberOfServer = 4;
        public static string covertLanguageIsoCodeToDescription(string isoCode)
        {
            switch (isoCode)
            {
                case "de": return "deutsch";
                case "en": return "englisch";
                case "jp": return "japanisch";
                default: return "deutsch";
            }
        }
    }
}
