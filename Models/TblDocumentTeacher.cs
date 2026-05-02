using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Document_Teacher")]
public partial class TblDocumentTeacher
{
    [Key]
    [Column("Id_Document_Student")]
    public int IdDocumentStudent { get; set; }

    [Column("Teacher_Id")]
    public int TeacherId { get; set; }

    [Column("Document_Type_Id")]
    public int DocumentTypeId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Document { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("TblDocumentTeachers")]
    public virtual CatDocumentType DocumentType { get; set; } = null!;

    [ForeignKey("TeacherId")]
    [InverseProperty("TblDocumentTeachers")]
    public virtual CatTeacher Teacher { get; set; } = null!;
}
