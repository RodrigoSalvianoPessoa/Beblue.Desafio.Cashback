using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Beblue.Desafio.Cashback.Generico.Repository
{
    public interface IRepository<TClass, TPrimaryKey> where TClass : class, new()
    {
        void Open();
        void Close();

        IQueryable<TClass> Query();
        IQueryOver<TClass, TClass> QueryOver();

        TClass Get(Expression<Func<TClass, bool>> where);
        TClass GetById(TPrimaryKey id);
        List<TClass> GetAll();

        List<TClass> GetPage(int currentPage, int pageSize);
        long TotalPages(int pageSize);
        long Count();
        long Count(Expression<Func<TClass, bool>> where);

        void Delete(TClass entity);
        void Save(TClass entity);
        void Update(TClass entity);
        void Merge(TClass entity);
    }
}
