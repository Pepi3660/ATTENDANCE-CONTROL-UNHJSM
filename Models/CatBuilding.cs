using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Building")]
public partial class CatBuilding
{
    [Key]
    [Column("Building_Id")]
    public int BuildingId { get; set; }

    [Column("Area_Id")]
    public int AreaId { get; set; }

    [Column("Building_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string BuildingName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("AreaId")]
    [InverseProperty("CatBuildings")]
    public virtual CatArea Area { get; set; } = null!;

    [InverseProperty("Building")]
    public virtual ICollection<CatClassroom> CatClassrooms { get; set; } = new List<CatClassroom>();
}
