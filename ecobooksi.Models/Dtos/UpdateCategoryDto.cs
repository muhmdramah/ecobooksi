using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ecobooksi.Models.Dtos
{
    public record UpdateCategoryDto
    {
        public int CategoryId { get; set; }

        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category Name is required!")]
        [StringLength(50, ErrorMessage = "Category name cannot exceed 50 characters")]
        public string CategoryName { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        [Required(ErrorMessage = "Display Order is required!")]
        public int DisplayOrder { get; set; }
    }
}
