using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PizzaApp.Domain.Entities
{
	public class Cart
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		public List<Order> Orders { get; set; } = new List<Order>();
		public DateTime OrderTime { get; set; }
		public int FinalPrice { get; set; }
		public int Quantity { get; set; }

		public void AddOrder(Order order)
		{
			Orders.Add(order);
			CalculateTotals();
		}
		public void RemoveOrder(int orderId)
		{
			var order = Orders.FirstOrDefault(o => o.Id == orderId);
			if (order != null)
			{
				Orders.Remove(order);
				CalculateTotals();
			}
		}
		public int TotalPrice()
		{
			return Orders.Sum(order => order.TotalAmount);
		}
		public void Clear()
		{
			Orders.Clear();
			CalculateTotals();
		}
		public void CalculateTotals()
		{
			Quantity = Orders.Count();
			FinalPrice = Orders.Sum(order => order.TotalAmount);
		}
	}
}