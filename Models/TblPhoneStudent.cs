using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Phone_Student")]
public partial class TblPhoneStudent
{
    [Key]
    [Column("Id_Phone_Student")]
    public int IdPhoneStudent { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    [Column("Id_Telephone_Company")]
    public int IdTelephoneCompany { get; set; }

    [StringLength(30)]
    [Unicode(false)]
    public string Phone { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("IdTelephoneCompany")]
    [InverseProperty("TblPhoneStudents")]
    public virtual CatTelephoneCompany IdTelephoneCompanyNavigation { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TblPhoneStudents")]
    public virtual CatStudent Student { get; set; } = null!;
}
