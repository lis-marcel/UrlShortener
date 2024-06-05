namespace UrlShortener.Entities
{
    public class UserData
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public UserData(string Name, string Email, string Password)
        {
            Id = Guid.NewGuid();
            this.Name = Name;
            this.Email = Email;
            this.Password = Password;
        }
    }
}