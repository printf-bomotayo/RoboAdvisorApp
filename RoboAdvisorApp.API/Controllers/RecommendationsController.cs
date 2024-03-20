using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Data;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Services.Interface;

namespace RoboAdvisorApp.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecommendationsController : ControllerBase
    {
        private readonly IRecommendationService _recommendationService;

        private readonly RoboAppDbContext _context;

        public RecommendationsController(IRecommendationService recommendationService, RoboAppDbContext context)
        {
            _recommendationService = recommendationService;
            _context = context;
        }

        [HttpPost]
        [Route("user/{userId:Guid}")]
        public async Task<IActionResult> GetRecommendationsForQuestionnaire([FromBody] QuestionnaireDto questionnaireDto, Guid userId)
        {
            try
            {
               var recommendations = await _recommendationService.GetRecommendationsAsync(questionnaireDto, userId);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpGet]
        [Route("questionnaire/{questionnaireId:Guid}")]
        public async Task<IActionResult> FetchRecommendationsForQuestionnaire(Guid questionnaireId)
        {
            try
            {
                var recommendations = await _recommendationService.FetchRecommendationsByQuestionnaireAsync(questionnaireId);
                return Ok(recommendations);
            }
            catch (Exception ex)
            {
                
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
