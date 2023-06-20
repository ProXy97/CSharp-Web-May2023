using Homies.Common;
using System.ComponentModel.DataAnnotations;

namespace Homies.Models
{
    public class EventViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(GlobalConstants.EventNameMaxLength, MinimumLength = GlobalConstants.EventNameMinLength)]
        public string Name { get; set; } = null!;

        [Required]
        [StringLength(GlobalConstants.EventDescriptionMaxLength, MinimumLength = GlobalConstants.EventDescriptionMinLength)]
        public string Description { get; set; } = null!;

        [Required]
        public string Start { get; set; } = null!;

        [Required]
        public string End { get; set; } = null!;

        public string? OrganiserId { get; set; }

        [Range(1, int.MaxValue)]
        public int TypeId { get; set; }
    }
}
