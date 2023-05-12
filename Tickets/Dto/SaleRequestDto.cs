using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Tickets.CustomValidationAttributes;

namespace Tickets.Dto
{
    [DocumentNumberIsValid]
    public class SaleRequestDto
    {
        [Required]
        [RegularExpression(@"sale", ErrorMessage = "{0} must be {1}")]
        [JsonPropertyName("operation_type")]
        public string OperationType { get; set; }
        [Required]
        [JsonPropertyName("operation_time")]
        public DateTimeOffset OperationTime { get; set; }
        [Required]
        [JsonPropertyName("operation_place")]
        public string OperationPlace { get; set; }
        [Required]
        [JsonPropertyName("passenger")]
        public Passenger Passenger { get; set; }
        [Required]
        [JsonPropertyName("routes")]
        public IEnumerable<Route> Routes { get; set; }
    }
    public class Passenger
    {
        [Required]
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [Required]
        [JsonPropertyName("surname")]
        public string Surname { get; set; }
        [Required]
        [JsonPropertyName("patronymic")]
        public string Patronymic { get; set; }
        [Required]
        [JsonPropertyName("doc_type")]
        public string DocType { get; set; }
        [Required]
        [JsonPropertyName("doc_number")]
        public string DocNumber { get; set; }
        [Required]
        [JsonPropertyName("birthdate")]
        public DateOnly Birthdate { get; set; }
        [Required]
        [JsonPropertyName("gender")]
        [RegularExpression(@"[M|F]", ErrorMessage = "{0} field accepts only M or F letters.")]
        public string Gender { get; set; }
        [Required]
        [JsonPropertyName("passenger_type")]
        public string PassengerType { get; set; }
        [Required]
        [RegularExpression(@"\d{13}", ErrorMessage = "{0} must consist of 13 digits")]
        [JsonPropertyName("ticket_number")]
        public string TicketNumber { get; set; }
        [Required]
        [JsonPropertyName("ticket_type")]
        public int TicketType { get; set; }
    }

    public class Route
    {
        [Required]
        [JsonPropertyName("airline_code")]
        public string? AirlineCode { get; set; }
        [Required]
        [JsonPropertyName("flight_num")]
        public int FlightNum { get; set; }
        [Required]
        [JsonPropertyName("depart_place")]
        public string? DepartPlace { get; set; }
        [Required]
        [JsonPropertyName("depart_datetime")]
        public DateTimeOffset DepartDatetime { get; set; }
        [Required]
        [JsonPropertyName("arrive_place")]
        public string? ArrivePlace { get; set; }
        [Required]
        [JsonPropertyName("arrive_datetime")]
        public DateTimeOffset ArriveDatetime { get; set; }
        [Required]
        [JsonPropertyName("pnr_id")]
        public string? PnrId { get; set; }
    }
}
