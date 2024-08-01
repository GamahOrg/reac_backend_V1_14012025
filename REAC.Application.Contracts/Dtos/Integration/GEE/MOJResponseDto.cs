using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace REAC.Application.Contracts.Dtos.Integration.GEE
{
    public class MOJResponseDto
    {
        public ResponseObjectDto? Data { get; set; }
        public GeneralGEEErrorDto? Error { get; set; }
        public UnauthorizedErrorDto? UnauthorizedError { get; set; }
    }

    public class ResponseObjectDto
    {
        public ResultsDto responseObject { get; set; }
    }

    public class ResultsDto
    {
        public List<ResultsDataDto>? Results { get; set; }
        public string? ResponseMessage { get; set; }
    }

    public class ResultsDataDto
    {
        public ModelDto Model { get; set; }
    }

    public class ModelDto
    {
        public int Code { get; set; }
        public List<PartiesDto> FirstParties { get; set; }
        public List<PartiesDto> SecondParties { get; set; }
        public List<TextsValuesListDto> TextsValuesList { get; set; }
        public string EndDateGreg { get; set; }

    }

    public class PartiesDto
    {
        public string IdentityNo { get; set; }
        //public string FullName { get; set; }
        //public string NationalityId { get; set; }
        //public string IsValid { get; set; }
        //public string PartySefaTypeName { get; set; }
        //public string PersonId { get; set; }
    }

    public class TextsValuesListDto
    {
        public int ControlID { get; set; }
        public string ControlLable { get; set; }
    }

}
