using System.Text.Json.Serialization;

namespace Tickets.Dto
{
    public class RefundRequestDto
    {
        [JsonPropertyName("operation_type")]
        public string? OperationType { get; set; }
        [JsonPropertyName("operation_time")]
        public string? OperationTime { get; set; }
        [JsonPropertyName("operation_place")]
        public string? OperationPlace { get; set; }
        [JsonPropertyName("ticket_number")]
        public string? TicketNumber { get; set; }

    }
}
