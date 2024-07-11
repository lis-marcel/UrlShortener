namespace UrlShortener.Entities
{
    public class Session
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public Guid SessionKey { get; set; }
        public DateTime CreationTime { get; set; }
        public DateTime ExpirationTime { get; set; }

        public Session(string email)
        {
            Email = email;
            SessionKey = Guid.NewGuid();
            CreationTime = DateTime.Now;
            ExpirationTime = DateTime.Now.AddMinutes(20);
        }
    }
}
