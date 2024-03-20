using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Data;
using RoboAdvisorApp.API.Models.Domain;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Models.DTO.Request;
using RoboAdvisorApp.API.Models.DTO.Response;
using RoboAdvisorApp.API.Services.Interface;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace RoboAdvisorApp.API.Services
{
    public class RecommendationService : IRecommendationService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly RoboAppDbContext _context;

        public RecommendationService(HttpClient httpClient, IConfiguration configuration, RoboAppDbContext context)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _context = context;
        }

        public async Task<List<RecommendationDto>> FetchRecommendationsByQuestionnaireAsync(Guid questionnaireId)
        {
            var recommendations = await _context.Recommendations
               .Where(r => r.QuestionnaireId == questionnaireId)
               .Select(r => MapToDTO(r))
               .ToListAsync();

            return recommendations;
        }

        async Task<List<RecommendationResponseDto>> IRecommendationService.GetRecommendationsAsync(QuestionnaireDto questionnaireDto, Guid userId)
        {
            var apiBaseUrl = _configuration["ExternalApi:BaseUrl"];
            var apiRequestEndpoint = _configuration["ExternalApi:RequestEndpoint"];
            var apiResponseEndpoint = _configuration["ExternalApi:RecommendationsResponseEndpoint"];

            var questionnaireRequest = MapQuestionnaireDtoToQuestionnaireRequestDto(questionnaireDto);

            var requestBody = JsonSerializer.Serialize(questionnaireRequest);
            var content = new StringContent(requestBody, Encoding.UTF8, "application/json");

            var requestFeedback = await _httpClient.PostAsync(apiBaseUrl + apiRequestEndpoint, content);

            if (!requestFeedback.IsSuccessStatusCode)
            {
                // Handle API error
                throw new HttpRequestException($"Failed to submit questionnaire. Status code: {requestFeedback.StatusCode}");
            }

            var response = await _httpClient.GetAsync(apiBaseUrl + apiResponseEndpoint);

            if (!response.IsSuccessStatusCode)
            {
                // Handle API error
                throw new HttpRequestException($"Failed to get recommendations. Status code: {response.StatusCode}");
            }


            // persist questionnaire object based on associated userId and having confirmed that a valid response has been retrieved from the external ai service
            var questionnaire = MapQuestionnaireDtoToQuestionnaireObject(questionnaireDto, userId);
            _context.Questionnaires.Add(questionnaire);
            await _context.SaveChangesAsync();

            // Retrieve the Id of the persisted questionnaire
            var questionnaireId = _context.Entry(questionnaire).Property(q => q.Id).CurrentValue;


            // response stream coming from the Get request sent to the external AI service
            using var responseStream = await response.Content.ReadAsStreamAsync();
            var recommendations = await JsonSerializer.DeserializeAsync<List<RecommendationResponseDto>>(responseStream);

            if(recommendations == null)
            {
                return null;
            }
            foreach (var recommendationResponseDto in recommendations)
            {
                var recommendation = MapRecommendationDTOToRecommendationObject(recommendationResponseDto, questionnaireId);

                _context.Recommendations.Add(recommendation);
                await _context.SaveChangesAsync();
            }
            return recommendations;
        }

        private Recommendation MapRecommendationDTOToRecommendationObject(RecommendationResponseDto recommendationResponseDto, Guid questionnaireId)
        {
            return new Recommendation
            {
                QuestionnaireId = questionnaireId,
                BriefDescription = recommendationResponseDto.BriefDescription,
                ExpectedReturn = recommendationResponseDto.ExpectedReturn,
                EstimatedReturnValue = recommendationResponseDto.EstimatedReturnValue,
                Ticker = recommendationResponseDto.Ticker,
                Composition = recommendationResponseDto.Composition,
                Principal = recommendationResponseDto.Principal,
                Provider = recommendationResponseDto.Provider,
                FinancialProduct = recommendationResponseDto.FinancialProduct,
                Currency = recommendationResponseDto.Currency
            };
        }



        private static RecommendationDto MapToDTO(Recommendation recommendation)
        {
            return new RecommendationDto
            {
                QuestionnaireId = recommendation.QuestionnaireId,
                BriefDescription = recommendation.BriefDescription,
                Composition = recommendation.Composition,
                ExpectedReturn = recommendation.ExpectedReturn,
                EstimatedReturnValue = recommendation.EstimatedReturnValue,
                FinancialProduct = recommendation.FinancialProduct,
                Provider = recommendation.Provider,
                Ticker = recommendation.Ticker,
                Currency = recommendation.Currency
            };
        }

        private QuestionnaireRequestDto MapQuestionnaireDtoToQuestionnaireRequestDto(QuestionnaireDto questionnaireDto)
        {
            return new QuestionnaireRequestDto
            {
                Age = questionnaireDto.Age,
                Location = questionnaireDto.Location,
                Currency = questionnaireDto.Currency,
                InvestmentHorizon = questionnaireDto.InvestmentHorizon,
                InvestmentPurpose = questionnaireDto.InvestmentPurpose,
                InvestmentKnowledge = questionnaireDto.InvestmentKnowledge,
                RiskTolerance = questionnaireDto.RiskTolerance,
                Amount = questionnaireDto.Amount
            };
        }

        private Questionnaire MapQuestionnaireDtoToQuestionnaireObject(QuestionnaireDto questionnaireDto, Guid userId)
        {
            return new Questionnaire
            {
                UserId = userId,
                Age = questionnaireDto.Age,
                Location = questionnaireDto.Location,
                Currency = questionnaireDto.Currency,
                InvestmentHorizon = questionnaireDto.InvestmentHorizon,
                InvestmentPurpose = questionnaireDto.InvestmentPurpose,
                InvestmentKnowledge = questionnaireDto.InvestmentKnowledge,
                RiskTolerance = questionnaireDto.RiskTolerance,
                Amount = questionnaireDto.Amount
            };
        }
    }
}
