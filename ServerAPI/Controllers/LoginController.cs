using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServerAPI.Models;
using System.Data.SqlClient;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;
        public LoginController(IConfiguration configuration)
        {

            _config = configuration;

        }

        private Users AuthenticateUser(Users user)
        {
            Users _user = null;

            if (user.Username == "admin" && user.Password == "12345") {
                _user = new Users { Username = "Dastine" };
            }
            return _user;
        }

        private string GenerateTken(Users users)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Jwt:Issuer"], _config["Jwt:Audience"], null,
                expires: DateTime.Now.AddMinutes(1),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [AllowAnonymous]
        [HttpPost]

        public IActionResult Login(Users user)
        {
            IActionResult response = Unauthorized();
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("crudCS").ToString()))
            {
                connection.Open();

                string query = "SELECT * FROM Users_cred WHERE Username = @Username AND Password = @Password AND IsActive = 1";
                using (SqlCommand cmd = new SqlCommand(query, connection))
                {
                    cmd.Parameters.AddWithValue("@Username", user.Username);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            var token = GenerateTken(user);
                            response = Ok(new { token = token });
                        }
                    }
                }
            }
            return response;
        }


    }
}
