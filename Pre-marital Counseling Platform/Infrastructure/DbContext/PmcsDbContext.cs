﻿using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SWP391.Domain;

namespace SWP391.Infrastructure.DbContext;

public class PmcsDbContext : IdentityDbContext
{
    public new DbSet<User> Users { get; set; }
    public new DbSet<Therapist> Therapists { get; set; }
    public new DbSet<Wallet> Wallets { get; set; }
    public new DbSet<Transaction> Transactions { get; set; }
    public new DbSet<Schedule> Schedules { get; set; }
    public new DbSet<Booking> Bookings { get; set; }
    public new DbSet<BookingResult> BookingResults { get; set; }
    public new DbSet<Feedback> Feedbacks { get; set; }
    public new DbSet<TherapistSpecification> TherapistSpecifications { get; set; }
    public new DbSet<Specification> Specifications { get; set; }
    public new DbSet<Category> Categories { get; set; }
    public new DbSet<Quiz> Quizes { get; set; }
    public new DbSet<QuizResult> QuizResults { get; set; }
    public new DbSet<MemberResult> MemberResults { get; set; }
    public new DbSet<Question> Questions { get; set; }
    public new DbSet<Answer> Answers { get; set; }
    public new DbSet<MemberAnswer> MemberAnswers { get; set; }

    private readonly string COLLATION = "SQL_Latin1_General_CP1_CI_AI";

    public PmcsDbContext(DbContextOptions<PmcsDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.UserId);
            entity.Property(u => u.FullName).UseCollation(COLLATION).HasMaxLength(100);
            entity.Property(u => u.Phone).IsRequired().HasMaxLength(11);
            entity.Property(u => u.Password).HasMaxLength(100);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).IsRequired();
            entity.Property(u => u.AvatarUrl).IsRequired(false).HasMaxLength(100);
            entity.Property(u => u.IsActive).IsRequired();
            entity.Property(u => u.IsAdmin).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
        });

        modelBuilder.Entity<Therapist>(entity =>
        {
            entity.HasKey(u => u.TherapistId);
            entity.Property(u => u.ConsultationFee).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(u => u.WalletId);
            entity.Property(u => u.Balance).HasDefaultValue(0);
            entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(u => u.TransactionId);
            entity.Property(u => u.Amount).HasDefaultValue(0);
            entity.Property(u => u.Description).HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Schedule>(entity =>
        {
            entity.HasKey(u => u.ScheduleId);
            entity.HasOne(u => u.Therapist).WithMany().HasForeignKey(u => u.TherapistId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Date).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(u => u.Slot).HasDefaultValue(0);
            entity.Property(u => u.IsAvailable).HasDefaultValue(true);
        });

        modelBuilder.Entity<Booking>(entity =>
        {
            entity.HasKey(u => u.BookingId);
            entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.MemberId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.Therapist).WithMany().HasForeignKey(u => u.TherapistId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.MemberResult).WithMany().HasForeignKey(u => u.MemberResultId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.Slot).WithMany().HasForeignKey(u => u.SlotId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Status).HasMaxLength(100);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<BookingResult>(entity =>
        {
            entity.HasKey(u => u.BookingResultId);
            entity.HasOne(u => u.Booking).WithMany().HasForeignKey(u => u.BookingId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Description).IsRequired().HasMaxLength(100);
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(u => u.FeedbackId);
            entity.HasOne(u => u.Booking).WithMany().HasForeignKey(u => u.BookingId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Rating).HasDefaultValue(0);
            entity.Property(u => u.FeedbackTitle).IsRequired().HasMaxLength(100);
            entity.Property(u => u.FeedbackContent).IsRequired().HasMaxLength(100);
            entity.Property(u => u.IsSatisfied).HasDefaultValue(true);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<TherapistSpecification>(entity =>
        {
            entity.HasNoKey();
            entity.HasOne(u => u.Therapist).WithMany().HasForeignKey(u => u.TherapistId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.Specification).WithMany().HasForeignKey(u => u.SpecificationId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Specification>(entity =>
        {
            entity.HasKey(u => u.SpecificationId);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Description).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Level).HasDefaultValue(1);
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(u => u.CategoryId);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Quiz>(entity =>
        {
            entity.HasKey(u => u.QuizId);
            entity.HasOne(u => u.Category).WithMany().HasForeignKey(u => u.CategoryId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<QuizResult>(entity =>
        {
            entity.HasKey(u => u.QuizResultId);
            entity.HasOne(u => u.Quiz).WithMany().HasForeignKey(u => u.QuizId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Score).HasDefaultValue(0);
            entity.Property(u => u.Level).HasDefaultValue(1);
            entity.Property(u => u.Title).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Description).IsRequired().HasMaxLength(200);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<MemberResult>(entity =>
        {
            entity.HasKey(u => u.MemberResultId);
            entity.HasOne(u => u.Quiz).WithMany().HasForeignKey(u => u.QuizId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.User).WithMany().HasForeignKey(u => u.MemberId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.QuizResult).WithMany().HasForeignKey(u => u.QuizResultId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.Score).HasDefaultValue(0);
        });

        modelBuilder.Entity<Question>(entity =>
        {
            entity.HasKey(u => u.QuestionId);
            entity.Property(u => u.QuestionContent).IsRequired().HasMaxLength(200);
            entity.HasOne(u => u.Quiz).WithMany().HasForeignKey(u => u.QuizId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<Answer>(entity =>
        {
            entity.HasKey(u => u.AnswerId);
            entity.HasOne(u => u.Question).WithMany().HasForeignKey(u => u.QuestionId).OnDelete(DeleteBehavior.NoAction);
            entity.Property(u => u.AnswerContent).IsRequired().HasMaxLength(200);
            entity.Property(u => u.Score).HasDefaultValue(0);
            entity.Property(e => e.CreatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.Property(e => e.UpdatedAt).IsRequired().HasDefaultValueSql("GETDATE()");
            entity.HasOne(e => e.CreatedUser).WithMany().HasForeignKey(e => e.CreatedBy).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.UpdatedUser).WithMany().HasForeignKey(e => e.UpdatedBy).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<MemberAnswer>(entity =>
        {
            entity.HasKey(u => u.AnswerId);
            entity.HasOne(u => u.Member).WithMany().HasForeignKey(u => u.MemberId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(e => e.Question).WithMany().HasForeignKey(e => e.QuestionId).OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(u => u.Answer).WithMany().HasForeignKey(u => u.AnswerId).OnDelete(DeleteBehavior.NoAction);
        });
    }
}
