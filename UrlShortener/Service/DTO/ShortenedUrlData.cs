namespace UrlShortener.Entities
{
    public class ShortenedUrlData
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public DateTime CreationTime { get; set; }
    }
}
