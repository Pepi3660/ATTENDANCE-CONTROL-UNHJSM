using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_User_Profile")]
public partial class CatUserProfile
{
    [Key]
    [Column("Id_User_Profile")]
    public int IdUserProfile { get; set; }

    [Column("Id_User")]
    public int? IdUser { get; set; }

    [Column("First_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("Last_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string? Phone { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("CatUserProfiles")]
    public virtual CatUser? IdUserNavigation { get; set; }
}
