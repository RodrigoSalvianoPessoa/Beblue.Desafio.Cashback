using Beblue.Desafio.Cashback.Generico.Connection;
using Beblue.Desafio.Cashback.Generico.Repository;
using Beblue.Desafio.Cashback.Negocio.Modelo;
using System.Collections.Generic;

namespace Beblue.Desafio.Cashback.Negocio.Repository
{
    public class AlbumRepository : Repository<Album,int>, IAlbumRepository
    {
        public AlbumRepository(NHibernateFactory nHibernateFactory) : base(nHibernateFactory)
        {
        }

        IList<Album> IAlbumRepository.GetAllByGenreOrderNameAsc(string genre)
        {
            return GetAll(a => a.Genre == genre);
        }
    }
}
