using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Email_Student")]
public partial class TblEmailStudent
{
    [Key]
    [Column("Id_Email_Student")]
    public int IdEmailStudent { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Email { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("StudentId")]
    [InverseProperty("TblEmailStudents")]
    public virtual CatStudent Student { get; set; } = null!;
}
