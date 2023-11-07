using Core.Models;
using Core.Specifications;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data
{
    public class SpecificationEvaluator<T> where T : BaseModel
    {
        public static IQueryable<T> GetQuery(IQueryable<T> entityQuery, ISpecification<T> spec)
        {
            var query = entityQuery; //entity query(DbSet<T>). ex: _context.Products. // (DbSet<Product>)

            if (spec.Expression != null)    //caso haja especificações
            {
                query = query.Where(spec.Expression);   //entity query + especificação/expressão. ex: _context.Products(entity query) . Where(spec.Expression == p => p.Id == id);
            }

            if (spec.OrderBy != null)
            {
                query = query.OrderBy(spec.OrderBy);    //entity query + especificação/expressão. ex: _context.Products(entity query) . OrderBy(spec.Expression == p => p.Id == id);
            }

            if (spec.OrderByDescending != null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);    //entity query + especificação/expressão. ex: _context.Products(entity query) . OrderByDescending(spec.Expression == p => p.Id == id);
            }

            if (spec.IsPagingEnabled)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);  //se houver paginação, dá Skip em x produtos e Take "mostra/apresenta/vai buscar à db" x produtos
            }

            query = spec.Includes.Aggregate(query, (current, include) => current.Include(include)); //vai buscar o(s) Include(s) à spec.Includes e agregar/juntar tudo à query, retornando um IQueryable

            return query;
        }
    }
}
