/* This code is a part of Core.Net
  * Developer Rodrigo Pessoa
  * You can use and share with the binaries of your project
  * If you want the latest version, please use the core.net instead of this part of code
  */
using Beblue.Desafio.Cashback.Generico.Connection;
using Beblue.Desafio.Cashback.Generico.Domain;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Beblue.Desafio.Cashback.Generico.Repository
{
    public class Repository<TClass, TPrimaryKey> : IDisposable, IRepository<TClass, TPrimaryKey> where TClass : class, IModelWithId<TPrimaryKey>, new()
    {

        private ISession _session;
        private readonly NHibernateFactory _nHibernateFactory;
        private readonly string _className;

        public Repository(NHibernateFactory nHibernateFactory)
        {
            _className = typeof(TClass).Name;
            _nHibernateFactory = nHibernateFactory;
        }

        public virtual void Open() => _session = _nHibernateFactory.OpenSession();

        public virtual void Close() => _session?.Close();

        public virtual void Dispose()
        {
            Close();
            _session = null;
        }

        protected virtual ISession GetSession() => _session ??= _nHibernateFactory.OpenSession();

        public virtual IQueryable<TClass> Query() => GetSession().Query<TClass>();

        public virtual IQueryOver<TClass, TClass> QueryOver()
        {
            return GetSession().QueryOver<TClass>();
        }

        public virtual List<TClass> GetAll(Expression<Func<TClass, bool>> where)
        {
            return QueryOver().Where(where).List().ToList();
        }

        public virtual List<TClass> GetAll()
        {
            try
            {
                return QueryOver().List().ToList();
            }
            catch (Exception exception)
            {
                throw new Exception($"{_className} | GetAll", exception);
            }
        }

        public virtual List<TClass> GetPage(Expression<Func<TClass, bool>> where, int currentPage, int pageSize)
        {
            return QueryOver()
                .Where(where)
                .OrderBy(x => x.Id).Asc
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .List()
                .ToList();
        }

        public virtual List<TClass> GetPage(int currentPage, int pageSize)
        {
            return QueryOver()
                .OrderBy(x => x.Id).Asc
                .Skip((currentPage - 1) * pageSize)
                .Take(pageSize)
                .List()
                .ToList();
        }

        public virtual long TotalPages(int pageSize) => Count() / pageSize;

        public virtual long TotalPages(Expression<Func<TClass, bool>> where, int pageSize) => Count(where) / pageSize;

        public virtual long Count() => QueryOver().RowCountInt64();

        public virtual long Count(Expression<Func<TClass, bool>> where) => QueryOver().Where(where).RowCountInt64();

        public virtual TClass Get(Expression<Func<TClass, bool>> where) => Query().Where(where).FirstOrDefault();

        public virtual TClass GetById(TPrimaryKey id) => GetSession().Get<TClass>(id);

        public virtual void Delete(TClass entity)
        {
            GetSession().Delete(entity);
            GetSession().Flush();
        }

        public virtual void Save(TClass entity)
        {
            GetSession().Save(entity);
            GetSession().Flush();
        }

        public virtual void SaveOrUpdate(TClass entity)
        {
            GetSession().SaveOrUpdate(entity);
            GetSession().Flush();
        }

        public virtual void Update(TClass entity)
        {
            GetSession().Update(entity);
            GetSession().Flush();
        }

        public virtual void Merge(TClass entity)
        {
            GetSession().Merge(entity);
            GetSession().Flush();
        }

        public void Flush() => GetSession().Flush();
        
        public virtual ITransaction BeginTransaction() => GetSession().BeginTransaction();

        public virtual void Commit()
        {
            if (GetSession().Transaction == null)
            {
                throw new Exception($"{_className} | There is no transaction to commit!");
            }
            GetSession().Transaction.Commit();
        }

        public virtual void Rollback()
        {
            if (GetSession().Transaction == null)
            {
                throw new Exception($"{_className} | There is no transaction to Abort!");
            }
            GetSession().Transaction.Rollback();
        }
    }
}
