using RegistrationForm.Data.Models;

namespace RegistrationForm.Data.Repositories
{
    public interface IRepository
    {
        Task<UserDetail?> GetUserByIdAsync(int id);
        Task<UserDetail?> GetUserByEmailAsync(string email);
        Task<bool> LoginUserAsync(string email, string password);
        Task<List<UserDetail>> GetAllUsersAsync();
        Task AddUserAsync(UserDetail user);
        Task DeleteUserAsync(UserDetail user);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> UpdateUserAsync(int id, UserDetail user);
        //Task SaveChangesAsync();
    }
}