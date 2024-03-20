using RoboAdvisorApp.API.Models.DTO;

namespace RoboAdvisorApp.API.Services.Interface
{
    // IQuestionnaireService.cs
    public interface IQuestionnaireService
    {
        Task<QuestionnaireDto> CreateQuestionnaireAsync(Guid userId, QuestionnaireDto questionnaireDto);
        Task<QuestionnaireDto> GetQuestionnaireAsync(Guid questionnaireId);
        Task<List<QuestionnaireDto>> GetQuestionnairesByUserAsync(Guid userId);


    }
}
