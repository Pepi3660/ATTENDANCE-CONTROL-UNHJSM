using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Class_Enrollment")]
[Index("ClassId", "StudentId", Name = "UQ_Class_Enrollment", IsUnique = true)]
public partial class TblClassEnrollment
{
    [Key]
    [Column("Enrollment_Id")]
    public int EnrollmentId { get; set; }

    [Column("Class_Id")]
    public int ClassId { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("ClassId")]
    [InverseProperty("TblClassEnrollments")]
    public virtual TblClass Class { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TblClassEnrollments")]
    public virtual CatStudent Student { get; set; } = null!;
}
