using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Models.DTO.Response;

namespace RoboAdvisorApp.API.Services.Interface
{
    public interface IUserService
    {
        Task<RegistrationResponse> RegisterAsync(UserDto userDto);
        Task<AuthenticationResponse> AuthenticateAsync(string username, string password);
    }

}
