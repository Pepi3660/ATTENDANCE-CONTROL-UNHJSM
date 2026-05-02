using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_User")]
[Index("Email", Name = "UQ__Cat_User__A9D10534D0EFF142", IsUnique = true)]
[Index("UserName", Name = "UQ__Cat_User__C9F28456C9A8D933", IsUnique = true)]
public partial class CatUser
{
    [Key]
    [Column("Id_User")]
    public int IdUser { get; set; }

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    [StringLength(50)]
    [Unicode(false)]
    public string UserName { get; set; } = null!;

    [StringLength(150)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    [Column("Password_Hash")]
    [StringLength(255)]
    [Unicode(false)]
    public string PasswordHash { get; set; } = null!;

    [Column("Last_Login", TypeName = "datetime")]
    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<CatUserProfile> CatUserProfiles { get; set; } = new List<CatUserProfile>();

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<TblUserAccessLog> TblUserAccessLogs { get; set; } = new List<TblUserAccessLog>();

    [InverseProperty("IdUserNavigation")]
    public virtual ICollection<TblUserRole> TblUserRoles { get; set; } = new List<TblUserRole>();

    [ForeignKey("TeacherId")]
    [InverseProperty("CatUsers")]
    public virtual CatTeacher Teacher { get; set; } = null!;
}
