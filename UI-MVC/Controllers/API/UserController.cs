using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Domain.Identity;
using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using UIMVC.Services;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace UIMVC.Controllers.API
{
    [Route("api/[controller]/[action]")]
    public class UserController : Controller
    {
        private readonly SignInManager<UimvcUser> _signInManager;
        private readonly UserManager<UimvcUser> _userManager;
        private readonly IConfiguration _configuration;
        private readonly RoleService _roleService;


        public UserController(SignInManager<UimvcUser> signInManager, UserManager<UimvcUser> userManager,
            IConfiguration configuration, RoleService roleService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _configuration = configuration;
            _roleService = roleService;
        }

        [HttpPost]
        public async Task<object> Login([FromBody] LoginInput model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);

            if (result.Succeeded)
            {
                var appUser = _userManager.Users.SingleOrDefault(r => r.Email == model.Email);
                return await GenerateJwtToken(model.Email, appUser);
            }

            throw new ApplicationException("INVALID_LOGIN_ATTEMPT");
        }

        [HttpPost]
        public async Task<object> Register([FromBody] RegisterInput model)
        {
            if (ModelState.IsValid)
            {
                var user = new UimvcUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Name = model.Name,
                    DateOfBirth = model.DateOfBirth,
                   
                };

                var result = await _userManager.CreateAsync(user, model.Password);

                if (result.Succeeded)
                {
                    //TODO() CONFIRMATION OF EMAIL 

                    var userFound = await _userManager.FindByEmailAsync(user.UserName);
                    _roleService.AssignToRole(userFound, Role.LoggedIn);

                    //TODO() Als email confirmation goed is, dan hoeft SignInAsync niet meer
                    await _signInManager.SignInAsync(user, false);
                    return await GenerateJwtToken(model.Email, user);
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return null;
        }

        private async Task<object> GenerateJwtToken(string email, IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtTokens:JwtKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                _configuration["JwtTokens:JwtIssuer"],
                _configuration["JwtTokens:JwtIssuer"],
                claims,
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public class LoginInput
        {
            [Required] [EmailAddress] public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Display(Name = "Remember me?")] public bool RememberMe { get; set; }
        }

        public class RegisterInput
        {
            [DataType(DataType.Text)]
            [Display(Name = "Naam")]
            public string Name { get; set; }

            [DataType(DataType.Date)]
            [Display(Name = "Geboortedatum")]
            public DateTime DateOfBirth { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
                MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }
    }
}