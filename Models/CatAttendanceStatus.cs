using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Attendance_Status")]
public partial class CatAttendanceStatus
{
    [Key]
    [Column("Attendance_Status_Id")]
    public int AttendanceStatusId { get; set; }

    [Column("Attendance_Status_Name")]
    [StringLength(30)]
    [Unicode(false)]
    public string AttendanceStatusName { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("AttendanceStatus")]
    public virtual ICollection<TblAttendance> TblAttendances { get; set; } = new List<TblAttendance>();
}
