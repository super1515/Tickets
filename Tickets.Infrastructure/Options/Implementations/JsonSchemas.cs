using Tickets.Infrastructure.Options.Interfaces;
namespace Tickets.WebAPI.Options.Implementations
{
    public record JsonSchema(string Name, string Schema);

    public class JsonSchemas : IOptionStorage<JsonSchema>
    {
        private IReadOnlyCollection<JsonSchema> _jsonSchemas;
        public IReadOnlyCollection<JsonSchema> GetAll()
        {
            return _jsonSchemas;
        }
        public JsonSchema? GetBy(string name)
        {
            return _jsonSchemas.FirstOrDefault(t =>
                t.Name.Contains(name, StringComparison.CurrentCultureIgnoreCase));
        }
        public void LoadSchemas(string queriesPath)
        {
            var jsonSchemas = new List<JsonSchema>();
            string[] paths = Directory.GetFiles(queriesPath, "*", SearchOption.AllDirectories);
            foreach (string path in paths)
                using (StreamReader reader = File.OpenText(path))
                    jsonSchemas.Add(new JsonSchema(path, reader.ReadToEnd()));
            _jsonSchemas = jsonSchemas;
        }
    }
}
