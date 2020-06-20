using System.Threading.Tasks;
using EsatateApp.Data.Entities;
using EstateApp.web.Models;

namespace EstateApp.web.Interfaces
{
    public interface IAccountsService
    {
        Task<ApplicationUser> CreateUserAsync(LoginModel Model);
    }
}