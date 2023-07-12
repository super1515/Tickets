﻿namespace Tickets.BAL.Utility
{
    public class ResponseMessages
    {
        public const string RequestInNotValidMsg = "Request is not valid, сheck the request body.";
        public const string RefundIsNotSuccessMsg = "Refund is not success, the ticket does not exist or has already been refunded.";
        public const string SaleSuccessMsg = "Ticket has been sold.";
        public const string RefundSuccessMsg = "Ticket has been refunded.";
    }
}
