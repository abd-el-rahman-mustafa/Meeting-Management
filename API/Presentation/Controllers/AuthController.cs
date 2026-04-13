using API.Application.DTOs;
using API.Application.Interfaces;
using API.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace API.Presentation.Controllers;

public class AuthController : BaseController
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
    {
        try
        {
            var authResponse = await _authService.RegisterAsync(registerDto);
            return authResponse.IsSuccess
                    ? Ok(authResponse)
                    : Problem(title: authResponse.Title, detail: authResponse.Detail, statusCode: authResponse.StatusCode);

        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: 400);
        }
    }

    [HttpPost("login-request")]
    public async Task<IActionResult> loginRequest([FromBody] LoginRequestDto loginDto)
    {
        try
        {
            var authResponse = await _authService.loginRequestAsync(loginDto);
            return authResponse.IsSuccess
                    ? Ok(authResponse)
                    : Problem(title: authResponse.Title, detail: authResponse.Detail, statusCode: authResponse.StatusCode);
        }
        catch (InvalidOperationException ex)
        {
            return Problem(detail: ex.Message, statusCode: 400);
        }
    }



}
