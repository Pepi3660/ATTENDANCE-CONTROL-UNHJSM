using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Student")]
public partial class CatStudent
{
    [Key]
    [Column("Student_Id")]
    public int StudentId { get; set; }

    [Column("First_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string FirstName { get; set; } = null!;

    [Column("Second_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string? SecondName { get; set; }

    [Column("Last_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string LastName { get; set; } = null!;

    [Column("Second_SurName")]
    [StringLength(30)]
    [Unicode(false)]
    public string? SecondSurName { get; set; }

    [StringLength(1)]
    [Unicode(false)]
    public string Gender { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Student")]
    public virtual ICollection<TblAttendance> TblAttendances { get; set; } = new List<TblAttendance>();

    [InverseProperty("Student")]
    public virtual ICollection<TblClassEnrollment> TblClassEnrollments { get; set; } = new List<TblClassEnrollment>();

    [InverseProperty("Student")]
    public virtual ICollection<TblDocumentStudent> TblDocumentStudents { get; set; } = new List<TblDocumentStudent>();

    [InverseProperty("Student")]
    public virtual ICollection<TblEmailStudent> TblEmailStudents { get; set; } = new List<TblEmailStudent>();

    [InverseProperty("Student")]
    public virtual ICollection<TblPhoneStudent> TblPhoneStudents { get; set; } = new List<TblPhoneStudent>();

    [InverseProperty("Student")]
    public virtual ICollection<TblStudentGroup> TblStudentGroups { get; set; } = new List<TblStudentGroup>();
}
