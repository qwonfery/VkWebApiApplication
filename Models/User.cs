namespace VkWebApiApplication.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";

        public DateTime Created { get; set; } = DateTime.UtcNow;

        public int UserGroupId { get; set; } = 1;
        public int UserStateId { get; set; } = 1;

        public User()
        {
        }

        public User(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public User(AddUser addUser)
        {
            Login = addUser.Login;
            Password = addUser.Password;
            UserGroupId = addUser.UserGroupId;
        }


    }
}
