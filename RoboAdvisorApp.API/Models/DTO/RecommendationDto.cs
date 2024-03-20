namespace RoboAdvisorApp.API.Models.DTO
{
    public class RecommendationDto
    {
        public Guid QuestionnaireId { get; set; }
        public string BriefDescription { get; set; }
        public string Composition { get; set; }
        public string Currency { get; set; }
        public double EstimatedReturnValue { get; set; }
        public string ExpectedReturn { get; set; }
        public string FinancialProduct { get; set; }
        public double Principal { get; set; }
        public string Provider { get; set; }
        public string Ticker { get; set; }
    }
}
