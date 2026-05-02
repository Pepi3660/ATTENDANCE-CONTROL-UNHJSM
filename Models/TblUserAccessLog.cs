using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_User_Access_Log")]
public partial class TblUserAccessLog
{
    [Key]
    [Column("Id_Access_Log")]
    public int IdAccessLog { get; set; }

    [Column("Id_User")]
    public int IdUser { get; set; }

    [Column("Action_Name")]
    [StringLength(100)]
    [Unicode(false)]
    public string ActionName { get; set; } = null!;

    [Column("Ip_Address")]
    [StringLength(45)]
    [Unicode(false)]
    public string? IpAddress { get; set; }

    [Column("Device_Info")]
    [StringLength(200)]
    [Unicode(false)]
    public string? DeviceInfo { get; set; }

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdUser")]
    [InverseProperty("TblUserAccessLogs")]
    public virtual CatUser IdUserNavigation { get; set; } = null!;
}
