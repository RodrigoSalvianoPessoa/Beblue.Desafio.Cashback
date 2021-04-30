using Beblue.Desafio.Cashback.Generico.Connection;
using Beblue.Desafio.Cashback.Generico.Domain;
using Beblue.Desafio.Cashback.Generico.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Beblue.Desafio.Cashback.WebApi.Controllers.Generic
{
    [ApiController]
    public class BaseController<TClass, TPrimaryKey> : ControllerBase
        where TClass : class, IModelWithId<TPrimaryKey>, new()
    {
        protected readonly Repository<TClass, TPrimaryKey> repository;
        private readonly string className;

        public BaseController(NHibernateFactory nhibernateFactory)
        {
            this.className = typeof(TClass).Name;
            repository = new Repository<TClass, TPrimaryKey>(nhibernateFactory);
        }

        /// <summary>
        /// Returns all items
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public virtual ActionResult<IList<TClass>> GetAll(int actualPage = 0, int pageSize = 10)
        {
            try
            {
                return actualPage == 0 ? repository.GetAll() : repository.GetPage(actualPage, pageSize);
            }
            catch (Exception exception)
            {
                throw new Exception($"{className} | GetAll | Page: {actualPage} | Size: {pageSize}", exception);
            }
        }

        /// <summary>
        /// Returns an item by the key
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("{id}")]
        public virtual ActionResult<TClass> GetById([FromRoute] TPrimaryKey id)
        {

            var obj = new TClass();
            try
            {
                obj = repository.GetById(id);
            }
            catch (Exception exception)
            {
                throw new Exception($"{className} | GetById", exception);
            }

            return obj;
        }
        /// <summary>
        /// Save a new item
        /// </summary>
        /// <param name="item"></param>
        [HttpPost]
        public virtual void Post([FromBody] TClass item)
        {
            try
            {
                repository.Save(item);
            }
            catch (Exception exception)
            {
                throw new Exception($"{className} | Post | {item}", exception);
            }
        }
        /// <summary>
        /// Change an item
        /// </summary>
        /// <param name="item"></param>
        [HttpPut]
        public virtual void Put([FromBody] TClass item)
        {
            try
            {
                repository.SaveOrUpdate(item);
            }
            catch (Exception exception)
            {
                throw new Exception($"{className} | Put | {item}", exception);
            }
        }

        /// <summary>
        /// Deletes an item by Id
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete]
        [Route("{id}")]
        public virtual void DeleteById([FromRoute] TPrimaryKey id)
        {
            try
            {
                var item = repository.GetById(id);
                repository.Delete(item);
            }
            catch (Exception exception)
            {
                throw new Exception($"{className} | DeleteById | {id}", exception);
            }
        }
    }
}
