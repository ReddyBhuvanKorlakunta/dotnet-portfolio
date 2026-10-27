using Microsoft.EntityFrameworkCore.Migrations;
namespace EnterprisePortal.Infrastructure.Migrations;
/// <summary>Initial migration, hand-checked. Generated with:
/// dotnet ef migrations add Initial --project Infrastructure --startup-project Web
/// then reviewed before commit — never auto-apply destructive migrations.</summary>
public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder b)
    {
        b.CreateTable(name: "Cases", columns: t => new
        {
            Id = t.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            Reference = t.Column<string>(maxLength: 50, nullable: false),
            Title = t.Column<string>(maxLength: 200, nullable: false),
            Description = t.Column<string>(maxLength: 2000, nullable: false),
            Type = t.Column<int>(nullable: false),
            Status = t.Column<int>(nullable: false),
            Priority = t.Column<int>(nullable: false),
            ApplicantName = t.Column<string>(maxLength: 200, nullable: false),
            ApplicantEmail = t.Column<string>(maxLength: 200, nullable: false),
            EncryptedNotes = t.Column<string>(nullable: true),
            AssignedTo = t.Column<string>(nullable: true),
            RejectionReason = t.Column<string>(nullable: true),
            CreatedAt = t.Column<DateTime>(nullable: false),
            UpdatedAt = t.Column<DateTime>(nullable: true),
            CreatedBy = t.Column<string>(nullable: false),
            IsDeleted = t.Column<bool>(nullable: false),
            RowVersion = t.Column<byte[]>(rowVersion: true, nullable: true),
        }, constraints: t => t.PrimaryKey("PK_Cases", x => x.Id));
        b.CreateIndex("IX_Cases_Reference", "Cases", "Reference", unique: true);
        b.CreateIndex("IX_Cases_Status", "Cases", "Status");
        b.CreateTable(name: "CaseNotes", columns: t => new
        {
            Id = t.Column<int>(nullable: false).Annotation("SqlServer:Identity", "1, 1"),
            CaseId = t.Column<int>(nullable: false),
            Author = t.Column<string>(nullable: false),
            Text = t.Column<string>(nullable: false),
            CreatedAt = t.Column<DateTime>(nullable: false),
            UpdatedAt = t.Column<DateTime>(nullable: true),
            CreatedBy = t.Column<string>(nullable: false),
            IsDeleted = t.Column<bool>(nullable: false),
            RowVersion = t.Column<byte[]>(rowVersion: true, nullable: true),
        }, constraints: t => { t.PrimaryKey("PK_CaseNotes", x => x.Id); t.ForeignKey("FK_CaseNotes_Cases_CaseId", x => x.CaseId, "Cases", "Id", onDelete: ReferentialAction.Cascade); });
    }
    protected override void Down(MigrationBuilder b)
    {
        b.DropTable("CaseNotes"); b.DropTable("Cases");
    }
}
