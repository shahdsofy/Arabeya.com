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
        SignInManager<ApplicationUser>signInManager
        ,IOptions<JwtSettings>jwt
        , IAttachmentService _attachmentService
        , IConfiguration configuration
        ,IEmailService emailService) : IAuthService
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
            var result = await signInManager.CheckPasswordSignInAsync(user, model.Password,lockoutOnFailure: false);

            if (result.IsNotAllowed)
                return  Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Account is not confirmed yet.");

            if(result.IsLockedOut)
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Account is lock Out.");

            if (!result.Succeeded)
                return Response<UserDto>.Fail(HttpStatusCode.Unauthorized, ErrorType.Unauthorized.ToString(), "Invalid login");


            var response =  new UserDto()
            {
                Id = user.Id,
                DisplayName = user.DisplayName,
                Email = user.Email!,
                Token = await GenerateToken(user)

            };


            return Response<UserDto>.Success(response);

           


           
        }

        

        async Task<Response<UserDto>> IAuthService.RegisterAsync(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                DisplayName = model.DisplayName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                UserName = model.UserName,
               
            };


            if(model.DrivingLicence is not null)
            {
                 var pictureUrl= await  _attachmentService.UploadAsync(model.DrivingLicence, "images");

                if (pictureUrl != null) 
                    user.DrivingLicence= pictureUrl;
                else
                    user.DrivingLicence= null;
            }



            var result=await userManager.CreateAsync(user,model.Password);

             

            if (!result.Succeeded)
              return  Response<UserDto>.Fail(HttpStatusCode.BadRequest, ErrorType.Validation.ToString(), "Invalid Login!");

           result=  await userManager.AddToRoleAsync(user, model.Role!);

            if (!result.Succeeded)
                return Response<UserDto>.Fail(HttpStatusCode.BadRequest, ErrorType.Validation.ToString(), "Invalid Login!");


            // ✅ ✅ توليد التوكن هنا
            var token = await userManager.GenerateEmailConfirmationTokenAsync(user);
            var encodedToken = Uri.EscapeDataString(token);

            // ✅ تجهيز الإيميل
            var emailBody = $"Your Email Confirmation Token is:\n\n{encodedToken}\n\nUse this token in your app to confirm your email.";

            var emailModel = new Arabeya.Core.Application.Abstraction.Models.Emails.Email
            {
                To = user.Email!,
                Subject = "Confirm Your Email",
                Body = emailBody
            };

            // ✅ ✅ إرسال الإيميل
            await emailService.SendEmail(emailModel);


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
            var userClaims=await userManager.GetClaimsAsync(user);

            var rolesAsClaims = new List<Claim>();

            var roles=await userManager.GetRolesAsync(user);

            foreach (var role in roles)
                rolesAsClaims.Add(new Claim(ClaimTypes.Role,role));



            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.PrimarySid,user.Id),
                new Claim(ClaimTypes.Email,user.Email!),
                new Claim(ClaimTypes.GivenName,user.UserName!)
            }.Union(userClaims)
            .Union(rolesAsClaims);

            var symmtricClaims = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key));


            var signinCredinatials=new SigningCredentials(symmtricClaims,SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken
                (
                issuer: jwtSettings.Issuer,
                audience: jwtSettings.Audience,
                expires:DateTime.UtcNow.AddMinutes(jwtSettings.DurationsinMinutes),
                claims: claims,
                signingCredentials: signinCredinatials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<Response<string>> ConfirmUserEmail(ConfirmEmailDto email)
        {
            if (string.IsNullOrEmpty(email.Email) || string.IsNullOrEmpty(email.Token))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Email and Token are required.");

            var user = await userManager.FindByEmailAsync(email.Email);

            if (user == null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User not found.");

            var decodedToken = Uri.UnescapeDataString(email.Token);

            var result = await userManager.ConfirmEmailAsync(user, decodedToken);

            if (result.Succeeded)
                return Response<string>.Success("Email is successfully confirmed.");

            return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Email confirmation failed.");
        }

        public async  Task<Response<string>> ForgetPasswordAsync(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "Email is required");
           

            var user=await userManager.FindByEmailAsync(email);

            if (user == null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User is not found");


            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var encondedToken=Uri.EscapeDataString(token);
            Console.WriteLine("Token: " + encondedToken);
            var body = $"Your password reset token:\n\n{encondedToken}\n\nUse it with your email to reset your password.";


            var Emailbuilder = new Arabeya.Core.Application.Abstraction.Models.Emails.Email()
            {
                To = user.Email!,
                Body = body,
                Subject = "Reset Password"

            };

            await emailService.SendEmail(Emailbuilder);
            
            return Response<string>.Success("Password reset token has been sent to your email.");
        }

        public async Task<Response<string>> ResetPasswordAsync(ResetPasswordDto passwordDto)
        {
            if(string.IsNullOrEmpty(passwordDto.NewPassword)||
                string.IsNullOrEmpty(passwordDto.Token)||
                string.IsNullOrEmpty(passwordDto.Email))
             return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), "All fields Are required");

            var user = await userManager.FindByEmailAsync(passwordDto.Email);

            if (user==null)
                return Response<string>.Fail(HttpStatusCode.NotFound, ErrorType.NotFound.ToString(), "User is not found");


            var decodedToken = Uri.UnescapeDataString(passwordDto.Token);

            var result=await userManager.ResetPasswordAsync(user,decodedToken,passwordDto.NewPassword);

            if(result.Succeeded)
              return  Response<string>.Success("Password reset successfully.");

            var errors = result.Errors.Select(e => e.Description).ToList();
            return Response<string>.Fail(HttpStatusCode.BadRequest, ErrorType.BadRequest.ToString(), string.Join(", ", errors));
            
        }
    }
}
