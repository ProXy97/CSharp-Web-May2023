using Homies.Common;
using System.ComponentModel.DataAnnotations;

namespace Homies.Data.Models
{
    public class Type
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(GlobalConstants.TypeNameMaxLength)]
        public string Name { get; set; } = null!;

        
        public ICollection<Event> Events { get; set; } = new HashSet<Event>();
    }
}
