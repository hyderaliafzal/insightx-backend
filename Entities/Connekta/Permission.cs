namespace ConnektaViz.API.Entities.Connekta;
public class Permission
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
    public bool View { get; set; }
    public bool Edit { get; set; }
    public bool Delete { get; set; }
    public short ModuleId { get; set; }
    [ForeignKey("PermissionGroup")]
    public long PermissionGroupID { get; set; }
}
