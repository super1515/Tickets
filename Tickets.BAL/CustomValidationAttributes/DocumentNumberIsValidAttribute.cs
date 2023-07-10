using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Tickets.BAL.Dto;

namespace Tickets.BAL.CustomValidationAttributes
{
    public class DocumentNumberIsValidAttribute : ValidationAttribute
    {
        public DocumentNumberIsValidAttribute()
        {
            ErrorMessage = "If doc_type is \"00\" then the document number must have 10 digits.";
        }
        public override bool IsValid(object value)
        {
            if (value == null) return false;
            SaleRequestDto requestDto = value as SaleRequestDto;
            if (requestDto!.Passenger == null) return false;
            if (requestDto.Passenger.DocType == "00" && !Regex.IsMatch(requestDto.Passenger.DocNumber!, @"^\d{10}$")) return false;
            return true;
        }
    }
}
