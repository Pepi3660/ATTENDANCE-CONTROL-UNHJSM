using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Class_Status")]
public partial class CatClassStatus
{
    [Key]
    [Column("Class_Status_Id")]
    public int ClassStatusId { get; set; }

    [Column("Class_Status_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string ClassStatusName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("ClassStatus")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();
}
