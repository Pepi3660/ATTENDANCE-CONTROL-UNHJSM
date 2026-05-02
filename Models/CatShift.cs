using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Shift")]
public partial class CatShift
{
    [Key]
    [Column("Shift_Id")]
    public int ShiftId { get; set; }

    [Column("Shift_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string ShiftName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Shift")]
    public virtual ICollection<CatGroup> CatGroups { get; set; } = new List<CatGroup>();
}
