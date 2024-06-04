namespace Core.Shared.Responses
{
    public class ExceptionResponse : BaseResponse 
    {
        public ExceptionResponse()
        {
        }

        public ExceptionResponse(string exception)
        {
            Exception = exception;
        }
        public ExceptionResponse(List<string>? exceptions)
        {
            Exceptions = exceptions;
        }

        /// <summary>
        /// Process Return Exception
        /// </summary>
        public string? Exception { get; set; }
        public List<string>? Exceptions { get; set; }
    }
}