﻿using LogolendarzAuthAPI.Models;
using LogolendarzAuthAPI.Service.IService;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LogolendarzAuthAPI.Service
{
  public class JwtTokenGenerator : IJwtTokenGenerator
  {
    private readonly JwtOptions _jwtOptions;
    public JwtTokenGenerator(IOptions<JwtOptions> jwtOptions)
    {
      _jwtOptions = jwtOptions.Value;
    }

    public string GenerateToken(AppUser appUser)
    {
      var tokenHandler = new JwtSecurityTokenHandler();

      var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

      var claimList = new List<Claim>
      {
        new Claim(JwtRegisteredClaimNames.Email, appUser.Email),
        new Claim(JwtRegisteredClaimNames.Sub, appUser.Id),
        new Claim(JwtRegisteredClaimNames.Name, appUser.UserName.ToString())
      };

      var tokenDesc = new SecurityTokenDescriptor
      {
        Audience = _jwtOptions.Audience,
        Issuer = _jwtOptions.Issuer,
        Subject = new ClaimsIdentity(claimList),
        Expires = DateTime.UtcNow.AddDays(7),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDesc);

      return tokenHandler.WriteToken(token);
    }
  }
}
