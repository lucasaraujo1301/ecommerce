using ECommerce.AuthService.Domain.Entities;

namespace ECommerce.AuthService.Domain.Interfaces
{
    public interface IJwtService
    {
        string GenerateJwtToken(User user);
    }
}
