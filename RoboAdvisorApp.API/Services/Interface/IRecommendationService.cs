using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Models.DTO.Response;

namespace RoboAdvisorApp.API.Services.Interface
{
    public interface IRecommendationService
    {
        Task<List<RecommendationResponseDto>> GetRecommendationsAsync(QuestionnaireDto questionnaireDto, Guid userId);

        Task<List<RecommendationDto>> FetchRecommendationsByQuestionnaireAsync(Guid questionnaireId);

    }
}
