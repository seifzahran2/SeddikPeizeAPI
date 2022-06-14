using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using SeddikPeizeAPI.Helpers;
using SeddikPeizeAPI.Models;
using SeddikPeizeAPI.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace SeddikPeizeAPI.Services
{
    public class UserService : IUserServices
    {
        private readonly IConfiguration configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly JWT _jwt;
        public UserService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IOptions<JWT> jwt, SignInManager<ApplicationUser> signManager)
        {
            this.configuration = configuration;
            _userManager = userManager;
            this._jwt = jwt.Value;
            this.signInManager = signManager;
        }

        public async Task<AuthVM> LoginAsync(LoginVM model)
        {
            var authModel = new AuthVM();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                authModel.Message = "Email or Password is incorrect!";
                authModel.Check = false;
                return authModel;
            }

            var jwtSecurityToken = await CreateJwtToken(user);
            var rolesList = await _userManager.GetRolesAsync(user);

            authModel.IsAuthed = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            authModel.ExpireOn = jwtSecurityToken.ValidTo;
            authModel.Roles = rolesList.ToList();
            authModel.Check = true;
            return authModel;
        }

        public async Task<AuthVM> LogoutAsync()
        {
            await signInManager.SignOutAsync();
            return new AuthVM { Message = "تم تسجيل الخروج", Check = true };
        }





        public async Task<AuthVM> RegisterAsync(RegisterVM model)
        {

            if (await _userManager.FindByEmailAsync(model.Email) is not null)
                return new AuthVM { Message = "يوجد حساب بهذا البريد الالكتروني", Check = false };

            var user = new ApplicationUser
            {
                Name = model.Name,
                Email = model.Email,
                UserName = new MailAddress(model.Email).User,
                age = model.age,
                mobileNumber = model.mobileNumber,
                NationalID = model.NationalID,
                gender = model.gender,
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                var errors = string.Empty;
                foreach (var error in result.Errors)
                {
                    errors += $"{error.Description},";
                }
                return new AuthVM { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthVM
            {
                Email = user.Email,
                ExpireOn = jwtSecurityToken.ValidTo,
                IsAuthed = true,
                Roles = new List<string> { "User" },
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                UserName = user.UserName,
                Check = true
            };
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDays),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }


        public void decodeToken(string token)
        {
            var handler = new JwtSecurityTokenHandler()
                .ValidateToken(token, new TokenValidationParameters()
                {

                }, out SecurityToken stoken);
        }
    }
}
