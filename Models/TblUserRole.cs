using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_User_Role")]
public partial class TblUserRole
{
    [Key]
    [Column("Id_User_Role")]
    public int IdUserRole { get; set; }

    [Column("Id_User")]
    public int IdUser { get; set; }

    [Column("Id_Role")]
    public int IdRole { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdRole")]
    [InverseProperty("TblUserRoles")]
    public virtual CatRole IdRoleNavigation { get; set; } = null!;

    [ForeignKey("IdUser")]
    [InverseProperty("TblUserRoles")]
    public virtual CatUser IdUserNavigation { get; set; } = null!;
}
