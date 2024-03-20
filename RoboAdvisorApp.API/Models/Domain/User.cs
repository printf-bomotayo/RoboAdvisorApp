namespace RoboAdvisorApp.API.Models.Domain
{
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string PhoneNo { get; set; }

        public string PasswordHash { get; set; }
        public ICollection<Questionnaire> Questionnaires { get; set; }
    }
}
