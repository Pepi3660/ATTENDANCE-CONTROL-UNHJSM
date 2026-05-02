using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Class")]
public partial class TblClass
{
    [Key]
    [Column("Class_Id")]
    public int ClassId { get; set; }

    [Column("Class_Date")]
    public DateOnly ClassDate { get; set; }

    [Column("Start_Time")]
    public TimeOnly? StartTime { get; set; }

    [Column("End_Time")]
    public TimeOnly? EndTime { get; set; }

    [Column("Token_Link")]
    [StringLength(150)]
    [Unicode(false)]
    public string? TokenLink { get; set; }

    [Column("Open_DateTime", TypeName = "datetime")]
    public DateTime? OpenDateTime { get; set; }

    [Column("Close_DateTime", TypeName = "datetime")]
    public DateTime? CloseDateTime { get; set; }

    [Column("Allow_Self_Register")]
    public bool? AllowSelfRegister { get; set; }

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    [Column("Group_Id")]
    public int GroupId { get; set; }

    [Column("Subject_Id")]
    public int SubjectId { get; set; }

    [Column("Classroom_Id")]
    public int ClassroomId { get; set; }

    [Column("Class_Status_Id")]
    public int ClassStatusId { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("ClassStatusId")]
    [InverseProperty("TblClasses")]
    public virtual CatClassStatus ClassStatus { get; set; } = null!;

    [ForeignKey("ClassroomId")]
    [InverseProperty("TblClasses")]
    public virtual CatClassroom Classroom { get; set; } = null!;

    [ForeignKey("GroupId")]
    [InverseProperty("TblClasses")]
    public virtual CatGroup Group { get; set; } = null!;

    [ForeignKey("SubjectId")]
    [InverseProperty("TblClasses")]
    public virtual CatSubject Subject { get; set; } = null!;

    [InverseProperty("Class")]
    public virtual ICollection<TblAttendance> TblAttendances { get; set; } = new List<TblAttendance>();

    [InverseProperty("Class")]
    public virtual ICollection<TblClassEnrollment> TblClassEnrollments { get; set; } = new List<TblClassEnrollment>();

    [ForeignKey("TeacherId")]
    [InverseProperty("TblClasses")]
    public virtual CatTeacher Teacher { get; set; } = null!;
}
