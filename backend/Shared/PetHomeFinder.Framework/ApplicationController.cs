using Microsoft.AspNetCore.Mvc;

namespace PetHomeFinder.Framework
{
    [ApiController]
    [Route("[controller]")]
    public abstract class ApplicationController : ControllerBase { }
}