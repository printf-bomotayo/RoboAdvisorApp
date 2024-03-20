using Microsoft.EntityFrameworkCore;
using RoboAdvisorApp.API.Models.Domain;

namespace RoboAdvisorApp.API.Data
{
    public class RoboAppDbContext : DbContext
    {
        public RoboAppDbContext(DbContextOptions<RoboAppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Questionnaire> Questionnaires { get; set; }
        public DbSet<Recommendation> Recommendations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureRecommendation(modelBuilder);
            ConfigureUser(modelBuilder);
            ConfigureQuestionnaire(modelBuilder);
        }

        private void ConfigureRecommendation(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Recommendation>()
                .HasOne(r => r.Questionnaire)
                .WithMany(q => q.Recommendations)
                .HasForeignKey(r => r.QuestionnaireId);
        }

        private void ConfigureUser(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.Questionnaires)
                .WithOne(q => q.User)
                .HasForeignKey(q => q.UserId);
        }

        private void ConfigureQuestionnaire(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Questionnaire>()
                .HasMany(q => q.Recommendations)
                .WithOne(r => r.Questionnaire)
                .HasForeignKey(r => r.QuestionnaireId);
        }
    }
}
