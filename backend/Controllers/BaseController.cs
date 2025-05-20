using Microsoft.AspNetCore.Mvc;

namespace ekart.Controllers
{
    public abstract class BaseController : ControllerBase
    {
        // Common method to validate request models
        protected IActionResult ValidateModel()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return null;
        }
    }
}
