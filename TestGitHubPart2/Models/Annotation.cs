

namespace PostMVPProject.Models;
    public class Annotation
    {
        public int AnnotationId { get; set; } // Primary Key
        public string UserId { get; set; } // Foreign Key
        public int RepositoryId { get; set; } // Foreign Key
        public string Type { get; set; } // "note" or "tag"
        public string Content { get; set; }
        public DateTime CreatedAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public Repository Repository { get; set; }
    }
