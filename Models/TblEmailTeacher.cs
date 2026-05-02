using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Email_Teacher")]
public partial class TblEmailTeacher
{
    [Key]
    [Column("Id_Email_Teacher")]
    public int IdEmailTeacher { get; set; }

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("TeacherId")]
    [InverseProperty("TblEmailTeachers")]
    public virtual CatTeacher Teacher { get; set; } = null!;
}
