using ECommerce.AuthService.Api.ViewsModel;
using ECommerce.AuthService.Application.ViewsModel;

namespace ECommerce.AuthService.Domain.Interfaces
{
    public interface IUserUseCases
    {
        Task<ResponseViewModel<UserViewModel>> Register(RegisterRequestViewModel request);
        Task<ResponseViewModel<LoginViewModel>> Login(LoginRequestViewModel request);
        Task <ResponseViewModel<UserViewModel>> GetUserById (int id);
    }
}