using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Register_Method")]
public partial class CatRegisterMethod
{
    [Key]
    [Column("Register_Method_Id")]
    public int RegisterMethodId { get; set; }

    [Column("Register_Method_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string RegisterMethodName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("RegisterMethod")]
    public virtual ICollection<TblAttendance> TblAttendances { get; set; } = new List<TblAttendance>();
}
