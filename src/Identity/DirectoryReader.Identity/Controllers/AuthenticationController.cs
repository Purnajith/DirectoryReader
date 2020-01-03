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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace DirectoryReader.Identity.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
		//https://jasonwatmore.com/post/2019/10/11/aspnet-core-3-jwt-authentication-tutorial-with-example-api
		//https://www.c-sharpcorner.com/article/jwt-json-web-token-authentication-in-asp-net-core/
		//https://www.codeproject.com/Articles/5160941/ASP-NET-CORE-Token-Authentication-and-Authorizatio
		//https://fullstackmark.com/post/19/jwt-authentication-flow-with-refresh-tokens-in-aspnet-core-web-api
		//https://garywoodfine.com/asp-net-core-2-2-jwt-authentication-tutorial/
		#region Members

		private readonly IOptionsSnapshot<AppSettings>		_settings;

		#endregion

		private IConfiguration _config;  
  
        public AuthenticationController(IConfiguration config, IOptionsSnapshot<AppSettings> _settings)  
        {  
            this._config = config;
			this._settings = _settings;
			
        }  

		public IActionResult Get()
		{
			return Ok(new List<string>(){ "Test1" });
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
                response = Ok(new { token = tokenString });  
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
            if (login.Username == "Jignesh")  
            {  
                user = new UserModel { Username = "Jignesh Trivedi", EmailAddress = "test.btest@gmail.com" };  
            }  
            return user;  
        }  
    }
}