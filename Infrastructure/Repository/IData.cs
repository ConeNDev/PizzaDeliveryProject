using PizzaApp.Domain.Entities;
using System.Security.Claims;

namespace PizzaApp.Infrastructure.Repository
{
    public interface IData
    {
        Task<ApplicationUser> GetUser(ClaimsPrincipal claims);
    }
}
