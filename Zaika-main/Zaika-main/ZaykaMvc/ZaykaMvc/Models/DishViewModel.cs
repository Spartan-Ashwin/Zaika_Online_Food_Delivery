using System;
namespace ZaykaMvc.Models
{
    public class DishViewModel
    {
        public int Id { set; get; }
        public int Index { set; get; }
        public string? Name { get; set; }
        public decimal? Price { get; set; }
        public int Quantity { get; set; }

        public override bool Equals(object? obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            DishViewModel other = (DishViewModel)obj;
            return Id == other.Id;
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
    
}

