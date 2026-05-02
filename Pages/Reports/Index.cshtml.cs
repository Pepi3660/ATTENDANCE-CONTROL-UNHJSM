using AttendanceControl.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System.Security.Claims;
using System.Text;

namespace AttendanceControl.Web.Pages.Reports
{
    [Authorize(Policy = "TeacherOrAdmin")]
    public class IndexModel : PageModel
    {
        private readonly AttendanceDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public IndexModel(AttendanceDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty(SupportsGet = true)]
        public DateOnly? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CareerId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? ShiftId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? TeacherId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? GroupId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SubjectId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? StudentId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? AttendanceStatusId { get; set; }

        public bool IsAdminUser => User.IsInRole("ADMIN");

        public List<SelectListItem> Careers { get; set; } = new();
        public List<SelectListItem> Shifts { get; set; } = new();
        public List<SelectListItem> Teachers { get; set; } = new();
        public List<SelectListItem> Groups { get; set; } = new();
        public List<SelectListItem> Subjects { get; set; } = new();
        public List<SelectListItem> Students { get; set; } = new();
        public List<SelectListItem> AttendanceStatuses { get; set; } = new();

        public List<ReportRow> Results { get; set; } = new();

        public int TotalRecords { get; set; }
        public int TotalPresent { get; set; }
        public int TotalLate { get; set; }
        public int TotalAbsent { get; set; }
        public int TotalMale { get; set; }
        public int TotalFemale { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetReportDataAsync();
            LoadSummary();

            return Page();
        }

        public async Task<IActionResult> OnGetExportCsvAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetReportDataAsync();
            LoadSummary();

            var csv = new StringBuilder();

            csv.AppendLine("Reporte General de Asistencia");
            csv.AppendLine($"Generado,{DateTime.Now:dd/MM/yyyy HH:mm}");
            csv.AppendLine($"Fecha inicial,{StartDate?.ToString("dd/MM/yyyy") ?? "Todas"}");
            csv.AppendLine($"Fecha final,{EndDate?.ToString("dd/MM/yyyy") ?? "Todas"}");
            csv.AppendLine($"Carrera,{GetSelectedText(Careers, CareerId) ?? "Todas"}");
            csv.AppendLine($"Turno,{GetSelectedText(Shifts, ShiftId) ?? "Todos"}");
            csv.AppendLine($"Docente,{GetSelectedText(Teachers, TeacherId) ?? (IsAdminUser ? "Todos" : "Docente actual")}");
            csv.AppendLine($"Grupo,{GetSelectedText(Groups, GroupId) ?? "Todos"}");
            csv.AppendLine($"Asignatura,{GetSelectedText(Subjects, SubjectId) ?? "Todas"}");
            csv.AppendLine($"Estudiante,{GetSelectedText(Students, StudentId) ?? "Todos"}");
            csv.AppendLine($"Estado,{GetSelectedText(AttendanceStatuses, AttendanceStatusId) ?? "Todos"}");
            csv.AppendLine();

            csv.AppendLine("Fecha,Carrera,Asignatura,Docente,Grupo,Turno,Area,Edificio,Aula,Carnet,Estudiante,Genero,Estado,Hora,Justificada,Observacion");

            foreach (var row in Results)
            {
                csv.AppendLine(
                    $"{EscapeCsv(row.ClassDate)}," +
                    $"{EscapeCsv(row.CareerName)}," +
                    $"{EscapeCsv(row.SubjectName)}," +
                    $"{EscapeCsv(row.TeacherName)}," +
                    $"{EscapeCsv(row.GroupName)}," +
                    $"{EscapeCsv(row.ShiftName)}," +
                    $"{EscapeCsv(row.AreaName)}," +
                    $"{EscapeCsv(row.BuildingName)}," +
                    $"{EscapeCsv(row.ClassroomName)}," +
                    $"{EscapeCsv(row.StudentCarnet)}," +
                    $"{EscapeCsv(row.StudentName)}," +
                    $"{EscapeCsv(row.Gender)}," +
                    $"{EscapeCsv(row.AttendanceStatus)}," +
                    $"{EscapeCsv(row.RegisterTime)}," +
                    $"{EscapeCsv(row.IsJustified)}," +
                    $"{EscapeCsv(row.Observation)}"
                );
            }

            csv.AppendLine();
            csv.AppendLine("Resumen");
            csv.AppendLine($"Total registros,{TotalRecords}");
            csv.AppendLine($"Presentes,{TotalPresent}");
            csv.AppendLine($"Tardes,{TotalLate}");
            csv.AppendLine($"Ausentes,{TotalAbsent}");
            csv.AppendLine($"Masculino,{TotalMale}");
            csv.AppendLine($"Femenino,{TotalFemale}");

            var bytes = Encoding.UTF8.GetPreamble()
                .Concat(Encoding.UTF8.GetBytes(csv.ToString()))
                .ToArray();

            var fileName = $"reporte_asistencia_{DateTime.Now:yyyyMMddHHmm}.csv";

            return File(bytes, "text/csv", fileName);
        }

        public async Task<IActionResult> OnGetExportPdfAsync()
        {
            await LoadCatalogsAsync();
            Results = await GetReportDataAsync();
            LoadSummary();

            QuestPDF.Settings.License = LicenseType.Community;

            var logoBytes = GetLogoBytes();

            var pdfBytes = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Margin(26);
                    page.Size(PageSizes.A4.Landscape());

                    page.Header().Column(header =>
                    {
                        header.Item().Row(row =>
                        {
                            if (logoBytes != null)
                                row.ConstantItem(180).Image(logoBytes).FitWidth();

                            row.RelativeItem().AlignMiddle().Column(col =>
                            {
                                col.Item().AlignCenter().Text("Reporte General de Asistencia")
                                    .Bold()
                                    .FontSize(16)
                                    .FontColor(Colors.Blue.Darken3);

                                col.Item().AlignCenter().Text("Universidad Nacional Héroes San José de las Mulas")
                                    .FontSize(10)
                                    .FontColor(Colors.Grey.Darken2);

                                col.Item().AlignCenter().Text($"Generado: {DateTime.Now:dd/MM/yyyy HH:mm}")
                                    .FontSize(8)
                                    .FontColor(Colors.Grey.Darken1);
                            });
                        });

                        header.Item().PaddingTop(8).LineHorizontal(1).LineColor(Colors.Blue.Darken3);
                    });

                    page.Content().PaddingVertical(10).Column(content =>
                    {
                        content.Item().Text("Datos generales del reporte")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(6).Element(container =>
                        {
                            container
                                .Background(Colors.Grey.Lighten4)
                                .Border(0.5f)
                                .BorderColor(Colors.Grey.Lighten2)
                                .Padding(10)
                                .Column(col =>
                                {
                                    col.Item().Text($"Período: {(StartDate?.ToString("dd/MM/yyyy") ?? "Todas")} - {(EndDate?.ToString("dd/MM/yyyy") ?? "Todas")}")
                                        .Bold()
                                        .FontSize(9);

                                    col.Item().Text($"Carrera: {GetSelectedText(Careers, CareerId) ?? "Todas"}")
                                        .FontSize(8);

                                    col.Item().Text($"Grupo: {GetSelectedText(Groups, GroupId) ?? "Todos"}")
                                        .FontSize(8);

                                    col.Item().Text($"Turno: {GetSelectedText(Shifts, ShiftId) ?? "Todos"}")
                                        .FontSize(8);

                                    col.Item().Text($"Docente: {GetSelectedText(Teachers, TeacherId) ?? (IsAdminUser ? "Todos" : "Docente actual")}")
                                        .FontSize(8);

                                    col.Item().Text($"Asignatura: {GetSelectedText(Subjects, SubjectId) ?? "Todas"}")
                                        .FontSize(8);

                                    col.Item().Text($"Estudiante: {GetSelectedText(Students, StudentId) ?? "Todos"}")
                                        .FontSize(8);
                                    col.Item().Text($"Estado: {GetSelectedText(AttendanceStatuses, AttendanceStatusId) ?? "Todos"}")
                                        .FontSize(8);
                                });
                        });

                        content.Item().PaddingTop(12).Text("Resumen")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Row(row =>
                        {
                            AddSummaryBox(row, "Total", TotalRecords.ToString(), Colors.Blue.Darken2, Colors.White);
                            AddSummaryBox(row, "Presentes", TotalPresent.ToString(), Colors.Green.Darken2, Colors.White);
                            AddSummaryBox(row, "Tardes", TotalLate.ToString(), Colors.Orange.Lighten1, Colors.Black);
                            AddSummaryBox(row, "Ausentes", TotalAbsent.ToString(), Colors.Red.Darken2, Colors.White);
                            AddSummaryBox(row, "Masculino", TotalMale.ToString(), Colors.Cyan.Darken2, Colors.White);
                            AddSummaryBox(row, "Femenino", TotalFemale.ToString(), Colors.Pink.Darken2, Colors.White);
                        });

                        content.Item().PaddingTop(15).Text("Listado de estudiantes")
                            .Bold()
                            .FontSize(11)
                            .FontColor(Colors.Blue.Darken3);

                        content.Item().PaddingTop(5).Table(table =>
                        {
                            table.ColumnsDefinition(columns =>
                            {
                                columns.ConstantColumn(55);
                                columns.RelativeColumn(1.1f);
                                columns.RelativeColumn(1.9f);
                                columns.ConstantColumn(45);
                                columns.RelativeColumn(1.0f);
                                columns.ConstantColumn(45);
                                columns.ConstantColumn(65);
                            });

                            table.Header(header =>
                            {
                                header.Cell().Element(HeaderCell).Text("Fecha");
                                header.Cell().Element(HeaderCell).Text("Carnet");
                                header.Cell().Element(HeaderCell).Text("Estudiante");
                                header.Cell().Element(HeaderCell).Text("Género");
                                header.Cell().Element(HeaderCell).Text("Estado");
                                header.Cell().Element(HeaderCell).Text("Hora");
                                header.Cell().Element(HeaderCell).Text("Justificada");
                            });

                            foreach (var item in Results)
                            {
                                table.Cell().Element(BodyCell).Text(item.ClassDate);
                                table.Cell().Element(BodyCell).Text(item.StudentCarnet);
                                table.Cell().Element(BodyCell).Text(item.StudentName);
                                table.Cell().Element(BodyCell).Text(item.Gender);
                                table.Cell().Element(BodyCell).Text(item.AttendanceStatus);
                                table.Cell().Element(BodyCell).Text(item.RegisterTime);
                                table.Cell().Element(BodyCell).Text(item.IsJustified);
                            }
                        });
                    });

                    page.Footer().AlignCenter().Text(text =>
                    {
                        text.Span("Página ");
                        text.CurrentPageNumber();
                        text.Span(" de ");
                        text.TotalPages();
                    });
                });
            }).GeneratePdf();

            var fileName = $"reporte_asistencia_{DateTime.Now:yyyyMMddHHmm}.pdf";

            return File(pdfBytes, "application/pdf", fileName);
        }

        public async Task<JsonResult> OnGetFilterOptionsAsync(int? careerId, int? shiftId, int? groupId, int? teacherId, int? subjectId)
        {
            var classesQuery = _context.TblClasses
                .Include(c => c.Group)
                    .ThenInclude(g => g.Career)
                .Include(c => c.Group)
                    .ThenInclude(g => g.Shift)
                .Include(c => c.Subject)
                .Include(c => c.Teacher)
                .Where(c => c.IsActive || c.ClassStatusId == 2)
                .AsQueryable();

            if (!IsAdminUser)
            {
                var currentTeacherId = await GetCurrentTeacherIdAsync();
                if (currentTeacherId == null)
                {
                    return new JsonResult(new
                    {
                        groups = Array.Empty<object>(),
                        shifts = Array.Empty<object>(),
                        subjects = Array.Empty<object>(),
                        students = Array.Empty<object>()
                    });
                }

                classesQuery = classesQuery.Where(c => c.TeacherId == currentTeacherId.Value);
            }
            else if (teacherId.HasValue)
            {
                classesQuery = classesQuery.Where(c => c.TeacherId == teacherId.Value);
            }

            if (careerId.HasValue)
                classesQuery = classesQuery.Where(c => c.Group.CareerId == careerId.Value);

            if (shiftId.HasValue)
                classesQuery = classesQuery.Where(c => c.Group.ShiftId == shiftId.Value);

            if (groupId.HasValue)
                classesQuery = classesQuery.Where(c => c.GroupId == groupId.Value);

            if (subjectId.HasValue)
                classesQuery = classesQuery.Where(c => c.SubjectId == subjectId.Value);

            var classIds = await classesQuery.Select(c => c.ClassId).ToListAsync();

            var groups = await classesQuery
                .Select(c => new
                {
                    value = c.GroupId.ToString(),
                    text = c.Group.Career.CareerName + " - " + c.Group.GroupName + " - " + c.Group.Shift.ShiftName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var shifts = await classesQuery
                .Select(c => new
                {
                    value = c.Group.ShiftId.ToString(),
                    text = c.Group.Shift.ShiftName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var subjects = await classesQuery
                .Select(c => new
                {
                    value = c.SubjectId.ToString(),
                    text = c.Subject.SubjectName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            var students = await _context.TblClassEnrollments
                .Include(e => e.Student)
                .Where(e => classIds.Contains(e.ClassId) && e.IsActive && e.Student.IsActive)
                .Select(e => new
                {
                    value = e.StudentId.ToString(),
                    text = e.Student.FirstName + " " + e.Student.LastName
                })
                .Distinct()
                .OrderBy(x => x.text)
                .ToListAsync();

            return new JsonResult(new
            {
                groups,
                shifts,
                subjects,
                students
            });
        }

        private async Task<List<ReportRow>> GetReportDataAsync()
        {
            var query = _context.TblAttendances
                .Include(a => a.Student)
                    .ThenInclude(s => s.TblDocumentStudents)
                        .ThenInclude(d => d.DocumentType)
                .Include(a => a.AttendanceStatus)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Teacher)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Subject)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Group)
                        .ThenInclude(g => g.Career)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Group)
                        .ThenInclude(g => g.Shift)
                .Include(a => a.Class)
                    .ThenInclude(c => c.Classroom)
                        .ThenInclude(cr => cr.Building)
                            .ThenInclude(b => b.Area)
                .Where(a => a.IsActive)
                .AsQueryable();

            if (!IsAdminUser)
            {
                var teacherId = await GetCurrentTeacherIdAsync();

                if (teacherId == null)
                    return new List<ReportRow>();

                query = query.Where(a => a.Class.TeacherId == teacherId.Value);
                TeacherId = null;
            }
            else if (TeacherId.HasValue)
            {
                query = query.Where(a => a.Class.TeacherId == TeacherId.Value);
            }

            if (StartDate.HasValue)
                query = query.Where(a => a.Class.ClassDate >= StartDate.Value);

            if (EndDate.HasValue)
                query = query.Where(a => a.Class.ClassDate <= EndDate.Value);

            if (CareerId.HasValue)
                query = query.Where(a => a.Class.Group.CareerId == CareerId.Value);

            if (ShiftId.HasValue)
                query = query.Where(a => a.Class.Group.ShiftId == ShiftId.Value);

            if (GroupId.HasValue)
                query = query.Where(a => a.Class.GroupId == GroupId.Value);

            if (SubjectId.HasValue)
                query = query.Where(a => a.Class.SubjectId == SubjectId.Value);

            if (StudentId.HasValue)
                query = query.Where(a => a.StudentId == StudentId.Value);

            if (AttendanceStatusId.HasValue)
                query = query.Where(a => a.AttendanceStatusId == AttendanceStatusId.Value);

            var data = await query
                .OrderByDescending(a => a.Class.ClassDate)
                .ThenBy(a => a.Class.StartTime)
                .ThenBy(a => a.Student.LastName)
                .ThenBy(a => a.Student.FirstName)
                .ToListAsync();

            return data.Select(a => new ReportRow
            {
                ClassDate = a.Class.ClassDate.ToString("dd/MM/yyyy"),
                CareerName = a.Class.Group.Career.CareerName,
                SubjectName = a.Class.Subject.SubjectName,
                TeacherName = a.Class.Teacher.FirstName + " " + a.Class.Teacher.LastName,
                GroupName = a.Class.Group.GroupName,
                ShiftName = a.Class.Group.Shift.ShiftName,
                AreaName = a.Class.Classroom.Building.Area.Initial,
                BuildingName = a.Class.Classroom.Building.BuildingName,
                ClassroomName = a.Class.Classroom.ClassroomName,
                StudentCarnet = GetStudentCarnet(a.Student),
                StudentName = a.Student.FirstName + " " + a.Student.LastName,
                Gender = a.Student.Gender,
                AttendanceStatusId = a.AttendanceStatusId,
                AttendanceStatus = a.AttendanceStatus.AttendanceStatusName,
                RegisterTime = a.RegisterTime.ToString("HH:mm"),
                IsJustified = a.IsJustified ? "Sí" : "No",
                Observation = a.Observation ?? ""
            }).ToList();
        }

        private async Task LoadCatalogsAsync()
        {
            Careers = await _context.CatCareers
                .Where(c => c.IsActive)
                .OrderBy(c => c.CareerName)
                .Select(c => new SelectListItem
                {
                    Value = c.CareerId.ToString(),
                    Text = c.CareerName
                })
                .ToListAsync();

            Shifts = await _context.CatShifts
                .Where(s => s.IsActive)
                .OrderBy(s => s.ShiftName)
                .Select(s => new SelectListItem
                {
                    Value = s.ShiftId.ToString(),
                    Text = s.ShiftName
                })
                .ToListAsync();

            Teachers = IsAdminUser
                ? await _context.CatTeachers
                    .Where(t => t.IsActive)
                    .OrderBy(t => t.FirstName)
                    .Select(t => new SelectListItem
                    {
                        Value = t.TeacherId.ToString(),
                        Text = t.FirstName + " " + t.LastName
                    })
                    .ToListAsync()
                : new List<SelectListItem>();

            Groups = await _context.CatGroups
                .Include(g => g.Career)
                .Include(g => g.Shift)
                .Where(g => g.IsActive)
                .OrderBy(g => g.Career.CareerName)
                .ThenBy(g => g.GroupName)
                .Select(g => new SelectListItem
                {
                    Value = g.GroupId.ToString(),
                    Text = g.GroupName
                    //Text = g.Career.CareerName + " - " + g.GroupName + " - " + g.Shift.ShiftName
                })
                .ToListAsync();

            Subjects = await _context.CatSubjects
                .Where(s => s.IsActive)
                .OrderBy(s => s.SubjectName)
                .Select(s => new SelectListItem
                {
                    Value = s.SubjectId.ToString(),
                    Text = s.SubjectName
                })
                .ToListAsync();

            Students = await _context.CatStudents
                .Where(s => s.IsActive)
                .OrderBy(s => s.LastName)
                .ThenBy(s => s.FirstName)
                .Select(s => new SelectListItem
                {
                    Value = s.StudentId.ToString(),
                    Text = s.FirstName + " " + s.LastName
                })
                .ToListAsync();

            AttendanceStatuses = await _context.CatAttendanceStatuses
                .Where(s => s.IsActive)
                .OrderBy(s => s.AttendanceStatusId)
                .Select(s => new SelectListItem
                {
                    Value = s.AttendanceStatusId.ToString(),
                    Text = s.AttendanceStatusName
                })
                .ToListAsync();
        }

        private void LoadSummary()
        {
            TotalRecords = Results.Count;
            TotalPresent = Results.Count(x => x.AttendanceStatusId == 1);
            TotalLate = Results.Count(x => x.AttendanceStatusId == 2);
            TotalAbsent = Results.Count(x => x.AttendanceStatusId == 3);
            TotalMale = Results.Count(x => x.Gender == "M");
            TotalFemale = Results.Count(x => x.Gender == "F");
        }

        private static string GetStudentCarnet(CatStudent student)
        {
            return student.TblDocumentStudents
                .FirstOrDefault(d =>
                    d.IsActive &&
                    d.DocumentType.DocumentType.ToLower().Contains("carnet"))?.Document
                ?? "Sin carnet";
        }

        private async Task<int?> GetCurrentTeacherIdAsync()
        {
            var userIdValue = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (!int.TryParse(userIdValue, out var userId))
                return null;

            return await _context.CatUsers
                .Where(u => u.IdUser == userId && u.IsActive)
                .Select(u => (int?)u.TeacherId)
                .FirstOrDefaultAsync();
        }

        private byte[]? GetLogoBytes()
        {
            var logoPath = Path.Combine(
                _environment.WebRootPath,
                "images",
                "log_UNHSJM_Texto.jpg"
            );

            if (!System.IO.File.Exists(logoPath))
                return null;

            return System.IO.File.ReadAllBytes(logoPath);
        }

        private static string? GetSelectedText(List<SelectListItem> items, int? selectedValue)
        {
            if (!selectedValue.HasValue)
                return null;

            return items.FirstOrDefault(x => x.Value == selectedValue.Value.ToString())?.Text;
        }

        private static void AddSummaryBox(RowDescriptor row, string label, string value, string background, string foreground)
        {
            row.RelativeItem()
                .PaddingRight(5)
                .Background(background)
                .Padding(7)
                .Column(col =>
                {
                    col.Item().Text(label).FontColor(foreground).FontSize(8);
                    col.Item().Text(value).FontColor(foreground).Bold().FontSize(12);
                });
        }

        private static void AddInfoRow(TableDescriptor table, string label1, string value1, string label2, string value2)
        {
            table.Cell().Element(InfoLabelCell).Text(label1);
            table.Cell().Element(InfoValueCell).Text(value1);
            table.Cell().Element(InfoLabelCell).Text(label2);
            table.Cell().Element(InfoValueCell).Text(value2);
        }

        private static QuestPDF.Infrastructure.IContainer InfoLabelCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(Colors.Grey.Lighten3)
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(5)
                .DefaultTextStyle(x => x.Bold().FontSize(8));
        }

        private static QuestPDF.Infrastructure.IContainer InfoValueCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten1)
                .Padding(5)
                .DefaultTextStyle(x => x.FontSize(8));
        }

        private static QuestPDF.Infrastructure.IContainer HeaderCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Background(Colors.Blue.Darken3)
                .Padding(4)
                .DefaultTextStyle(x => x.FontColor(Colors.White).Bold().FontSize(7));
        }

        private static QuestPDF.Infrastructure.IContainer BodyCell(QuestPDF.Infrastructure.IContainer container)
        {
            return container
                .Border(0.5f)
                .BorderColor(Colors.Grey.Lighten2)
                .Padding(4)
                .DefaultTextStyle(x => x.FontSize(7));
        }

        private static string EscapeCsv(string value)
        {
            value ??= "";

            if (value.Contains(",") || value.Contains("\"") || value.Contains("\n"))
                return $"\"{value.Replace("\"", "\"\"")}\"";

            return value;
        }

        public class ReportRow
        {
            public string ClassDate { get; set; } = "";
            public string CareerName { get; set; } = "";
            public string SubjectName { get; set; } = "";
            public string TeacherName { get; set; } = "";
            public string GroupName { get; set; } = "";
            public string ShiftName { get; set; } = "";
            public string AreaName { get; set; } = "";
            public string BuildingName { get; set; } = "";
            public string ClassroomName { get; set; } = "";
            public string StudentCarnet { get; set; } = "";
            public string StudentName { get; set; } = "";
            public string Gender { get; set; } = "";
            public int AttendanceStatusId { get; set; }
            public string AttendanceStatus { get; set; } = "";
            public string RegisterTime { get; set; } = "";
            public string IsJustified { get; set; } = "";
            public string Observation { get; set; } = "";
        }
    }
}