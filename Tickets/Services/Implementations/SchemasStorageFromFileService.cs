using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class SchemasStorageFromFileService : ISchemasStorageService
    {
        public Dictionary<string, string> SchemasData { get; private set; } = new Dictionary<string, string>();
        public SchemasStorageFromFileService(string schemasPath)
        {
            LoadSchemas(schemasPath);
        }
        private void LoadSchemas(string schemasPath)
        {
            string[] paths = Directory.GetFiles(schemasPath, "*", SearchOption.AllDirectories);
            foreach (string path in paths)
                using (StreamReader reader = File.OpenText(path))
                    SchemasData.Add(path, reader.ReadToEnd());
        }
    }
}
