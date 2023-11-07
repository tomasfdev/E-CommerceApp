using System.ComponentModel.DataAnnotations;

namespace Core.Models
{
    public class ProductType : BaseModel
    {
        [Required, MaxLength(100)]
        public string? Name { get; set; }
        //public virtual ICollection<Product>? Products { get; set; }
    }
}