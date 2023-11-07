using Core.Models;

namespace Core.Specifications
{
    public class ProductWithFiltersCountSpec : BaseSpecification<Product>
    {
        public ProductWithFiltersCountSpec(ProductSpecParams productParams)
            : base(p =>
                (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&   //Se BrandId ñ tiver value(!productParams.BrandId.HasValue)retorna true e continua/ignora, ...   
                (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)         //...caso tenha value retorna falso e executa à direita de || (x.ProductBrandId == productParams.BrandId)
            )
        {  
        }
    }
}
