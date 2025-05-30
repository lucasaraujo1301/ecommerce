using ECommerce.AuthService.Application.ViewsModel;
using ECommerce.AuthService.Api.ViewsModel;
using ECommerce.AuthService.Domain.Entities;
using ECommerce.AuthService.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace ECommerce.AuthService.Application.UseCase
{
    public class UserUseCases(IUserRepository userRepository, IJwtService jwtService) : IUserUseCases
    {
        private readonly IUserRepository _userRepository = userRepository;
        private readonly IJwtService _jwtService = jwtService;

        public async Task<ResponseViewModel<UserViewModel>> Register(RegisterRequestViewModel request)
        {
            User? user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user != null)
            {
                Dictionary<string, object> errors = new() {
                    {
                        "Email",
                        new List<string> {
                            "An account with this email address already exists. Please log in or use a different email."
                        }
                    }
                };
                return ResponseViewModel<UserViewModel>.Failure(errors);
            }

            User newUser = new()
            {
                Email = request.Email,
                Password = request.Password,
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            newUser = await _userRepository.CreateAsync(newUser);

            UserViewModel response = new(
                newUser.UserId,
                newUser.FirstName,
                newUser.LastName,
                newUser.Email
            );

            return ResponseViewModel<UserViewModel>.Success(response);
        }

        public async Task<ResponseViewModel<LoginViewModel>> Login(LoginRequestViewModel request)
        {
            User? user = await _userRepository.GetUserByEmailAsync(request.Email);

            if (user == null || !_userRepository.IsValidPassword(user, request.Password) || !user.IsActice)
            {
                Dictionary<string, object> errors = new() {
                    {
                        "General",
                        new List<string> {
                            "Email or password invalid."
                        }
                    }
                };
                return ResponseViewModel<LoginViewModel>.Failure(errors);
            }

            string token = _jwtService.GenerateJwtToken(user);

            LoginViewModel response = new(
                token,
                user.Email,
                user.GetFullName()
            );

            return ResponseViewModel<LoginViewModel>.Success(response);
        }

        public async Task<ResponseViewModel<UserViewModel>> GetUserById(int userId)
        {
            User? user = await _userRepository.GetUserByIdAsync(userId);

            if (user == null || !user.IsActice)
            {
                Dictionary<string, object> errors = new() {
                    {
                        "General",
                        new List<string> {
                            "Fail to find the user."
                        }
                    }
                };
                return ResponseViewModel<UserViewModel>.Failure(errors);
            }

            UserViewModel response = new(
                user.UserId,
                user.FirstName,
                user.LastName,
                user.Email
            );

            return ResponseViewModel<UserViewModel>.Success(response);
        }
    }
    
}