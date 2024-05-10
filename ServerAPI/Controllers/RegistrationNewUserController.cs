using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerAPI.Models;
using System.Data.SqlClient;

namespace ServerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationNewUserController : ControllerBase
    {
        private readonly IConfiguration _config;

        public RegistrationNewUserController(IConfiguration configuration)
        {
            _config = configuration; 
        }

        [HttpPost]
        [Route("registration")]

        public IActionResult RegistrationNewUser(RegistrationNewUser registrationNewUser)
        {

            IActionResult response = Unauthorized();
            using (SqlConnection connection = new SqlConnection(_config.GetConnectionString("crudCS").ToString()))
            {
                connection.Open();

                using (SqlCommand cmd = new SqlCommand("INSERT INTO Users_cred (Username, Password, Email, IsActive) VALUES (@Username, @Password, @Email, @IsActive)", connection))
                {
                    cmd.Parameters.AddWithValue("@Username", registrationNewUser.Username);
                    cmd.Parameters.AddWithValue("@Password", registrationNewUser.Password);
                    cmd.Parameters.AddWithValue("@Email", registrationNewUser.Email);
                    cmd.Parameters.AddWithValue("@IsActive", registrationNewUser.IsActive);

                    int executeNonQuery = cmd.ExecuteNonQuery();
                    if (executeNonQuery > 0)
                    {
                        response = Ok("Data Inserted");
                    }
                        return response;
                }
            }
        }
    }
}
