using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace AttendanceControl.Web.Models;

[Table("Cat_Document_Type")]
public partial class CatDocumentType
{
    [Key]
    [Column("Document_Type_Id")]
    public int DocumentTypeId { get; set; }

    [Column("Document_Type")]
    [StringLength(50)]
    [Unicode(false)]
    public string DocumentType { get; set; } = null!;

    public bool IsActive { get; set; }

    [Column(TypeName = "datetime")]
    public DateTime CreatedDate { get; set; }

    [InverseProperty("DocumentType")]
    public virtual ICollection<TblDocumentStudent> TblDocumentStudents { get; set; } = new List<TblDocumentStudent>();

    [InverseProperty("DocumentType")]
    public virtual ICollection<TblDocumentTeacher> TblDocumentTeachers { get; set; } = new List<TblDocumentTeacher>();
}
