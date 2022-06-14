using SeddikPeizeAPI.Models.ViewModels;
using System.Threading.Tasks;

namespace SeddikPeizeAPI.Services
{
    public interface IUserServices
    {
        Task<AuthVM> RegisterAsync(RegisterVM model);
        Task<AuthVM> LoginAsync(LoginVM model);
        Task<AuthVM> LogoutAsync();
    }
}
