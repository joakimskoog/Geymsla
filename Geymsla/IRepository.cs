namespace Geymsla
{
    public interface IRepository<T> : IReadOnlyRepository<T> where T : class
    {
         //Methods for writing items
    }
}