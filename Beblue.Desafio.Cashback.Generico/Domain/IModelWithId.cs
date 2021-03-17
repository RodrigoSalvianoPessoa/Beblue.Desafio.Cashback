namespace Beblue.Desafio.Cashback.Generico.Domain
{
    public interface IModelWithId<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}