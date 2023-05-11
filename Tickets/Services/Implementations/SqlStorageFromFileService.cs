using Tickets.Services.Interfaces;

namespace Tickets.Services.Implementations
{
    public class SqlStorageFromFileService : ISqlStorageService
    {
        public Dictionary<string, string> Queries { get; private set; } = new Dictionary<string, string>();
        public SqlStorageFromFileService(string queriesPath)
        {
            LoadQueries(queriesPath);
        }
        private void LoadQueries(string queriesPath)
        {
            string[] paths = Directory.GetFiles(queriesPath, "*", SearchOption.AllDirectories);
            foreach (string path in paths)
                using (StreamReader reader = File.OpenText(path))
                    Queries.Add(path, reader.ReadToEnd());
        }
    }
}
