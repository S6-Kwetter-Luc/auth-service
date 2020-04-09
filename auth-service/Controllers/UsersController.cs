using System.Threading.Tasks;
using auth_service.Helpers;
using auth_service.Models;
using auth_service.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace auth_service.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] AuthenticateModel view)
        {
            var user = await _userService.Authenticate(view.Email, view.Password);

            if (user == null)
                return BadRequest(new {message = "Username or password is incorrect"});

            return Ok(user);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterModel view)
        {
            try
            {
                // create user
                _userService.Create(view.Name, view.Email, view.Password);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(new {message = ex.Message});
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _userService.GetAll());
        }

        [HttpPost("fill")]
        public async Task<IActionResult> Fill()
        {
            await _userService.Fill();
            return Ok();
        }
    }
}