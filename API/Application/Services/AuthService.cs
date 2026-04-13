using API.Application.DTOs;
using API.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using API.Application.Interfaces;
using API.Application.Common;
using API.Infrastructure.Data;

namespace API.Application.Services;

public class AuthService : IAuthService
{
    private readonly UserManager<AppUser> _userManager;
    private readonly IOtpService _otpService;
    private readonly IEmailService _emailService;
    private readonly IJwtService _jwtService;
    private readonly DataContext _dbContext;
    private readonly string language;

    public AuthService(UserManager<AppUser> userManager, IOtpService otpService, IEmailService emailService,
     IJwtService jwtService, DataContext dbContext, IRequestContext requestContext)
    {
        _userManager = userManager;
        _otpService = otpService;
        _jwtService = jwtService;
        _emailService = emailService;
        _dbContext = dbContext;
        language = requestContext.Language;
    }

    // -------------------------------------------------------------------------
    // Registration
    // -------------------------------------------------------------------------
    public async Task<ServiceResult<AuthResponseDto>> RegisterAsync(RegisterDto registerDto)
    {

        // verify that email is unique (not already taken by another user)
        if (await _userManager.FindByEmailAsync(registerDto.Email) != null)
            return ServiceResult<AuthResponseDto>.Failure(
                title: language == "ar" ? "البريد الإلكتروني مستخدم" : "Email Already In Use",
                detail: language == "ar" ? "يوجد حساب آخر يستخدم هذا البريد الإلكتروني. يرجى استخدام بريد إلكتروني مختلف." : "An account with that email already exists. Please use a different email address.",
                statusCode: StatusCodes.Status409Conflict
            );

        var user = new AppUser
        {
            FirstName = registerDto.FirstName,
            LastName = registerDto.LastName,
            Email = registerDto.Email,
            UserName = registerDto.Email,
            // PhoneNumber = registerDto.Phone,
            Gender = Gender.NotSpecified,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
        };


        var createUserResult = await _userManager.CreateAsync(user, registerDto.Password);

        if (!createUserResult.Succeeded)
        {
            var errors = string.Join(", ", createUserResult.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Registration failed: {errors}");
        }

        var result = new AuthResponseDto
        {
            Id = user.Id,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Email = user.Email,
        };

        return ServiceResult<AuthResponseDto>.Success(
            data: result,
            title: language == "ar" ? "التسجيل ناجح" : "Registration successful",
            detail: language == "ar" ? "تم إنشاء حسابك بنجاح." : "Your account has been created successfully."
        );
    }

    // -------------------------------------------------------------------------
    // Login
    // -------------------------------------------------------------------------

    public async Task<ServiceResult<string>> loginRequestAsync(LoginRequestDto loginDto)
    {
        var user = await _userManager.FindByEmailAsync(loginDto.Email);
        if (user == null)
            return ServiceResult<string>.Failure(
                title: language == "ar" ? "بيانات دخول غير صالحة" : "Invalid Credentials",
                detail: language == "ar" ? "لم يتم العثور على حساب بهذا البريد الإلكتروني." : "No account found with that email address.",
                statusCode: StatusCodes.Status404NotFound
            );

        var passwordValid = await _userManager.CheckPasswordAsync(user, loginDto.Password);
        if (!passwordValid)
            return ServiceResult<string>.Failure(
                title: language == "ar" ? "بيانات دخول غير صالحة" : "Invalid Credentials",
                detail: language == "ar" ? "كلمة المرور غير صحيحة." : "Incorrect password.",
                statusCode: StatusCodes.Status400BadRequest
            );

        
    // Generate a JWT token for the user
            var result = await _jwtService.GenerateTokenAsync(user);
            if (!result.IsSuccess)
                return ServiceResult<TokenResponseDto>.Failure(
                    "Token Generation Failed",
                    result.Detail,
                    result.StatusCode
                );

            return ServiceResult<TokenResponseDto>.Success(
                data: result.Data!,
                title: language == "ar" ? "تسجيل الدخول ناجح" : "Login successful",
                detail: language == "ar" ? "تم تسجيل الدخول بنجاح." : "You have been logged in successfully."
            );
        
    }

}
