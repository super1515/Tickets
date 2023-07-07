using Tickets.Application.Utility;
using Tickets.Infrastructure.Options.Interfaces;
namespace Tickets.WebAPI.Options.Implementations
{
    public class SqlQueries : IOptionStorage<FileData>
    {
        private IReadOnlyCollection<FileData> _sqlQueries;
        public SqlQueries(IReadOnlyCollection<FileData> files)
        {
            _sqlQueries = files;
        }
        public IReadOnlyCollection<FileData> GetAll()
        {
            return _sqlQueries;
        }
        public FileData? GetBy(string name)
        {
            return _sqlQueries.FirstOrDefault(t =>
                t.FullPath.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
