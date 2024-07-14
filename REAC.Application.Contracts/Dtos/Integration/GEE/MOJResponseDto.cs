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
        public ResponseObjectDto ResponseObject { get; set; }
    }

    public class ResponseObjectDto
    {
        public List<ResultsDto>? Results { get; set; }
        public string? ResponseMessage { get; set; }
    }

    public class ResultsDto
    {
        public ModelDto Model { get; set; }
    }

    public class ModelDto
    {
        public string EndDateHijri { get; set; }
        public string EndDateHijriArabic { get; set; }
        public string EndDateGreg { get; set; }

    }

}
