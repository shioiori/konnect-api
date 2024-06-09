namespace Konnect.API.Data
{
    public class UserInfo
    {
        public string Avatar { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string DisplayName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string GroupId { get; set; }
        public string RoleId { get; set; }
        public string RoleName { get; set; }
    }

    public class UserGroupData
    {
        public string UserName { get; set; }
        public string GroupId { get; set; }
        public string RoleName { get; set; }
    }
}
