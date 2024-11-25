using RegistrationForm.Data.Models;

namespace RegistrationForm.Business.Services
{
    public interface IAuthService
    {
        string GenerateJwtToken(UserDetail user);
    }
}