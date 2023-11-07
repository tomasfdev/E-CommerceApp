﻿using System.Linq.Expressions;

namespace Core.Specifications
{
    public class BaseSpecification<T> : ISpecification<T>
    {
        public BaseSpecification()
        {
        }

        public BaseSpecification(Expression<Func<T, bool>> expression)  //expression is when we want to get a product with a specific ID. ex:(p => p.Id == id)
        {
            Expression = expression;
        }

        public Expression<Func<T, bool>> Expression { get; }    //where clause

        public List<Expression<Func<T, object>>> Includes { get; } = new List<Expression<Func<T, object>>>();   //inicia com uma lista vazia

        public Expression<Func<T, object>> OrderBy { get; private set; }

        public Expression<Func<T, object>> OrderByDescending { get; private set; }

        public int Take { get; private set; }

        public int Skip { get; private set; }

        public bool IsPagingEnabled { get; private set; }

        //protected: estes methods(AddInclude,AddOrderBy,AddOrderByDescending) só podem ser acedidos nesta Class(BaseSpecification) e nas classes que derivem desta, "child classes"
        protected void AddInclude(Expression<Func<T, object>> includeExpression)
        {
            Includes.Add(includeExpression);
        }

        protected void AddOrderBy(Expression<Func<T, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }

        protected void AddOrderByDescending(Expression<Func<T, object>> orderByDescendingExpression)
        {
            OrderByDescending = orderByDescendingExpression;
        }

        protected void ApplyPaging(int skip, int take)
        {
            Skip = skip;
            Take = take;
            IsPagingEnabled = true;
        }
    }
}
