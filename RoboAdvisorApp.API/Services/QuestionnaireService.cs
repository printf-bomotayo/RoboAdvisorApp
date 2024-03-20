using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Data;
using RoboAdvisorApp.API.Models.Domain;
using RoboAdvisorApp.API.Models.DTO;
using RoboAdvisorApp.API.Services.Interface;
using System;

namespace RoboAdvisorApp.API.Services
{
    public class QuestionnaireService : IQuestionnaireService
    {
        private readonly RoboAppDbContext _context;
        public QuestionnaireService(RoboAppDbContext context)
        {
            _context = context;
        }

        public async Task<QuestionnaireDto> CreateQuestionnaireAsync(Guid userId, QuestionnaireDto questionnaireDto)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                throw new ArgumentException("User not found");

            var questionnaire = new Questionnaire
            {
                UserId = userId,
                Age = questionnaireDto.Age,
                Location = questionnaireDto.Location,
                InvestmentKnowledge = questionnaireDto.InvestmentKnowledge,
                InvestmentPurpose = questionnaireDto.InvestmentPurpose,
                InvestmentHorizon = questionnaireDto.InvestmentHorizon,
                RiskTolerance = questionnaireDto.RiskTolerance,
                Amount = questionnaireDto.Amount,
                Currency = questionnaireDto.Currency
            };

            _context.Questionnaires.Add(questionnaire);
            await _context.SaveChangesAsync();

            return MapToDTO(questionnaire);
        }

        public async Task<QuestionnaireDto> GetQuestionnaireAsync(Guid questionnaireId)
        {
            var questionnaire = await _context.Questionnaires.FindAsync(questionnaireId);

            if (questionnaire == null)
                return null;

            return MapToDTO(questionnaire);
        }


        public async Task<List<QuestionnaireDto>> GetQuestionnairesByUserAsync(Guid userId)
        {
            var questionnaires = await _context.Questionnaires
                .Where(q => q.UserId == userId)
                .Select(q => MapToDTO(q))
                .ToListAsync();

            return questionnaires;
        }


        private QuestionnaireDto MapToDTO(Questionnaire questionnaire)
        {
            return new QuestionnaireDto
            {
                UserId = questionnaire.UserId,
                Age = questionnaire.Age,
                Location = questionnaire.Location,
                InvestmentKnowledge = questionnaire.InvestmentKnowledge,
                InvestmentPurpose = questionnaire.InvestmentPurpose,
                InvestmentHorizon = questionnaire.InvestmentHorizon,
                RiskTolerance = questionnaire.RiskTolerance,
                Amount = questionnaire.Amount,
                Currency = questionnaire.Currency
            };
        }
    }
    
}
