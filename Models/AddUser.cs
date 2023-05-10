namespace VkWebApiApplication.Models
{
    public class AddUser
    {
        public string Login { get; set; } = "";
        public string Password { get; set; } = "";
        public int UserGroupId { get; set; } = 0;
    }
}
