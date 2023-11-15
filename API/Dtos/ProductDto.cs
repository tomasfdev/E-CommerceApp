using Core.Models;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class ProductDto
    {
        [Required]
        public int Id { get; set; }
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string? PictureUrl { get; set; }
        [Required]
        public string? ProductTypeName { get; set; }
        [Required]
        public string? ProductBrandName { get; set; }
    }
}
