namespace Holod.Models.Database
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User()
        {
            Login = Password = string.Empty;
        }
    }
}
