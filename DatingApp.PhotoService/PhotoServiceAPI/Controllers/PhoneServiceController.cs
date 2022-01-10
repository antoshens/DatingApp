using Microsoft.AspNetCore.Mvc;

namespace PhotoServiceAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PhoneServiceController : ControllerBase
    {
        public PhoneServiceController()
        {
        }

        [HttpGet("Index")]
        public bool Get()
        {
            return true;
        }
    }
}