namespace RoboAdvisorApp.API.Models.Domain
{
    public class Questionnaire
    {
        public Guid Id { get; set; }
        public int Age { get; set; }
        public string Location { get; set; }
        public string InvestmentKnowledge { get; set; }
        public string InvestmentPurpose { get; set; }
        public int InvestmentHorizon { get; set; }
        public string RiskTolerance { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }

        public Guid UserId { get; set; }
        public User User { get; set; }
        public ICollection<Recommendation> Recommendations { get; set; }

        }
}
