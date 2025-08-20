namespace ConnektaViz.API.Entities.Connekta;
public class PermissionGroup
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


    public string Name { get; set; }
    public PortalTypeEnum PortalType { get; set; }
    public List<Permission> Permissions { get; set; }
    public List<User> Users { get; set; }
}