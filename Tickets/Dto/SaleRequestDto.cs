using System.Text.Json.Serialization;

namespace Tickets.Dto
{
    public class SaleRequestDto
    {
        [JsonPropertyName("operation_type")]
        public string? OperationType { get; set; }
        [JsonPropertyName("operation_time")]
        public DateTimeOffset OperationTime { get; set; }
        [JsonPropertyName("operation_place")]
        public string? OperationPlace { get; set; }
        [JsonPropertyName("passenger")]
        public Passenger Passenger { get; set; }
        [JsonPropertyName("routes")]
        public IEnumerable<Route> Routes { get; set; }
    }
    public class Passenger
    {
        [JsonPropertyName("name")]
        public string? Name { get; set; }
        [JsonPropertyName("surname")]
        public string? Surname { get; set; }
        [JsonPropertyName("patronymic")]
        public string? Patronymic { get; set; }
        [JsonPropertyName("doc_type")]
        public string? DocType { get; set; }
        [JsonPropertyName("doc_number")]
        public string? DocNumber { get; set; }
        [JsonPropertyName("birthdate")]
        public string? Birthdate { get; set; }
        [JsonPropertyName("gender")]
        public string? Gender { get; set; }
        [JsonPropertyName("passenger_type")]
        public string? PassengerType { get; set; }
        [JsonPropertyName("ticket_number")]
        public string? TicketNumber { get; set; }
        [JsonPropertyName("ticket_type")]
        public int TicketType { get; set; }
    }

    public class Route
    {
        [JsonPropertyName("airline_code")]
        public string? AirlineCode { get; set; }
        [JsonPropertyName("flight_num")]
        public int FlightNum { get; set; }
        [JsonPropertyName("depart_place")]
        public string? DepartPlace { get; set; }
        [JsonPropertyName("depart_datetime")]
        public DateTimeOffset DepartDatetime { get; set; }
        [JsonPropertyName("arrive_place")]
        public string? ArrivePlace { get; set; }
        [JsonPropertyName("arrive_datetime")]
        public DateTimeOffset ArriveDatetime { get; set; }
        [JsonPropertyName("pnr_id")]
        public string? PnrId { get; set; }
    }
}
