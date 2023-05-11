using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tickets.Dto
{
    public class RefundRequestDto
    {
        [JsonPropertyName("operation_type")]
        public string? OperationType { get; set; }
        [JsonPropertyName("operation_time")]
        public DateTimeOffset OperationTime { get; set; }
        [JsonPropertyName("operation_place")]
        public string? OperationPlace { get; set; }
        [RegularExpression(@"\d{13}", ErrorMessage = "{0} must consist of 13 digits")]
        [JsonPropertyName("ticket_number")]
        public string? TicketNumber { get; set; }

    }
}
