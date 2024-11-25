namespace RegistrationForm.Data.Models
{
    public class UserAddress
    {
    public int UserAddressId { get; set; }
    public int UserId { get; set; }
    public string? StreetAddress { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    
    // Navigation property
    public UserDetail? User { get; set; }
    }
}