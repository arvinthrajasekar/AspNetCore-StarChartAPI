using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;

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
       


    }
}
