using ECommerce.AuthService.Domain.Entities;

namespace ECommerce.AuthService.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User?> GetUserByIdAsync(int id);
        Task<User?> GetUserByEmailAsync(string email);
        Task<User> CreateAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(User user);
        bool IsValidPassword(User user, string password);
    }
    
}