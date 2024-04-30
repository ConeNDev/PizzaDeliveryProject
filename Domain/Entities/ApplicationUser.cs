using Microsoft.AspNetCore.Identity;

namespace PizzaApp.Domain.Entities
{
	public class ApplicationUser : IdentityUser
	{
		public string? Name { get; set; }

		public string? Address { get; set; }
	}
}