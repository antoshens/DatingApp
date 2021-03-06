using DatingApp.Core.Model;
using DatingApp.Core.Model.DomainModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using System.ComponentModel.DataAnnotations;

namespace DatingApp.Core.Data
{
    public class DataContext : IdentityDbContext<User, Role, int,
        IdentityUserClaim<int>, UserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        private readonly int _userId;
        private readonly ILogger<DataContext> _logger;

        // To be used for development purpose only
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DataContext(DbContextOptions<DataContext> options,
            ILogger<DataContext> logger, int ? userId = null) : base(options)
        {
            _userId = userId.HasValue ? userId.Value : 0;
            _logger = logger;
        }

        /* 
         * ===============================================================
         * ========================== Db Tables ==========================
         * ===============================================================
         */
        public virtual DbSet<AuditInfo> AuditInfoes { get; set; }
        public virtual DbSet<Photo> Photos { get; set; }
        public virtual DbSet<UserLike> UserLikes { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Connection> Connections { get; set; }
        public virtual DbSet<Group> Groups { get; set; }

        /* 
         * ===============================================================
         * ======================= Override methods ======================
         * ===============================================================
         */
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<User>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.User)
                .HasForeignKey(u => u.UserId)
                .IsRequired();

            builder.Entity<Role>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId)
                .IsRequired();

            builder.Entity<UserLike>()
                .HasOne(s => s.SourceUser)
                .WithMany(l => l.LikedUsers)
                .HasForeignKey(s => s.SourceUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLike>()
                .HasOne(s => s.LikedUser)
                .WithMany(l => l.LikedByUsers)
                .HasForeignKey(s => s.LikedUserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Message>()
                .HasOne(s => s.SenderUser)
                .WithMany(l => l.SentMessages)
                .HasForeignKey(s => s.SenderId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
                .HasOne(s => s.RecipientUser)
                .WithMany(l => l.ReceivedMessages)
                .HasForeignKey(s => s.RecipientId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        public override int SaveChanges()
        {
            IEnumerable<EntityEntry<IEntityAudit>> newEntities;

            PreSaveHandler(out newEntities);

            ValidateEntities();

            int resultValue = base.SaveChanges();

            PostSaveHandler(newEntities);

            return resultValue;
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            IEnumerable<EntityEntry<IEntityAudit>> newEntities;

            PreSaveHandler(out newEntities);

            ValidateEntities();

            int resultValue = await base.SaveChangesAsync(cancellationToken);

            PostSaveHandler(newEntities);

            return resultValue;
        }

        private void PreSaveHandler(out IEnumerable<EntityEntry<IEntityAudit>> newEntities)
        {
            AddAuditInfoRecords(out newEntities);

            ModifyAuditInfoRecords();
        }

        private void AddAuditInfoRecords(out IEnumerable<EntityEntry<IEntityAudit>> newEntities)
        {
            var addedEntities = ChangeTracker.Entries<IEntityAudit>().Where(x => x.State == EntityState.Added).ToList();

            var createdDate = DateTime.UtcNow;

            foreach (var entity in addedEntities)
            {
                var audit = new AuditInfo
                {
                    CreatedBy = _userId,
                    CreatedDate = createdDate,
                    ModifiedDate = createdDate,
                    ModifiedBy = _userId,
                    PrimaryKey = entity.Entity.AuditEntityKey
                };

                entity.Entity.AuditInfo = audit;
            }

            newEntities = addedEntities;
        }

        private void ModifyAuditInfoRecords()
        {
            var modifiedEntities = ChangeTracker.Entries<IEntityAudit>().Where(x => x.State == EntityState.Modified).ToList();

            if (modifiedEntities.Count == 0) return;

            var existingAuditRecords = ChangeTracker
                .Entries<AuditInfo>()
                .Where(x => x.Entity != null)
                .Select(x => x.Entity)
                .ToList();

            var logDate = DateTime.UtcNow;

            foreach (var entity in modifiedEntities)
            {
                var audit = entity.Entity.AuditInfo;

                if (audit == null)
                {
                    audit = existingAuditRecords.FirstOrDefault(x => x.AuditInfoId == entity.Entity.AuditInfoId);
                    if (audit == null)
                    {
                        audit = new AuditInfo { AuditInfoId = entity.Entity.AuditInfoId };
                        entity.Entity.AuditInfo = audit;
                    }
                }

                audit.ModifiedBy = _userId;
                audit.ModifiedDate = DateTime.UtcNow;
            }
        }

        private void PostSaveHandler(IEnumerable<EntityEntry<IEntityAudit>> newEntities)
        {
            var hasNewAuditRecords = false;

            foreach (var entries in newEntities)
            {
                // Set PrimaryKey field for added audit info records
                entries.Entity.AuditInfo.PrimaryKey = entries.Entity.AuditEntityKey;
                hasNewAuditRecords = true;
            }

            if (hasNewAuditRecords)
            {
                base.SaveChanges();
            }
        }

        private void ValidateEntities()
        {
            var entities = from e in ChangeTracker.Entries<IEntity>()
                           where e.State == EntityState.Added
                               || e.State == EntityState.Modified
                           select e.Entity;

            bool isFailedEntity = false;

            foreach (var entity in entities)
            {
                var validationContext = new ValidationContext(entity);

                try
                {
                    Validator.ValidateObject(entity, validationContext);
                }
                catch (ValidationException ex)
                {
                    _logger.LogWarning($"Entity:({entity.GetType().Name}) - ID:{entity.PrimaryKey}; Error Message:({ex.Message})");
                    isFailedEntity = true;
                }
            }

            if (isFailedEntity)
            {
                throw new ValidationException("Some entities are in not valid state");
            }
        }

    }
}
