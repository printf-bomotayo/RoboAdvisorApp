namespace RoboAdvisorApp.API.Models.DTO.Request
{
    public class QuestionnaireRequestDto
    {
        public int Age { get; set; }
        public string Location { get; set; }
        public string InvestmentKnowledge { get; set; }
        public string InvestmentPurpose { get; set; }
        public int InvestmentHorizon { get; set; }
        public string RiskTolerance { get; set; }
        public double Amount { get; set; }
        public string Currency { get; set; }
    }
}
