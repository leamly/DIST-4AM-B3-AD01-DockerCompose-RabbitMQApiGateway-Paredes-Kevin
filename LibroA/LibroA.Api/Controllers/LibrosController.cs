using LibroA.Api.Data;
using LibroA.Api.Models;
using LibroA.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LibroA.Api.Controllers
{
    [ApiController]

    // Se utiliza el atributo Route para definir la ruta base de la API
    // En este caso, la ruta base es "api/libros", lo que significa que todas las acciones del controlador estarán disponibles bajo esta ruta
    // https://localhost:44393/api/libros

    [Route("api/[controller]")]
    public class LibrosController: ControllerBase
    {
        // Se utiliza el atributo ApiController para habilitar características específicas de la API, como la validación automática de modelos y la generación de respuestas de error consistentes
        private readonly LibroA.Api.Data.LibrosDBContext _dbcontext;

        // Se utiliza el atributo RabbitMQPublisher para publicar mensajes en RabbitMQ
        private readonly RabbitMQPublisher _rabbitMQPublisher;


        // Se utiliza el constructor del controlador para inyectar el contexto de la base de datos (LibrosDBContext) a través de la inyección de dependencias
        public LibrosController(LibroA.Api.Data.LibrosDBContext context, RabbitMQPublisher rabbitMQPublisher)
        {
            _dbcontext = context;

            // Se asigna la instancia de RabbitMQPublisher al campo privado _rabbitMQPublisher para poder utilizarlo en los métodos del controlador
            _rabbitMQPublisher = rabbitMQPublisher;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<LibroA.Api.Models.Libros>>> GetLibros()
        {
            // Se utiliza el contexto de la base de datos para obtener todos los registros de la tabla "Libros" y devolverlos como una lista
            // Se utiliza AsNoTracking() para indicar que no se realizará seguimiento de cambios en los objetos obtenidos, lo que mejora el rendimiento al leer datos
            var Libros = await _dbcontext.Libros.AsNoTracking().ToListAsync();
            return Ok(Libros);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Libros>> GetLibro(int id)
        {
            // Se utiliza el contexto de la base de datos para buscar un registro específico en la tabla "Libros" según el Id proporcionado
            var libro = await _dbcontext.Libros.AsNoTracking().FirstOrDefaultAsync(l => l.IdLibro == id);
            if (libro == null)
            {
                return NotFound();
            }
            return Ok(libro);
        }

        
        [HttpPost]
        public async Task<ActionResult<Libros>> CrearLibro(Libros libro)
        {
            // Se utiliza el contexto de la base de datos para agregar un nuevo registro a la tabla "Libros"
            _dbcontext.Libros.Add(libro);
            await _dbcontext.SaveChangesAsync();
            await _rabbitMQPublisher.PublicarLibroCreadoAsync(libro);

            // informacion de metadata de la respuesta HTTP, indicando que se ha creado un nuevo recurso y proporcionando la ubicación del mismo
            return CreatedAtAction(nameof(GetLibro), new { id = libro.IdLibro }, libro);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> ActualizarLibro(int id, Libros libro)
        {
            if (libro.IdLibro != id || id == 0)
            {
                return BadRequest();
            }

            _dbcontext.Entry(libro).State = EntityState.Modified;
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarLibro(int id)
        {
            var libro = await _dbcontext.Libros.FindAsync(id);
            if (libro == null)
            {
                return NotFound();
            }

            _dbcontext.Libros.Remove(libro);
            await _dbcontext.SaveChangesAsync();
            return NoContent();
        }

    }
}
