using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace Tickets.WebAPI.Services.Interfaces
{
    public interface ISchemasValidatorService
    {
        public bool ContentIsValidBySchema(ControllerActionDescriptor descriptor, 
            ApiVersion apiVersion, string content);
    }
}
