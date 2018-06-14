using InquirerAPI.Website.Data;
using Microsoft.EntityFrameworkCore;

namespace InquirerAPI.Website.Models
{
    public class DatabaseContext : DbContext
    {
        public virtual DbSet<Key> Key { get; set; }
        public virtual DbSet<KeyType> KeyType { get; set; }
        public virtual DbSet<Activity> Activity { get; set; }
        public virtual DbSet<User> User { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Activity>(entity =>
            {
                entity.ToTable("activity");

                entity.HasIndex(e => e.KeyId)
                    .HasName("activity_fk0");

                entity.HasIndex(e => e.UserId)
                    .HasName("activity_fk1");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Status)
                    .HasColumnName("status")
                    .HasColumnType("tinyint(1)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(1024);

                entity.Property(e => e.ExternalUserId)
                    .HasColumnName("external_user_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.KeyId)
                    .HasColumnName("key_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.OccuredOn)
                    .HasColumnName("occured_on")
                    .HasColumnType("datetime");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Key)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.KeyId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("activity_fk0");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Activities)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("activity_fk1");
            });

            modelBuilder.Entity<Key>(entity =>
            {
                entity.ToTable("key");

                entity.HasIndex(e => e.TypeId)
                    .HasName("key_fk1");

                entity.HasIndex(e => e.UserId)
                    .HasName("key_fk0");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasColumnName("content")
                    .HasMaxLength(64);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.TypeId)
                    .HasColumnName("type_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.UserId)
                    .HasColumnName("user_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Type)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.TypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("key_fk1");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Keys)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("key_fk0");
            });

            modelBuilder.Entity<KeyType>(entity =>
            {
                entity.ToTable("key_type");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(32);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("user");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasMaxLength(128);

                entity.Property(e => e.LastSeenOn)
                    .HasColumnName("last_seen_on")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasMaxLength(128);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasColumnName("password")
                    .HasMaxLength(256);

                entity.Property(e => e.RegisteredOn)
                    .HasColumnName("registered_on")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("'CURRENT_TIMESTAMP'");
            });
        }
    }
}
