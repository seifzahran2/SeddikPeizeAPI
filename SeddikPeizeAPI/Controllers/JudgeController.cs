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
    //[Authorize("Judge")]
    [Route("api/[controller]")]
    [ApiController]
    public class JudgeController : ControllerBase
    {
        private AppDbContext _App;
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        public JudgeController(AppDbContext App, UserManager<ApplicationUser> applicationUser, SignInManager<ApplicationUser> signInManager)
        {
            _App = App;
            _userManager = applicationUser;
            _signInManager = signInManager;
        }
        [HttpGet]
        [Route("JudgeDeg")]
        public async Task<IActionResult> JudgeDeg()
        {
            var ProjectModel = new compRegsVM();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var judge = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var users = _App.Projects.Where(c => c.Specialization == judge.Specialization).ToList();
            if(users.Count == 0)
            {
                ProjectModel.Message = "لا يوجد مشاريع لهذا التخصص";
                ProjectModel.Check = false;
                return BadRequest(new { ProjectModel.Message , ProjectModel.Check });
            }
            return Ok(users);
        }
        [HttpGet("compRate/{id}")]
        public async Task<IActionResult> CompRate(int id)
        {
            var ProjectModel = new compRegsVM();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var judge = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var userProj = _App.Projects.Where(c => c.CompId == id).FirstOrDefault();
            if(userProj == null)
            {
                ProjectModel.Message = "لم يتم العثور على هذا المتسابق";
                ProjectModel.Check = false;
                return BadRequest(new { ProjectModel.Message, ProjectModel.Check });
            }
            if(userProj.Result==null)
            {
                userProj.Result = "لما يتم تقييمه";
                return Ok(userProj);
            }
            return Ok(userProj);
        }

        [HttpGet]
        [Route("Profile")]
        public async Task<IActionResult> Profile()
        {
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var judge = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var profUser = new ProfileJudgeVM();
            profUser = new ProfileJudgeVM
            {
                Name = judge.Name,
                Email = judge.Email,
                age = judge.age,
                mobileNumber = judge.mobileNumber,
                NationalID = judge.NationalID,
                gender = judge.gender,
                Specialization = judge.Specialization
            };

            return Ok(profUser);
        }

        [HttpPost("compRate/{id}")]
        public async Task<IActionResult> CompRate(int id, RateVM model)
        {
            var ProjectModel = new compRegsVM();
            var username = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var judge = await _userManager.Users.Where(u => u.UserName == username).FirstOrDefaultAsync();
            var Project = _App.Projects.Where(b => b.CompId == id).FirstOrDefault();
            Project.Result = model.Result;
            var Comp = _App.compRegs.Where(c => c.Id == Project.CompId).FirstOrDefault();
            Comp.Result = model.Result;
            var user = await _userManager.Users.Where(u => u.Email == Comp.Email).FirstOrDefaultAsync();
            user.Result = model.Result;
            _App.Update(Project);
            _App.Update(Comp);
            _App.SaveChanges();
            await _userManager.UpdateAsync(user);
            ProjectModel.Message = "تم التقييم بنجاح";
            ProjectModel.Check = true;
            return BadRequest(new { ProjectModel.Message, ProjectModel.Check });
        }
    }
}
