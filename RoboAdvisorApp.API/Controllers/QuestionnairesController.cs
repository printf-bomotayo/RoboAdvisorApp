using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Services;
using RoboAdvisorApp.API.Services.Interface;

namespace RoboAdvisorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QuestionnairesController : ControllerBase
    {
        private readonly IQuestionnaireService _questionnaireService;

        public QuestionnairesController(IQuestionnaireService questionnaireService)
        {
            _questionnaireService = questionnaireService;
        }

        [HttpPost]
        [Route("{userId:Guid}")]
        public async Task<IActionResult> CreateQuestionnaire(Guid userId, QuestionnaireDto questionnaireDto)
        {
            var questionnaire = await _questionnaireService.CreateQuestionnaireAsync(userId, questionnaireDto);
            return Ok(questionnaire);
        }

        [HttpGet]
        [Route("{questionnaireId:Guid}")]
        public async Task<IActionResult> GetQuestionnaire(Guid questionnaireId)
        {
            var questionnaire = await _questionnaireService.GetQuestionnaireAsync(questionnaireId);

            if (questionnaire == null)
                return NotFound();

            return Ok(questionnaire);
        }

        [HttpGet]
        [Route("user/{userId:Guid}")]
        public async Task<IActionResult> GetQuestionnairesByUser(Guid userId)
        {
            var questionnaires = await _questionnaireService.GetQuestionnairesByUserAsync(userId);

            if (questionnaires == null || questionnaires.Count() == 0)
                return NotFound();

            return Ok(questionnaires);
        }
    }
}