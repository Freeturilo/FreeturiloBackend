using System.ComponentModel.DataAnnotations;

namespace FreeturiloWebApi.Models
{
    public class State
    {
        [Key]
        public int Id { get; set; }
        public int Value { get; set; }
    }
}
