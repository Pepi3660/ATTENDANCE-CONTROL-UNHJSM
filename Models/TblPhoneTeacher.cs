using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Phone_Teacher")]
public partial class TblPhoneTeacher
{
    [Key]
    [Column("Id_Phone_Teacher")]
    public int IdPhoneTeacher { get; set; }

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    [Column("Id_Telephone_Company")]
    public int IdTelephoneCompany { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdTelephoneCompany")]
    [InverseProperty("TblPhoneTeachers")]
    public virtual CatTelephoneCompany IdTelephoneCompanyNavigation { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TblPhoneTeachers")]
    public virtual CatTeacher Teacher { get; set; } = null!;
}
