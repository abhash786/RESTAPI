namespace RManjusha.RestServices.Models
{
    public class SearchInput
    {
        public string Keyword { get; set; }
        public string Location { get; set; }
        public string Category { get; set; }
        public bool IsJob { get; set; }
    }
}
