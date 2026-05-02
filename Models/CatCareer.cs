using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Career")]
public partial class CatCareer
{
    [Key]
    [Column("Career_Id")]
    public int CareerId { get; set; }

    [Column("Career_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string CareerName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Career")]
    public virtual ICollection<CatGroup> CatGroups { get; set; } = new List<CatGroup>();
}
