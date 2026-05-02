using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Subject")]
public partial class CatSubject
{
    [Key]
    [Column("Subject_Id")]
    public int SubjectId { get; set; }

    [Column("Subject_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string SubjectName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Subject")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();
}
