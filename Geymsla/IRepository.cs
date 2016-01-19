namespace Geymsla
{
    public interface IRepository<T, in TId> : IReadOnlyRepository<T,TId> where T : class
    {
         //Methods for writing items
    }
}