using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SeddikPeizeAPI.Models;
using SeddikPeizeAPI.Models.ViewModels;
using SeddikPeizeAPI.Services;
using System.Threading.Tasks;

namespace SeddikPeizeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IUserServices userService;
        public AccountController(IUserServices userService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
            this.userService = userService;
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await userService.RegisterAsync(registerVM);
            if (!result.IsAuthed)
            {
                result.Message = "يوجد حساب بهذا البريد الالكتروني";
                result.Check = false;
                return BadRequest(new { result.Message, result.Check });
            }
                
            return Ok(new { token = result.Token,  Check = true });
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await userService.LoginAsync(loginVM);
            if (!result.IsAuthed)
                return BadRequest(new { result.Message, result.Check });
            return Ok(new { token = result.Token,  Check = true });

        }

        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            var result = await userService.LogoutAsync();
            result.Message = "تم تسجيل الخروج";
            result.Check = true;
            return Ok(new { result.Message, result.Check });
        }
    }
}
