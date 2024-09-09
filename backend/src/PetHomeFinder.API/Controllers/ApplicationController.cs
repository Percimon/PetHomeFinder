using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace PetHomeFinder.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApplicationController : ControllerBase { }
}
