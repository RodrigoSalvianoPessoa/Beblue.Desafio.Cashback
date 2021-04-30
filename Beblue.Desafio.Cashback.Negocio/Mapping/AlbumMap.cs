using System;
using System.Diagnostics;
using Beblue.Desafio.Cashback.Negocio.Enumeration;
using Beblue.Desafio.Cashback.Negocio.Modelo;
using FluentNHibernate.Mapping;

namespace Beblue.Desafio.Cashback.Negocio.Mapping
{
    public class AlbumMap : ClassMap<Album>
    {
        public AlbumMap()
        {
            try
            {
                Table("ALBUM");

                Id(a => a.Id).Column("ID_ALBUM").GeneratedBy.Identity();
                Map(a => a.Name).Column("NAME");
                Map(a => a.IdSpotify).Column("ID_SPOTIFY");
                Map(a => a.ArtistName).Column("ARTIST_NAME");
                Map(a => a.Genre).Column("GENRE");
                Map(a => a.Value).Column("VALUE");
            }
            catch (Exception exception)
            {
                Trace.TraceError(exception.ToString());
            }
        }
    }
}
