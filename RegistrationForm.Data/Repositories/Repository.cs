using RegistrationForm.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace RegistrationForm.Data.Repositories
{
    public class Repository : IRepository
{
    private readonly ApplicationDbContext _context;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UserDetail?> GetUserByIdAsync(int id)
    {
        return await _context.Users
            .Include(u => u.Credential)
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.UserId == id);
    }

    public async Task<UserDetail?> GetUserByEmailAsync(string email)
    {
        return await _context.Users
            .Include(u => u.Credential)
            .Include(u => u.Address)
            .FirstOrDefaultAsync(u => u.Credential.Email == email);
    }

    

    public async Task<bool> LoginUserAsync(string email, string password){
        return await _context.Users
            .Include(u => u.Credential)
            .AnyAsync(u => u.Credential.Email == email && u.Credential.PasswordHash == password);
    }

    public async Task<List<UserDetail>> GetAllUsersAsync()
    {
        return await _context.Users
            .Include(u => u.Credential)
            .Include(u => u.Address)
            .ToListAsync();
    }

    public async Task AddUserAsync(UserDetail user)
    {
        _context.Users.Add(user);
    }

    public async Task DeleteUserAsync(UserDetail user)
    {
        _context.Users.Remove(user);
    }

    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _context.UserCredentials.AnyAsync(u => u.Email == email);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }
}
}