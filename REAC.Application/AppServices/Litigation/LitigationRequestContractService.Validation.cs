using Core.Localizations;
using Microsoft.Extensions.Localization;
using REAC.Application.Contracts.Dtos.Litigation;
using REAC.Application.Contracts.Dtos.Litigation.Create.Contract;
using REAC.Application.Contracts.Dtos.Litigation.CreateUpdate.Contract;
using REAC.Application.Contracts.IAppServices;
using REAC.Domain.Shared.Enum;
using REAC.EntityFrameworkCore.Entities.Litigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.AppServices.Litigation
{
    public partial class LitigationRequestContractService
    {
        private static List<string> ValidateRentAsync(IQueryable<LitigationRequestContract> contracts, RentCreateUpdateDto model, DateTime? identityExpiration)
        {
            var errors = new List<string>();
            #region Validations
            var checkContract = contracts.Any(x => x.ContractKey == model.ContractKey && x.LitigationRequests.Any(x => x.InternalStatusId != LitigationRequestStatus.SaveAndClose));
            if (checkContract)
                errors.Add(Localizations.CanNotAddNewLitigation);

            if (DateTime.Now > identityExpiration)
                errors.Add(Localizations.IdentityExpired);

            #endregion
            return errors;
        }

        private static List<string> ValidateRealStateAsync(IQueryable<LitigationRequestContract> contracts, RealEstateBrokerageCreateUpdateDto model, DateTime? identityExpiration)
        {
            var errors = new List<string>();
            #region Validations
            var checkContract = contracts.Any(x => x.ContractKey == model.ContractKey && x.LitigationRequests.Any(x => x.InternalStatusId != LitigationRequestStatus.SaveAndClose));
            if (checkContract)
                errors.Add(Localizations.CanNotAddNewLitigation);

            if (DateTime.Now > identityExpiration)
                errors.Add(Localizations.IdentityExpired);

            #endregion
            return errors;
        }

        private static List<string> ValidatePropertiesAsync(DateTime? identityExpiration)
        {
            var errors = new List<string>();
            #region Validations
            if (DateTime.Now > identityExpiration)
                errors.Add(Localizations.IdentityExpired);
            #endregion
            return errors;
        }   

        private static List<string> ValidateOtherAsync(DateTime? identityExpiration)
        {
            var errors = new List<string>();
            #region Validations
            if (DateTime.Now > identityExpiration)
                errors.Add(Localizations.IdentityExpired);
            #endregion
            return errors;
        }

        private static List<string> ValidateWafiAsync(IQueryable<LitigationRequestContract> contracts, WafiCreateUpdateDto model, DateTime? identityExpiration)
        {
            var errors = new List<string>();
            #region Validations
            var checkContract = contracts.Any(x => x.ContractKey == model.ContractKey && x.LitigationRequests.Any(x => x.InternalStatusId != LitigationRequestStatus.SaveAndClose));
            if (checkContract)
                errors.Add(Localizations.CanNotAddNewLitigation);

            if (DateTime.Now > identityExpiration)
                errors.Add(Localizations.IdentityExpired);

            #endregion
            return errors;
        }
    }
}
