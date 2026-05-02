using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Group")]
public partial class CatGroup
{
    [Key]
    [Column("Group_Id")]
    public int GroupId { get; set; }

    [Column("Career_Id")]
    public int CareerId { get; set; }

    [Column("Shift_Id")]
    public int ShiftId { get; set; }

    [Column("Group_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string GroupName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("CareerId")]
    [InverseProperty("CatGroups")]
    public virtual CatCareer Career { get; set; } = null!;

    [ForeignKey("ShiftId")]
    [InverseProperty("CatGroups")]
    public virtual CatShift Shift { get; set; } = null!;

    [InverseProperty("Group")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();

    [InverseProperty("Group")]
    public virtual ICollection<TblStudentGroup> TblStudentGroups { get; set; } = new List<TblStudentGroup>();
}
