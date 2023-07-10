namespace Tickets.BAL.Options.Interfaces
{
    public interface IOptionStorage<T>
    {
        public IReadOnlyCollection<T> GetAll();
        public T? GetBy(string name);
    }
}
