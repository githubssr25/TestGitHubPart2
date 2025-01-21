namespace PostMVPProject.Models;


public class UserIssue
{
    public string UserId { get; set; }  // Foreign Key to User
    public int IssueId { get; set; }  // Foreign Key to Issue

    public User User { get; set; }
    public Issue Issue { get; set; }

    public DateTime AssignedAt { get; set; }  // Timestamp for when the user was assigned

     public DateTime SavedAt { get; set; }  // Timestamp for when the issue was saved
}
