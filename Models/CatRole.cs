using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Role")]
[Index("RoleName", Name = "UQ__Cat_Role__035DB749439F518C", IsUnique = true)]
public partial class CatRole
{
    [Key]
    [Column("Id_Role")]
    public int IdRole { get; set; }

    [Column("Role_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string RoleName { get; set; } = null!;

    [Column("Role_Description")]
    [StringLength(150)]
    [Unicode(false)]
    public string? RoleDescription { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("IdRoleNavigation")]
    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();
}
