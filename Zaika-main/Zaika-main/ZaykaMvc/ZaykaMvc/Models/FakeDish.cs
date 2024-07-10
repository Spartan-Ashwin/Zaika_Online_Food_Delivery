using System;
using System.ComponentModel.DataAnnotations;

namespace ZaykaMvc.Models
{
	public class FakeDish
	{
        [Required(ErrorMessage = "Dish Name is required")]
        public String? Name { set; get; }

        [Required(ErrorMessage = "Price is required")]
        public decimal? Price { set; get; }

        [Required(ErrorMessage = "Quantity is required")]
        public string? Quantity { set; get; }

        [Required(ErrorMessage = "Image is required")]
        public IFormFile? ImageFile { set; get; }

        public FakeDish()
		{
		}
	}
}

