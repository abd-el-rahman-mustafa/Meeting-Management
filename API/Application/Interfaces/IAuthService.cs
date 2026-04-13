using API.Application.Common;
using API.Application.DTOs;
using API.Domain.Entities;

namespace API.Application.Interfaces;

public interface IAuthService
{
    /// <summary>
    /// Registers a new user with the provided detail.
    /// Returns the created user's info on success, or throws on failure.
    /// </summary>
    Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto);

    /// <summary>
    /// Authenticates a user with email and password.
    /// Send otp to email if credentials are valid.
    /// </summary>
    Task<ServiceResult<string>> loginRequestAsync(LoginRequestDto loginDto);

    
}
