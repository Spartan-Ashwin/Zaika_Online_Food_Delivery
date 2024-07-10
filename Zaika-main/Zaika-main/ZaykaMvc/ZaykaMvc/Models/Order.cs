using System;
using System.ComponentModel.DataAnnotations;

namespace ZaykaMvc.Models
{
	public class Order
	{
		[Key]
		public int Id { set; get; }
        public string? Username { set; get; }
        public string? DishName { set; get; }
        public decimal? Price { set; get; }
        public int Quantity { set; get; }
        public Order()
		{
		}
	}
}

