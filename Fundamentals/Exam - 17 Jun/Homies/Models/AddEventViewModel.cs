using Homies.Common;
using System.ComponentModel.DataAnnotations;
using System.Drawing;

namespace Homies.Models
{
    public class AddEventViewModel
    {
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

        public IEnumerable<TypeViewModel> Types { get; set; } = new HashSet<TypeViewModel>();
    }
}
