namespace API.Errors
{
    public class ApiResponse
    {
        public ApiResponse(int statusCode, string messageError = null)
        {
            StatusCode = statusCode;
            MessageError = messageError ?? GetDefaultMessageForStatusCode(statusCode);
        }

        public int StatusCode { get; set; }
        public string MessageError { get; set; }

        private string GetDefaultMessageForStatusCode(int statusCode)
        {
            return StatusCode switch
            {
                400 => "Bad request made",
                401 => "Not authorized",
                404 => "Resource not found",
                500 => "Internal error",
                _ => null
            };
        }
    }
}
