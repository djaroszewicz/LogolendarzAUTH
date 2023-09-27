using LogolendarzAuthAPI.Models.Dto;
using LogolendarzAuthAPI.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace LogolendarzAuthAPI.Controllers
{
  [Route("api/auth")]
  [ApiController]
  public class AuthAPIController : ControllerBase
  {
    private readonly IAuthService _authService;
    protected ResponseDto _response;
    public AuthAPIController(IAuthService authService)
    {
      _authService = authService;
      _response = new();
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequestDto model)
    {
      var errorMessage = await _authService.Register(model);
      if(!string.IsNullOrEmpty(errorMessage))
      {
        _response.IsSucces = false;
        _response.Message = errorMessage;
        return BadRequest(_response);
      }
      return Ok();
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
    {
      var loginResponse = await _authService.Login(model);
      if(loginResponse.User == null)
      {
        _response.IsSucces = false;
        _response.Message = "Username or password is incorrect";
        return BadRequest(_response);
      }
      _response.Result = loginResponse;
      _response.Result = loginResponse;
      return Ok(loginResponse);
    }
  }
}
