using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{

    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet("{id:int}")]
        [ActionName("GetById")]
        public IActionResult GetById(int id)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);
            if (celestialObject != null)
            {
                return Ok(celestialObject);
            }
            return NotFound();
        }
    }
}
