using Microsoft.EntityFrameworkCore;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using REAC.EntityFrameworkCore.Entities.Reconciliation;
using REAC.EntityFrameworkCore.Entities.Wpf.Business;
using REAC.EntityFrameworkCore.Entities.Wpf.System;

namespace REAC.EntityFrameworkCore.Entities.System;

public partial class REACDBContext : DbContext
{
    public REACDBContext()
    {
    }

    public REACDBContext(DbContextOptions<REACDBContext> options)
        : base(options)
    {
    }
    public virtual DbSet<WorkFlowStepEventActionMode> WorkFlowStepEventActionModes { get; set; }

    public virtual DbSet<WorkFlowStepEventSecurityLevel> WorkFlowStepEventSecurityLevel { get; set; }

    public virtual DbSet<WorkFlow> WorkFlows { get; set; }

    public virtual DbSet<SysExemptionReductionCategory> SysExemptionReductionCategories { get; set; }

    public virtual DbSet<SysVacationRequest> SysVacations { get; set; }

    public virtual DbSet<WorkFlowStepEvent> WorkFlowStepEvents { get; set; }

    public virtual DbSet<WorkFlowNotification> WorkFlowNotifications { get; set; }

    public virtual DbSet<WorkFlowNotificationDestinationType> WorkFlowNotificationDestinationTypes { get; set; }

    public virtual DbSet<WorkFlowNotificationPlan> WorkFlowNotificationPlans { get; set; }

    public virtual DbSet<WorkFlowNotificationType> WorkFlowNotificationTypes { get; set; }

    public virtual DbSet<WorkFlowLog> WorkFlowLogs { get; set; }

    public virtual DbSet<WorkFlowProcessingLog> WorkFlowProcessingLogs { get; set; }

    public virtual DbSet<WorkFlowProcessingLogFile> WorkFlowProcessingLogFiles { get; set; }


    public virtual DbSet<WorkFlowStep> WorkFlowSteps { get; set; }

    public virtual DbSet<WorkFlowStepStage> WorkFlowStepStages { get; set; }

    public virtual DbSet<WorkFlowStepWorkMode> WorkFlowStepWorkModes { get; set; }

    public virtual DbSet<WorkFlowStepsActorType> WorkFlowStepsActorTypes { get; set; }

    public virtual DbSet<WorkFlowStepEventSecurityLevelActor> WorkFlowStepEventSecurityLevelActors { get; set; }

    public virtual DbSet<WorkFlowStepEventSecurityLevelActorType> WorkFlowStepEventSecurityLevelActorTypes { get; set; }

    public virtual DbSet<SysVacationRequest> VacationRequests { get; set; }

    public virtual DbSet<Citizen> Citizens { get; set; }

    public virtual DbSet<DelegationRequest> DelegationRequests { get; set; }

    public virtual DbSet<Facility> Facilities { get; set; }

    public virtual DbSet<FacilityDelegator> FacilityDelegators { get; set; }

    public virtual DbSet<CitizenLitigationRequestLawyerAgent> CitizenLitigationRequestLawyerAgents { get; set; }
    public virtual DbSet<LitigationCompanyRepresentative> LitigationCompanyRepresentatives { get; set; }
    public virtual DbSet<LitigationRequestLawyerAgent> LitigationRequestLawyerAgents { get; set; }
    public virtual DbSet<LitigationRequest> LitigationRequests { get; set; }

    public virtual DbSet<LitigationRequestContract> LitigationRequestContracts { get; set; }

    public virtual DbSet<LitigationRequestCorrespondence> LitigationRequestCorrespondences { get; set; }

    public virtual DbSet<LitigationRequestCorrespondenceFile> LitigationRequestCorrespondenceFiles { get; set; }

    public virtual DbSet<LitigationRequestExtendPeriodRequest> LitigationRequestExtendPeriodRequests { get; set; }

    public virtual DbSet<LitigationRequestExtendPeriodRequestFile> LitigationRequestExtendPeriodRequestFiles { get; set; }

    public virtual DbSet<LitigationRequestGround> LitigationRequestGrounds { get; set; }

    public virtual DbSet<LitigationRequestGroundFile> LitigationRequestGroundFiles { get; set; }

    public virtual DbSet<LitigationRequestReconciliationMeeting> LitigationRequestMeetings { get; set; }

    public virtual DbSet<LitigationRequestMeetingReconciliationItem> LitigationRequestMeetingReconciliationItems { get; set; }

    public virtual DbSet<LitigationRequestNote> LitigationRequestNotes { get; set; }

    public virtual DbSet<LitigationRequestNoteFile> LitigationRequestNoteFiles { get; set; }

    public virtual DbSet<LitigationRequestRequest> LitigationRequestRequests { get; set; }

    public virtual DbSet<LitigationRequestRequestFile> LitigationRequestRequestFiles { get; set; }

    public virtual DbSet<SysDepartment> SysDepartments { get; set; }

    public virtual DbSet<SysEmployee> SysEmployees { get; set; }

    public virtual DbSet<SysFileManager> SysFileManagers { get; set; }

    public virtual DbSet<SysGender> SysGenders { get; set; }

    public virtual DbSet<SysIdentityType> SysIdentityTypes { get; set; }

    public virtual DbSet<SysLitigationRequestAmount> SysLitigationRequestAmounts { get; set; }

    public virtual DbSet<SysLitigationRequestCategory> SysLitigationRequestCategories { get; set; }

    public virtual DbSet<SysLitigationRequestContractType> SysLitigationRequestContractTypes { get; set; }

    public virtual DbSet<SysLitigationRequestCorrespondenceDestination> SysLitigationRequestCorrespondenceDestinations { get; set; }

    public virtual DbSet<SysLitigationRequestExtendPeriodRequestStatus> SysLitigationRequestExtendPeriodRequestStatuses { get; set; }

    public virtual DbSet<SysLitigationRequestMeetingConfiguration> SysLitigationRequestMeetingConfigurations { get; set; }

    public virtual DbSet<SysLitigationRequestMeetingDecision> SysLitigationRequestMeetingDecisions { get; set; }

    public virtual DbSet<SysLitigationRequestOwnerType> SysLitigationRequestOwnerTypes { get; set; }

    public virtual DbSet<SysLitigationRequestStatus> SysLitigationRequestStatuses { get; set; }

    public virtual DbSet<SysRegistrationRequest> SysRegistrationRequests { get; set; }

    public virtual DbSet<SysSetting> SysSettings { get; set; }
    public virtual DbSet<LitigationArea> LitigationAreas { get; set; }
    public virtual DbSet<LitigationCity> LitigationCitys { get; set; }
    public virtual DbSet<OtherCategory> OtherCategories { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<WorkFlow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC07F533D1CD");

            entity.ToTable("WorkFlow");

            entity.HasIndex(e => e.WorkFlowName, "UQ__WorkFlow__44466DF02F20F5FC").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlow_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.WorkFlowName).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });

            entity.HasOne(d => d.Department).WithMany(p => p.WorkFlows)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FKWorkFlowDepartment11186");


        });

        modelBuilder.Entity<WorkFlowStepEvent>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC07C98BCC1F");

            entity.ToTable("WorkFlowStepEvent");

            entity.HasIndex(e => e.Id, "WorkFlowStepEvent_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.EventName).HasMaxLength(50);
            entity.Property(e => e.LogMessage).HasMaxLength(250);

            entity.HasOne(d => d.CompletionNotificationPlan).WithMany(p => p.WorkFlowStepEventCompletionNotificationPlans)
                .HasForeignKey(d => d.CompletionNotificationPlanId)
                .HasConstraintName("FKWorkFlowEv60082");

            entity.HasOne(d => d.NextWorkFlowStepDestination).WithMany(p => p.WorkFlowStepEventNextWorkFlowStepDestinations)
                .HasForeignKey(d => d.NextWorkFlowStepDestinationId)
                .HasConstraintName("FKWorkFlowEv458686");

            entity.HasOne(d => d.WorkFlowSteps).WithMany(p => p.WorkFlowStepEvents)
                .HasForeignKey(d => d.WorkFlowStepsId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowEv231388");

            entity.HasOne(d => d.WorkFlowStepEventActionMode).WithMany(p => p.WorkFlowStepEvents)
                .HasForeignKey(d => d.ActionModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowStepEventWorkFlowStepEventActionMode231388");

            entity.HasOne(d => d.WorkFlowStepEventSecurityLevel).WithMany(p => p.WorkFlowStepEvents)
                .HasForeignKey(d => d.EventSecurityLevelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowStepEventEventSecurityLevel231388");

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowNotification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC07D57D00A2");

            entity.ToTable("WorkFlowNotification");

            entity.HasIndex(e => e.Id, "WorkFlowNotification_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NotificationMessage).HasMaxLength(50);

            entity.HasOne(d => d.Department).WithMany(p => p.WorkFlowNotifications)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FKWorkFlowEv746150");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkFlowNotifications)
                .HasForeignKey(d => d.EmployeeReferenceId)
                .HasConstraintName("FKWorkFlowEv659410");

            entity.HasOne(d => d.WorkFlowNotificationDestinationType).WithMany(p => p.WorkFlowNotifications)
                .HasForeignKey(d => d.WorkFlowNotificationDestinationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowEv485259");

            entity.HasOne(d => d.WorkFlowNotificationPlan).WithMany(p => p.WorkFlowNotifications)
                .HasForeignKey(d => d.WorkFlowNotificationPlanId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowEv986122");

            entity.HasOne(d => d.WorkFlowNotificationType).WithMany(p => p.WorkFlowNotifications)
                .HasForeignKey(d => d.WorkFlowNotificationTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowEv78956");

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowNotificationDestinationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC070F04AA46");

            entity.ToTable("WorkFlowNotificationDestinationType");

            entity.HasIndex(e => e.DestinationType, "UQ__WorkFlow__2798CCB6BCC465AF").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowNotificationDestinationType_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DestinationType).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowNotificationPlan>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC070E19C938");

            entity.ToTable("WorkFlowNotificationPlan");

            entity.HasIndex(e => e.Id, "WorkFlowNotificationPlan_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NotificationPlan).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<SysExemptionReductionCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SysExemptionReductionCategory_E19C938");

            entity.ToTable("SysExemptionReductionCategory");

            entity.HasIndex(e => e.Id, "SysExemptionReductionCategory_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ExemptionReductionCategoryName).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepEventSecurityLevel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlowStepEventSecurityLevel__32143A96");

            entity.ToTable("WorkFlowStepEventSecurityLevel");

            entity.HasIndex(e => e.SecurityLevel, "UQ__WorkFlowStepEventSecurityLevel__591971B00EEC9096").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepEventSecurityLevel_Id").IsUnique();

            entity.Property(e => e.SecurityLevel).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepEventActionMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlowStepEventActionMode__32143A96");

            entity.ToTable("WorkFlowStepEventActionMode");

            entity.HasIndex(e => e.ActionMode, "UQ__ActionMode__591971B00EEC9096").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepEventActionMode_Id").IsUnique();

            entity.Property(e => e.ActionMode).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowNotificationType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC07BAAB3A96");

            entity.ToTable("WorkFlowNotificationType");

            entity.HasIndex(e => e.NotificationType, "UQ__WorkFlow__591971B00EEC9096").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowNotificationType_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.NotificationType).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC070B2A996F");

            entity.ToTable("WorkFlowLog");

            entity.HasIndex(e => e.Id, "WorkFlowLog_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LogMessage).HasMaxLength(250);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });

        });

        modelBuilder.Entity<WorkFlowProcessingLog>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC079C2E52E3");

            entity.ToTable("WorkFlowProcessingLog");

            entity.HasIndex(e => e.Id, "WorkFlowProcessingLog_Id").IsUnique();

            entity.Property(e => e.FinishedOn).HasColumnType("datetime");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkFlowProcessingLogs)
                .HasForeignKey(d => d.EmployeeReferenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowPr231724");

            entity.HasOne(d => d.WorkFlowStepEvent).WithMany(p => p.WorkFlowProcessingLogs)
                .HasForeignKey(d => d.WorkFlowStepEventId)
                .HasConstraintName("FKWorkFlowPr909055");

            entity.HasOne(d => d.WorkFlowStep).WithMany(p => p.WorkFlowProcessingLogs)
                .HasForeignKey(d => d.WorkFlowStepId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowPr467617");

            entity.HasOne(d => d.SourceWorkFlowProcessingLog).WithMany(p => p.WorkFlowDestinationProcessingLogs)
                .HasForeignKey(d => d.SourceWorkFlowProcessingLogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkDestination_SourceWorkFlowProcessingLogPr467617");
        });

        modelBuilder.Entity<SysVacationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysVacationRequest__A5BB1A2");

            entity.ToTable("SysVacationRequest");

            entity.HasIndex(e => e.Id, "SysVacationRequest_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.SysEmployee).WithMany(p => p.EmployeeVacationRequests)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__EmployeeVacationRequests__3271");

            entity.HasOne(d => d.SysApprovedByEmployee).WithMany(p => p.EmployeeApprovedVacationRequests)
                .HasForeignKey(d => d.ApprovedBy)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__SysApprovedByEmployee__3271");
        });



        modelBuilder.Entity<WorkFlowProcessingLogFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC076A5BB1A2");

            entity.ToTable("WorkFlowProcessingLogFile");

            entity.HasIndex(e => e.Id, "WorkFlowProcessingLogFile_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.WorkFlowProcessingLog).WithMany(p => p.WorkFlowProcessingLogFiles)
                .HasForeignKey(d => d.WorkFlowProcessingLogId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowPr653271");
        });

        modelBuilder.Entity<WorkFlowStep>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC072AD866D7");

            entity.ToTable("WorkFlowStep");

            entity.HasIndex(e => e.Id, "WorkFlowStep_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StepName).HasMaxLength(50);
            entity.Property(e => e.CreationLogMessage).HasMaxLength(250);

            entity.HasOne(d => d.ActorType).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.ActorTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowSt287452");

            entity.HasOne(d => d.Department).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FKWorkFlowSt200163");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.EmployeeReferenceId)
                .HasConstraintName("FKWorkFlowSt113423");

            entity.HasOne(d => d.Stage).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.StageId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowSt932171");

            entity.HasOne(d => d.WorkFlow).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.WorkFlowId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowSt486458");

            entity.HasOne(d => d.WorkMode).WithMany(p => p.WorkFlowSteps)
                .HasForeignKey(d => d.WorkModeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowSt648097");

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepStage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC073B964C3D");

            entity.ToTable("WorkFlowStepStage");

            entity.HasIndex(e => e.StageName, "UQ__WorkFlow__8FE31B33CF02E70B").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepStage_Id").IsUnique();

            entity.Property(e => e.StageName).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepWorkMode>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlow__3214EC079D121DED");

            entity.ToTable("WorkFlowStepWorkMode");

            entity.HasIndex(e => e.WorkMode, "UQ__WorkFlow__CDA7E410DEE00B0E").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepWorkMode_Id").IsUnique();

            entity.Property(e => e.WorkMode).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepsActorType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlowStepsActorType__3214EC07C94B22C3");

            entity.ToTable("WorkFlowStepsActorType");

            entity.HasIndex(e => e.ActorType, "UQ__WorkFlowStepsActorType__AA06C8BAA119B1A4").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepsActorType_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ActorType).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepEventSecurityLevelActorType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlowStepEventSecurityLevelActorType__3214EC07C94B22C3");

            entity.ToTable("WorkFlowStepEventSecurityLevelActorType");

            entity.HasIndex(e => e.SecurityLevelActorType, "UQ__WorkFlowStepEventSecurityLevelActorType__AA06C8BAA119B1A4").IsUnique();

            entity.HasIndex(e => e.Id, "WorkFlowStepEventSecurityLevelActorType_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.SecurityLevelActorType).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder =>
            {
                ownedNavigationBuilder.ToJson();
            });
        });

        modelBuilder.Entity<WorkFlowStepEventSecurityLevelActor>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__WorkFlowStepEventSecurityLevelActor__3214EC072AD866D7");

            entity.ToTable("WorkFlowStepEventSecurityLevelActor");

            entity.HasIndex(e => e.Id, "WorkFlowStepEventSecurityLevelActor_Id").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.WorkFlowStepEventSecurityLevelActorType).WithMany(p => p.WorkFlowStepEventSecurityLevelActors)
                .HasForeignKey(d => d.SecurityLevelActorId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowStepEventSecurityLevelActorType287452");

            entity.HasOne(d => d.Department).WithMany(p => p.WorkFlowStepEventSecurityLevelActors)
                .HasForeignKey(d => d.DepartmentId)
                .HasConstraintName("FKWorkFlowStepEventSecurityLevelActorDepartment200163");

            entity.HasOne(d => d.Employee).WithMany(p => p.WorkFlowStepEventSecurityLevelActors)
                .HasForeignKey(d => d.EmployeeReferenceId)
                .HasConstraintName("FKWorkFlowStepEventSecurityLevelActorEmployeeReferenceSt113423");

            entity.HasOne(d => d.WorkFlowStepEvent).WithMany(p => p.WorkFlowStepEventSecurityLevelActors)
                .HasForeignKey(d => d.WorkFlowStepEventId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKWorkFlowStepEventSecurityLevelActorsWorkFlowStepEvent486458");

        });
        modelBuilder.Entity<Citizen>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Citizen__3214EC0756CF4E03");

            entity.ToTable("Citizen");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuildingNumber).HasMaxLength(10);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Employer).HasMaxLength(100);
            entity.Property(e => e.Fax).HasMaxLength(20);
            entity.Property(e => e.FullName).HasMaxLength(250);
            entity.Property(e => e.HomeTelephone).HasMaxLength(20);
            entity.Property(e => e.IdentityExpiryDate).HasColumnType("date");
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.IdentityNumber).HasMaxLength(100);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.Otp).HasMaxLength(10);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.ShortName).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.SubNumber).HasMaxLength(20);
            entity.Property(e => e.UnitNumber).HasMaxLength(10);
            entity.Property(e => e.WorkTelephone).HasMaxLength(20);

            entity.HasOne(d => d.Gender).WithMany(p => p.Citizens)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCitizen92455");

            entity.HasOne(d => d.IdentityFile).WithMany(p => p.Citizens)
                .HasForeignKey(d => d.IdentityFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCitizen616003");

            entity.HasOne(d => d.IdentityType).WithMany(p => p.Citizens)
                .HasForeignKey(d => d.IdentityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKCitizen807894");
        });

        modelBuilder.Entity<DelegationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Delegati__3214EC074548DE5A");

            entity.ToTable("DelegationRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ApprovedOn).HasColumnType("datetime");
            entity.Property(e => e.BirthDate).HasColumnType("date");
            entity.Property(e => e.DelegationNumber).HasMaxLength(20);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.IdentityNumber).HasMaxLength(20);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.RequestedOn).HasColumnType("date");

            entity.HasOne(d => d.DelegationFile).WithMany(p => p.DelegationRequests)
                .HasForeignKey(d => d.DelegationFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDelegation266019");

            entity.HasOne(d => d.IdentityType).WithMany(p => p.DelegationRequests)
                .HasForeignKey(d => d.IdentityTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKDelegation632464");
        });

        modelBuilder.Entity<Facility>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Facility__3214EC07AE06877A");

            entity.ToTable("Facility");

            entity.HasIndex(e => e.Facility700Number, "UQ__Facility__E3FD678860036876").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.BuildingNumber).HasMaxLength(10);
            entity.Property(e => e.City).HasMaxLength(50);
            entity.Property(e => e.CommercialRegisterNumber).HasMaxLength(20);
            entity.Property(e => e.District).HasMaxLength(50);
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Facility700Number).HasMaxLength(20);
            entity.Property(e => e.PostalCode).HasMaxLength(20);
            entity.Property(e => e.Code).HasMaxLength(20);
            entity.Property(e => e.Street).HasMaxLength(50);
            entity.Property(e => e.SubNumber).HasMaxLength(20);
            entity.Property(e => e.UnitNumber).HasMaxLength(10);

            entity.HasOne(d => d.Citizen).WithMany(p => p.Facilities)
                .HasForeignKey(d => d.CitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFacility36705");
        });

        modelBuilder.Entity<FacilityDelegator>(entity =>
        {
            entity.HasKey(e => new { e.Id, e.CitizenId, e.FacilityId }).HasName("PK__Facility__340286D707D66911");

            entity.ToTable("FacilityDelegator");

            entity.HasOne(d => d.Citizen).WithMany(p => p.FacilityDelegators)
                .HasForeignKey(d => d.CitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFacilityDe118143");

            entity.HasOne(d => d.Facility).WithMany(p => p.FacilityDelegators)
                .HasForeignKey(d => d.FacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKFacilityDe200220");
        });

        modelBuilder.Entity<LitigationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC077962DFCA");

            entity.ToTable("LitigationRequest");

            entity.HasIndex(e => e.RequestCode, "UQ__Litigati__CBAB82F68D5B9E0C").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.PaidOn).HasColumnType("datetime");
            entity.Property(e => e.RequestCode).HasMaxLength(20);
            entity.Property(e => e.SentOn).HasColumnType("datetime");
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.ApplicantCitizen).WithMany(p => p.LitigationRequests)
                .HasForeignKey(d => d.ApplicantCitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation638578");

            entity.HasOne(d => d.Contract).WithMany(p => p.LitigationRequests)
                .HasForeignKey(d => d.ContractId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation216575");

            entity.HasOne(d => d.ExternalStatus).WithMany(p => p.LitigationRequestExternalStatuses)
                .HasForeignKey(d => d.ExternalStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation676880");

            entity.HasOne(d => d.InternalStatus).WithMany(p => p.LitigationRequestInternalStatuses)
                .HasForeignKey(d => d.InternalStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation891328");

            entity.HasOne(d => d.LitigationRequestAmount).WithMany(p => p.LitigationRequests)
                .HasForeignKey(d => d.LitigationRequestAmountId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation464519");

            entity.HasOne(d => d.LitigationRequestCategory).WithMany(p => p.LitigationRequests)
                .HasForeignKey(d => d.LitigationRequestCategoryId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation446010");

            entity.HasOne(d => d.LitigationRequestOwnerType).WithMany(p => p.LitigationRequests)
                .HasForeignKey(d => d.LitigationRequestOwnerTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation580725");

            entity.HasOne(d => d.SysExemptionReductionCategory).WithMany(p => p.LitigationRequests)
              .HasForeignKey(d => d.ExemptionReductionCategoryId)
              .OnDelete(DeleteBehavior.ClientSetNull)
              .HasConstraintName("FKExemptionReductionCategory580725");

        });

        modelBuilder.Entity<LitigationRequestContract>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC079F19E3F8");

            entity.ToTable("LitigationRequestContract");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.ContractKey).HasMaxLength(250);
            entity.Property(e => e.FirstPartyCapacity).HasMaxLength(100);
            entity.Property(e => e.SecondPartyCapacity).HasMaxLength(100);
            entity.Property(e => e.VersionNumber).HasMaxLength(20);
            entity.Property(e => e.SecondPartyMobile).HasMaxLength(20);
            entity.Property(e => e.SecondPartyEmail).HasMaxLength(100);

            entity.HasOne(d => d.FirstPartyCitizen).WithMany(p => p.LitigationRequestContractFirstPartyCitizens)
                .HasForeignKey(d => d.FirstPartyCitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation253862");

            entity.HasOne(d => d.FirstPartyFacility).WithMany(p => p.LitigationRequestContractFirstPartyFacilities)
                .HasForeignKey(d => d.FirstPartyFacilityId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation263926");

            entity.HasOne(d => d.LeaseContractFile).WithMany(p => p.LitigationRequestContracts)
                .HasForeignKey(d => d.LeaseContractFileId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation945532");

            entity.HasOne(d => d.LitigationRequestContractType).WithMany(p => p.LitigationRequestContracts)
                .HasForeignKey(d => d.LitigationRequestContractTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation989644");


            entity.HasOne(d => d.SecondPartyRegistrationRequest).WithMany(p => p.SecondPartyLitigationRequestContracts)
                .HasForeignKey(d => d.SecondPartyRegistrationRequestId)
                .HasConstraintName("FKSecondPartyLitigation238798");

            entity.HasOne(d => d.SecondPartyCitizen).WithMany(p => p.LitigationRequestContractSecondPartyCitizens)
                .HasForeignKey(d => d.SecondPartyCitizenId)
                .HasConstraintName("FKLitigation28965");

            entity.HasOne(d => d.SecondPartyFacility).WithMany(p => p.LitigationRequestContractSecondPartyFacilities)
                .HasForeignKey(d => d.SecondPartyFacilityId)
                .HasConstraintName("FKLitigation986672");
        });

        modelBuilder.Entity<LitigationRequestCorrespondence>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07868BEDDE");

            entity.ToTable("LitigationRequestCorrespondence");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.CreatedOn).HasColumnType("datetime");
            entity.Property(e => e.LitigationRequestId);
            entity.Property(e => e.Subject).HasMaxLength(500);

            entity.HasOne(d => d.LitigationRequestCorrespondenceDestination).WithMany(p => p.LitigationRequestCorrespondences)
                .HasForeignKey(d => d.LitigationRequestCorrespondenceDestinationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation401380");

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestCorrespondences)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation887015");
        });

        modelBuilder.Entity<LitigationRequestCorrespondenceFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07B506B090");

            entity.ToTable("LitigationRequestCorrespondenceFile");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileManager).WithMany(p => p.LitigationRequestCorrespondenceFiles)
                .HasForeignKey(d => d.FileManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation81269");

            entity.HasOne(d => d.LitigationRequestCorrespondence).WithMany(p => p.LitigationRequestCorrespondenceFiles)
                .HasForeignKey(d => d.LitigationRequestCorrespondenceId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation804409");
        });

        modelBuilder.Entity<SysVacationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_SysVacationRequestEC07276A4089");

            entity.ToTable("SysVacationRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.StartDate).HasColumnType("date");
            entity.Property(e => e.EndDate).HasColumnType("date");
            entity.Property(e => e.ApprovedOn).HasColumnType("date");

            entity.HasOne(d => d.SysApprovedByEmployee).WithMany(p => p.EmployeeApprovedVacationRequests)
                .HasForeignKey(d => d.ApprovedBy)
                .HasConstraintName("FKSysApprovedByEmployee__9495");

            entity.HasOne(d => d.SysEmployee).WithMany(p => p.EmployeeVacationRequests)
                .HasForeignKey(d => d.EmployeeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSysEmployee__09723");
        });


        modelBuilder.Entity<LitigationRequestExtendPeriodRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07276A4089");

            entity.ToTable("LitigationRequestExtendPeriodRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FinishedOn).HasColumnType("datetime");
            entity.Property(e => e.LitigationRequestId);

            entity.HasOne(d => d.FinishedByNavigation).WithMany(p => p.LitigationRequestExtendPeriodRequests)
                .HasForeignKey(d => d.FinishedBy)
                .HasConstraintName("FKLitigation219495");

            entity.HasOne(d => d.LitigationRequestExtendPeriodRequestStatus).WithMany(p => p.LitigationRequestExtendPeriodRequests)
                .HasForeignKey(d => d.LitigationRequestExtendPeriodRequestStatusId)
                .HasConstraintName("FKLitigation194598");

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestExtendPeriodRequests)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation809723");
        });

        modelBuilder.Entity<LitigationRequestExtendPeriodRequestFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC076F56C6AF");

            entity.ToTable("LitigationRequestExtendPeriodRequestFile");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileManager).WithMany(p => p.LitigationRequestExtendPeriodRequestFiles)
                .HasForeignKey(d => d.FileManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation788993");

            entity.HasOne(d => d.LitigationRequestExtendPeriodRequest).WithMany(p => p.LitigationRequestExtendPeriodRequestFiles)
                .HasForeignKey(d => d.LitigationRequestExtendPeriodRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation662845");
        });

        modelBuilder.Entity<LitigationRequestGround>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07B596385A");

            entity.ToTable("LitigationRequestGround");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LitigationRequestId);
            entity.Property(e => e.Title).HasMaxLength(100);

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestGrounds)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation62808");
        });

        modelBuilder.Entity<LitigationRequestGroundFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07E98400B7");

            entity.ToTable("LitigationRequestGroundFile");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileManager).WithMany(p => p.LitigationRequestGroundFiles)
                .HasForeignKey(d => d.FileManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation81857");

            entity.HasOne(d => d.LitigationRequestGround).WithMany(p => p.LitigationRequestGroundFiles)
                .HasForeignKey(d => d.LitigationRequestGroundId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation488105");
        });

        modelBuilder.Entity<LitigationRequestReconciliationMeeting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07F3A8E5CA");

            entity.ToTable("LitigationRequestReconciliationMeeting");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FirstPartyApprovedOn).HasColumnType("datetime");
            entity.Property(e => e.LitigationRequestId);
            entity.Property(e => e.MeetingDate).HasColumnType("datetime");
            entity.Property(e => e.SecondPartyApprovedOn).HasColumnType("datetime");

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestReconciliationMeetings)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation334620");

            entity.HasOne(d => d.LitigationRequestMeetingConfiguration).WithMany(p => p.LitigationRequestMeetings)
                .HasForeignKey(d => d.LitigationRequestMeetingConfigurationId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation600098");

            entity.HasOne(d => d.LitigationRequestMeetingDecision).WithMany(p => p.LitigationRequestMeetings)
                .HasForeignKey(d => d.LitigationRequestMeetingDecisionId)
                .HasConstraintName("FKLitigation358393");
        });

        modelBuilder.Entity<LitigationRequestMeetingReconciliationItem>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07A8D6A249");

            entity.ToTable("LitigationRequestMeetingReconciliationItem");

            entity.HasOne(d => d.LitigationRequestMeeting).WithMany(p => p.LitigationRequestMeetingReconciliationItems)
                .HasForeignKey(d => d.LitigationRequestMeetingId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation688593");
        });

        modelBuilder.Entity<LitigationRequestNote>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC07C1F98E04");

            entity.ToTable("LitigationRequestNote");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LitigationRequestId);
            entity.Property(e => e.Title).HasMaxLength(250);

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestNotes)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation535233");
        });

        modelBuilder.Entity<LitigationRequestNoteFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC070E49794B");

            entity.ToTable("LitigationRequestNoteFile");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileManager).WithMany(p => p.LitigationRequestNoteFiles)
                .HasForeignKey(d => d.FileManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation910435");

            entity.HasOne(d => d.Note).WithMany(p => p.LitigationRequestNoteFiles)
                .HasForeignKey(d => d.NoteId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation888272");
        });

        modelBuilder.Entity<LitigationRequestRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC0751C3A7B1");

            entity.ToTable("LitigationRequestRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.LitigationRequestId);
            entity.Property(e => e.RequestContent).HasMaxLength(200);

            entity.HasOne(d => d.LitigationRequest).WithMany(p => p.LitigationRequestRequests)
                .HasForeignKey(d => d.LitigationRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation674990");
        });

        modelBuilder.Entity<LitigationRequestRequestFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__Litigati__3214EC076FBEF9E2");

            entity.ToTable("LitigationRequestRequestFile");

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.HasOne(d => d.FileManager).WithMany(p => p.LitigationRequestRequestFiles)
                .HasForeignKey(d => d.FileManagerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation620435");

            entity.HasOne(d => d.LitigationRequestRequest).WithMany(p => p.LitigationRequestRequestFiles)
                .HasForeignKey(d => d.LitigationRequestRequestId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKLitigation662656");
        });

        modelBuilder.Entity<SysDepartment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysDepar__3214EC07035C6297");

            entity.ToTable("SysDepartment");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.DepartmentName).HasMaxLength(50);
            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysEmployee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysEmplo__3214EC0760D0BD3B");

            entity.ToTable("SysEmployee");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.EmployeeName).HasMaxLength(100);
            entity.Property(e => e.IsActive)
                .IsRequired()
                .HasDefaultValueSql("('true')");
            entity.Property(e => e.IsLocked)
                .IsRequired()
                .HasDefaultValueSql("('false')");
            entity.Property(e => e.Mobile).HasMaxLength(20);

            entity.HasOne(d => d.Department).WithMany(p => p.SysEmployees)
                .HasForeignKey(d => d.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSysEmploye454468");
            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysFileManager>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysFileM__3214EC07904841A6");

            entity.ToTable("SysFileManager");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.FilePath).HasMaxLength(1000);
            entity.Property(e => e.FileType).HasMaxLength(10);
        });

        modelBuilder.Entity<SysGender>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysGende__3214EC0743341C99");

            entity.ToTable("SysGender");

            entity.Property(e => e.Gender).HasMaxLength(10);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysIdentityType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysIdent__3214EC0765CD90E5");

            entity.ToTable("SysIdentityType");

            entity.Property(e => e.IdentityType).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestAmount>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC07298413D2");

            entity.ToTable("SysLitigationRequestAmount");

            entity.HasIndex(e => e.LitigationAmount, "UQ__SysLitig__1931ED8347FF283F").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Fees).HasColumnType("money");
            entity.Property(e => e.LitigationAmount).HasMaxLength(50);
        });

        modelBuilder.Entity<SysLitigationRequestCategory>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC072E49D791");

            entity.ToTable("SysLitigationRequestCategory");

            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .HasComment("صلح – تحكيم-صلح وتحكيم");

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestContractType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC074D10EA1A");

            entity.ToTable("SysLitigationRequestContractType");

            entity.HasIndex(e => e.TypeName, "UQ__SysLitig__D4E7DFA8A7F5E85A").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TypeName).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestCorrespondenceDestination>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC071298CB03");

            entity.ToTable("SysLitigationRequestCorrespondenceDestination");

            entity.HasIndex(e => e.Destination, "UQ__SysLitig__06968C68F7DD93E4").IsUnique();

            entity.Property(e => e.Destination).HasMaxLength(20);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestExtendPeriodRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC07708342EA");

            entity.ToTable("SysLitigationRequestExtendPeriodRequestStatus");

            entity.HasIndex(e => e.StatusName, "UQ__SysLitig__05E7698A4ABD62EC").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();

            entity.Property(e => e.StatusName).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestMeetingConfiguration>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC076879FC7E");

            entity.ToTable("SysLitigationRequestMeetingConfiguration");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.From).HasColumnType("datetime");
            entity.Property(e => e.To).HasColumnType("datetime");
        });

        modelBuilder.Entity<SysLitigationRequestMeetingDecision>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC07318BAEC8");

            entity.ToTable("SysLitigationRequestMeetingDecision");

            entity.Property(e => e.MeetingDecision).HasMaxLength(20);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestOwnerType>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC070BF201CD");

            entity.ToTable("SysLitigationRequestOwnerType");

            entity.Property(e => e.LitigationRequestOwnerType)
                .HasMaxLength(50)
                .HasComment("مقدم عن نفسه – مفوض");

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysLitigationRequestStatus>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysLitig__3214EC0715A02324");

            entity.ToTable("SysLitigationRequestStatus");

            entity.Property(e => e.RequestStatus).HasMaxLength(50);

            entity.OwnsMany(d => d.Translations, ownedNavigationBuilder => { ownedNavigationBuilder.ToJson(); });

        });

        modelBuilder.Entity<SysRegistrationRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysRegis__3214EC07CCB73928");

            entity.ToTable("SysRegistrationRequest");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Cranumber)
                .HasMaxLength(20)
                .HasColumnName("CRANumber");
            entity.Property(e => e.Email).HasMaxLength(250);
            entity.Property(e => e.Facility700Number).HasMaxLength(20);
            entity.Property(e => e.IdentityNumber).HasMaxLength(20);
            entity.Property(e => e.Mobile).HasMaxLength(20);
            entity.Property(e => e.RegistrationDoneOn).HasColumnType("date");
            entity.Property(e => e.RequestedOn).HasColumnType("date");

            entity.HasOne(d => d.Citizen).WithMany(p => p.SysRegistrationRequestCitizens)
                .HasForeignKey(d => d.CitizenId)
                .HasConstraintName("FKSysRegistr415781");

            entity.HasOne(d => d.SourceCitizen).WithMany(p => p.SysRegistrationRequestSourceCitizens)
                .HasForeignKey(d => d.SourceCitizenId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FKSysRegistr676101");
        });

        modelBuilder.Entity<SysSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__SysSetti__3214EC07BA9C86EA");

            entity.ToTable("SysSetting");

            entity.HasIndex(e => e.Key, "UQ__SysSetti__C41E0289172F62B6").IsUnique();

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.Key).HasMaxLength(100);
            entity.Property(e => e.Value).HasMaxLength(225);
        });

        modelBuilder.Entity<OtherCategory>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Id).HasColumnType("uniqueidentifier");
        });




        // By abdulrahman
        //modelBuilder.Entity<SysLitigationRequestContractType>()
        //  .HasKey(lr => new { lr.Id, lr.CodingPrefixSymbol });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
