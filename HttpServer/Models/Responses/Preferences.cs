using System.Xml.Serialization;

namespace HttpServer.Models.Responses
{
    public class Preferences
    {
        [XmlAttribute("domain")]
        public string Domain { get; set; }
        [XmlAttribute("ip_address")]
        public string IPAddress { get; set; }
        [XmlAttribute("language_code")]
        public string LanguageCode { get; set; }
        [XmlAttribute("region_code")]
        public string RegionCode { get; set; }
        [XmlAttribute("timezone")]
        public string Timezone { get; set; }
    }
}
