using System.Linq.Expressions;

namespace Core.Specifications
{
    public interface ISpecification<T> 
    {
        Expression<Func<T, bool>> Expression { get; } //Iqueryable, uma expression, que tem uma function que recebe um T(model/type) e retorna um bool. ex: (p => p.Id == id)
        List<Expression<Func<T, object>>> Includes { get; }  //Lista do tipo expression, que leva uma function que recebe um T(model/type) e retorna um obj, para Includes querys
        Expression<Func<T, object>> OrderBy { get; }    //OderBy querys/clauses
        Expression<Func<T, object>> OrderByDescending { get; }  //OderByDescending querys/clauses
        int Take { get; }
        int Skip { get; }
        bool IsPagingEnabled { get; }
    }
}
