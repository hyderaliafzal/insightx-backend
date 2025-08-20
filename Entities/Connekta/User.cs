namespace ConnektaViz.API.Entities.Connekta;
public class User
{
    [Key]
    public long ID { get; set; }
    public bool IsDeleted { get; set; } = false;
    public string PhotoURL { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public DateTime? LoginSession { get; set; }

    [ForeignKey("PermissionGroup")]
    public long? PermissionGroupID { get; set; }
    public virtual PermissionGroup PermissionGroup { get; set; }
}