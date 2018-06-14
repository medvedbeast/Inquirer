using System;
using InquirerAPI.PublicAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace InquirerAPI.PublicAPI.Models
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Answer> Answers { get; set; }
        public virtual DbSet<Collector> Collectors { get; set; }
        public virtual DbSet<Option> Options { get; set; }
        public virtual DbSet<Question> Questions { get; set; }
        public virtual DbSet<QuestionType> QuestionTypes { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Respondent> Respondents { get; set; }
        public virtual DbSet<Survey> Surveys { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserEducationProgress> UserEducationProgresses { get; set; }
        public virtual DbSet<UserEducationType> UserEducationTypes { get; set; }
        public virtual DbSet<UserGroup> UserGroups { get; set; }
        public virtual DbSet<UserLanguage> UserLanguages { get; set; }
        public virtual DbSet<UserLocation> UserLocations { get; set; }
        public virtual DbSet<UserOccupation> UserOccupations { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Answer>(entity =>
            {
                entity.ToTable("answer");

                entity.HasIndex(e => e.OptionId)
                    .HasName("answer_fk0");

                entity.HasIndex(e => e.QuestionId)
                    .HasName("answer_fk1");

                entity.HasIndex(e => e.RespondentId)
                    .HasName("answer_fk2");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .HasColumnName("content")
                    .HasMaxLength(1024);

                entity.Property(e => e.OptionId)
                    .HasColumnName("option_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.RespondentId)
                    .HasColumnName("respondent_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Option)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.OptionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("answer_fk0");

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.QuestionId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("answer_fk1");

                entity.HasOne(d => d.Respondent)
                    .WithMany(p => p.Answers)
                    .HasForeignKey(d => d.RespondentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("answer_fk2");
            });

            modelBuilder.Entity<Collector>(entity =>
            {
                entity.ToTable("collector");

                entity.HasIndex(e => e.SurveyId)
                    .HasName("collector_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsOpen)
                    .HasColumnName("is_open")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.SurveyId)
                    .HasColumnName("survey_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Collectors)
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("collector_fk0");
            });

            modelBuilder.Entity<Option>(entity =>
            {
                entity.ToTable("option");

                entity.HasIndex(e => e.QuestionId)
                    .HasName("option_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.IsCustom)
                    .HasColumnName("is_custom")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Label)
                    .HasColumnName("label")
                    .HasMaxLength(128);

                entity.Property(e => e.Index)
                    .HasColumnName("index")
                    .HasColumnType("int(11)");

                entity.Property(e => e.QuestionId)
                    .HasColumnName("question_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Value)
                    .HasColumnName("value")
                    .HasMaxLength(256);

                entity.HasOne(d => d.Question)
                    .WithMany(p => p.Options)
                    .HasForeignKey(d => d.QuestionId)
                    .HasConstraintName("option_fk0");
            });

            modelBuilder.Entity<Question>(entity =>
            {
                entity.ToTable("question");

                entity.HasIndex(e => e.SurveyId)
                    .HasName("question_fk0");

                entity.HasIndex(e => e.TypeId)
                    .HasName("question_fk1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.SurveyId)
                    .HasColumnName("survey_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Index)
                    .HasColumnName("index")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IsRequired)
                    .HasColumnName("is_required")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(256);

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Survey)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.SurveyId)
                    .HasConstraintName("question_fk0");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Questions)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("question_fk1");
            });

            modelBuilder.Entity<QuestionType>(entity =>
            {
                entity.ToTable("question_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<Report>(entity =>
            {
                entity.ToTable("report");

                entity.HasIndex(e => e.CreatorId)
                    .HasName("report_fk1");

                entity.HasIndex(e => e.UserId)
                    .HasName("report_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(2048);

                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");
            });

            modelBuilder.Entity<Respondent>(entity =>
            {
                entity.ToTable("respondent");

                entity.HasIndex(e => e.AssociatedUserId)
                    .HasName("respondent_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.IpAddress)
                    .IsRequired()
                    .HasColumnName("ip_address")
                    .HasMaxLength(16);

                entity.Property(e => e.UserAgent)
                    .IsRequired()
                    .HasColumnName("user_agent")
                    .HasMaxLength(128);

                entity.Property(e => e.AssociatedUserId)
                    .HasColumnName("associated_user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.AssociatedUser)
                    .WithMany(p => p.Respondents)
                    .HasForeignKey(d => d.AssociatedUserId)
                    .OnDelete(DeleteBehavior.Cascade)
                    .HasConstraintName("respondent_fk0");
            });

            modelBuilder.Entity<Survey>(entity =>
            {
                entity.ToTable("survey");

                entity.HasIndex(e => e.CreatorId)
                    .HasName("survey_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.CreatorId)
                    .HasColumnName("creator_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Description)
                    .HasColumnName("description")
                    .HasMaxLength(4096);

                entity.Property(e => e.EndDate)
                    .HasColumnName("end_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.IsAuthenticationRequired)
                    .HasColumnName("is_authentication_required")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.IsOpen)
                    .HasColumnName("is_open")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.StartDate)
                    .HasColumnName("start_date")
                    .HasColumnType("datetime");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasColumnName("title")
                    .HasMaxLength(256);

                entity.HasOne(d => d.Creator)
                    .WithMany(p => p.Surveys)
                    .HasForeignKey(d => d.CreatorId)
                    .HasConstraintName("survey_fk0");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.HasIndex(e => e.EducationProgressId)
                    .HasName("user_fk2");

                entity.HasIndex(e => e.EducationTypeId)
                    .HasName("user_fk1");

                entity.HasIndex(e => e.GroupId)
                    .HasName("user_fk0");

                entity.HasIndex(e => e.LanguageId)
                    .HasName("user_fk3");

                entity.HasIndex(e => e.LocationId)
                    .HasName("user_fk5");

                entity.HasIndex(e => e.OccupationId)
                    .HasName("user_fk4");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EducationProgressId)
                    .HasColumnName("education_progress_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.EducationTypeId)
                    .HasColumnName("education_type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(128);

                entity.Property(e => e.GroupId)
                    .HasColumnName("group_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Image)
                    .HasColumnName("image")
                    .HasColumnType("mediumtext");

                entity.Property(e => e.LanguageId)
                    .HasColumnName("language_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.LocationId)
                    .HasColumnName("location_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.OccupationId)
                    .HasColumnName("occupation_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.Sex)
                    .HasColumnName("sex")
                    .HasColumnType("tinyint(1)");

                entity.HasOne(d => d.EducationProgress)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EducationProgressId)
                    .HasConstraintName("user_fk2");

                entity.HasOne(d => d.EducationType)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.EducationTypeId)
                    .HasConstraintName("user_fk1");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("user_fk0");

                entity.HasOne(d => d.Language)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LanguageId)
                    .HasConstraintName("user_fk3");

                entity.HasOne(d => d.Location)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.LocationId)
                    .HasConstraintName("user_fk5");

                entity.HasOne(d => d.Occupation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.OccupationId)
                    .HasConstraintName("user_fk4");
            });

            modelBuilder.Entity<UserEducationProgress>(entity =>
            {
                entity.ToTable("user_education_progress");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<UserEducationType>(entity =>
            {
                entity.ToTable("user_education_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<UserGroup>(entity =>
            {
                entity.ToTable("user_group");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<UserLanguage>(entity =>
            {
                entity.ToTable("user_language");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<UserLocation>(entity =>
            {
                entity.ToTable("user_location");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(64);
            });

            modelBuilder.Entity<UserOccupation>(entity =>
            {
                entity.ToTable("user_occupation");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });
        }
    }
}
