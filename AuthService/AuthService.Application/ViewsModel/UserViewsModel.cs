namespace ECommerce.AuthService.Application.ViewsModel
{
    

    public class UserViewModel (int userId, string firstName, string lastName, string email)
    {
        public int UserId { get; set; } = userId;
        public string FirstName { get; set; } = firstName;
        public string LastName { get; set;} = lastName;
        public string Email { get; set; } = email;
    }

    public class LoginViewModel (string token, string email, string fullName)
    {
        public string Token { get; set; } = token;
        public string Email { get; set; } = email;
        public string FullName { get; set; } = fullName;
    }
}