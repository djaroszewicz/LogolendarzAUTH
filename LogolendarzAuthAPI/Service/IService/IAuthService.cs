using LogolendarzAuthAPI.Models.Dto;

namespace LogolendarzAuthAPI.Service.IService
{
  public interface IAuthService
  {
    Task<string> Register(RegisterRequestDto registerRequestDto);
    Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto);
  }
}
