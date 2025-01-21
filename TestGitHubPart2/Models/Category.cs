

namespace PostMVPProject.Models;

    public class Category
    {
        public int CategoryId { get; set; } // Primary Key
        public string Description { get; set; }

        // Navigation Properties
        public ICollection<Repository> Repositories { get; set; }
    }

