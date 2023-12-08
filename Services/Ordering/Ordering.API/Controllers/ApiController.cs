using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ordering.API.Controllers;
[Route("api/v{version:apiVersion}/[controller]")]
[ApiVersion("1")]
[ApiController]
public class ApiController : ControllerBase
{

}
