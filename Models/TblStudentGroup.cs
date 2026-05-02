using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Student_Group")]
public partial class TblStudentGroup
{
    [Key]
    [Column("Id_Student_Group")]
    public int IdStudentGroup { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    [Column("Group_Id")]
    public int GroupId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("GroupId")]
    [InverseProperty("TblStudentGroups")]
    public virtual CatGroup Group { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TblStudentGroups")]
    public virtual CatStudent Student { get; set; } = null!;
}
