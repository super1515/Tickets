namespace Tickets.Application.Utility
{
    public record FileData(string FullPath, string Data);
    public class FileUtility
    {
        public static IReadOnlyCollection<FileData> GetAllFilesData(string basePath)
        {
            var data = new List<FileData>();
            string[] paths = Directory.GetFiles(basePath, "*", SearchOption.AllDirectories);
            foreach (string path in paths)
                using (StreamReader reader = File.OpenText(path))
                    data.Add(new FileData(path, reader.ReadToEnd()));
            return data;
        }
    }
}
