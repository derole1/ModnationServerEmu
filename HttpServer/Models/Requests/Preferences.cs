namespace HttpServer.Models.Requests
{
    public class Preferences
    {
        public string LanguageCode { get; set; }
        public string Timezone { get; set; }
        public string RegionCode { get; set; }
        public string Domain { get; set; }
    }
}
