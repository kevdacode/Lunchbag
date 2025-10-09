namespace Lunchbag.API.Models
{
    public class RegisterCustomerDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; }
        public string Password {  get; set; }

    }
}
