using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using WebAPIDotNet7.Dtos.DtoRequest;
using WebAPIDotNet7.Interfaces.IRepos;
using WebAPIDotNet7.Interfaces.IServices;

namespace WebAPIDotNet7.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly ILoginRepository _loginRepository;
        private readonly ILoginService _loginService;

        public LoginController(ILoginRepository loginRepository, ILoginService loginService)
        {
            _loginRepository = loginRepository;
            _loginService = loginService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration(RegistrationRequest request)
        {
            try
            {
                var response = await _loginRepository.Registration(request);
                return StatusCode(response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            try
            {
                var response = await _loginRepository.Login(request);
                return StatusCode(response.StatusCode, response);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
