using Tickets.Infrastructure.Options.Interfaces;
/*
* 
* Сервис для хранения SQL запросов
* 
*/
namespace Tickets.WebAPI.Options.Implementations
{
    public record SqlQuery(string Name, string Query);

    public class SqlQueries : IOptionStorage<SqlQuery>
    {
        private IReadOnlyCollection<SqlQuery> _sqlQueries;
        public IReadOnlyCollection<SqlQuery> GetAll()
        {
            return _sqlQueries;
        }
        public SqlQuery? GetBy(string name)
        {
            return _sqlQueries.FirstOrDefault(t =>
                t.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }
        public void LoadQueries(string queriesPath)
        {
            var sqlQueries = new List<SqlQuery>();
            string[] paths = Directory.GetFiles(queriesPath, "*", SearchOption.AllDirectories);
            foreach (string path in paths)
                using (StreamReader reader = File.OpenText(path))
                    sqlQueries.Add(new SqlQuery(path, reader.ReadToEnd()));
            _sqlQueries = sqlQueries;
        }
    }
}
