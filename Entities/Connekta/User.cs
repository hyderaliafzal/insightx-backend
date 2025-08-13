namespace ConnektaViz.API.Entities.Connekta;
public class User
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public long ID { get; set; }
    public long CreatedById { get; set; }
    public long UpdatedById { get; set; }
    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; } = false;
    [NotMapped]
    public string TenantType { get; set; }

    public string PhotoURL { get; set; }
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
    [Required]
    public string Email { get; set; }
    public string Password { get; set; }
    public string Designation { get; set; }
    public string Domain { get; set; }
    public UserStatus UserStatus { get; set; }
    public string Role { get; set; }
    public string PhoneCode { get; set; }
    public string PhoneNumber { get; set; }
    public string CompanyPhone { get; set; }
    public DateTime? LoginSession { get; set; }

    [ForeignKey("PermissionGroup")]
    public long? PermissionGroupID { get; set; }
    public virtual PermissionGroup PermissionGroup { get; set; }
}