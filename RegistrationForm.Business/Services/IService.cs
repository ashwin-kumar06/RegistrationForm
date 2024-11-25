using RegistrationForm.Business.ViewModels;
using RegistrationForm.Data.Models;

namespace RegistrationForm.Business.Services
{
    public interface IService
{
    Task<RegistrationViewModel?> GetUserViewModelAsync(int id);
    Task<UserDetail?> GetUserByEmailAsync(string email);
    Task<bool> LoginUserAsync(LoginViewModel model);

    Task<List<RegistrationViewModel>> GetAllUserViewModelsAsync();
    Task<bool> RegisterUserAsync(RegistrationViewModel model);
    Task<bool> EditUserAsync(int id, RegistrationViewModel model);
    Task<bool> DeleteUserAsync(int id);
    Task<bool> EmailExistsAsync(string email);
}
}