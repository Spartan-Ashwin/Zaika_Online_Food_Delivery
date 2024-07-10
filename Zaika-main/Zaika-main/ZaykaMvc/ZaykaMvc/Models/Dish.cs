using System;
using System.ComponentModel.DataAnnotations;
namespace ZaykaMvc.Models
{
	public class Dish
	{
		[Key]
		public int Id { set; get; }
        public String? Name { set; get; }
        public decimal? Price { set; get; }
        public string? Quantity { set; get; }
        public String? ImageFileName { set; get; }

        public Dish()
		{
		}

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            Dish other = (Dish)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}

