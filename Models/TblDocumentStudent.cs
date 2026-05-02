using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Tbl_Document_Student")]
public partial class TblDocumentStudent
{
    [Key]
    [Column("Id_Document_Student")]
    public int IdDocumentStudent { get; set; }

    [Column("Student_Id")]
    public int StudentId { get; set; }

    [Column("Document_Type_Id")]
    public int DocumentTypeId { get; set; }

    [StringLength(20)]
    [Unicode(false)]
    public string Document { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [ForeignKey("DocumentTypeId")]
    [InverseProperty("TblDocumentStudents")]
    public virtual CatDocumentType DocumentType { get; set; } = null!;

    [ForeignKey("StudentId")]
    [InverseProperty("TblDocumentStudents")]
    public virtual CatStudent Student { get; set; } = null!;
}
