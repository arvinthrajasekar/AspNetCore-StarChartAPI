using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route ("")]
    [ApiController]

    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public CelestialObjectController(ApplicationDbContext context)
        {
            _context = context;
        }
        [HttpGet("{id:int}" ,Name ="GetById")]
        public IActionResult GetById(int id)
        {
            var selectedId = _context.CelestialObjects.Find(id);
            if(selectedId == null)
            {
                return NotFound();
            }
            selectedId.Satellites = _context.CelestialObjects.Where(s=> s.OrbitedObjectId== id).ToList();
                return Ok(selectedId);
       
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var selectedName = _context.CelestialObjects.Where(p => p.Name == name).ToList();
            if(!selectedName.Any())
            {
                return NotFound();
            }
            foreach (var celestialObject in selectedName)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
            }
            
            return Ok(selectedName);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialobject = _context.CelestialObjects.ToList();
            foreach (var celestialObject in celestialobject)
            {
                celestialObject.Satellites = _context.CelestialObjects.Where(s => s.OrbitedObjectId == celestialObject.Id).ToList();
            }
            return Ok(_context.CelestialObjects);
        } 
        [HttpPost]
        public IActionResult Create([FromBody]CelestialObject celestialobject)
        {
            _context.CelestialObjects.Add(celestialobject);
            _context.SaveChanges();            
            return CreatedAtRoute("GetById", new { id = celestialobject.Id }, celestialobject);
        }
        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject) 
        {
            var existingObject = _context.CelestialObjects.Find(id);
            if (existingObject == null)
            {
                return NotFound();

            }
            existingObject.Name = celestialObject.Name;
            existingObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            existingObject.OrbitedObjectId = celestialObject.OrbitedObjectId;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var existingObject = _context.CelestialObjects.Find(id);
            if(existingObject == null)
            {
                return NotFound();
            }
            existingObject.Name = name;
            _context.CelestialObjects.Update(existingObject);
            _context.SaveChanges();
            return NoContent();
        }
        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObject = _context.CelestialObjects.Where(e => e.Id == id || e.OrbitedObjectId == id);
            if (celestialObject == null)
            {
                return NotFound();
            }
            else
            {
                _context.CelestialObjects.RemoveRange(celestialObject);
                _context.SaveChanges();

            }
            return NoContent();
        }
    }
}
