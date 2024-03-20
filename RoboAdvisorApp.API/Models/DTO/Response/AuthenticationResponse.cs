namespace RoboAdvisorApp.API.Models.DTO.Response
{
    public class AuthenticationResponse
    {
        public Guid UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNo { get; set; }
    }
}
