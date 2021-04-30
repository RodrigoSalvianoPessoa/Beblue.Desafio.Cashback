using Beblue.Desafio.Cashback.Generico.Domain;

namespace Beblue.Desafio.Cashback.Negocio.Modelo
{
    public class Album : IModelWithId<int>
    {
        public virtual int Id { get; set; }

        public virtual string Name { get; set; }

        public virtual string IdSpotify { get; set; }

        public virtual string ArtistName { get; set; }

        public virtual string Genre { get; set; }

        public virtual decimal Value { get; set; }
    }
}
