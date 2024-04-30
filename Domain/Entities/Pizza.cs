using System.ComponentModel.DataAnnotations;

namespace PizzaApp.Domain.Entities
{
	public class Pizza
	{
		[Key]
		public string Id { get; set; }
		public string Publisher { get; set; }
		public string? Image_Url { get; set; }
		public string? Title { get; set; }
	}
}