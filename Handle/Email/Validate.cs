using System.ComponentModel.DataAnnotations;

namespace MovieManagementSystem.Handle.Email
{
    public class Validate
    {
        public static bool IsValidEmail(string email)
        {
            var checkEmail = new EmailAddressAttribute();
            return checkEmail.IsValid(email);
        }
    }
}
