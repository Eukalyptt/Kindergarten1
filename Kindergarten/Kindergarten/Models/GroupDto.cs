using System.ComponentModel.DataAnnotations;

namespace Kindergarten.Models
{
    public class GroupDto
    {
        [Required]
        public string Name { get; set; } = "";
        [Required]
        public string ChildrenCount { get; set; } = "";
        [Required]
        public string KindergartenName { get; set; } = "";
        [Required]
        public string Teacher { get; set; } = "";
        public IFormFile? ImageFile { get; set; }
    }
}
