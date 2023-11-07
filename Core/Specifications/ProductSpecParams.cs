namespace Core.Specifications
{
    public class ProductSpecParams
    {
        private const int MaxPageSize = 50; //nº max produtos apresentados por página
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 6;
        public int PageSize 
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize) ? MaxPageSize : value; //caso value para _pageSize seja maior q MaxPageSize(o permitido) retorna MaxPageSize, else retorna value
        }
        public int? BrandId { get; set; }
        public int? TypeId { get; set; }
        public string? Sort { get; set; }

        private string _search;
        public string? Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}
