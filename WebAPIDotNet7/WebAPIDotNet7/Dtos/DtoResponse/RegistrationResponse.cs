namespace WebAPIDotNet7.Dtos.DtoResponse
{
    public class RegistrationResponse:ResponseStatus
    {
        public int UserId { get; set; }
        public string Token { get; set; }
    }
}
