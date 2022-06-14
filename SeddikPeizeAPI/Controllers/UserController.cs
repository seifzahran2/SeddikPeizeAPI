using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SeddikPeizeAPI.Data;
using SeddikPeizeAPI.Models;
using SeddikPeizeAPI.Models.ViewModels;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SeddikPeizeAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private AppDbContext _App;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        public UserController(AppDbContext App, UserManager<ApplicationUser> applicationUser, SignInManager<ApplicationUser> signInManager)
        {
            _App = App;
            _userManager = applicationUser;
            _signInManager = signInManager;
        }
        [HttpPost]
        [Route("CompRegs")]
        public async Task<IActionResult> CompRegs(compRegsVM compRegVM)
        {
            var RegModel = new compRegsVM();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            if (user.IsRegSent == true)
            {
                RegModel.Message = "تم التسجيل في المسابقه سابقا";
                RegModel.Check = false;
                return BadRequest(new { RegModel.Message, RegModel.Check });
            }


            var compReg = new compRegs()
            {
                FullName = compRegVM.FullName,
                Email = compRegVM.Email,
                Address = compRegVM.Tittle,
                BankAccount = compRegVM.BankAccount,
                NationalId = compRegVM.NationalId,
                Age = compRegVM.Age,
                Gender = compRegVM.Gender,
                Specialization = compRegVM.Specialization,
                MobileNumber = compRegVM.MobileNumber,
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (user.Email != compRegVM.Email || user.NationalID != compRegVM.NationalId)
            {
                RegModel.Message = "يجب ان يكون الرقم القومي و البريد الالكتروني مطابق لتسجيل الدخول ، راجع صفحتك الشخصية.";
                RegModel.Check = false;
                return BadRequest(new { RegModel.Message, RegModel.Check });
            }

            _App.compRegs.Add(compReg);
            _App.SaveChanges();
            user.IsRegSent = true;
            user.Specialization = compRegVM.Specialization;
            await _userManager.UpdateAsync(user);
            RegModel.Message = "تم التسجيل بنجاح";
            RegModel.Check = true;
            return Ok(new { RegModel.Message, RegModel.Check });
        }

        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var profUser = new ProfileUserVM();
            profUser = new ProfileUserVM
            {
                Name = user.Name,
                Email = user.Email,
                age = user.age,
                mobileNumber = user.mobileNumber,
                NationalID = user.NationalID,
                gender = user.gender,
            };

            return Ok(profUser);
        }
    }
}
