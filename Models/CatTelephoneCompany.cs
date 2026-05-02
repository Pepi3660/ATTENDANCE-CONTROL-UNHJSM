using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Telephone_Company")]
public partial class CatTelephoneCompany
{
    [Key]
    [Column("Id_Telephone_Company")]
    public int IdTelephoneCompany { get; set; }

    [StringLength(100)]
    [Unicode(false)]
    public string Company { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("IdTelephoneCompanyNavigation")]
    public virtual ICollection<TblPhoneStudent> TblPhoneStudents { get; set; } = new List<TblPhoneStudent>();

    [InverseProperty("IdTelephoneCompanyNavigation")]
    public virtual ICollection<TblPhoneTeacher> TblPhoneTeachers { get; set; } = new List<TblPhoneTeacher>();
}
