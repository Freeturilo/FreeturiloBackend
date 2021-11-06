using System.ComponentModel.DataAnnotations;

namespace FreeturiloWebApi.Models
{
    public class Administrator
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public int NotifyThreshold { get; set; }
    }
}
