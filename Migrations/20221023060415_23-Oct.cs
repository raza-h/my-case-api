using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyCaseApi.Migrations
{
    public partial class _23Oct : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AdminActivity",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OperationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ActionDetail = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminActivity", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ArchiveContact",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactOne = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactTwo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GroupId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ArchiveContact", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    VerificationStatus = table.Column<int>(type: "int", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseRate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserTitleId = table.Column<int>(type: "int", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedUserName = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: true),
                    EmailConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecurityStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumberConfirmed = table.Column<bool>(type: "bit", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "bit", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "bit", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BillingMethod",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BillingMethod", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CaseDetail",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseNumber = table.Column<int>(type: "int", nullable: true),
                    PracticeArea = table.Column<int>(type: "int", nullable: true),
                    CaseStage = table.Column<int>(type: "int", nullable: true),
                    DateAppend = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Office = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatueOfLimitation = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ConflictCheckNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseRate = table.Column<int>(type: "int", nullable: true),
                    BillingContact = table.Column<int>(type: "int", nullable: true),
                    BillingMethod = table.Column<int>(type: "int", nullable: true),
                    LeadAttorney = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OriginatingLeadAttorney = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    CaseAddedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CaseDetail", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ChatGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AdminId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ChatGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ClientTransaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    IsCash = table.Column<bool>(type: "bit", nullable: false),
                    IsCredit = table.Column<bool>(type: "bit", nullable: false),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckAmount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientTransaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Company",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Website = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MainPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    PrivateNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Company", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contact",
                columns: table => new
                {
                    ContactId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MidName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsClientEnable = table.Column<bool>(type: "bit", nullable: false),
                    ContactGroupId = table.Column<int>(type: "int", nullable: false),
                    CellPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true),
                    CountryId = table.Column<int>(type: "int", nullable: true),
                    JobTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WebSite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    DrivingLicense = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DrivingLicenseState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FaxNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contact", x => x.ContactId);
                });

            migrationBuilder.CreateTable(
                name: "ContactGroup",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ContactGroupName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactUs",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactUs", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "Country",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsoCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Country", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Decuments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DecumentTags = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecumentTittle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecumentType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecumentDescripation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DecumentPath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    UserType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decuments", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DocumentTagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    EventId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Start = table.Column<DateTime>(type: "datetime2", nullable: true),
                    End = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThemeColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllDay = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.EventId);
                });

            migrationBuilder.CreateTable(
                name: "Expenses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpenseType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckTitle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CheckDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CheckImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsCash = table.Column<bool>(type: "bit", nullable: false),
                    IsCredit = table.Column<bool>(type: "bit", nullable: false),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Expenses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Faq",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Question = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Answer = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faq", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FinancialDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ParentId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialDetails", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Firm",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RegistrationNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NumberofEmployees = table.Column<int>(type: "int", nullable: false),
                    FirmWebsite = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Firm", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirmOffice",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmId = table.Column<int>(type: "int", nullable: false),
                    OfficeName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsPrimary = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmOffice", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FirmOfficeAddress",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirmOfficeId = table.Column<int>(type: "int", nullable: false),
                    Country = table.Column<int>(type: "int", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirmOfficeAddress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GroupUser",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsArchive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GroupUser", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Lead",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MidName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CellPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    WorkPhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    HomePhone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ZipCode = table.Column<int>(type: "int", nullable: true),
                    Country = table.Column<int>(type: "int", nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DriverLicence = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DriverLicenceState = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LeadDetails = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateAdded = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: false),
                    PracticeAreaId = table.Column<int>(type: "int", nullable: true),
                    PotentialValueCase = table.Column<int>(type: "int", nullable: true),
                    AssignTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Office = table.Column<int>(type: "int", nullable: false),
                    PotentailCaseDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ConflictCheck = table.Column<bool>(type: "bit", nullable: false),
                    ConflictCheckNotes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefferelSource = table.Column<int>(type: "int", nullable: true),
                    ContactId = table.Column<int>(type: "int", nullable: true),
                    CompanyId = table.Column<int>(type: "int", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Lead", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Message",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SenderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ReceiverId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    IsGroupMessage = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Message", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "News",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NewsTittle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NewsType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SendTo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Time = table.Column<DateTime>(type: "datetime2", nullable: true),
                    status = table.Column<bool>(type: "bit", nullable: false),
                    ExpireDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_News", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Notes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotesTag = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesTittle = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NotesDescripation = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedDate = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Notes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NotesTag",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    NotesTagName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotesTag", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "packageService",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ServiceId = table.Column<int>(type: "int", nullable: true),
                    PricePlanId = table.Column<int>(type: "int", nullable: true),
                    IsSeleced = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_packageService", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Payment",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    SubsciptionId = table.Column<int>(type: "int", nullable: true),
                    FromScreen = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Payment", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PracticeArea",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PracticeAreaName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PracticeArea", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PricePlan",
                columns: table => new
                {
                    PlanID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PriceRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TimeRange = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: true),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PricePlan", x => x.PlanID);
                });

            migrationBuilder.CreateTable(
                name: "RefferalSource",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RefferalSourceName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefferalSource", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RequestUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PostCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanID = table.Column<int>(type: "int", nullable: true),
                    VerificationStatus = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RequestUsers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Service",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Service", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "States",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_States", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tasks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Priority = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StaffId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tasks", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TimeLine",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DueDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ClientId = table.Column<int>(type: "int", nullable: true),
                    FirmId = table.Column<int>(type: "int", nullable: true),
                    TimeLineType = table.Column<int>(type: "int", nullable: false),
                    HostLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JoinLink = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MeetingTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsReminder = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TimeLine", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transaction",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    InvoiceNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    transactionType = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DetailAccountId = table.Column<int>(type: "int", nullable: false),
                    ClosingBalance = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountType = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transaction", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserAgainstCase",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CaseId = table.Column<int>(type: "int", nullable: true),
                    BillingType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Rate = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAgainstCase", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserGroupMessage",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    GroupId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsRead = table.Column<bool>(type: "bit", nullable: false),
                    CreatedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserGroupMessage", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSubcription",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PricePlanId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    PaymentStatus = table.Column<int>(type: "int", nullable: false),
                    Paymenttype = table.Column<int>(type: "int", nullable: false),
                    CreationTime = table.Column<TimeSpan>(type: "time", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSubcription", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserTitle",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserTitleName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTitle", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserVerification",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AutoApproval = table.Column<bool>(type: "bit", nullable: false),
                    AdminApproval = table.Column<bool>(type: "bit", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentId = table.Column<int>(type: "int", nullable: true),
                    PaymentType = table.Column<int>(type: "int", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserVerification", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetRoleClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ClaimType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClaimValue = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserClaims", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AspNetUserClaims_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderKey = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    ProviderDisplayName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserLogins", x => new { x.LoginProvider, x.ProviderKey });
                    table.ForeignKey(
                        name: "FK_AspNetUserLogins_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    RoleId = table.Column<string>(type: "nvarchar(450)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AspNetRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AspNetUserRoles_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LoginProvider = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AspNetUserTokens", x => new { x.UserId, x.LoginProvider, x.Name });
                    table.ForeignKey(
                        name: "FK_AspNetUserTokens_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "a7251255-75fc-4b74-a1a2-dcff91eb1bb2", "7fa1a159-2c38-4205-9125-34db975e9c3b", "Admin", "ADMIN" },
                    { "f4e833a7-e496-4448-b762-90e0630a6f91", "f454d783-28c1-4ded-b54d-7403070c7b1f", "Customer", "CUSTOMER" },
                    { "c0fd0c4f-1fdf-4ad0-9594-320e9dbd2b15", "035844f6-e408-418e-b51b-99f986f9a8e2", "Attorney", "ATTORNEY" },
                    { "f19c181c-c43d-42a2-aa90-e06bf7ac33ba", "81474c2e-36b2-4cb4-8367-3633b5b10848", "Client", "CLIENT" },
                    { "33ea1ca0-5973-4eac-a242-94ac321bb916", "04bc9749-050e-4a3a-8b64-684689412755", "Staff", "STAFF" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 166, "PAN", "Panama" },
                    { 165, "PSE", "Palestinian Territory" },
                    { 164, "PLW", "Palau" },
                    { 163, "PAK", "Pakistan" },
                    { 162, "OMN", "Oman" },
                    { 159, "NFK", "Norfolk Island" },
                    { 160, "MNP", "Northern Mariana Islands" },
                    { 167, "PNG", "Papua New Guinea" },
                    { 158, "NIU", "Niue" },
                    { 157, "NER", "Niger" },
                    { 156, "NIC", "Nicaragua" },
                    { 161, "NOR", "Norway" },
                    { 168, "PRY", "Paraguay" },
                    { 170, "PHL", "Philippines" },
                    { 155, "NZL", "New Zealand" },
                    { 171, "PCN", "Pitcairn" },
                    { 172, "POL", "Poland" },
                    { 173, "PRT", "Portugal" },
                    { 174, "PRI", "Puerto Rico" },
                    { 175, "QAT", "Qatar" },
                    { 176, "REU", "Reunion" },
                    { 177, "ROU", "Romania" },
                    { 178, "RUS", "Russian Federation" },
                    { 179, "RWA", "Rwanda" },
                    { 180, "BLM", "Saint-Barthelemy" },
                    { 181, "SHN", "Saint Helena" },
                    { 169, "PER", "Peru" },
                    { 154, "NCL", "New Caledonia" },
                    { 150, "NRU", "Nauru" },
                    { 151, "NPL", "Nepal" },
                    { 124, "LIE", "Liechtenstein" },
                    { 125, "LTU", "Lithuania" },
                    { 126, "LUX", "Luxembourg" },
                    { 127, "MKD", "Macedonia" },
                    { 128, "MDG", "Madagascar" },
                    { 129, "MWI", "Malawi" },
                    { 130, "MDV", "Malaysia" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 131, "MKD", "Maldives" },
                    { 132, "MLI", "Mali" },
                    { 133, "MLT", "Malta" },
                    { 134, "MHL", "Marshall Islands" },
                    { 135, "MTQ", "Martinique" },
                    { 136, "MRT", "Mauritania" },
                    { 137, "MUS", "Mauritius" },
                    { 138, "MYT", "Mayotte" },
                    { 139, "MEX", "Mexico" },
                    { 140, "FSM", "Micronesia" },
                    { 141, "MDA", "Moldova" },
                    { 142, "MCO", "Monaco" },
                    { 143, "MNG", "Mongolia" },
                    { 144, "MNE", "Montenegro" },
                    { 145, "MSR", "Montserrat" },
                    { 146, "MAR", "Morocco" },
                    { 147, "MOZ", "Mozambique" },
                    { 148, "MMR", "Myanmar" },
                    { 149, "NAM", "Namibia" },
                    { 182, "KNA", "Saint Kitts and Nevis" },
                    { 153, "ANT", "Netherlands Antilles" },
                    { 183, "LCA", "Saint Lucia" },
                    { 185, "SPM", "Saint Pierre and Miquelon" },
                    { 123, "LBY", "Libya" },
                    { 217, "TGO", "Togo" },
                    { 218, "TKL", "Tokelau" },
                    { 219, "TON", "Tonga" },
                    { 220, "TTO", "Trinidad and Tobago" },
                    { 221, "TUN", "Tunisia" },
                    { 222, "TUR", "Turkey" },
                    { 223, "TKM", "Turkmenistan" },
                    { 224, "TCA", "Turks and Caicos Islands" },
                    { 225, "TUV", "Tuvalu" },
                    { 226, "UGA", "Uganda" },
                    { 227, "UKR", "Ukraine" },
                    { 228, "ARE", "United Arab Emirates" },
                    { 216, "TLS", "Timor-Leste" },
                    { 229, "GBR", "United Kingdom" },
                    { 231, "UMI", "US Minor Outlying Islands" },
                    { 232, "URY", "Uruguay" },
                    { 233, "UZB", "Uzbekistan" },
                    { 234, "VUT", "Vanuatu" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 235, "VEN", "Venezuela" },
                    { 236, "VNM", "Viet Nam" },
                    { 237, "VIR", "Virgin Islands" },
                    { 238, "WLF", "Wallis and Futuna Islands" },
                    { 239, "ESH", "Western Sahara" },
                    { 240, "YEM", "Yemen" },
                    { 241, "ZMB", "Zambia" },
                    { 242, "ZWE", "Zimbabwe" },
                    { 230, "USA", "United States of America" },
                    { 215, "THA", "Thailand" },
                    { 214, "TZA", "Tanzania" },
                    { 213, "TJK", "Tajikistan" },
                    { 186, "VCT", "Saint Vincent and Grenadines" },
                    { 187, "WSM", "Samoa" },
                    { 188, "SMR", "San Marino" },
                    { 189, "STP", "Sao Tome and Principe" },
                    { 190, "SAU", "Saudi Arabia" },
                    { 191, "SEN", "Senegal" },
                    { 192, "SRB", "Serbia" },
                    { 193, "SYC", "Seychelles" },
                    { 194, "SLE", "Sierra Leone" },
                    { 195, "SGP", "Singapore" },
                    { 196, "SVK", "Slovakia" },
                    { 197, "SVN", "Slovenia" },
                    { 198, "SLB", "Solomon Islands" },
                    { 199, "SOM", "Somalia" },
                    { 200, "ZAF", "South Africa" },
                    { 201, "SGS", "South Georgia" },
                    { 202, "ESP", "South Sudan" },
                    { 203, "SVN", "Spain" },
                    { 204, "LKA", "Sri Lanka" },
                    { 205, "SDN", "Sudan" },
                    { 206, "SUR", "Suriname" },
                    { 207, "SJM", "Svalbard and Jan Mayen Islands" },
                    { 208, "SWZ", "Swaziland" },
                    { 209, "SWE", "Sweden" },
                    { 210, "CHE", "Switzerland" },
                    { 211, "SYR", "Syria" },
                    { 212, "TWN", "Taiwan" },
                    { 184, "MAF", "Saint-Martin" },
                    { 122, "LBR", "Liberia" },
                    { 152, "NLD", "Netherlands" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 120, "LBN", "Lebanon" },
                    { 32, "VGB", "British Virgin Islands" },
                    { 33, "IOT", "British Indian Ocean Territory" },
                    { 34, "BRN", "Brunei Darussalam" },
                    { 35, "BGR", "Bulgaria" },
                    { 36, "BFA", "Burkina Faso" },
                    { 37, "BDI", "Burundi" },
                    { 38, "KHM", "Cambodia" },
                    { 39, "CMR", "Cameroon" },
                    { 40, "CAN", "Canada" },
                    { 41, "CPV", "Cape Verde" },
                    { 42, "CYM", "Cayman Islands" },
                    { 43, "CAF", "Central African Republic" },
                    { 44, "TCD", "Chad" },
                    { 45, "CHL", "Chile" },
                    { 46, "CHN", "China" },
                    { 47, "DOM", "Hong Kong" },
                    { 48, "MAC", "Macao" },
                    { 49, "CXR", "Christmas Island" },
                    { 50, "CCK", "Cocos (Keeling) Islands" },
                    { 51, "COL", "Colombia" },
                    { 52, "COM", "Comoros" },
                    { 53, "COG", "Congo (Brazzaville)" },
                    { 54, "COD", "Congo (Kinshasa)" },
                    { 55, "COK", "Cook Islands" },
                    { 56, "CRI", "Costa Rica" },
                    { 57, "CIV", "Cote d'Ivoire" },
                    { 58, "HRV", "Croatia" },
                    { 31, "BRA", "Brazil" },
                    { 59, "CUB", "Cuba" },
                    { 30, "BVT", "Bouvet Island" },
                    { 121, "LSO", "Lesotho" },
                    { 1, "AFG", "Afghanistan" },
                    { 2, "ALA", "Aland Islands" },
                    { 3, "ALB", "Albania" },
                    { 4, "DZA", "Algeria" },
                    { 5, "ASM", "American Samoa" },
                    { 6, "AND", "Andorra" },
                    { 7, "AGO", "Angola" },
                    { 8, "AIA", "Anguilla" },
                    { 9, "ATA", "Antarctica" },
                    { 10, "ATG", "Antigua and Barbuda" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 11, "ARG", "Argentina" },
                    { 12, "ARM", "Armenia" },
                    { 13, "ABW", "Aruba" },
                    { 14, "AUS", "Australia" },
                    { 15, "AUT", "Austria" },
                    { 16, "AZE", "Azerbaijan" },
                    { 17, "BHS", "Bahamas" },
                    { 18, "BHR", "Bahrain" },
                    { 19, "BGD", "Bangladesh" },
                    { 20, "BRB", "Barbados" },
                    { 21, "BLR", "Belarus" },
                    { 22, "BEL", "Belgium" },
                    { 23, "BLZ", "Belize" },
                    { 24, "BEN", "Benin" },
                    { 25, "BMU", "Bermuda" },
                    { 26, "BTN", "Bhutan" },
                    { 27, "BOL", "Bolivia" },
                    { 29, "BWA", "Botswana" },
                    { 60, "CYP", "Cyprus" },
                    { 28, "BIH", "Bosnia and Herzegovina" },
                    { 62, "DNK", "Denmark" },
                    { 94, "HMD", "Heard and Mcdonald Islands" },
                    { 95, "VAT", "Holy See (Vatican City State)" },
                    { 96, "HND", "Honduras" },
                    { 97, "HUN", "Hungary" },
                    { 98, "ISL", "Iceland" },
                    { 99, "IND", "India" },
                    { 100, "IDN", "Indonesia" },
                    { 101, "IRN", "Iran" },
                    { 102, "IRQ", "Iraq" },
                    { 103, "IRL", "Ireland" },
                    { 104, "IMN", "Isle of Man" },
                    { 105, "ISR", "Israel" },
                    { 93, "HTI", "Haiti" },
                    { 106, "ITA", "Italy" },
                    { 108, "JPN", "Japan" },
                    { 109, "JEY", "Jersey" },
                    { 110, "JOR", "Jordan" },
                    { 111, "KAZ", "Kazakhstan" },
                    { 112, "KEN", "Kenya" },
                    { 113, "KIR", "Kiribati" },
                    { 114, "PRK", "Korea (North)" }
                });

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "Id", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 115, "KOR", "Korea (South)" },
                    { 116, "KWT", "Kuwait" },
                    { 117, "KGZ", "Kyrgyzstan" },
                    { 118, "LAO", "Lao PDR" },
                    { 119, "LVA", "Latvia" },
                    { 107, "JAM", "Jamaica" },
                    { 92, "GUY", "Guyana" },
                    { 91, "GNB", "Guinea-Bissau" },
                    { 76, "GUF", "French Guiana" },
                    { 63, "DJI", "Djibouti" },
                    { 64, "DMA", "Dominica" },
                    { 65, "DOM", "Dominican Republic" },
                    { 66, "ECU", "Ecuador" },
                    { 67, "EGY", "Egypt" },
                    { 68, "GNQ", "Equatorial Guinea" },
                    { 69, "ERI", "Eritrea" },
                    { 70, "EST", "Estonia" },
                    { 71, "FLK", "Falkland Islands" },
                    { 72, "FRO", "Faroe Islands" },
                    { 73, "FJI", "Fiji" },
                    { 74, "FIN", "Finland" },
                    { 75, "FRA", "France" },
                    { 61, "CZE", "Czech Republic" },
                    { 78, "ATF", "French Southern Territories" },
                    { 79, "GAB", "Gabon" },
                    { 80, "GMB", "Gambia" },
                    { 81, "GHA", "Georgia" },
                    { 82, "GAB", "Ghana" },
                    { 83, "GIB", "Gibraltar" },
                    { 84, "GRC", "Greece" },
                    { 85, "GRD", "Grenada" },
                    { 86, "GLP", "Guadeloupe" },
                    { 87, "GUM", "Guam" },
                    { 88, "GTM", "Guatemala" },
                    { 89, "GGY", "Guernsey" },
                    { 90, "GIN", "Guinea" },
                    { 77, "PYF", "French Polynesia" }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 6, "Notes Handling" },
                    { 5, "Documents Handling" },
                    { 4, "Reporting" },
                    { 2, "Timeline" },
                    { 3, "Calendar Events" }
                });

            migrationBuilder.InsertData(
                table: "Service",
                columns: new[] { "Id", "Name" },
                values: new object[] { 1, "Unlimited Messages" });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 59, "MP", "Northern Mariana Islands" },
                    { 32, "NJ", "New Jersey" },
                    { 33, "NM", "New Mexico" },
                    { 34, "AV", "Nevada" },
                    { 35, "AY", "New York" },
                    { 36, "OH", "Ohio" },
                    { 37, "OK", "Oklahoma" },
                    { 38, "OR", "Oregon" },
                    { 39, "PA", "Pennsylvania" },
                    { 40, "RI", "Rhode Island" },
                    { 41, "SC", "South Carolina" },
                    { 42, "SD", "South Dakota" },
                    { 43, "TN", "Tennessee" },
                    { 44, "TX", "Texas" },
                    { 45, "UT", "Utah" },
                    { 47, "VT", "Vermont" },
                    { 48, "WA", "Washington" },
                    { 49, "WI", "Wisconsin" },
                    { 50, "WV", "West Virginia" },
                    { 51, "WY", "Wyoming" },
                    { 52, "AA", "U.S. Armed Forces Americas" },
                    { 53, "AE", "U.S. Armed Forces Europe" },
                    { 54, "AP", "U.S. Armed Forces Pacific" },
                    { 55, "AS", "American Samoa" },
                    { 56, "FM", "Micronesia" },
                    { 57, "GU", "Guam" },
                    { 58, "MH", "Marshall Islands" },
                    { 31, "NH", "New Hampshire" },
                    { 46, "VA", "Virginia" },
                    { 29, "ND", "North Dakota" },
                    { 28, "NC", "North Carolina" },
                    { 60, "PR", "Puerto Rico" },
                    { 1, "AK", "Alaska" },
                    { 2, "AL", "Alabama" },
                    { 3, "AR", "Arkansas" },
                    { 4, "AZ", "Arizona" },
                    { 5, "CO", "California" },
                    { 6, "CA", "Colorado" },
                    { 7, "CT", "Connecticut" },
                    { 8, "DC", "Washington, D.C." },
                    { 9, "DE", "Delaware" }
                });

            migrationBuilder.InsertData(
                table: "States",
                columns: new[] { "Id", "Code", "Name" },
                values: new object[,]
                {
                    { 10, "FL", "Florida" },
                    { 11, "GA", "Georgia" },
                    { 12, "HI", "Hawaii" },
                    { 30, "NE", "Nebraska" },
                    { 13, "IA", "Iowa" },
                    { 15, "IL", "Illinois" },
                    { 16, "IN", "Indiana" },
                    { 17, "KS", "Kansas" },
                    { 18, "KY", "Kentucky" },
                    { 19, "LA", "Louisiana" },
                    { 20, "MA", "Massachusetts" },
                    { 21, "MD", "Maryland" },
                    { 22, "ME", "Maine" },
                    { 23, "MI", "Michigan" },
                    { 24, "MN", "Minnesota" },
                    { 25, "MO", "Missouri" },
                    { 26, "MS", "Mississippi" },
                    { 27, "MT", "Montana" },
                    { 14, "ID", "Idaho" },
                    { 61, "VI", "Virgin Islands" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetRoleClaims_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "NormalizedName",
                unique: true,
                filter: "[NormalizedName] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserClaims_UserId",
                table: "AspNetUserClaims",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserLogins_UserId",
                table: "AspNetUserLogins",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUserRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "NormalizedEmail");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "NormalizedUserName",
                unique: true,
                filter: "[NormalizedUserName] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AdminActivity");

            migrationBuilder.DropTable(
                name: "ArchiveContact");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "BillingMethod");

            migrationBuilder.DropTable(
                name: "CaseDetail");

            migrationBuilder.DropTable(
                name: "ChatGroup");

            migrationBuilder.DropTable(
                name: "ClientTransaction");

            migrationBuilder.DropTable(
                name: "Company");

            migrationBuilder.DropTable(
                name: "Contact");

            migrationBuilder.DropTable(
                name: "ContactGroup");

            migrationBuilder.DropTable(
                name: "ContactUs");

            migrationBuilder.DropTable(
                name: "Country");

            migrationBuilder.DropTable(
                name: "Decuments");

            migrationBuilder.DropTable(
                name: "DocumentTag");

            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "Expenses");

            migrationBuilder.DropTable(
                name: "Faq");

            migrationBuilder.DropTable(
                name: "FinancialDetails");

            migrationBuilder.DropTable(
                name: "Firm");

            migrationBuilder.DropTable(
                name: "FirmOffice");

            migrationBuilder.DropTable(
                name: "FirmOfficeAddress");

            migrationBuilder.DropTable(
                name: "GroupMessage");

            migrationBuilder.DropTable(
                name: "GroupUser");

            migrationBuilder.DropTable(
                name: "Lead");

            migrationBuilder.DropTable(
                name: "Message");

            migrationBuilder.DropTable(
                name: "News");

            migrationBuilder.DropTable(
                name: "Notes");

            migrationBuilder.DropTable(
                name: "NotesTag");

            migrationBuilder.DropTable(
                name: "packageService");

            migrationBuilder.DropTable(
                name: "Payment");

            migrationBuilder.DropTable(
                name: "PaymentTypes");

            migrationBuilder.DropTable(
                name: "PracticeArea");

            migrationBuilder.DropTable(
                name: "PricePlan");

            migrationBuilder.DropTable(
                name: "RefferalSource");

            migrationBuilder.DropTable(
                name: "RequestUsers");

            migrationBuilder.DropTable(
                name: "Service");

            migrationBuilder.DropTable(
                name: "States");

            migrationBuilder.DropTable(
                name: "Tasks");

            migrationBuilder.DropTable(
                name: "TimeLine");

            migrationBuilder.DropTable(
                name: "Transaction");

            migrationBuilder.DropTable(
                name: "UserAgainstCase");

            migrationBuilder.DropTable(
                name: "UserGroupMessage");

            migrationBuilder.DropTable(
                name: "UserSubcription");

            migrationBuilder.DropTable(
                name: "UserTitle");

            migrationBuilder.DropTable(
                name: "UserVerification");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");
        }
    }
}
