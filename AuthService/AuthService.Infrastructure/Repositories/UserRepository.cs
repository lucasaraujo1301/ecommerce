using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


using ECommerce.AuthService.Domain.Entities;
using ECommerce.AuthService.Domain.Interfaces;
using ECommerce.AuthService.Infrastructure.Data;

namespace ECommerce.AuthService.Infrastructure.Repositories
{
    public class UserRepository (ECommerceAuthServiceDbContext dbContext, IPasswordHasher<User> passwordHasher) : IUserRepository
    {
        private readonly ECommerceAuthServiceDbContext _dbContext = dbContext;
        private readonly IPasswordHasher<User> _passwordHasher  = passwordHasher;

        private string NormalizeEmail(string email)
        {
            if (!email.Contains('@')) {
                throw new ArgumentException("Email in wrong format.");
            }

            int indexOf = email.IndexOf('@');

            string local = email[..indexOf];
            string domain = email[indexOf..];
            domain = domain.ToLowerInvariant();

            return $"{local}{domain}".Trim();
        }

        public async Task<User?> GetUserByIdAsync(int id) {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task<User?> GetUserByEmailAsync(string email) {
            string normalizedEmail = NormalizeEmail(email);
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.Email == normalizedEmail);
        }

        public async Task<User> CreateAsync(User user)
        {
            string normalizedEmail = NormalizeEmail(user.Email);
            user.Email = normalizedEmail;

            string passwordHashed = _passwordHasher.HashPassword(user, user.Password);
            user.Password = passwordHashed;

            await _dbContext.Users.AddAsync(user);
            await _dbContext.SaveChangesAsync();

            return user;
        }

        public async Task UpdateAsync(User user)
        {
            string normalizedEmail = NormalizeEmail(user.Email);
            user.Email = normalizedEmail;
            user.UpdatedAt = DateTime.Now;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(User user) {
            user.IsActice = false;
            user.UpdatedAt = DateTime.Now;

            _dbContext.Users.Update(user);
            await _dbContext.SaveChangesAsync();
        }

        public bool IsValidPassword(User user, string password)
        {
            PasswordVerificationResult verificationResult = _passwordHasher.VerifyHashedPassword(user, user.Password, password);

            return verificationResult == PasswordVerificationResult.Success ||
                   verificationResult == PasswordVerificationResult.SuccessRehashNeeded;
        }
    }
    
}