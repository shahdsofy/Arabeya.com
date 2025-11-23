using Arabeya.Core.Application.Abstraction.Models.Auth;
using Arabeya.Core.Application.Abstraction.Sevices.Auth;
using Arabeya.Core.Application.Abstraction.Sevices.Emails;
using Arabeya.Core.Domain.Contracts.Infrastructure;
using Arabeya.Core.Domain.Entities.Identity;
using Arabeya.Infrastructure;
using Arabeya.Shared.Errors;
using Arabeya.Shared.Responses;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Arabeya.Core.Application.Services.Auth
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
        , IOptions<JwtSettings> jwt
        , IAttachmentService _attachmentService
        , IConfiguration configuration
        , IEmailService emailService
        , IMemoryCache memory) : IAuthService
    {
        private readonly JwtSettings jwtSettings = jwt.Value;
        public async Task<Response<UserDto>> LoginAsync(LoginDto model)
        {
            var user = await userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Invalid Login!");
            }

            var check = await userManager.CheckPasswordAsync(user, model.Password);
            Console.WriteLine(check);
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password, lockoutOnFailure: false);

            if (result.IsNotAllowed)
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Account is not confirmed yet.");

            if (result.IsLockedOut)
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Account is lock Out.");

            if (!result.Succeeded)
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Invalid login");


            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateToken(user)

            };


            return Response<UserDto>.Success(response);





        }

        public async Task<Response<UserDto>> RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,

            };


            if (model.DrivingLicence is not null)
            {
                var pictureUrl = await _attachmentService.UploadAsync(model.DrivingLicence, "images");

                if (pictureUrl != null)
                    user.DrivingLicence = pictureUrl;
                else
                    user.DrivingLicence = null;
            }



            var result = await userManager.CreateAsync(user, model.Password);



            if (!result.Succeeded)
                return Response<UserDto>.Fail(HttpStatusCode.BadRequest, ErrorType.Validation.ToString(), "Invalid Login!");

            result = await userManager.AddToRoleAsync(user, model.Role!);

            if (!result.Succeeded)
                return Response<UserDto>.Fail(HttpStatusCode.BadRequest, ErrorType.Validation.ToString(), "Invalid Login!");


            #region Generate OTP and send email
            var otp = new Random().Next(100000, 999999).ToString();
            var expiration = DateTime.UtcNow.AddMinutes(10);

            await userManager.AddClaimAsync(user, new Claim("email_otp", otp));
            await userManager.AddClaimAsync(user, new Claim("email_otp_expiration", expiration.ToString()));





            var emailModel = new Arabeya.Core.Application.Abstraction.Models.Emails.Email
            {
                To = user.Email!,
                Subject = "Confirm Your Email",
                Body = $"Your verification code is: {otp}\n\nThis code expires in 10 minutes."
            };

            await emailService.SendEmail(emailModel);

            #endregion

            var response = new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                DrivingLicence = $"{configuration["URLs:BaseURL"]}/{user.DrivingLicence}",
                Token = await GenerateToken(user)
            };

            return Response<UserDto>.Success(response);

        }

        private async Task<string> GenerateToken(ApplicationUser user)
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            var rolesAsClaims = new List<Claim>();

            var roles = await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                rolesAsClaims.Add(new Claim(ClaimTypes.Role, role));



            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.GivenName,user.UserName!)
            }.Union(userClaims)
            .Union(rolesAsClaims);

            var symmtricClaims = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));


            var signinCredinatials = new SigningCredentials(symmtricClaims, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken
                (
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                expires: DateTime.UtcNow.AddMinutes(jwtSettings.DurationsinMinutes),
                claims: claims,
                signingCredentials: signinCredinatials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response<string>> ConfirmUserEmail(ConfirmEmailDto email)
        {

            var user = await userManager.FindByEmailAsync(email.Email);

            if (user == null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User not found.");

            var claims = await userManager.GetClaimsAsync(user);

            var otp = claims.FirstOrDefault(c => c.Type == "email_otp")?.Value;
            var expirationDate = claims.FirstOrDefault(c => c.Type == "email_otp_expiration")?.Value;


            if (otp == null || expirationDate == null)
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "No OTP found. Please request a new one.");

            if (otp != email.OTP)
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Invalid OTP.");

            if (DateTime.UtcNow > DateTime.Parse(expirationDate))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "OTP has expired. Please request a new one.");


            user.EmailConfirmed = true;

            var result = await userManager.UpdateAsync(user);

            await userManager.RemoveClaimAsync(user, new Claim("email_otp", otp));
            await userManager.RemoveClaimAsync(user, new Claim("email_otp_expiration", expirationDate));


            if (result.Succeeded)
                return Response<string>.Success("Email is successfully confirmed.");

            return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Email confirmation failed.");
        }

        public async Task<Response<string>> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Email is required");


            var user = await userManager.FindByEmailAsync(email);

            if (user == null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User is not found");





            var otp = new Random().Next(100000, 999999).ToString();

            // Store OTP in cache for 5 minutes
            memory.Set($"OTP_{email}", otp, TimeSpan.FromMinutes(5));

            // Send Email
            await emailService.SendEmail(new Arabeya.Core.Application.Abstraction.Models.Emails.Email
            {
                To = user.Email!,
                Subject = "Reset Password",
                Body = $"Your OTP for password reset is: {otp}. It is valid for 5 minutes."
            });    

            return Response<string>.Success("Reset password link has been sent to your email.");

        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDto passwordDto)
        {
            if (!memory.TryGetValue($"OTP_{passwordDto.Email}", out string savedOtp))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "OTP expired or not found.");



            if (string.IsNullOrEmpty(passwordDto.NewPassword) ||
               string.IsNullOrEmpty(passwordDto.Email) ||
                   string.IsNullOrEmpty(passwordDto.OldPassword))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "All fields Are required");

            if (savedOtp != passwordDto.OTP)
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Invalid OTP.");




            var user = await userManager.FindByEmailAsync(passwordDto.Email);

            if (user == null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User is not found");


            var result = await userManager.ChangePasswordAsync(user, passwordDto.OldPassword, passwordDto.NewPassword);

            if (result.Succeeded)
                return Response<string>.Success("Password reset successfully.");

            memory.Remove($"OTP_{passwordDto.Email}");

            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), string.Join(", ", errors));

        }
    }
}
