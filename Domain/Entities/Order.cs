using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.Domain.Entities
{
	public class Order
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string? PizzaId { get; set; }
		[Required]
		public string? PizzaName { get; set; }
		[Required]
		public string? ImageUrl { get; set; }
		[Required]
		public string? UserId { get; set; }
		[Required]
		public string? Address { get; set; }
		[Required]
		public int Price { get; set; }

		[Required]
		public int Quantity { get; set; }
		[Required]
		public int TotalAmount { get; set; }
		public DateTime OrderTime { get; set; }

		//foreign key
		public int CartId { get; set; }
		public Cart Cart { get; set; }
	}
}