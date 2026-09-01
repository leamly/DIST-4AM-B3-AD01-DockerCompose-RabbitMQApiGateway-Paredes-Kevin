using CategoriaA.Api.Data;
using CategoriaA.Api.Models;
using CategoriaA.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CategoriaA.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriaController : Controller
    {
        private readonly CategoriaA.Api.Data.CategoriasDBContext _dbcontext;
        private readonly RabbitMQPublisher _rabbitMQPublisher;

        public CategoriaController(CategoriaA.Api.Data.CategoriasDBContext context, RabbitMQPublisher rabbitMQPublisher)
        {
            _dbcontext = context;
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Categorias>>> GetCategorias()
        {
            var categorias = await _dbcontext.Categorias.AsNoTracking().ToListAsync();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Categorias>> GetCategoria(int id)
        {
            var categoria = await _dbcontext.Categorias.AsNoTracking().FirstOrDefaultAsync(c => c.IdCategoria == id);

            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        public async Task<ActionResult<Categorias>> CrearCategoria(Categorias categoria)
        {
            _dbcontext.Categorias.Add(categoria);
            await _dbcontext.SaveChangesAsync();
            await _rabbitMQPublisher.PublicarCategoriaCreadaAsync(categoria);

            return CreatedAtAction(nameof(GetCategoria), new { id = categoria.IdCategoria }, categoria);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, Categorias categoria)
        {
            if (categoria.IdCategoria != id || id == 0)
            {
                return BadRequest();
            }

            _dbcontext.Entry(categoria).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _dbcontext.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound();
            }

            _dbcontext.Categorias.Remove(categoria);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }
    }

}
