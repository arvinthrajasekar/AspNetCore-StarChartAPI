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
            selectedId.Satellites = _context.CelestialObjects.FirstOrDefault(p => p.Id == OrbitedObjectId);
                return Ok(selectedId);
       
        }
        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var selectedName = _context.CelestialObjects.FirstOrDefault(p => p.Name == name);
            if(selectedName == null)
            {
                return NotFound();
            }
            selectedName.Satellites.ForEach(s => s.Name = name);
            return Ok(selectedName);
        }
        [HttpGet]
        public IActionResult GetAll()
        {
            
            return Ok(_context.CelestialObjects);
        } 
       


    }
}
