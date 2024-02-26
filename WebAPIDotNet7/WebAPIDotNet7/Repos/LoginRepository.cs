using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using WebAPIDotNet7.Context;
using WebAPIDotNet7.Dtos.DtoRequest;
using WebAPIDotNet7.Dtos.DtoResponse;
using WebAPIDotNet7.Interfaces.IRepos;
using WebAPIDotNet7.Models;

namespace WebAPIDotNet7.Repos
{
    public class LoginRepository : ILoginRepository
    {
        private readonly ApplicationDBContext _dbContext;
        private readonly IConfiguration _configuration;

        public LoginRepository(ApplicationDBContext dbContext, IConfiguration configuration)
        {
            _dbContext = dbContext;
            _configuration = configuration;
        }

        public async Task<RegistrationResponse> Registration(RegistrationRequest request)
        {
            try
            {
                var isUserEmailexist = await _dbContext.User.AnyAsync(x => x.IsActive == true && x.Email == request.Email);
                if (isUserEmailexist == true)
                {
                    return new RegistrationResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "User Email has already registered!"
                    };
                }

                CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

                User user = new User
                {
                    Name = request.Name,
                    Email = request.Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedDate = DateTime.Now,
                    IsActive = true
                };
                await _dbContext.User.AddAsync(user);
                await _dbContext.SaveChangesAsync();

                user.CreatedBy = user.Id;
                await _dbContext.SaveChangesAsync();

                var token = CreateToken(user);

                return new RegistrationResponse
                {
                    UserId = user.Id,
                    Token = token,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "User Registration Success."
                };
            }
            catch (Exception ex)
            {
                return new RegistrationResponse
                {

                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };

            }

        }

        public async Task<LoginResponse> Login(LoginRequest request)
        {
            try
            {
                var user = await _dbContext.User.Where(x => x.Email == request.Email && x.IsActive == true).FirstOrDefaultAsync();
                if (user is null)
                {
                    return new LoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Invalid Email!"
                    };
                }

                var isAuthorize = VerifyPasswordHash(request.Password, user.PasswordHash, user.PasswordSalt);
                if (!isAuthorize)
                {
                    return new LoginResponse
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        Message = "Wrong Password"
                    };
                }

                return new LoginResponse
                {
                    UserId = user.Id,
                    Token = CreateToken(user),
                    Name = user.Name,
                    Email = user.Email,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Success"
                };

            }
            catch (Exception ex)
            {

                return new LoginResponse
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message
                };
            }
        }

        #region privateMethod
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA256(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.RoleId.ToString()),
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("JWT:SecretKey").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration.GetSection("JWT:ValidIssuer").Value,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var jwt = new JwtSecurityTokenHandler().CreateToken(tokenDescriptor);

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);

            return token;
        }
        #endregion
    }
}
