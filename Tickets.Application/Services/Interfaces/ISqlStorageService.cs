namespace Tickets.Application.Services.Interfaces
{
    public interface ISqlStorageService
    {
        Dictionary<string, string> Queries { get; }
    }
}
