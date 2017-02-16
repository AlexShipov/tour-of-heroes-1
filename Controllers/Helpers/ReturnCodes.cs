using Microsoft.AspNetCore.Mvc;

namespace tour_of_heroes_1.Controllers.Helpers
{
    public static class ReturnCodes
    {
        public static ConflictResult Conflict(this ControllerBase controller)
        {
            return new ConflictResult();
        }

        public static ConflictObjectResult Conflict(this ControllerBase controller, object error)
        {
            return new ConflictObjectResult(error);
        }
    }
}
