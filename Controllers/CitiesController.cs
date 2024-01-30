using CitiesManager.WebAPI.DataBaseContext;
using CitiesManager.WebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CitiesManager.WebAPI.Controllers
{
    public class CitiesController : CustomControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CitiesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Cities
        /// <summary>
        /// To get list of cities (including city ID and city name) from 'cities' table
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<City>>> GetCities()
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }
            return await _context.Cities.OrderBy(city => city.CityName).ToListAsync();
        }

        // GET: api/Cities/5
        [HttpGet("{cityId}")]
        public async Task<ActionResult<City>> GetCity(Guid cityId)
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(cityId);

            if (city == null)
            {
                return Problem(detail: "Invalid City Id", statusCode: 400, title: "City search");
            }

            return city;
        }

        // PUT: api/Cities/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{cityId}")]
        public async Task<IActionResult> PutCity(
            Guid cityId,
            [Bind(nameof(City.CityId), nameof(City.CityName))] City city
        )
        {
            if (cityId != city.CityId)
            {
                return BadRequest();
            }

            _context.Entry(city).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CityExists(cityId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Cities
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<City>> PostCity(
            [Bind(nameof(City.CityId), nameof(City.CityName))] City city
        )
        {
            if (_context.Cities == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Cities'  is null.");
            }
            _context.Cities.Add(city);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetCity", new { id = city.CityId }, city);
        }

        // DELETE: api/Cities/5
        [HttpDelete("{cityId}")]
        public async Task<IActionResult> DeleteCity(Guid cityId)
        {
            if (_context.Cities == null)
            {
                return NotFound();
            }
            var city = await _context.Cities.FindAsync(cityId);
            if (city == null)
            {
                return NotFound();
            }

            _context.Cities.Remove(city);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CityExists(Guid cityId)
        {
            return (_context.Cities?.Any(e => e.CityId == cityId)).GetValueOrDefault();
        }
    }
}
