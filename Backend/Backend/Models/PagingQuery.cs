namespace Backend.Models
{
    public class PagingQuery
    {
        public string Limit { get; set; } = "20";
        public string Offset { get; set; } = "0";
    }
}