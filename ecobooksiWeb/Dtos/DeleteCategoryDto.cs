namespace ecobooksiWeb.Dtos
{
    public record DeleteCategoryDto
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public int DisplayOrder { get; set; }
    }
}
