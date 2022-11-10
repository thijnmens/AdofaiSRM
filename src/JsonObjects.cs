using System.Collections.Generic;

namespace AdofaiSRM
{
    internal class JsonObjects
    {
        public class AnalysisResult // https://developers.virustotal.com/reference/file-info
        {
            public FileInfoData data { get; set; }

            public class Scanners
            {
                public string category { get; set; }
                public string engine_name { get; set; }
                public string engine_version { get; set; }
                public object result { get; set; }
                public string method { get; set; }
                public string engine_update { get; set; }
            }

            public class Attributes
            {
                public string type_description { get; set; }
                public string tlsh { get; set; }
                public List<string> names { get; set; }
                public int last_modification_date { get; set; }
                public string type_tag { get; set; }
                public int times_submitted { get; set; }
                public TotalVotes total_votes { get; set; }
                public int size { get; set; }
                public string type_extension { get; set; }
                public int last_submission_date { get; set; }
                public LastAnalysisResults last_analysis_results { get; set; }
                public string sha256 { get; set; }
                public List<string> tags { get; set; }
                public int last_analysis_date { get; set; }
                public int unique_sources { get; set; }
                public int first_submission_date { get; set; }
                public string ssdeep { get; set; }
                public string md5 { get; set; }
                public string sha1 { get; set; }
                public string magic { get; set; }
                public LastAnalysisStats last_analysis_stats { get; set; }
                public string meaningful_name { get; set; }
                public int reputation { get; set; }
            }

            public class FileInfoData
            {
                public Attributes attributes { get; set; }
                public string type { get; set; }
                public string id { get; set; }
                public Links links { get; set; }
            }

            public class LastAnalysisResults
            {
                public Scanners Bkav { get; set; }
                public Scanners Lionic { get; set; }
                public Scanners tehtris { get; set; }
                public Scanners ClamAV { get; set; }
                public Scanners FireEye { get; set; }
                public Scanners CATQuickHeal { get; set; }
                public Scanners McAfee { get; set; }
                public Scanners Malwarebytes { get; set; }
                public Scanners Zillya { get; set; }
                public Scanners Paloalto { get; set; }
                public Scanners Sangfor { get; set; }
                public Scanners K7AntiVirus { get; set; }
                public Scanners Alibaba { get; set; }
                public Scanners K7GW { get; set; }
                public Scanners Trustlook { get; set; }
                public Scanners BitDefenderTheta { get; set; }
                public Scanners VirIT { get; set; }
                public Scanners Cyren { get; set; }
                public Scanners SymantecMobileInsight { get; set; }
                public Scanners Symantec { get; set; }
                public Scanners Elastic { get; set; }
                public Scanners ESETNOD32 { get; set; }
                public Scanners Baidu { get; set; }
                public Scanners TrendMicroHouseCall { get; set; }
                public Scanners Avast { get; set; }
                public Scanners Cynet { get; set; }
                public Scanners Kaspersky { get; set; }
                public Scanners BitDefender { get; set; }
                public Scanners NANOAntivirus { get; set; }
                public Scanners SUPERAntiSpyware { get; set; }
                public Scanners MicroWorldEScan { get; set; }
                public Scanners APEX { get; set; }
                public Scanners Tencent { get; set; }
                public Scanners AdAware { get; set; }
                public Scanners Emsisoft { get; set; }
                public Scanners Comodo { get; set; }
                public Scanners FSecure { get; set; }
                public Scanners DrWeb { get; set; }
                public Scanners VIPRE { get; set; }
                public Scanners TrendMicro { get; set; }
                public Scanners McAfeeGWEdition { get; set; }
                public Scanners SentinelOne { get; set; }
                public Scanners Trapmine { get; set; }
                public Scanners CMC { get; set; }
                public Scanners Sophos { get; set; }
                public Scanners Ikarus { get; set; }
                public Scanners GData { get; set; }
                public Scanners Jiangmin { get; set; }
                public Scanners Webroot { get; set; }
                public Scanners Avira { get; set; }
                public Scanners AntiyAVL { get; set; }
                public Scanners Kingsoft { get; set; }
                public Scanners Gridinsoft { get; set; }
                public Scanners Arcabit { get; set; }
                public Scanners ViRobot { get; set; }
                public Scanners ZoneAlarm { get; set; }
                public Scanners AvastMobile { get; set; }
                public Scanners Microsoft { get; set; }
                public Scanners Google { get; set; }
                public Scanners BitDefenderFalx { get; set; }
                public Scanners AhnLabV3 { get; set; }
                public Scanners Acronis { get; set; }
                public Scanners VBA32 { get; set; }
                public Scanners ALYac { get; set; }
                public Scanners MAX { get; set; }
                public Scanners Cylance { get; set; }
                public Scanners Zoner { get; set; }
                public Scanners Rising { get; set; }
                public Scanners Yandex { get; set; }
                public Scanners TACHYON { get; set; }
                public Scanners MaxSecure { get; set; }
                public Scanners Fortinet { get; set; }
                public Scanners AVG { get; set; }
                public Scanners Cybereason { get; set; }
                public Scanners Panda { get; set; }
                public Scanners CrowdStrike { get; set; }
            }

            public class LastAnalysisStats
            {
                public int harmless { get; set; }
                public int TypeUnsupported { get; set; }
                public int suspicious { get; set; }
                public int ConfirmedTimeout { get; set; }
                public int timeout { get; set; }
                public int failure { get; set; }
                public int malicious { get; set; }
                public int undetected { get; set; }
            }

            public class Links
            {
                public string self { get; set; }
            }

            public class TotalVotes
            {
                public int harmless { get; set; }
                public int malicious { get; set; }
            }
        }

        public class UploadResult // https://developers.virustotal.com/reference/files-scan
        {
            public FilesData data { get; set; }

            public class FilesData
            {
                public string type { get; set; }
                public string id { get; set; }
            }
        }

        public class LevelResult // https://adofai.gg:9200/api/v1/levels/{id}
        {
            public int id { get; set; }
            public string title { get; set; }
            public double difficulty { get; set; }
            public List<Creator> creators { get; set; }
            public Music music { get; set; }
            public int tiles { get; set; }
            public int comments { get; set; }
            public int likes { get; set; }
            public bool epilepsyWarning { get; set; }
            public bool censored { get; set; }
            public string description { get; set; }
            public string video { get; set; }
            public string download { get; set; }
            public string workshop { get; set; }
            public List<Tag> tags { get; set; }

            public class Artist
            {
                public int id { get; set; }
                public string name { get; set; }
            }

            public class Creator
            {
                public int id { get; set; }
                public string name { get; set; }
            }

            public class Music
            {
                public int id { get; set; }
                public string name { get; set; }
                public double minBpm { get; set; }
                public double maxBpm { get; set; }
                public List<Artist> artists { get; set; }
            }

            public class Tag
            {
                public int id { get; set; }
                public string name { get; set; }
            }
        }

        public class ConfigJson
        {
            public string Id { get; set; }
            public string Author { get; set; }
            public string DisplayName { get; set; }
            public string Version { get; set; }
            public string EntryMethod { get; set; }
            public ConfigData Config { get; set; }

            public class ConfigData
            {
                public string VirusTotalKey { get; set; }
                public string TwitchKey { get; set; }
                public string YoutubeKey { get; set; }
                public string Language { get; set; }
                public bool WorkshopOnlyMode { get; set; }
                public List<string> Channels { get; set; }
            }
        }
    }
}