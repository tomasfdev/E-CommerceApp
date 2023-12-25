using Core.Interfaces;
using Core.Models;
using System.Collections;

namespace Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _context;
        private Hashtable _repositories;

        public UnitOfWork(AppDbContext context)
        {
            _context = context;
        }

        public async Task<int> Complete()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }

        public IGenericRepository<TEntity> Repository<TEntity>() where TEntity : BaseModel
        {
            if (_repositories is null) _repositories = new Hashtable(); //check if have anything inside repo/hashtable ?? create hashtable

            var entityName = typeof(TEntity).Name;    //get the name/type of the TEntity

            if (!_repositories.ContainsKey(entityName)) //check if hashtable contains an entry for the specific "entityName"
            {
                var repositoryType = typeof(GenericRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);   //creates an instance/repository of the "repositoryType"

                _repositories.Add(entityName, repositoryInstance);  //add new entry into hashtable, hashtable is going to store all of the repos in use inside the UnitOfWork.cs
            }

            return (IGenericRepository<TEntity>)_repositories[entityName];
            //wherever we use this method we're going to give the type/name of the entity(TEntity), is going to check if there's already a hashtable created.(if (_repositories is null) _repositories = new Hashtable())
            //check the entity name/type that we passed.(var entityName = typeof(TEntity).Name;). ex: "entityName" passed is "Product" GenericRepository<Product>
            //check if our "_repositories" hastable already contains a repo with that particular name/type.(if (!_repositories.ContainsKey(entityName)))
            //if it doesn't, creates a "repositoryType" of GenericRepository.(var repositoryType = typeof(GenericRepository<>);). ex: GenericRepository<Product>
            //and then generate/create an instance of that repo(ex: GenericRepository<Product>) and pass the "_context" that we're going to get from UnitOfWork.(var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(TEntity)), _context);)
            //and finaly we add that repo(ex: GenericRepository<Product>) to the hashtable(_repositories.Add(entityName, repositoryInstance);)
            //and return it.(return (IGenericRepository<TEntity>)_repositories[entityName];)
        }
    }
}
