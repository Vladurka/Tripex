using Microsoft.EntityFrameworkCore;
using Tripex.Core.Domain.Entities;

namespace Tripex.Infrastructure.Persistence
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Like<Post>> PostLikes { get; set; }
        public DbSet<Like<Comment>> CommentLikes { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Follower> Followers { get; set; }
        public DbSet<Watcher<Post>> PostWatchers { get; set; }
        public DbSet<Notification> Notifications { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            ConfigurePostEntity(modelBuilder);
            ConfigureCommentEntity(modelBuilder);
            ConfigureLikeEntity(modelBuilder);
            ConfigureFollowerEntity(modelBuilder);
            ConfigureWatchers(modelBuilder);
        }

        private void ConfigurePostEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasOne(p => p.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(p => p.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureLikeEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Like<Post>>()
                .HasOne(l => l.User)
                .WithMany(u => u.PostLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like<Post>>()
                .HasOne(l => l.Entity)
                .WithMany(p => p.Likes)
                .HasForeignKey(l => l.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like<Comment>>()
                .HasOne(l => l.User)
                .WithMany(u => u.CommentLikes)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Like<Comment>>()
                .HasOne(l => l.Entity)
                .WithMany(c => c.Likes)
                .HasForeignKey(l => l.EntityId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureCommentEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.User)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.Cascade); 

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade); 
        }

        private void ConfigureFollowerEntity(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Follower>()
                .HasOne(c => c.FollowerEntity)
                .WithMany(u => u.Followers)
                .HasForeignKey(c => c.FollowerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Follower>()
                .HasOne(c => c.FollowingEntity)
                .WithMany(p => p.Following)
                .HasForeignKey(c => c.FollowingPersonId)
                .OnDelete(DeleteBehavior.Cascade);
        }

        private void ConfigureWatchers(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Watcher<Post>>()
                .HasOne(l => l.User)
                .WithMany(u => u.PostWatchers)
                .HasForeignKey(l => l.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Watcher<Post>>()
                .HasOne(l => l.Entity)
                .WithMany(p => p.PostWatchers)
                .HasForeignKey(l => l.EntityId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Watcher<Post>>()
               .HasIndex(w => w.UserId)
               .HasDatabaseName("IX_Watcher_UserId");

            modelBuilder.Entity<Watcher<Post>>()
                .HasIndex(w => w.EntityId)
                .HasDatabaseName("IX_Watcher_EntityId");

            modelBuilder.Entity<Watcher<Post>>()
                .HasIndex(w => new { w.UserId, w.EntityId })
                .HasDatabaseName("IX_Watcher_UserId_EntityId");
        }
    }
}

