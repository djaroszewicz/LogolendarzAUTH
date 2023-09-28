using LogolendarzAuthAPI.Models;

namespace LogolendarzAuthAPI.Service.IService
{
  public interface IJwtTokenGenerator
  {
    string GenerateToken(AppUser appUser);
  }
}
