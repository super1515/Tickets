using Microsoft.EntityFrameworkCore;

namespace Tickets.Models
{
    [Index(nameof(TicketNumber), nameof(SerialNumber), IsUnique = true)]
    public class Segments
    {
        public int? ID { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public string? Patronymic { get; set; }
        public string? DocType { get; set; }
        public string? DocNumber { get; set; }
        public DateTimeOffset Birthdate { get; set; }
        public string? Gender { get; set; }
        public string? PassengerType { get; set; }
        public string? TicketNumber { get; set; }
        public string? SerialNumber { get; set; }
        public int TicketType { get; set; }
        public string? OperationType { get; set; }
        public DateTimeOffset OperationTime { get; set; }
        public string? OperationPlace { get; set; }
        public string? AirlineCode { get; set; }
        public int FlightNum { get; set; }
        public string? DepartPlace { get; set; }
        public DateTimeOffset DepartDatetime { get; set; }
        public string? ArrivePlace { get; set; }
        public DateTimeOffset ArriveDatetime { get; set; }
        public string? PnrId { get; set; }
        public short OperationTimeTimezone { get; set; }
        public short DepartDatetimeTimezone { get; set; }
        public short ArriveDatetimeTimezone { get; set; }
    }
}
