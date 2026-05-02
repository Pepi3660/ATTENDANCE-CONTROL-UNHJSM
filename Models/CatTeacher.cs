using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Teacher")]
public partial class CatTeacher
{
    [Key]
    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

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

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Teacher")]
    public virtual ICollection<CatUser> CatUsers { get; set; } = new List<CatUser>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblDocumentTeacher> TblDocumentTeachers { get; set; } = new List<TblDocumentTeacher>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblEmailTeacher> TblEmailTeachers { get; set; } = new List<TblEmailTeacher>();

    [InverseProperty("Teacher")]
    public virtual ICollection<TblPhoneTeacher> TblPhoneTeachers { get; set; } = new List<TblPhoneTeacher>();
}
