namespace UrlShortener.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid SessionKey { get; set; }
        public DateTime CreationTime { get; set; }

        public Session(string email)
        {
            Id = Guid.NewGuid();
            Email = email;
            SessionKey = Guid.NewGuid();
            CreationTime = DateTime.Now;
        }
    }
}
