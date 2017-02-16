using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace tour_of_heroes_1.Controllers.Helpers
{
    public class ConflictResult : StatusCodeResult
    {
        public ConflictResult() : base((int)HttpStatusCode.Conflict)
        {
        }
    }
}
