
using Bpf.Application.ModuleServices;
using Core.Contracts;
using Core.Log.Application.Contracts.IAppServices;
using Core.Log.Application.Instance;
using Core.Log.Application.Manager;
using Core.Services;
using REAC.Application.AppServices;
using REAC.Application.AppServices.Integration;
using REAC.Application.AppServices.Litigation;
using REAC.Application.AppServices.Reconciliation;
using REAC.Application.AppServices.System;
using REAC.Application.AppServices.Wpf.Business;
using REAC.Application.AppServices.Wpf.System;
//using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.IAppServices;
using REAC.Application.Contracts.IAppServices.Litigation;
using REAC.Application.Contracts.IAppServices.Reconciliation;
using REAC.Application.Contracts.IAppServices.System;
using REAC.Application.Contracts.IAppServices.Wpf.Business;
using REAC.Application.Contracts.IAppServices.Wpf.System;
using REAC.Application.Contracts.IModuleServices;
using REAC.Application.Services.Reconciliation;
using REAC.IAppServices.System;
using CitizenService = REAC.Application.AppServices.CitizenService;
namespace REAC.Api
{
    public static class AppServicesDi
    {
        public static void AddAppServices(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<DbLogger>();
            services.AddScoped<FileLogger>();
            services.AddScoped<MailLogger>();
            services.AddScoped<IHttpService, HttpService>();

            //services.AddScoped < IFacilityService, FacilityService>();
            // services.AddScoped<ICitizenService, CitizenService>();
            // Register other services
            services.AddScoped<IWorkFlowProcessingLogService, WorkFlowProcessingLogService>();
            services.AddScoped<IWorkFlowLitigationActionsService, WorkFlowLitigationActionsService>();
            services.AddScoped<IBpfService, BpfService>();
            services.AddScoped<IWorkFlowLogService, WorkFlowLogService>();
            services.AddScoped<IWorkFlowStepService, WorkFlowStepService>();
            //services.AddScoped < IDelegationRequestService, DelegationRequestService>();
            //services.AddScoped < IFacilityService, FacilityService>();
            //services.AddScoped < IFacilityDelegatorService, FacilityDelegatorService>();
            services.AddScoped<ICitizenLitigationRequestLawyerAgentService, CitizenLitigationRequestLawyerAgentService>();
            services.AddScoped<ICitizenService, CitizenService>();
            services.AddScoped<ILitigationAreaService, LitigationAreaService>();
            services.AddScoped<ILitigationCityService, LitigationCityService>();
            services.AddScoped<ISysExemptionReductionCategoryService, SysExemptionReductionCategoryService>();
            services.AddScoped<ILitigationRequestService, LitigationRequestService>();
            services.AddScoped<IUserProviderService, LitigationRequestContractService>();
            services.AddScoped<ILitigationRequestCorrespondenceService, LitigationRequestCorrespondenceService>();
            services.AddScoped<ILitigationRequestCorrespondenceFileService, LitigationRequestCorrespondenceFileService>();
            services.AddScoped<ILitigationRequestExtendPeriodRequestService, LitigationRequestExtendPeriodRequestService>();
            services.AddScoped<ILitigationRequestExtendPeriodRequestFileService, LitigationRequestExtendPeriodRequestFileService>();
            services.AddScoped<ILitigationRequestGroundService, Application.AppServices.Litigation.CitizenService>();
            services.AddScoped<ILitigationRequestGroundFileService, LitigationRequestGroundFileService>();
            //services.AddScoped < ILitigationRequestMeetingService, LitigationRequestMeetingService>();
            services.AddScoped<ILitigationRequestMeetingReconciliationItemService, LitigationRequestMeetingReconciliationItemService>();
            services.AddScoped<ILitigationRequestNoteService, LitigationRequestNoteService>();
            services.AddScoped<ILitigationRequestNoteFileService, LitigationRequestNoteFileService>();
            services.AddScoped<ILitigationRequestRequestService, LitigationRequestRequestService>();
            services.AddScoped<ILitigationRequestRequestFileService, LitigationRequestRequestFileService>();
            services.AddScoped<ILitigationRatingService, LitigationRatingService>();
            services.AddScoped<ISysDepartmentService, SysDepartmentService>();
            services.AddScoped<ISysEmployeeService, SysEmployeeService>();
            services.AddScoped<ISysFileManagerService, SysFileManagerService>();
            services.AddScoped<ISysGenderService, SysGenderService>();
            services.AddScoped<ISysIdentityTypeService, SysIdentityTypeService>();
            services.AddScoped<ISysLitigationRequestAmountService, SysLitigationRequestAmountService>();
            services.AddScoped<ISysLitigationRequestCategoryService, SysLitigationRequestCategoryService>();
            services.AddScoped<ISysLitigationRequestContractTypeService, SysLitigationRequestContractTypeService>();
            services.AddScoped<ISysLitigationRequestCorrespondenceDestinationService, SysLitigationRequestCorrespondenceDestinationService>();
            services.AddScoped<ISysLitigationRequestExtendPeriodRequestStatusService, SysLitigationRequestExtendPeriodRequestStatusService>();
            services.AddScoped<ISysLitigationRequestMeetingConfigurationService, SysLitigationRequestMeetingConfigurationService>();
            services.AddScoped<ISysLitigationRequestMeetingDecisionService, SysLitigationRequestMeetingDecisionService>();
            services.AddScoped<ISysLitigationRequestOwnerTypeService, SysLitigationRequestOwnerTypeService>();
            services.AddScoped<ISysLitigationRequestStatusService, SysLitigationRequestStatusService>();
            services.AddScoped<ISysRegistrationRequestService, SysRegistrationRequestService>();
            services.AddScoped<ISysSettingService, SysSettingService>();
            services.AddScoped<ILoggerManager, LoggerManager>();
            services.AddScoped<DbLogger>();
            services.AddScoped<FileLogger>();
            services.AddScoped<MailLogger>();
            services.AddScoped<IMailService, EmailService>();
            services.AddScoped<ILitigationWorkFlowService, LitigationWorkFlowService>();

            services.AddScoped<IJwtReader, JwtReader>();

        }

       
    }
}