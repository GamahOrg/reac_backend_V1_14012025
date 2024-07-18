using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace REAC.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitDatabaseMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LitigationCompanyRepresentatives",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LitigationCompanyRepresentatives", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NafathTransactions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TransactionId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RandomNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestExpired = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NafathTransactions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OtherCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OtherCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysArbitratorClass",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ArbitratorClassName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    YearsFrom = table.Column<int>(type: "int", nullable: false),
                    YearsTo = table.Column<int>(type: "int", nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysArbitratorClass_3214EC0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysArea",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysArea_3214EC0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCitizenRequestType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CitizenRequestTypeName = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysCitizenRequestType__3214EC0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDelegationRelationshipType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    DelegationRelationshipTypeName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysDelegationRelationshipType3214EC0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysDepartment",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DistributionMode = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysDepar__3214EC07035C6297", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysExemptionReductionCategory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExemptionReductionCategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysExemptionReductionCategory_E19C938", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysFileManager",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileDescription = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FilePath = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    FileType = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysFileM__3214EC07904841A6", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysGender",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysGende__3214EC0743341C99", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysIdentityType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    IdentityType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysIdent__3214EC0765CD90E5", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestAmount",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationAmount = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Fees = table.Column<decimal>(type: "money", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC07298413D2", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestCategory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "صلح – تحكيم-صلح وتحكيم"),
                    AcknowledgmentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BpfId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC072E49D791", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestContractType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    TypeName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CodingPrefixSymbol = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcknowledgmentContent = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TermsAndConditions = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC074D10EA1A", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestCorrespondenceDestination",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Destination = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC071298CB03", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestExtendPeriodRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    StatusName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC07708342EA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestMeetingConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    From = table.Column<DateTime>(type: "datetime", nullable: false),
                    To = table.Column<DateTime>(type: "datetime", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC076879FC7E", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestMeetingDecision",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    MeetingDecision = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC07318BAEC8", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestOwnerType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    LitigationRequestOwnerType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false, comment: "مقدم عن نفسه – مفوض"),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC070BF201CD", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysLitigationRequestStatus",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysLitig__3214EC0715A02324", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysNationalities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NationalityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NationalityCode = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysNationalities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysSetting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Value = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysSetti__3214EC07BA9C86EA", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ShowForApplicant = table.Column<bool>(type: "bit", nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC070B2A996F", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowNotificationDestinationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DestinationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC070F04AA46", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowNotificationPlan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationPlan = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC070E19C938", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowNotificationType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NotificationType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC07BAAB3A96", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepEventActionMode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ActionMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlowStepEventActionMode__32143A96", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepEventSecurityLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SecurityLevel = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlowStepEventSecurityLevel__32143A96", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepEventSecurityLevelActorType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    SecurityLevelActorType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlowStepEventSecurityLevelActorType__3214EC07C94B22C3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepsActorType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    ActorType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlowStepsActorType__3214EC07C94B22C3", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepStage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: true),
                    StageName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC073B964C3D", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepWorkMode",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    WorkMode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC079D121DED", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SysCity",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysCity_3214EC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysAreaCity676101",
                        column: x => x.AreaId,
                        principalTable: "SysArea",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SysEmployee",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    IsAdmin = table.Column<bool>(type: "bit", nullable: false),
                    AllowAssign = table.Column<bool>(type: "bit", nullable: false),
                    WorkAsEmployee = table.Column<bool>(type: "bit", nullable: false),
                    AllowReAssign = table.Column<bool>(type: "bit", nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "('false')"),
                    IsActive = table.Column<bool>(type: "bit", nullable: false, defaultValueSql: "('true')"),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysEmplo__3214EC0760D0BD3B", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysEmploye454468",
                        column: x => x.DepartmentId,
                        principalTable: "SysDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlow",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC07F533D1CD", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowDepartment11186",
                        column: x => x.DepartmentId,
                        principalTable: "SysDepartment",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "DelegationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityTypeId = table.Column<int>(type: "int", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    BirthDate = table.Column<DateTime>(type: "date", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    SourceCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "date", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    DelegationNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    DelegationFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Delegati__3214EC074548DE5A", x => x.Id);
                    table.ForeignKey(
                        name: "FKDelegation266019",
                        column: x => x.DelegationFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKDelegation632464",
                        column: x => x.IdentityTypeId,
                        principalTable: "SysIdentityType",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Citizen",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    IdentityTypeId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IdentityNumber = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    NationalityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BirthDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NationalAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ShortName = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PassCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Otp = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    GenderId = table.Column<int>(type: "int", nullable: false),
                    IdentityExpiryDate = table.Column<DateTime>(type: "date", nullable: false),
                    IdentityFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Employer = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    HomeTelephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    WorkTelephone = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SubNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    IsLawyer = table.Column<bool>(type: "bit", nullable: false),
                    IsReformer = table.Column<bool>(type: "bit", nullable: false),
                    IsArbitrator = table.Column<bool>(type: "bit", nullable: false),
                    LawerLicenseNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LawerLicenseExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LawLicenseFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LitigationCompanyRepresentativeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Citizen__3214EC0756CF4E03", x => x.Id);
                    table.ForeignKey(
                        name: "FKCitizen807894",
                        column: x => x.IdentityTypeId,
                        principalTable: "SysIdentityType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizen92411",
                        column: x => x.NationalityId,
                        principalTable: "SysNationalities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizen92422",
                        column: x => x.CityId,
                        principalTable: "SysCity",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizen92455",
                        column: x => x.GenderId,
                        principalTable: "SysGender",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizen92459",
                        column: x => x.AreaId,
                        principalTable: "SysArea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKIndentityCitizens616003",
                        column: x => x.IdentityFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLawLicenseCitizens807894",
                        column: x => x.LawLicenseFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Citizen_LitigationCompanyRepresentatives_LitigationCompanyRepresentativeId",
                        column: x => x.LitigationCompanyRepresentativeId,
                        principalTable: "LitigationCompanyRepresentatives",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SysVacationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "date", nullable: false),
                    EndDate = table.Column<DateTime>(type: "date", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ApprovedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ApprovedOn = table.Column<DateTime>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SysVacationRequestEC07276A4089", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysApprovedByEmployee__9495",
                        column: x => x.ApprovedBy,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKSysEmployee__09723",
                        column: x => x.EmployeeId,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowNotification",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowNotificationPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowNotificationDestinationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowNotificationTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NotificationMessage = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC07D57D00A2", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowEv485259",
                        column: x => x.WorkFlowNotificationDestinationTypeId,
                        principalTable: "WorkFlowNotificationDestinationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv659410",
                        column: x => x.EmployeeId,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv746150",
                        column: x => x.DepartmentId,
                        principalTable: "SysDepartment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv78956",
                        column: x => x.WorkFlowNotificationTypeId,
                        principalTable: "WorkFlowNotificationType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv986122",
                        column: x => x.WorkFlowNotificationPlanId,
                        principalTable: "WorkFlowNotificationPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStep",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StepName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    CreationLogMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ShowForApplicant = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    ActorTypeId = table.Column<int>(type: "int", nullable: false),
                    WorkModeId = table.Column<int>(type: "int", nullable: false),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InternalStatusId = table.Column<int>(type: "int", nullable: false),
                    ExternalStatusId = table.Column<int>(type: "int", nullable: false),
                    CreationNotificationPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC072AD866D7", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowSt113423",
                        column: x => x.EmployeeId,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowSt200163",
                        column: x => x.DepartmentId,
                        principalTable: "SysDepartment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowSt287452",
                        column: x => x.ActorTypeId,
                        principalTable: "WorkFlowStepsActorType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowSt486458",
                        column: x => x.WorkFlowId,
                        principalTable: "WorkFlow",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowSt648097",
                        column: x => x.WorkModeId,
                        principalTable: "WorkFlowStepWorkMode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowSt932171",
                        column: x => x.StageId,
                        principalTable: "WorkFlowStepStage",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkFlowStep_WorkFlowNotificationPlan_CreationNotificationPlanId",
                        column: x => x.CreationNotificationPlanId,
                        principalTable: "WorkFlowNotificationPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenRequestTypeId = table.Column<int>(type: "int", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsVolunteerRepairmanEndorsement = table.Column<bool>(type: "bit", nullable: false),
                    AcademicDegreeName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ExperienceYears = table.Column<int>(type: "int", nullable: false),
                    IsApproved = table.Column<bool>(type: "bit", nullable: true),
                    ArbitratorClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Brief = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    SysArbitratorClassId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SysAreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequest_3214EC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysCitizenRequestTypeCitizenRequest676101",
                        column: x => x.CitizenRequestTypeId,
                        principalTable: "SysCitizenRequestType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CitizenRequest_Citizen_CitizenId",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CitizenRequest_SysArbitratorClass_SysArbitratorClassId",
                        column: x => x.SysArbitratorClassId,
                        principalTable: "SysArbitratorClass",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_CitizenRequest_SysArea_SysAreaId",
                        column: x => x.SysAreaId,
                        principalTable: "SysArea",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Facility",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Facility700Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    ArabicFacilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishFacilityName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommercialRegisterNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Fax = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    District = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Street = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    PostalCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SubNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    BuildingNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    UnitNumber = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Facility__3214EC07AE06877A", x => x.Id);
                    table.ForeignKey(
                        name: "FKFacility36705",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "SysRegistrationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IdentityNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Mobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    CRANumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    Facility700Number = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SourceCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedOn = table.Column<DateTime>(type: "date", nullable: false),
                    IsRegistrationDone = table.Column<bool>(type: "bit", nullable: false),
                    RegistrationDoneOn = table.Column<DateTime>(type: "date", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__SysRegis__3214EC07CCB73928", x => x.Id);
                    table.ForeignKey(
                        name: "FKSysRegistr415781",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKSysRegistr676101",
                        column: x => x.SourceCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepEvent",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowStepsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EventName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AllowFileUpload = table.Column<bool>(type: "bit", nullable: false),
                    AllowNotes = table.Column<bool>(type: "bit", nullable: false),
                    ShowProcessingLogForApplicant = table.Column<bool>(type: "bit", nullable: false),
                    ShowLogForApplicant = table.Column<bool>(type: "bit", nullable: false),
                    LogMessage = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    EventSecurityLevelId = table.Column<int>(type: "int", nullable: false),
                    IsAdminApprovalRequired = table.Column<bool>(type: "bit", nullable: false),
                    AllowChangeInternalStatus = table.Column<bool>(type: "bit", nullable: false),
                    InternalStatusId = table.Column<int>(type: "int", nullable: true),
                    AllowChangeExternalStatus = table.Column<bool>(type: "bit", nullable: false),
                    ExternalStatusId = table.Column<int>(type: "int", nullable: true),
                    SameEmployee = table.Column<bool>(type: "bit", nullable: false),
                    ActionModeId = table.Column<int>(type: "int", nullable: true),
                    NextWorkFlowStepDestinationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AllowCompletionNotificationPlan = table.Column<bool>(type: "bit", nullable: false),
                    CompletionNotificationPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AllowChangeIsAllowEdit = table.Column<bool>(type: "bit", nullable: false),
                    IsAllowEditValue = table.Column<bool>(type: "bit", nullable: true),
                    AllowChangeIsActive = table.Column<bool>(type: "bit", nullable: false),
                    IsActiveValue = table.Column<bool>(type: "bit", nullable: true),
                    AllowChangeIsFinished = table.Column<bool>(type: "bit", nullable: false),
                    IsFinishedValue = table.Column<bool>(type: "bit", nullable: true),
                    WorkFlowNotificationPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    DeletedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DeletedOn = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Translations = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC07C98BCC1F", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowEv231388",
                        column: x => x.WorkFlowStepsId,
                        principalTable: "WorkFlowStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv458686",
                        column: x => x.NextWorkFlowStepDestinationId,
                        principalTable: "WorkFlowStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowEv60082",
                        column: x => x.CompletionNotificationPlanId,
                        principalTable: "WorkFlowNotificationPlan",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventEventSecurityLevel231388",
                        column: x => x.EventSecurityLevelId,
                        principalTable: "WorkFlowStepEventSecurityLevel",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventWorkFlowStepEventActionMode231388",
                        column: x => x.ActionModeId,
                        principalTable: "WorkFlowStepEventActionMode",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_WorkFlowStepEvent_WorkFlowNotificationPlan_WorkFlowNotificationPlanId",
                        column: x => x.WorkFlowNotificationPlanId,
                        principalTable: "WorkFlowNotificationPlan",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequestAcademicDegreeFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcademicDegreeFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestAcademicDegreeFile3214EC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "CitizenRequestAcademicDegreeFiles616003",
                        column: x => x.AcademicDegreeFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizenRequestAcademicDegreeFileCitizenRequest676101",
                        column: x => x.CitizenRequestId,
                        principalTable: "CitizenRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequestExperienceCertificate",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CertificateName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AcquisitionDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IssuingEntityName = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestExperienceCertificate3214EC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKCitizenRequestExperienceCertificateCitizenRequest676101",
                        column: x => x.CitizenRequestId,
                        principalTable: "CitizenRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequestFreelanceWorkDocumentFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FreelanceWorkDocumentFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestFreelanceWorkDocumentFile3214EC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKCitizenRequestCitizenRequestFreelanceWorkDocumentFiles01111676101",
                        column: x => x.FreelanceWorkDocumentFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizenRequestFreelanceWorkDocumentFileCitizenRequest676101",
                        column: x => x.CitizenRequestId,
                        principalTable: "CitizenRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Delegator",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DelegateNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DelegationRelationshipTypeId = table.Column<int>(type: "int", nullable: false),
                    CitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    DelegatorCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DelegationFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Facility__340286D707D66911", x => x.Id);
                    table.ForeignKey(
                        name: "FKCDelegatorSysDelegationRelationshipTypes2455",
                        column: x => x.DelegationRelationshipTypeId,
                        principalTable: "SysDelegationRelationshipType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKDelegatorCitizenToDelegators18143",
                        column: x => x.DelegatorCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKDelegatorFromDelegators8143",
                        column: x => x.CitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKFacilityDelegators200220",
                        column: x => x.FacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Delegator_SysFileManager_DelegationFileId",
                        column: x => x.DelegationFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestContract",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestContractTypeId = table.Column<int>(type: "int", nullable: false),
                    ContractKey = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    VersionNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    AssociationName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AssociationRegistrationCertificateFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FoundingLetterFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    InternalRegulationsFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstPartyCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstPartyFacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FirstPartyCapacity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondPartyCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondPartyFacilityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SecondPartyName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyCapacity = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SecondPartyIdentityNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondPartyEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    SecondPartyMobile = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    SecondPartyRegistrationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC079F19E3F8", x => x.Id);
                    table.ForeignKey(
                        name: "FKAssociationRegistrationCertificateFile807894",
                        column: x => x.AssociationRegistrationCertificateFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKFoundingLetterFile807894",
                        column: x => x.FoundingLetterFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKInternalRegulationsFile807894",
                        column: x => x.InternalRegulationsFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation253862",
                        column: x => x.FirstPartyCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation263926",
                        column: x => x.FirstPartyFacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation28965",
                        column: x => x.SecondPartyCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation945532",
                        column: x => x.ContractFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation986672",
                        column: x => x.SecondPartyFacilityId,
                        principalTable: "Facility",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation989644",
                        column: x => x.LitigationRequestContractTypeId,
                        principalTable: "SysLitigationRequestContractType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKSecondPartyLitigation238798",
                        column: x => x.SecondPartyRegistrationRequestId,
                        principalTable: "SysRegistrationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowProcessingLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowStepEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SourceWorkFlowProcessingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    WorkFlowStepId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsWithNotes = table.Column<bool>(type: "bit", nullable: false),
                    ShowForApplicant = table.Column<bool>(type: "bit", nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC079C2E52E3", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkDestination_SourceWorkFlowProcessingLogPr467617",
                        column: x => x.SourceWorkFlowProcessingLogId,
                        principalTable: "WorkFlowProcessingLog",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowPr231724",
                        column: x => x.EmployeeId,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowPr467617",
                        column: x => x.WorkFlowStepId,
                        principalTable: "WorkFlowStep",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowPr909055",
                        column: x => x.WorkFlowStepEventId,
                        principalTable: "WorkFlowStepEvent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowStepEventSecurityLevelActor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowStepEventId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SecurityLevelActorId = table.Column<int>(type: "int", nullable: false),
                    DepartmentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    EmployeeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlowStepEventSecurityLevelActor__3214EC072AD866D7", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventSecurityLevelActorDepartment200163",
                        column: x => x.DepartmentId,
                        principalTable: "SysDepartment",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventSecurityLevelActorEmployeeReferenceSt113423",
                        column: x => x.EmployeeId,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventSecurityLevelActorType287452",
                        column: x => x.SecurityLevelActorId,
                        principalTable: "WorkFlowStepEventSecurityLevelActorType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKWorkFlowStepEventSecurityLevelActorsWorkFlowStepEvent486458",
                        column: x => x.WorkFlowStepEventId,
                        principalTable: "WorkFlowStepEvent",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CitizenRequestExperienceCertificateFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CitizenRequestExperienceCertificateId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ExperienceCertificateFileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CitizenRequestExperienceCertificateFileEC0743341C99", x => x.Id);
                    table.ForeignKey(
                        name: "FKCitizenRequestCitizenRequestExperienceCertificateFile000676101",
                        column: x => x.ExperienceCertificateFileId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKCitizenRequestExperienceCertificateFileCitizenRequestExperienceCertificate676101",
                        column: x => x.CitizenRequestExperienceCertificateId,
                        principalTable: "CitizenRequestExperienceCertificate",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestCode = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: true),
                    ContractId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    AreaId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ApplicantCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsAllowEdit = table.Column<bool>(type: "bit", nullable: false),
                    FirstPartyArbitratorsCount = table.Column<int>(type: "int", nullable: true),
                    SecondPartyArbitratorsCount = table.Column<int>(type: "int", nullable: true),
                    ArbitratorsCount = table.Column<int>(type: "int", nullable: true),
                    ReformerCitizenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LitigationRequestCategoryId = table.Column<int>(type: "int", nullable: true),
                    LitigationRequestOwnerTypeId = table.Column<int>(type: "int", nullable: false),
                    InternalStatusId = table.Column<int>(type: "int", nullable: false),
                    ExternalStatusId = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsFinancialLitigation = table.Column<bool>(type: "bit", nullable: true),
                    LitigationRequestAmountId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Fees = table.Column<decimal>(type: "money", nullable: false),
                    IsPaid = table.Column<bool>(type: "bit", nullable: false),
                    PaidOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    IsSent = table.Column<bool>(type: "bit", nullable: false),
                    SentOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: true),
                    IsFinished = table.Column<bool>(type: "bit", nullable: true),
                    ExemptionReductionCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PretenderRating = table.Column<int>(type: "int", nullable: true),
                    DefendantRating = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC077962DFCA", x => x.Id);
                    table.ForeignKey(
                        name: "FKExemptionReductionCategory580725",
                        column: x => x.ExemptionReductionCategoryId,
                        principalTable: "SysExemptionReductionCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation216575",
                        column: x => x.ContractId,
                        principalTable: "LitigationRequestContract",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation446010",
                        column: x => x.LitigationRequestCategoryId,
                        principalTable: "SysLitigationRequestCategory",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation464519",
                        column: x => x.LitigationRequestAmountId,
                        principalTable: "SysLitigationRequestAmount",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation580725",
                        column: x => x.LitigationRequestOwnerTypeId,
                        principalTable: "SysLitigationRequestOwnerType",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation638578",
                        column: x => x.ApplicantCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation676880",
                        column: x => x.ExternalStatusId,
                        principalTable: "SysLitigationRequestStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation891328",
                        column: x => x.InternalStatusId,
                        principalTable: "SysLitigationRequestStatus",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKReformerCitizenLitigation638578",
                        column: x => x.ReformerCitizenId,
                        principalTable: "Citizen",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LitigationRequest_SysArea_AreaId",
                        column: x => x.AreaId,
                        principalTable: "SysArea",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LitigationRequest_SysCity_CityId",
                        column: x => x.CityId,
                        principalTable: "SysCity",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "WorkFlowProcessingLogFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WorkFlowProcessingLogId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__WorkFlow__3214EC076A5BB1A2", x => x.Id);
                    table.ForeignKey(
                        name: "FKWorkFlowPr653271",
                        column: x => x.WorkFlowProcessingLogId,
                        principalTable: "WorkFlowProcessingLog",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestCorrespondence",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestCorrespondenceDestinationId = table.Column<int>(type: "int", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07868BEDDE", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation401380",
                        column: x => x.LitigationRequestCorrespondenceDestinationId,
                        principalTable: "SysLitigationRequestCorrespondenceDestination",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation887015",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestExtendPeriodRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequiredDays = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestExtendPeriodRequestStatusId = table.Column<int>(type: "int", nullable: false),
                    FinishedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    FinishedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FinishStatusReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07276A4089", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation194598",
                        column: x => x.LitigationRequestExtendPeriodRequestStatusId,
                        principalTable: "SysLitigationRequestExtendPeriodRequestStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FKLitigation219495",
                        column: x => x.FinishedBy,
                        principalTable: "SysEmployee",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation809723",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestGround",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07B596385A", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation62808",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestNote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07C1F98E04", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation535233",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestReconciliationMeeting",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MeetingDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    LitigationRequestMeetingConfigurationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestMeetingStatusId = table.Column<int>(type: "int", nullable: false),
                    ReconciliationMinutes = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LitigationRequestMeetingDecisionId = table.Column<int>(type: "int", nullable: true),
                    IsFirstPartyApprove = table.Column<bool>(type: "bit", nullable: true),
                    FirstPartyApprovedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    IsSecondPartyApprove = table.Column<bool>(type: "bit", nullable: true),
                    SecondPartyApprovedOn = table.Column<DateTime>(type: "datetime", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07F3A8E5CA", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation334620",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation358393",
                        column: x => x.LitigationRequestMeetingDecisionId,
                        principalTable: "SysLitigationRequestMeetingDecision",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation600098",
                        column: x => x.LitigationRequestMeetingConfigurationId,
                        principalTable: "SysLitigationRequestMeetingConfiguration",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestRequest",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RequestContent = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC0751C3A7B1", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation674990",
                        column: x => x.LitigationRequestId,
                        principalTable: "LitigationRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestCorrespondenceFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestCorrespondenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07B506B090", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation804409",
                        column: x => x.LitigationRequestCorrespondenceId,
                        principalTable: "LitigationRequestCorrespondence",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation81269",
                        column: x => x.FileManagerId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestExtendPeriodRequestFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestExtendPeriodRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC076F56C6AF", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation662845",
                        column: x => x.LitigationRequestExtendPeriodRequestId,
                        principalTable: "LitigationRequestExtendPeriodRequest",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation788993",
                        column: x => x.FileManagerId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestGroundFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestGroundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07E98400B7", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation488105",
                        column: x => x.LitigationRequestGroundId,
                        principalTable: "LitigationRequestGround",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation81857",
                        column: x => x.FileManagerId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestNoteFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NoteId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC070E49794B", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation888272",
                        column: x => x.NoteId,
                        principalTable: "LitigationRequestNote",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation910435",
                        column: x => x.FileManagerId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestMeetingReconciliationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestMeetingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ReconciliationItem = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC07A8D6A249", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation688593",
                        column: x => x.LitigationRequestMeetingId,
                        principalTable: "LitigationRequestReconciliationMeeting",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LitigationRequestRequestFile",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LitigationRequestRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Litigati__3214EC076FBEF9E2", x => x.Id);
                    table.ForeignKey(
                        name: "FKLitigation620435",
                        column: x => x.FileManagerId,
                        principalTable: "SysFileManager",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FKLitigation662656",
                        column: x => x.LitigationRequestRequestId,
                        principalTable: "LitigationRequestRequest",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_AreaId",
                table: "Citizen",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_CityId",
                table: "Citizen",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_GenderId",
                table: "Citizen",
                column: "GenderId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_IdentityFileId",
                table: "Citizen",
                column: "IdentityFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_IdentityTypeId",
                table: "Citizen",
                column: "IdentityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_LawLicenseFileId",
                table: "Citizen",
                column: "LawLicenseFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_LitigationCompanyRepresentativeId",
                table: "Citizen",
                column: "LitigationCompanyRepresentativeId");

            migrationBuilder.CreateIndex(
                name: "IX_Citizen_NationalityId",
                table: "Citizen",
                column: "NationalityId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequest_CitizenId",
                table: "CitizenRequest",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequest_CitizenRequestTypeId",
                table: "CitizenRequest",
                column: "CitizenRequestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequest_SysArbitratorClassId",
                table: "CitizenRequest",
                column: "SysArbitratorClassId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequest_SysAreaId",
                table: "CitizenRequest",
                column: "SysAreaId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestAcademicDegreeFile_AcademicDegreeFileId",
                table: "CitizenRequestAcademicDegreeFile",
                column: "AcademicDegreeFileId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestAcademicDegreeFile_CitizenRequestId",
                table: "CitizenRequestAcademicDegreeFile",
                column: "CitizenRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestExperienceCertificate_CitizenRequestId",
                table: "CitizenRequestExperienceCertificate",
                column: "CitizenRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestExperienceCertificateFile_CitizenRequestExperienceCertificateId",
                table: "CitizenRequestExperienceCertificateFile",
                column: "CitizenRequestExperienceCertificateId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestExperienceCertificateFile_ExperienceCertificateFileId",
                table: "CitizenRequestExperienceCertificateFile",
                column: "ExperienceCertificateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestFreelanceWorkDocumentFile_CitizenRequestId",
                table: "CitizenRequestFreelanceWorkDocumentFile",
                column: "CitizenRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_CitizenRequestFreelanceWorkDocumentFile_FreelanceWorkDocumentFileId",
                table: "CitizenRequestFreelanceWorkDocumentFile",
                column: "FreelanceWorkDocumentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationRequest_DelegationFileId",
                table: "DelegationRequest",
                column: "DelegationFileId");

            migrationBuilder.CreateIndex(
                name: "IX_DelegationRequest_IdentityTypeId",
                table: "DelegationRequest",
                column: "IdentityTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegator_CitizenId",
                table: "Delegator",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegator_DelegationFileId",
                table: "Delegator",
                column: "DelegationFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegator_DelegationRelationshipTypeId",
                table: "Delegator",
                column: "DelegationRelationshipTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegator_DelegatorCitizenId",
                table: "Delegator",
                column: "DelegatorCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_Delegator_FacilityId",
                table: "Delegator",
                column: "FacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_Facility_CitizenId",
                table: "Facility",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "UQ__Facility__E3FD678860036876",
                table: "Facility",
                column: "Facility700Number",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ApplicantCitizenId",
                table: "LitigationRequest",
                column: "ApplicantCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_AreaId",
                table: "LitigationRequest",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_CityId",
                table: "LitigationRequest",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ContractId",
                table: "LitigationRequest",
                column: "ContractId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ExemptionReductionCategoryId",
                table: "LitigationRequest",
                column: "ExemptionReductionCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ExternalStatusId",
                table: "LitigationRequest",
                column: "ExternalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_InternalStatusId",
                table: "LitigationRequest",
                column: "InternalStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_LitigationRequestAmountId",
                table: "LitigationRequest",
                column: "LitigationRequestAmountId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_LitigationRequestCategoryId",
                table: "LitigationRequest",
                column: "LitigationRequestCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_LitigationRequestOwnerTypeId",
                table: "LitigationRequest",
                column: "LitigationRequestOwnerTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequest_ReformerCitizenId",
                table: "LitigationRequest",
                column: "ReformerCitizenId");

            migrationBuilder.CreateIndex(
                name: "UQ__Litigati__CBAB82F68D5B9E0C",
                table: "LitigationRequest",
                column: "RequestCode",
                unique: true,
                filter: "[RequestCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_AssociationRegistrationCertificateFileId",
                table: "LitigationRequestContract",
                column: "AssociationRegistrationCertificateFileId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_ContractFileId",
                table: "LitigationRequestContract",
                column: "ContractFileId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_FirstPartyCitizenId",
                table: "LitigationRequestContract",
                column: "FirstPartyCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_FirstPartyFacilityId",
                table: "LitigationRequestContract",
                column: "FirstPartyFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_FoundingLetterFileId",
                table: "LitigationRequestContract",
                column: "FoundingLetterFileId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_InternalRegulationsFileId",
                table: "LitigationRequestContract",
                column: "InternalRegulationsFileId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_LitigationRequestContractTypeId",
                table: "LitigationRequestContract",
                column: "LitigationRequestContractTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_SecondPartyCitizenId",
                table: "LitigationRequestContract",
                column: "SecondPartyCitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_SecondPartyFacilityId",
                table: "LitigationRequestContract",
                column: "SecondPartyFacilityId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestContract_SecondPartyRegistrationRequestId",
                table: "LitigationRequestContract",
                column: "SecondPartyRegistrationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestCorrespondence_LitigationRequestCorrespondenceDestinationId",
                table: "LitigationRequestCorrespondence",
                column: "LitigationRequestCorrespondenceDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestCorrespondence_LitigationRequestId",
                table: "LitigationRequestCorrespondence",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestCorrespondenceFile_FileManagerId",
                table: "LitigationRequestCorrespondenceFile",
                column: "FileManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestCorrespondenceFile_LitigationRequestCorrespondenceId",
                table: "LitigationRequestCorrespondenceFile",
                column: "LitigationRequestCorrespondenceId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestExtendPeriodRequest_FinishedBy",
                table: "LitigationRequestExtendPeriodRequest",
                column: "FinishedBy");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestExtendPeriodRequest_LitigationRequestExtendPeriodRequestStatusId",
                table: "LitigationRequestExtendPeriodRequest",
                column: "LitigationRequestExtendPeriodRequestStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestExtendPeriodRequest_LitigationRequestId",
                table: "LitigationRequestExtendPeriodRequest",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestExtendPeriodRequestFile_FileManagerId",
                table: "LitigationRequestExtendPeriodRequestFile",
                column: "FileManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestExtendPeriodRequestFile_LitigationRequestExtendPeriodRequestId",
                table: "LitigationRequestExtendPeriodRequestFile",
                column: "LitigationRequestExtendPeriodRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestGround_LitigationRequestId",
                table: "LitigationRequestGround",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestGroundFile_FileManagerId",
                table: "LitigationRequestGroundFile",
                column: "FileManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestGroundFile_LitigationRequestGroundId",
                table: "LitigationRequestGroundFile",
                column: "LitigationRequestGroundId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestMeetingReconciliationItem_LitigationRequestMeetingId",
                table: "LitigationRequestMeetingReconciliationItem",
                column: "LitigationRequestMeetingId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestNote_LitigationRequestId",
                table: "LitigationRequestNote",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestNoteFile_FileManagerId",
                table: "LitigationRequestNoteFile",
                column: "FileManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestNoteFile_NoteId",
                table: "LitigationRequestNoteFile",
                column: "NoteId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestReconciliationMeeting_LitigationRequestId",
                table: "LitigationRequestReconciliationMeeting",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestReconciliationMeeting_LitigationRequestMeetingConfigurationId",
                table: "LitigationRequestReconciliationMeeting",
                column: "LitigationRequestMeetingConfigurationId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestReconciliationMeeting_LitigationRequestMeetingDecisionId",
                table: "LitigationRequestReconciliationMeeting",
                column: "LitigationRequestMeetingDecisionId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestRequest_LitigationRequestId",
                table: "LitigationRequestRequest",
                column: "LitigationRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestRequestFile_FileManagerId",
                table: "LitigationRequestRequestFile",
                column: "FileManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_LitigationRequestRequestFile_LitigationRequestRequestId",
                table: "LitigationRequestRequestFile",
                column: "LitigationRequestRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_SysCity_AreaId",
                table: "SysCity",
                column: "AreaId");

            migrationBuilder.CreateIndex(
                name: "IX_SysEmployee_DepartmentId",
                table: "SysEmployee",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "SysExemptionReductionCategory_Id",
                table: "SysExemptionReductionCategory",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SysLitig__1931ED8347FF283F",
                table: "SysLitigationRequestAmount",
                column: "LitigationAmount",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SysLitig__D4E7DFA8A7F5E85A",
                table: "SysLitigationRequestContractType",
                column: "TypeName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SysLitig__06968C68F7DD93E4",
                table: "SysLitigationRequestCorrespondenceDestination",
                column: "Destination",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__SysLitig__05E7698A4ABD62EC",
                table: "SysLitigationRequestExtendPeriodRequestStatus",
                column: "StatusName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysRegistrationRequest_CitizenId",
                table: "SysRegistrationRequest",
                column: "CitizenId");

            migrationBuilder.CreateIndex(
                name: "IX_SysRegistrationRequest_SourceCitizenId",
                table: "SysRegistrationRequest",
                column: "SourceCitizenId");

            migrationBuilder.CreateIndex(
                name: "UQ__SysSetti__C41E0289172F62B6",
                table: "SysSetting",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_SysVacationRequest_ApprovedBy",
                table: "SysVacationRequest",
                column: "ApprovedBy");

            migrationBuilder.CreateIndex(
                name: "IX_SysVacationRequest_EmployeeId",
                table: "SysVacationRequest",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "SysVacationRequest_Id",
                table: "SysVacationRequest",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlow_DepartmentId",
                table: "WorkFlow",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlow__44466DF02F20F5FC",
                table: "WorkFlow",
                column: "WorkFlowName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlow_Id",
                table: "WorkFlow",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowLog_Id",
                table: "WorkFlowLog",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowNotification_DepartmentId",
                table: "WorkFlowNotification",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowNotification_EmployeeId",
                table: "WorkFlowNotification",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowNotification_WorkFlowNotificationDestinationTypeId",
                table: "WorkFlowNotification",
                column: "WorkFlowNotificationDestinationTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowNotification_WorkFlowNotificationPlanId",
                table: "WorkFlowNotification",
                column: "WorkFlowNotificationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowNotification_WorkFlowNotificationTypeId",
                table: "WorkFlowNotification",
                column: "WorkFlowNotificationTypeId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowNotification_Id",
                table: "WorkFlowNotification",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlow__2798CCB6BCC465AF",
                table: "WorkFlowNotificationDestinationType",
                column: "DestinationType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowNotificationDestinationType_Id",
                table: "WorkFlowNotificationDestinationType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowNotificationPlan_Id",
                table: "WorkFlowNotificationPlan",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlow__591971B00EEC9096",
                table: "WorkFlowNotificationType",
                column: "NotificationType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowNotificationType_Id",
                table: "WorkFlowNotificationType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLog_EmployeeId",
                table: "WorkFlowProcessingLog",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLog_SourceWorkFlowProcessingLogId",
                table: "WorkFlowProcessingLog",
                column: "SourceWorkFlowProcessingLogId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLog_WorkFlowStepEventId",
                table: "WorkFlowProcessingLog",
                column: "WorkFlowStepEventId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLog_WorkFlowStepId",
                table: "WorkFlowProcessingLog",
                column: "WorkFlowStepId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowProcessingLog_Id",
                table: "WorkFlowProcessingLog",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowProcessingLogFile_WorkFlowProcessingLogId",
                table: "WorkFlowProcessingLogFile",
                column: "WorkFlowProcessingLogId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowProcessingLogFile_Id",
                table: "WorkFlowProcessingLogFile",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_ActorTypeId",
                table: "WorkFlowStep",
                column: "ActorTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_CreationNotificationPlanId",
                table: "WorkFlowStep",
                column: "CreationNotificationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_DepartmentId",
                table: "WorkFlowStep",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_EmployeeId",
                table: "WorkFlowStep",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_StageId",
                table: "WorkFlowStep",
                column: "StageId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_WorkFlowId",
                table: "WorkFlowStep",
                column: "WorkFlowId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStep_WorkModeId",
                table: "WorkFlowStep",
                column: "WorkModeId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowStep_Id",
                table: "WorkFlowStep",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_ActionModeId",
                table: "WorkFlowStepEvent",
                column: "ActionModeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_CompletionNotificationPlanId",
                table: "WorkFlowStepEvent",
                column: "CompletionNotificationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_EventSecurityLevelId",
                table: "WorkFlowStepEvent",
                column: "EventSecurityLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_NextWorkFlowStepDestinationId",
                table: "WorkFlowStepEvent",
                column: "NextWorkFlowStepDestinationId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_WorkFlowNotificationPlanId",
                table: "WorkFlowStepEvent",
                column: "WorkFlowNotificationPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEvent_WorkFlowStepsId",
                table: "WorkFlowStepEvent",
                column: "WorkFlowStepsId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepEvent_Id",
                table: "WorkFlowStepEvent",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__ActionMode__591971B00EEC9096",
                table: "WorkFlowStepEventActionMode",
                column: "ActionMode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepEventActionMode_Id",
                table: "WorkFlowStepEventActionMode",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlowStepEventSecurityLevel__591971B00EEC9096",
                table: "WorkFlowStepEventSecurityLevel",
                column: "SecurityLevel",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepEventSecurityLevel_Id",
                table: "WorkFlowStepEventSecurityLevel",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEventSecurityLevelActor_DepartmentId",
                table: "WorkFlowStepEventSecurityLevelActor",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEventSecurityLevelActor_EmployeeId",
                table: "WorkFlowStepEventSecurityLevelActor",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEventSecurityLevelActor_SecurityLevelActorId",
                table: "WorkFlowStepEventSecurityLevelActor",
                column: "SecurityLevelActorId");

            migrationBuilder.CreateIndex(
                name: "IX_WorkFlowStepEventSecurityLevelActor_WorkFlowStepEventId",
                table: "WorkFlowStepEventSecurityLevelActor",
                column: "WorkFlowStepEventId");

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepEventSecurityLevelActor_Id",
                table: "WorkFlowStepEventSecurityLevelActor",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlowStepEventSecurityLevelActorType__AA06C8BAA119B1A4",
                table: "WorkFlowStepEventSecurityLevelActorType",
                column: "SecurityLevelActorType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepEventSecurityLevelActorType_Id",
                table: "WorkFlowStepEventSecurityLevelActorType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlowStepsActorType__AA06C8BAA119B1A4",
                table: "WorkFlowStepsActorType",
                column: "ActorType",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepsActorType_Id",
                table: "WorkFlowStepsActorType",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlow__8FE31B33CF02E70B",
                table: "WorkFlowStepStage",
                column: "StageName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepStage_Id",
                table: "WorkFlowStepStage",
                column: "Id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "UQ__WorkFlow__CDA7E410DEE00B0E",
                table: "WorkFlowStepWorkMode",
                column: "WorkMode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "WorkFlowStepWorkMode_Id",
                table: "WorkFlowStepWorkMode",
                column: "Id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CitizenRequestAcademicDegreeFile");

            migrationBuilder.DropTable(
                name: "CitizenRequestExperienceCertificateFile");

            migrationBuilder.DropTable(
                name: "CitizenRequestFreelanceWorkDocumentFile");

            migrationBuilder.DropTable(
                name: "DelegationRequest");

            migrationBuilder.DropTable(
                name: "Delegator");

            migrationBuilder.DropTable(
                name: "LitigationRequestCorrespondenceFile");

            migrationBuilder.DropTable(
                name: "LitigationRequestExtendPeriodRequestFile");

            migrationBuilder.DropTable(
                name: "LitigationRequestGroundFile");

            migrationBuilder.DropTable(
                name: "LitigationRequestMeetingReconciliationItem");

            migrationBuilder.DropTable(
                name: "LitigationRequestNoteFile");

            migrationBuilder.DropTable(
                name: "LitigationRequestRequestFile");

            migrationBuilder.DropTable(
                name: "NafathTransactions");

            migrationBuilder.DropTable(
                name: "OtherCategories");

            migrationBuilder.DropTable(
                name: "SysSetting");

            migrationBuilder.DropTable(
                name: "SysVacationRequest");

            migrationBuilder.DropTable(
                name: "WorkFlowLog");

            migrationBuilder.DropTable(
                name: "WorkFlowNotification");

            migrationBuilder.DropTable(
                name: "WorkFlowProcessingLogFile");

            migrationBuilder.DropTable(
                name: "WorkFlowStepEventSecurityLevelActor");

            migrationBuilder.DropTable(
                name: "CitizenRequestExperienceCertificate");

            migrationBuilder.DropTable(
                name: "SysDelegationRelationshipType");

            migrationBuilder.DropTable(
                name: "LitigationRequestCorrespondence");

            migrationBuilder.DropTable(
                name: "LitigationRequestExtendPeriodRequest");

            migrationBuilder.DropTable(
                name: "LitigationRequestGround");

            migrationBuilder.DropTable(
                name: "LitigationRequestReconciliationMeeting");

            migrationBuilder.DropTable(
                name: "LitigationRequestNote");

            migrationBuilder.DropTable(
                name: "LitigationRequestRequest");

            migrationBuilder.DropTable(
                name: "WorkFlowNotificationDestinationType");

            migrationBuilder.DropTable(
                name: "WorkFlowNotificationType");

            migrationBuilder.DropTable(
                name: "WorkFlowProcessingLog");

            migrationBuilder.DropTable(
                name: "WorkFlowStepEventSecurityLevelActorType");

            migrationBuilder.DropTable(
                name: "CitizenRequest");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestCorrespondenceDestination");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestExtendPeriodRequestStatus");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestMeetingDecision");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestMeetingConfiguration");

            migrationBuilder.DropTable(
                name: "LitigationRequest");

            migrationBuilder.DropTable(
                name: "WorkFlowStepEvent");

            migrationBuilder.DropTable(
                name: "SysCitizenRequestType");

            migrationBuilder.DropTable(
                name: "SysArbitratorClass");

            migrationBuilder.DropTable(
                name: "SysExemptionReductionCategory");

            migrationBuilder.DropTable(
                name: "LitigationRequestContract");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestCategory");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestAmount");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestOwnerType");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestStatus");

            migrationBuilder.DropTable(
                name: "WorkFlowStep");

            migrationBuilder.DropTable(
                name: "WorkFlowStepEventSecurityLevel");

            migrationBuilder.DropTable(
                name: "WorkFlowStepEventActionMode");

            migrationBuilder.DropTable(
                name: "Facility");

            migrationBuilder.DropTable(
                name: "SysLitigationRequestContractType");

            migrationBuilder.DropTable(
                name: "SysRegistrationRequest");

            migrationBuilder.DropTable(
                name: "SysEmployee");

            migrationBuilder.DropTable(
                name: "WorkFlowStepsActorType");

            migrationBuilder.DropTable(
                name: "WorkFlow");

            migrationBuilder.DropTable(
                name: "WorkFlowStepWorkMode");

            migrationBuilder.DropTable(
                name: "WorkFlowStepStage");

            migrationBuilder.DropTable(
                name: "WorkFlowNotificationPlan");

            migrationBuilder.DropTable(
                name: "Citizen");

            migrationBuilder.DropTable(
                name: "SysDepartment");

            migrationBuilder.DropTable(
                name: "SysIdentityType");

            migrationBuilder.DropTable(
                name: "SysNationalities");

            migrationBuilder.DropTable(
                name: "SysCity");

            migrationBuilder.DropTable(
                name: "SysGender");

            migrationBuilder.DropTable(
                name: "SysFileManager");

            migrationBuilder.DropTable(
                name: "LitigationCompanyRepresentatives");

            migrationBuilder.DropTable(
                name: "SysArea");
        }
    }
}
