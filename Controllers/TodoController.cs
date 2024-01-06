using MeuTodo.Data;
using MeuTodo.Models;
using MeuTodo.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Threading.Tasks;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route(template:"v1")]
    public class TodoController : ControllerBase
    {
        [HttpGet]
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync([FromServices] AppDbContext context)
        {
            var todos = await context
                .Todos
                .AsNoTracking()
                .ToListAsync();
            return Ok(todos);
        }

        [HttpGet]
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] AppDbContext context, 
            [FromRoute] int id
        ){
            var todo = await context
                .Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
            return todo == null ? NotFound(): Ok(todo);
        }


        [HttpPost(template:"todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model
        ){
            if(!ModelState.IsValid){
                return BadRequest();
            }

            var todo = new Todo
            {
                Date = DateTime.Now,
                Done = false,
                Title = model.Title
            };

            try
            {
                Console.WriteLine(todo.Title);
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();                
                return Created(uri: $"v1/todos/{todo.Id}", todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



        [HttpPut(template:"todos/{id}")]
        public async Task<IActionResult> PutAsync(
            [FromServices] AppDbContext context,
            [FromBody] CreateTodoViewModel model,
            [FromRoute] int id
        ){
            if(!ModelState.IsValid){
                return BadRequest();
            }

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if(todo == null){
                return NotFound();
            }

            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);
                await context.SaveChangesAsync();                
                return Ok(todo);
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }



        [HttpDelete(template:"todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] AppDbContext context,
            [FromRoute] int id
        ){
            if(!ModelState.IsValid){
                return BadRequest();
            }

            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

            if(todo == null){
                return NotFound();
            }

            try
            {
                context.Todos.Remove(todo);
                await context.SaveChangesAsync();                
                return Ok();
            }
            catch (Exception)
            {
                return BadRequest();
            }

        }

    }

}