using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Classroom")]
public partial class CatClassroom
{
    [Key]
    [Column("Classroom_Id")]
    public int ClassroomId { get; set; }

    [Column("Building_Id")]
    public int BuildingId { get; set; }

    [Column("Classroom_Name")]
    [StringLength(50)]
    [Unicode(false)]
    public string ClassroomName { get; set; } = null!;

    public int? Capacity { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("BuildingId")]
    [InverseProperty("CatClassrooms")]
    public virtual CatBuilding Building { get; set; } = null!;

    [InverseProperty("Classroom")]
    public virtual ICollection<TblClass> TblClasses { get; set; } = new List<TblClass>();
}
