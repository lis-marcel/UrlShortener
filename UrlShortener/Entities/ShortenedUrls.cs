namespace UrlShortener.Entities
{
    public class ShortenedUrl
    {
        public Guid Id { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
        public string ShortUrl { get; set; } = string.Empty;
        public Guid? OwnerId { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
