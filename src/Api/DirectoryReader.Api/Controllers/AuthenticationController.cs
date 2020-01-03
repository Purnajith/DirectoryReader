using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DirectoryReader.Lib.Models.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DirectoryReader.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
		#region Members

		private readonly IOptionsSnapshot<AppSettings>		_settings;

		#endregion

		public AuthenticationController(IOptionsSnapshot<AppSettings> _settings)  
        {  
			this._settings = _settings;	
        } 

		[AllowAnonymous]  
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] UserModel login)
        {  
            IActionResult response = Unauthorized();  
            var user = AuthenticateUser(login);  
  
            if (user != null)  
            {  
                var tokenString = GenerateJSONWebToken(user);  
                response = Ok(new TokenModel() { Token = tokenString});  
            }  
  
            return response;  
        }  
  
        private string GenerateJSONWebToken(UserModel userInfo)  
        {  
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_settings.Value.Jwt.Key));  
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);  
  
            var token = new JwtSecurityToken(_settings.Value.Jwt.Issuer, 
											_settings.Value.Jwt.Issuer,  
											null,  
											expires: DateTime.Now.AddMinutes(120),  
											signingCredentials: credentials);  
  
            return new JwtSecurityTokenHandler().WriteToken(token);  
        }  
  
        private UserModel AuthenticateUser(UserModel login)  
        {  
            UserModel user = null;  
  
            //Validate the User Credentials  
            if (login.Username == "TestUser")  
            {  
                user = new UserModel { Username = "Test User", EmailAddress = "test.user@gmail.com" };  
            }  
            return user;  
        }  
    }
}