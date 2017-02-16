using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace tour_of_heroes_1.Controllers.Helpers
{
    public class ConflictObjectResult : ObjectResult
    {
        public ConflictObjectResult(object value) : base(value)
        {
            this.StatusCode = (int)HttpStatusCode.Conflict;
        }
    }
}
