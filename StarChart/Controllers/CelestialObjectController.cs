using System.Collections.Generic;
using System.Linq;
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
                celestialObject.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == id).ToList();
                return Ok(celestialObject);
            }
            return NotFound();
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            List<CelestialObject> celestialObjects = _context.CelestialObjects.Where(x => x.Name == name).ToList();
            if (celestialObjects?.Count > 0)
            {
                foreach (var item in celestialObjects)
                {
                    item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
                }
                return Ok(celestialObjects);
            }
            return NotFound();
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<CelestialObject> celestialObjects = _context.Set<CelestialObject>().ToList();
            foreach (var item in celestialObjects)
            {
                item.Satellites = _context.CelestialObjects.Where(x => x.OrbitedObjectId == item.Id).ToList();
            }
            return Ok(celestialObjects);
        }

        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialObject)
        {
            _context.Add(celestialObject);
            _context.SaveChanges();
            return CreatedAtRoute("GetById", new { id = celestialObject.Id }, celestialObject);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject data)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject != null)
            {
                celestialObject.Name = data.Name;
                celestialObject.OrbitalPeriod = data.OrbitalPeriod;
                celestialObject.OrbitedObjectId = data.OrbitedObjectId;
                _context.Update(celestialObject);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            CelestialObject celestialObject = _context.CelestialObjects.Find(id);

            if (celestialObject != null)
            {
                celestialObject.Name = name;
                _context.Update(celestialObject);
                _context.SaveChanges();
                return NoContent();
            }
            return NotFound();
        }

        [HttpDelete("{id}")]
        public IActionResult RenameObject(int id)
        {
            List<CelestialObject> celestialObjects = _context.Set<CelestialObject>().ToList();

            if (celestialObjects?.Count() > 0)
            {
                List<CelestialObject> celestialObjectsToDelete = new List<CelestialObject>();

                foreach (var item in celestialObjects)
                {
                    if (item.Id == id || item.OrbitedObjectId == id)
                    {
                        celestialObjectsToDelete.Add(item);
                    }
                }
                _context.RemoveRange(celestialObjectsToDelete);
                _context.SaveChanges();
            }
            return NotFound();
        }
    }
}
