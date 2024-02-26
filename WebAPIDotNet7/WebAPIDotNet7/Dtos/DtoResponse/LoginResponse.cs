namespace WebAPIDotNet7.Dtos.DtoResponse
{
    public class LoginResponse:ResponseStatus
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
