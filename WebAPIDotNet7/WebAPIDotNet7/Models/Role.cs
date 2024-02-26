using System.ComponentModel.DataAnnotations;

namespace WebAPIDotNet7.Models
{
    public class Role:BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
