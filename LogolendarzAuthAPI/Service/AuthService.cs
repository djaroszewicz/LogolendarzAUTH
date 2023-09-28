using LogolendarzAuthAPI.Data;
using LogolendarzAuthAPI.Models;
using LogolendarzAuthAPI.Models.Dto;
using LogolendarzAuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;

namespace LogolendarzAuthAPI.Service
{
  public class AuthService : IAuthService
  {
    private readonly AppDbContext _db;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IJwtTokenGenerator _jwtTokenGenerator;

    public AuthService(AppDbContext db, UserManager<AppUser> userManager, RoleManager<IdentityRole> roleManager, IJwtTokenGenerator jwtTokenGenerator)
    {
      _db = db;
      _userManager = userManager;
      _roleManager = roleManager;
      _jwtTokenGenerator = jwtTokenGenerator;
    }

    public async Task<LoginResponseDto> Login(LoginRequestDto loginRequestDto)
    {
      var user = _db.AppUser.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDto.UserName.ToLower());
      
      bool isValid = await _userManager.CheckPasswordAsync(user, loginRequestDto.Password);
      if(user == null || isValid == false)
      {
        return new LoginResponseDto() { User = null, Token = "" };
      }

      var token = _jwtTokenGenerator.GenerateToken(user);

      UserDto userDTO = new()
      {
        Email = user.Email,
        Id = user.Id,
        Name = user.Name,
        PhoneNumber = user.PhoneNumber
      };

      LoginResponseDto loginResponseDto = new LoginResponseDto()
      {
        User = userDTO,
        Token = ""
      };

      return loginResponseDto;
    }

    public async Task<string> Register(RegisterRequestDto registerRequestDto)
    {
      AppUser user = new()
      {
        UserName = registerRequestDto.Email,
        Email = registerRequestDto.Email,
        NormalizedEmail = registerRequestDto.Email.ToUpper(),
        Name = registerRequestDto.Name,
        PhoneNumber = registerRequestDto.PhoneNumber
      };

      try
      {
        var result = await _userManager.CreateAsync(user, registerRequestDto.Password);
        if (result.Succeeded)
        {
          var userToReturn = _db.AppUser.First(u => u.UserName == registerRequestDto.Email);
          UserDto userDto = new()
          {
            Email = userToReturn.Email,
            Id = userToReturn.Id,
            Name = userToReturn.Name,
            PhoneNumber = userToReturn.PhoneNumber
          };

          return "";
        }
        else
        {
          return result.Errors.FirstOrDefault().Description;
        }
      }
      catch (Exception ex)
      {

      }
      return "";
    }
  }
}
