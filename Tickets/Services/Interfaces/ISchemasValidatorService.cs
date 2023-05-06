using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.Services.Interfaces
{
    public interface ISchemasValidatorService
    {
        public bool ContentIsValidBySchema(ControllerActionDescriptor descriptor, 
            ApiVersion apiVersion, string content);
    }
}
