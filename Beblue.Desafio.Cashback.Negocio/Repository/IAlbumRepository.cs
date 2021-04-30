using Beblue.Desafio.Cashback.Negocio.Enumeration;
using Beblue.Desafio.Cashback.Negocio.Modelo;
using System.Collections.Generic;

namespace Beblue.Desafio.Cashback.Negocio.Repository
{
    public interface IAlbumRepository
    {
        IList<Album> GetAllByGenreOrderNameAsc(string genre);
    }
}
