using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Attendance")]
[Index("ClassId", "StudentId", Name = "UQ__Tbl_Atte__6AB84BAEB933487B", IsUnique = true)]
public partial class TblAttendance
{
    [Key]
    [Column("Attendance_Id")]
    public int AttendanceId { get; set; }

    [Column("Class_Id")]
    public int ClassId { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    [Column("Attendance_Status_Id")]
    public int AttendanceStatusId { get; set; }

    [Column("Register_Method_Id")]
    public int RegisterMethodId { get; set; }

    [Column("Is_Justified")]
    public bool IsJustified { get; set; }

    [StringLength(250)]
    [Unicode(false)]
    public string? Observation { get; set; }

    [Column("Register_Time", TypeName = "datetime")]
    public DateTime RegisterTime { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("AttendanceStatusId")]
    [InverseProperty("TblAttendances")]
    public virtual CatAttendanceStatus AttendanceStatus { get; set; } = null!;

    [ForeignKey("ClassId")]
    [InverseProperty("TblAttendances")]
    public virtual TblClass Class { get; set; } = null!;

    [ForeignKey("RegisterMethodId")]
    [InverseProperty("TblAttendances")]
    public virtual CatRegisterMethod RegisterMethod { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TblAttendances")]
    public virtual CatStudent Student { get; set; } = null!;
}
