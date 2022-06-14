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
    public class CompController : ControllerBase
    {
        private AppDbContext _App;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        public CompController(AppDbContext App, UserManager<ApplicationUser> applicationUser, SignInManager<ApplicationUser> signInManager)
        {
            _App = App;
            _userManager = applicationUser;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("UploadProjectAsync")]
        public async Task<IActionResult> UploadProjectAsync(ProjectsVM projectVM)
        {
            var RegModel = new compRegsVM();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var Comp = _App.compRegs.Where(c => c.Email == user.Email).FirstOrDefault();
            var compSp = _App.compRegs.Where(b => b.Email == user.Email).FirstOrDefault();
            if (projectVM.DriveLink == null)
            {
                RegModel.Message = "برجاء ادخال رابط درايف الخاص بمشروعك";
                RegModel.Check = false;
                return BadRequest(new { RegModel.Message, RegModel.Check });
            }
            if (compSp.IsProjSent == true)
            {
                RegModel.Message = "تم ارسال المشروع سابقا ، اذا كان هناك مشكلة برجاء لاتواصل معنا";
                RegModel.Check = false;
                return BadRequest(new { RegModel.Message, RegModel.Check });
            }
            var project = new Projects()
            {
            DriveLink = projectVM.DriveLink,
            Name = user.Name,
            Specialization = user.Specialization,
            Email = user.Email,
            CompId = Comp.Id
            };
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(user.IsAccepted == false)
            {
                RegModel.Message = "لم يتم قبول طلبك في المسابقه حتى الان";
                RegModel.Check = false;
                return Ok(new { RegModel.Message, RegModel.Check });
            }
            compSp.IsProjSent = true;
            _App.Add(project);
            _App.SaveChanges();
            user.IsProjSent = true;
            await _userManager.UpdateAsync(user);
            RegModel.Message = "تم ارسال المشروع";
            RegModel.Check = true;
            return Ok(new { RegModel.Message, RegModel.Check });
        }

        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var user = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var profUser = new ProfileCompVM();
            profUser = new ProfileCompVM
            {
                Name = user.Name,
                Email = user.Email,
                age = user.age,
                mobileNumber = user.mobileNumber,
                NationalID = user.NationalID,
                gender = user.gender,
                Specialization = user.Specialization,
                Result = user.Result
            };

            return Ok(profUser);
        }
    }
}
