using Core.Models;

namespace Core.Specifications
{
    public class ProductsWithTypesAndBrandsSpec : BaseSpecification<Product>
    {
        public ProductsWithTypesAndBrandsSpec(ProductSpecParams productParams) 
            : base(p =>
                (string.IsNullOrEmpty(productParams.Search) || p.Name.ToLower().Contains(productParams.Search)) &&
                (!productParams.BrandId.HasValue || p.ProductBrandId == productParams.BrandId) &&   //Se BrandId ñ tiver value(!productParams.BrandId.HasValue)retorna true e continua/ignora, ...   
                (!productParams.TypeId.HasValue || p.ProductTypeId == productParams.TypeId)         //...caso tenha value retorna falso e executa à direita de || (x.ProductBrandId == productParams.BrandId)
            )
        {
            AddInclude(p => p.ProductType);
            AddInclude(p => p.ProductBrand);
            AddOrderBy(s => s.Name);
            ApplyPaging(productParams.PageSize * (productParams.PageIndex - 1), productParams.PageSize);

            if (!string.IsNullOrEmpty(productParams.Sort))
            {
                switch (productParams.Sort)
                {
                    case "priceAsc":
                        AddOrderBy(p => p.Price);
                        break;
                    case "priceDesc":
                        AddOrderByDescending(p => p.Price);
                        break;
                    default:
                        AddOrderBy(n => n.Name);
                        break;
                }
            }
        }

        public ProductsWithTypesAndBrandsSpec(int id) : base(p => p.Id == id)  //base(p => p.Id == id) é a expressão que vai substituir a "expression" na BaseSpecification.cs, no ctor!! (Expression<Func<T, bool>> ->expression<-!!)
                                                                               //Pq esta class deriva de uma Spec do tipo Product "ProductsWithBrandsTypesSpec : BaseSpecification<Product>"
                                                                               //(p => p.Id == id) --> Obtem o produto que corresponde ao id passado como parâmetro
        {
            AddInclude(p => p.ProductType);                                   //incluindo ProductType and ProductBrand
            AddInclude(p => p.ProductBrand);
        }
    }
}
