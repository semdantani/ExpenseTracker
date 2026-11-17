using ExpenseTracker.DTOs;
using Microsoft.AspNetCore.Mvc;
using ExpenseTracker.Model;
using Microsoft.AspNetCore.Identity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using ExpenseTracker.Interfaces;


namespace ExpenseTracker.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository _authRepo;
        private readonly IConfiguration _config;
        private readonly PasswordHasher<User> _passwordHasher = new();

        public AuthController(IAuthRepository authRepo, IConfiguration config   )
        {
            _authRepo = authRepo;
            _config = config;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existingUser= await _authRepo.GetByEmailAsync(dto.Email);
            if(existingUser!=null)
            {
                return BadRequest("Email Already Registered");
            }
            var user = new User
            {
               FullName=dto.FullName, Email=dto.Email

            };
            user.PasswordHash = _passwordHasher.HashPassword(user, dto.Password);

            await _authRepo.AddAsync(user);


            return Ok("User Registered SuccessFully");
            
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await _authRepo.GetByEmailAsync(dto.Email);
            if (user == null)
            {
                return BadRequest("Invalid Email Or Password");
            }
            var result=_passwordHasher.VerifyHashedPassword(user,user.PasswordHash,dto.Password);
            if (result == PasswordVerificationResult.Failed)
            {
                return BadRequest("Invalid Email Or Password");
            }
            

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            claims.Add(new Claim(ClaimTypes.Name, user.FullName));
            claims.Add(new Claim(ClaimTypes.Email, user.Email));

            var jwtSettings = _config.GetSection("Jwt");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]!));

            var algorithm=SecurityAlgorithms.HmacSha256;

            var credentials=new SigningCredentials(key, algorithm);

            var duration =Convert.ToDouble(jwtSettings["DurationInMinutes"]);

            var token = new JwtSecurityToken(jwtSettings["Issuer"], jwtSettings["Audience"],claims,expires:DateTime.UtcNow.AddMinutes(duration),signingCredentials:credentials);

            var tokenHandlerr = new JwtSecurityTokenHandler();
            var tokenstr=tokenHandlerr.WriteToken(token);

            return Ok(new
            {
                Token = tokenstr,
            });
        }


        [Authorize]
        [HttpGet("profile")]
        public IActionResult Get()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            return Ok(userId);
        }
    }
}
