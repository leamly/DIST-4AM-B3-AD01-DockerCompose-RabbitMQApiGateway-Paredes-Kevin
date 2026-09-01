using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VehiculoA.Api.Data;
using VehiculoA.Api.Models;

namespace VehiculoA.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiculoController : ControllerBase
    {
        private readonly VehiculosDBContext _context;

        public VehiculoController(VehiculosDBContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vehiculos>>> GetVehiculos()
        {
            var vehiculos = await _context.Vehiculos.AsNoTracking().ToListAsync();
            return Ok(vehiculos);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Vehiculos>> GetVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.AsNoTracking().FirstOrDefaultAsync(v => v.IdVehiculo == id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            return Ok(vehiculo);
        }


        [HttpPost]
        public async Task<ActionResult<Vehiculos>> CreateVehiculo(Vehiculos vehiculo)
        {
            _context.Vehiculos.Add(vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVehiculo), new { id = vehiculo.IdVehiculo }, vehiculo);
        }


        [HttpPut("{id}")]
        public async Task<ActionResult<Vehiculos>> UpdateVehiculo(int id, Vehiculos vehiculo)
        {
            if (id != vehiculo.IdVehiculo)
            {
                return BadRequest();
            }

            _context.Entry(vehiculo).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }


        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteVehiculo(int id)
        {
            var vehiculo = await _context.Vehiculos.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound();
            }

            _context.Vehiculos.Remove(vehiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
