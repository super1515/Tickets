using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Tickets.BAL.Dto
{
    public class RefundRequestDto
    {
        [RegularExpression(@"refund", ErrorMessage = "{0} must be {1}.")]
        [Required]
        [JsonPropertyName("operation_type")]
        public string OperationType { get; set; }
        [Required]
        [JsonPropertyName("operation_time")]
        public DateTimeOffset OperationTime { get; set; }
        [Required]
        [JsonPropertyName("operation_place")]
        public string OperationPlace { get; set; }
        [Required]
        [RegularExpression(@"\d{13}", ErrorMessage = "{0} must consist of 13 digits.")]
        [JsonPropertyName("ticket_number")]
        public string TicketNumber { get; set; }

    }
}
