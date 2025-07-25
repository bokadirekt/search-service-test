namespace SearchEngine.Models
{
    public class ServiceOutputResponse
    {
        public int TotalHits { get; set; }
        public int TotalDocuments { get; set; }
        public List<ResultItem> Results { get; set; }
    }
    public class ResultItem
    {

        public int Id { get; set; }
        public string Name { get; set; }
        public geoPosition Position { get; set; }
        public string Distance { get; set; }
        public int Score { get; set; }
    }
}
