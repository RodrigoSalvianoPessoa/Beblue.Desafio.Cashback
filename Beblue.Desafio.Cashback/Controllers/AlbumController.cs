using Beblue.Desafio.Cashback.Generico.Connection;
using Beblue.Desafio.Cashback.Negocio.Modelo;
using Beblue.Desafio.Cashback.WebApi.Controllers.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Beblue.Desafio.Cashback.WebApi.Controllers
{
    [Route("api/v1/Albums")]
    [ApiController]
    public class AlbumController : BaseController<Album,int>
    {
        public AlbumController(NHibernateFactory nHibernateFactory) : base(nHibernateFactory)
        {
            
        }
    }
}
