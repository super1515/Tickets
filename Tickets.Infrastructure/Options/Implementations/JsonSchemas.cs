using Tickets.Application.Utility;
using Tickets.Infrastructure.Options.Interfaces;
namespace Tickets.WebAPI.Options.Implementations
{
    public class JsonSchemas : IOptionStorage<FileData>
    {
        private IReadOnlyCollection<FileData> _jsonSchemas;
        public JsonSchemas(IReadOnlyCollection<FileData> files)
        {
            _jsonSchemas = files;
        }
        public IReadOnlyCollection<FileData> GetAll()
        {
            return _jsonSchemas;
        }
        public FileData? GetBy(string name)
        {
            return _jsonSchemas.FirstOrDefault(t =>
                t.FullPath.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }
    }
}
