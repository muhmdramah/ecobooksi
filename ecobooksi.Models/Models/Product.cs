using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ecobooksi.Models.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }

        [DisplayName("Product Title")]
        [Required(ErrorMessage = "Product title is required!")]
        [StringLength(50, ErrorMessage = "Product title cannot exceed 50 characters")]
        public string Title { get; set; }
        public string Description { get; set; }

        [Required(ErrorMessage = "ISBN is required!")]
        public string ISBN { get; set; }


        [Required(ErrorMessage = "Auhtor is required!")]
        public string Author { get; set; }

        [Display(Name = "List Price")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        [Required(ErrorMessage = "List Price is required!")]
        public double ListPrice{ get; set; }

        [Display(Name = "Price for 1-50")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        [Required(ErrorMessage = "Price is required!")]
        public double Price { get; set; }

        [Display(Name = "Price for 50+")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        [Required(ErrorMessage = "Price is required!")]
        public double PriceFifty { get; set; }


        [Display(Name = "Price for 100+")]
        [Range(1, 10000, ErrorMessage = "Price must be between 1 and 10000")]
        [Required(ErrorMessage = "Price is required!")]
        public double PriceHundred { get; set; }


        [ForeignKey(nameof(Category))]
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
