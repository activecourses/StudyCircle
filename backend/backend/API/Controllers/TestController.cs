using API.Filters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        [HttpGet("Test")]
        public ActionResult Test()
        {
            return Ok("Test");
        }



        [HttpGet]
        [Route("get{id}")]
        [ServiceFilter(typeof(ClubMemberFilter))]
        public async Task<ActionResult<string>> TestGroupMemberPolicy(int id)
        {
            return "It worked successfully! :)";
        }


        [HttpGet]
        [Route("sysadm")]
        [Authorize(Roles = "SystemAdmin")]
        public string TestSystemAdminRole()
        {
            return "It worked successfully! :)";
        }
    }
}
