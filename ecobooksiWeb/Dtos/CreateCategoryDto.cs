using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace ecobooksiWeb.Dtos
{
    public record CreateCategoryDto
    {
        [DisplayName("Category Name")]
        [Required(ErrorMessage = "Category Name is required!")]
        public string CategoryName { get; set; }

        [DisplayName("Display Order")]
        [Range(1, 100, ErrorMessage = "Display Order must be between 1 and 100")]
        [Required(ErrorMessage = "Display Order is required!")]
        public int DisplayOrder { get; set; }
    }
}
