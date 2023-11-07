namespace API.Errors
{
    public class ApiException : ApiResponse
    {
        public ApiException(int statusCode, string messageError = null, string exceptionDetails = null) : base(statusCode, messageError)
        {
            ExceptionDetails = exceptionDetails;
        }

        public string ExceptionDetails { get; set; }
    }
}
