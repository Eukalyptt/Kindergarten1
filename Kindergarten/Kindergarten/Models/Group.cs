namespace Kindergarten.Models
{
    public class Group
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string ChildrenCount { get; set; } = "";
        public string KindergartenName { get; set; } = "";
        public string Teacher { get; set; } = "";
        public string ImageFileName { get; set; } = "";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
