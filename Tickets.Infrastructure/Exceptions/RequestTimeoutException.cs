using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace Tickets.Infrastructure.Exceptions
{
    public class RequestTimeoutException : Exception
    {
        public RequestTimeoutException()
        {

        }

        public RequestTimeoutException(SerializationInfo info, StreamingContext context) : base(info, context)
        {

        }

        public RequestTimeoutException(string message) : base(message)
        {

        }

        public RequestTimeoutException(string message, Exception innerException) : base(message, innerException)
        {

        }
    }
}
