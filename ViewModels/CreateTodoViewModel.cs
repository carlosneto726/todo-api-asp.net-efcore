using System.ComponentModel.DataAnnotations;

namespace MeuTodo.ViewModels
{
    public class CreateTodoViewModel
    {

        [Required]    
        public required string Title { get; set; }
    }   
}