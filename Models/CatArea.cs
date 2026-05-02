using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Area")]
public partial class CatArea
{
    [Key]
    [Column("Area_Id")]
    public int AreaId { get; set; }

    [Column("Area_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string AreaName { get; set; } = null!;

    [StringLength(20)]
    [Unicode(false)]
    public string Initial { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("Area")]
    public virtual ICollection<CatBuilding> CatBuildings { get; set; } = new List<CatBuilding>();
}
