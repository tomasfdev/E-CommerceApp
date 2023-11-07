using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class Product : BaseModel
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        [Required]
        public string? Description { get; set; }
        public decimal Price { get; set; }
        [Required]
        public string? PictureUrl { get; set; }
        #region Product 1-M ProductType
        public int ProductTypeId { get; set; }
        public virtual ProductType? ProductType { get; set; }
        #endregion
        #region Product 1-M ProductBrand
        public int ProductBrandId { get; set; }
        public virtual ProductBrand? ProductBrand { get; set; }
        #endregion
    }
}
