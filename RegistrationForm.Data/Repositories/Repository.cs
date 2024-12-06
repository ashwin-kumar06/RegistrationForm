using System.Data;
using System.Data.SqlClient;
using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RegistrationForm.Data.Models;

namespace RegistrationForm.Data.Repositories
{
    public class Repository : IRepository
    {
        private readonly string _connectionString;
        private readonly ApplicationDbContext _context;

        public Repository(IConfiguration configuration, ApplicationDbContext context)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
        }

        private IDbConnection CreateConnection() => new SqlConnection(_connectionString);

        public async Task<UserDetail?> GetUserByIdAsync(int id)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);

            var result = await connection.QueryFirstOrDefaultAsync<UserDetailDto>(
                "RegistrationForm_GetById",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null) return null;

            return new UserDetail
            {
                UserId = result.UserId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Gender = result.Gender,
                DateOfBirth = result.DateOfBirth,
                CreatedAt = result.CreatedAt,
                Credential = new UserCredential
                {
                    Email = result.Email,
                    PasswordHash = result.PasswordHash // Be cautious with password handling
                },
                Address = new UserAddress
                {
                    StreetAddress = result.StreetAddress,
                    City = result.City,
                    State = result.State,
                    Country = result.Country,
                    PostalCode = result.PostalCode
                }
            };
        }

        public async Task<UserDetail?> GetUserByEmailAsync(string email)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            var result = await connection.QueryFirstOrDefaultAsync<UserDetailDto>(
                "SP_GetUserByEmail",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            if (result == null) return null;

            return new UserDetail
            {
                UserId = result.UserId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Gender = result.Gender,
                DateOfBirth = result.DateOfBirth,
                Credential = new UserCredential
                {
                    Email = result.Email,
                    PasswordHash = result.PasswordHash // Be cautious with password handling
                },
                Address = new UserAddress
                {
                    StreetAddress = result.StreetAddress,
                    City = result.City,
                    State = result.State,
                    Country = result.Country,
                    PostalCode = result.PostalCode
                }
            };
        }

        public async Task<bool> LoginUserAsync(string email, string password)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);
            parameters.Add("@PasswordHash", password);

            var result = await connection.QueryFirstOrDefaultAsync<int?>(
                "SP_UserLogin",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result.HasValue;
        }

        public async Task<List<UserDetail>> GetAllUsersAsync()
        {
            using var connection = CreateConnection();

            var results = await connection.QueryAsync<UserDetailDto>(
                "RegistrationForm_GetAll",
                commandType: CommandType.StoredProcedure
            );

            return results.Select(result => new UserDetail
            {
                UserId = result.UserId,
                FirstName = result.FirstName,
                LastName = result.LastName,
                PhoneNumber = result.PhoneNumber,
                Gender = result.Gender,
                DateOfBirth = result.DateOfBirth,
                CreatedAt = result.CreatedAt,
                Credential = new UserCredential
                {
                    Email = result.Email,
                    PasswordHash = result.PasswordHash // Be cautious with password handling
                },
                Address = new UserAddress
                {
                    StreetAddress = result.StreetAddress,
                    City = result.City,
                    State = result.State,
                    Country = result.Country,
                    PostalCode = result.PostalCode
                }
            }).ToList();
        }

        public async Task AddUserAsync(UserDetail user)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@FirstName", user.FirstName);
            parameters.Add("@LastName", user.LastName);
            parameters.Add("@PhoneNumber", user.PhoneNumber);
            parameters.Add("@Gender", user.Gender);
            parameters.Add("@DateOfBirth", user.DateOfBirth);
            if (user.Credential != null)
            {
                parameters.Add("@Email", user.Credential?.Email);
                parameters.Add("@PasswordHash", user.Credential?.PasswordHash);
            }
            // parameters.Add("@Email", user.Credential?.Email);
            // parameters.Add("@PasswordHash", user.Credential?.PasswordHash);
            if (user.Address != null)
            {
                parameters.Add("@StreetAddress", user.Address?.StreetAddress);
                parameters.Add("@City", user.Address?.City);
                parameters.Add("@State", user.Address?.State);
                parameters.Add("@Country", user.Address?.Country);
                parameters.Add("@PostalCode", user.Address?.PostalCode);
            }
            // parameters.Add("@StreetAddress", user.Address?.StreetAddress);
            // parameters.Add("@City", user.Address?.City);
            // parameters.Add("@State", user.Address?.State);
            // parameters.Add("@Country", user.Address?.Country);
            // parameters.Add("@PostalCode", user.Address?.PostalCode);

            await connection.ExecuteAsync(
                "RegistrationForm_Add",
                parameters,
                commandType: CommandType.StoredProcedure
            );

        }

        public async Task DeleteUserAsync(UserDetail user)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@UserId", user.UserId);

            await connection.ExecuteAsync(
                "RegistrationForm_Delete",
                parameters,
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Email", email);

            var result = await connection.QueryFirstOrDefaultAsync<bool>(
                "SP_CheckEmailExists",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return result;
        }

        public async Task<bool> UpdateUserAsync(int id, UserDetail model)
        {
            using var connection = CreateConnection();

            var parameters = new DynamicParameters();
            parameters.Add("@Id", id);
            parameters.Add("@FirstName", model.FirstName);
            parameters.Add("@LastName", model.LastName);
            parameters.Add("@PhoneNumber", model.PhoneNumber);
            parameters.Add("@Gender", model.Gender);
            parameters.Add("@DateOfBirth", model.DateOfBirth);
            parameters.Add("@StreetAddress", model.Address.StreetAddress);
            parameters.Add("@City", model.Address.City);
            parameters.Add("@State", model.Address.State);
            parameters.Add("@Country", model.Address.Country);
            parameters.Add("@PostalCode", model.Address.PostalCode);

            int rowsAffected = await connection.ExecuteAsync(
                "RegistrationForm_Update",
                parameters,
                commandType: CommandType.StoredProcedure
            );

            return rowsAffected > 0;
        }

        //     public async Task SaveChangesAsync()
        // {
        //     await _context.SaveChangesAsync();
        // }

        // DTO to map stored procedure results
        private class UserDetailDto
        {
            public int UserId { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? PhoneNumber { get; set; }
            public string? Gender { get; set; }
            public DateTime DateOfBirth { get; set; }
            public DateTime CreatedAt { get; set; }
            public string? Email { get; set; }
            public string? PasswordHash { get; set; }
            public string? StreetAddress { get; set; }
            public string? City { get; set; }
            public string? State { get; set; }
            public string? Country { get; set; }
            public string? PostalCode { get; set; }
        }
    }
}