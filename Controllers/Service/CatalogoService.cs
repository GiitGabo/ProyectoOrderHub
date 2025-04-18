﻿using JarredsOrderHub.DbaseContext;
using JarredsOrderHub.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json;

namespace JarredsOrderHub.Controllers.Service
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatalogoService : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly AuditService _auditService;

        public CatalogoService(ApplicationDbContext context, AuditService auditService)
        {
            _context = context;
            _auditService = auditService;
        }

        //-----------------------------------------------------------------------------//
        // Métodos para Categorías
        //-----------------------------------------------------------------------------//

        [HttpGet("categorias")]
        public async Task<List<Categoria>> ObtenerTodasCategorias()
        {
            return await _context.Categorias.ToListAsync();
        }

        [HttpGet("categorias/{id}")]
        public async Task<ActionResult<Categoria>> ObtenerCategoriaPorId(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound("Categoría no encontrada");
            }
            return categoria;
        }

        [HttpPost("categorias")]
        public async Task<ActionResult<Categoria>> CrearCategoria([FromBody] Categoria categoria)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                // Asegurarse de que Platillos esté inicializado
                categoria.Platillos = new List<Platillo>();

                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();

                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Categoria",
                    entidadId: categoria.IdCategoria,
                    accion: "Creación",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(categoria),
                    descripcion: $"Se creó la categoria: {categoria.Nombre}"
                );

                // Crear un objeto anónimo con solo los datos necesarios
                var categoriaResponse = new
                {
                    idCategoria = categoria.IdCategoria,
                    nombre = categoria.Nombre,
                    descripcion = categoria.Descripcion,
                    activa = categoria.Activa
                };

                return Ok(categoriaResponse);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al crear la categoría", error = ex.Message });
            }
        }

        [HttpPut("categorias/{id}")]
        public async Task<IActionResult> ActualizarCategoria(int id, [FromBody] Categoria categoria)
        {
            if (id != categoria.IdCategoria)
            {
                return BadRequest();
            }

            var categoriaExistente = await _context.Categorias.FindAsync(id);
            if (categoriaExistente == null)
            {
                return NotFound("Categoría no encontrada");
            }

            categoriaExistente.Nombre = categoria.Nombre;
            categoriaExistente.Descripcion = categoria.Descripcion;
            categoriaExistente.Activa = categoria.Activa;

            _context.Entry(categoriaExistente).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

            await _auditService.RegistrarAuditoria(
                tipoEntidad: "Categoria",
                entidadId: categoria.IdCategoria,
                accion: "Edicion",
                usuario: usuario,
                detallesCambios: JsonSerializer.Serialize(categoria),
                descripcion: $"Se edito la categoria: {categoria.Nombre}"
            );


            return Ok(categoriaExistente);
        }

        [HttpDelete("categorias/{id}")]
        public async Task<IActionResult> EliminarCategoria(int id)
        {
            var categoria = await _context.Categorias.FindAsync(id);
            if (categoria == null)
            {
                return NotFound(new { mensaje = "Categoría no encontrada" });
            }

            try
            {
                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                
                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Categoria",
                    entidadId: categoria.IdCategoria,
                    accion: "Eliminacion",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(categoria),
                    descripcion: $"Se elimino la categoria: {categoria.Nombre}"


                 
                );

                return Ok(new { mensaje = "Categoría eliminada exitosamente" });

            }


            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar la categoría", error = ex.Message });
            }
        }

        //-----------------------------------------------------------------------------//
        // Métodos para Platillos
        //-----------------------------------------------------------------------------//





        [HttpGet("platillos")]
        public async Task<ActionResult<IEnumerable<Platillo>>> ObtenerTodosPlatos()
        {
            return await _context.Platillos.ToListAsync();
        }

        [HttpGet("platillos/{id}")]
        public async Task<ActionResult<Platillo>> ObtenerPlatoPorId(int id)
        {
            var platillo = await _context.Platillos.FindAsync(id);

            if (platillo == null)
            {
                return NotFound("Platillo no encontrado");
            }

            return platillo;
        }







        [HttpPost("platillos")]
        public async Task<ActionResult<Platillo>> CrearPlatillo([FromBody] Platillo platillo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si la categoría existe
            var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == platillo.IdCategoria);
            if (!categoriaExiste)
            {
                return BadRequest("La categoría especificada no existe");
            }

            _context.Platillos.Add(platillo);
            await _context.SaveChangesAsync();

            string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

            await _auditService.RegistrarAuditoria(
                tipoEntidad: "Platillo",
                entidadId: platillo.IdPlatillo,
                accion: "Creación",
                usuario: usuario,
                detallesCambios: JsonSerializer.Serialize(platillo),
                descripcion: $"Se creó el platilllo: {platillo.Nombre}"
            );



            return CreatedAtAction(nameof(ObtenerPlatoPorId), new { id = platillo.IdPlatillo }, platillo);
        }

        [HttpPut("platillos/{id}")]
        public async Task<IActionResult> ActualizarPlatillo(int id, [FromBody] Platillo platillo)
        {
            try
            {
                if (id != platillo.IdPlatillo)
                {
                    return BadRequest(new { mensaje = "El ID en la URL no coincide con el ID del platillo" });
                }

                // Primero verificamos si el platillo existe
                var platilloExistente = await _context.Platillos.FindAsync(id);
                if (platilloExistente == null)
                {
                    return NotFound(new { mensaje = "Platillo no encontrado" });
                }

                // Verificar si la categoría existe
                var categoriaExiste = await _context.Categorias.AnyAsync(c => c.IdCategoria == platillo.IdCategoria);
                if (!categoriaExiste)
                {
                    return BadRequest(new { mensaje = "La categoría especificada no existe" });
                }

                // Actualizamos los campos uno por uno
                platilloExistente.Nombre = platillo.Nombre;
                platilloExistente.Descripcion = platillo.Descripcion;
                platilloExistente.Precio = platillo.Precio;
                platilloExistente.Imagen = platillo.Imagen;
                platilloExistente.IdCategoria = platillo.IdCategoria;
                platilloExistente.Activo = platillo.Activo;

                // Marcamos la entidad como modificada
                _context.Entry(platilloExistente).State = EntityState.Modified;

                // Guardamos los cambios
                await _context.SaveChangesAsync();

                string usuario = HttpContext.Session.GetString("UserName") ?? "Sistema";

                await _auditService.RegistrarAuditoria(
                    tipoEntidad: "Platillo",
                    entidadId: platillo.IdPlatillo,
                    accion: "Creación",
                    usuario: usuario,
                    detallesCambios: JsonSerializer.Serialize(platillo),
                    descripcion: $"Se creó el platillo: {platillo.Nombre}"
                );


                // Retornamos el platillo actualizado
                return Ok(platilloExistente);
            }
            catch (Exception ex)
            {
                // Log del error
                Console.WriteLine($"Error al actualizar platillo: {ex.Message}");
                return StatusCode(500, new { mensaje = "Error interno al actualizar el platillo", error = ex.Message });
            }
        }

        [HttpDelete("platillos/{id}")]
        public async Task<IActionResult> EliminarPlatillo(int id)
        {
            var platillo = await _context.Platillos.FindAsync(id);
            if (platillo == null)
            {
                return NotFound(new { mensaje = "Platillo no encontrado" });
            }

            try
            {
                _context.Platillos.Remove(platillo);
                await _context.SaveChangesAsync();
                return Ok(new { mensaje = "Platillo eliminado exitosamente" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { mensaje = "Error al eliminar el platillo", error = ex.Message });
            }
        }
    }
}
