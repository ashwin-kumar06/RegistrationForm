using RegistrationForm.Data.Repositories;
using RegistrationForm.Data.Models;
using RegistrationForm.Business.ViewModels;

namespace RegistrationForm.Business.Services
{
    public class Service : IService
{
    private readonly IRepository _userRepository;

    public Service(IRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<RegistrationViewModel?> GetUserViewModelAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return null;

        return new RegistrationViewModel
        {
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Credential.Email,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
            StreetAddress = user.Address.StreetAddress,
            City = user.Address.City,
            State = user.Address.State,
            Country = user.Address.Country,
            PostalCode = user.Address.PostalCode
        };
    }

    public async Task<UserDetail?> GetUserByEmailAsync(string email)
    {
        var user = await _userRepository.GetUserByEmailAsync(email);
        if (user == null) return null;

        return new UserDetail
        {
            UserId = user.UserId,
            FirstName = user.FirstName,
            LastName = user.LastName,
            PhoneNumber = user.PhoneNumber,
            DateOfBirth = user.DateOfBirth,
            Gender = user.Gender,
        };
    }

    

    public async Task<bool> LoginUserAsync(LoginViewModel model){
        return await _userRepository.LoginUserAsync(model.Email, model.Password);
    }

    public async Task<List<RegistrationViewModel>> GetAllUserViewModelsAsync()
    {
        var users = await _userRepository.GetAllUsersAsync();
        return users.Select(u => new RegistrationViewModel
        {
            UserId = u.UserId,
            FirstName = u.FirstName,
            LastName = u.LastName,
            Email = u.Credential.Email,
            PhoneNumber = u.PhoneNumber,
            DateOfBirth = u.DateOfBirth,
            Gender = u.Gender,
            StreetAddress = u.Address.StreetAddress,
            City = u.Address.City,
            State = u.Address.State,
            Country = u.Address.Country,
            PostalCode = u.Address.PostalCode
        }).ToList();
    }

    public async Task<bool> RegisterUserAsync(RegistrationViewModel model)
    {
        if (await _userRepository.EmailExistsAsync(model.Email))
            return false;

        var user = new UserDetail
        {
            FirstName = model.FirstName,
            LastName = model.LastName,
            PhoneNumber = model.PhoneNumber,
            DateOfBirth = model.DateOfBirth,
            Gender = model.Gender,
            Credential = new UserCredential
            {
                Email = model.Email,
                PasswordHash = model.Password // Use proper password hashing
            },
            Address = new UserAddress
            {
                StreetAddress = model.StreetAddress,
                City = model.City,
                State = model.State,
                Country = model.Country,
                PostalCode = model.PostalCode
            }
        };

        await _userRepository.AddUserAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EditUserAsync(int id, RegistrationViewModel model)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return false;

        user.FirstName = model.FirstName;
        user.LastName = model.LastName;
        user.PhoneNumber = model.PhoneNumber;
        user.DateOfBirth = model.DateOfBirth;
        user.Address.StreetAddress = model.StreetAddress;
        user.Address.City = model.City;
        user.Address.State = model.State;
        user.Address.Country = model.Country;
        user.Address.PostalCode = model.PostalCode;

        await _userRepository.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteUserAsync(int id)
    {
        var user = await _userRepository.GetUserByIdAsync(id);
        if (user == null) return false;

        await _userRepository.DeleteUserAsync(user);
        await _userRepository.SaveChangesAsync();
        return true;
    }

    public Task<bool> EmailExistsAsync(string email)
    {
        return _userRepository.EmailExistsAsync(email);
    }
}
}