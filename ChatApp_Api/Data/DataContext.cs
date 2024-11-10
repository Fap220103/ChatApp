using ChatApp_Api.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace ChatApp_Api.Data
{
    public class DataContext : IdentityDbContext<AppUser, AppRole, int,
        IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>,
        IdentityRoleClaim<int>, IdentityUserToken<int>>
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<UserLike> Likes { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Connection> Connections { get; set; }
        public DbSet<GroupChat> GroupChats { get; set; }
        public DbSet<GroupMembersChat> GroupMembersChat { get; set; }
        public DbSet<MessagesChat> MessagesChat { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
              .HasMany(ur => ur.UserRoles)  
              .WithOne(u => u.User)
              .HasForeignKey(ur => ur.UserId)
              .IsRequired();

            builder.Entity<AppRole>()
                .HasMany(ur => ur.UserRoles)
                .WithOne(u => u.Role)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();


            builder.Entity<UserLike>()
                .HasKey(k => new { k.SourceUserId, k.LikedUserId });

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
                .HasOne(u => u.Recipient)
                .WithMany(m => m.MessageReceived)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Message>()
              .HasOne(u => u.Sender)
              .WithMany(m => m.MessageSent)
              .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<GroupMembersChat>()
              .HasKey(gm => new { gm.GroupId, gm.UserId });


            builder.Entity<GroupMembersChat>()
                .HasOne(gm => gm.GroupChat)
                .WithMany(g => g.GroupMembers)
                .HasForeignKey(gm => gm.GroupId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<GroupMembersChat>()
                .HasOne(gm => gm.User)
                .WithMany(u => u.GroupMembersChats)
                .HasForeignKey(gm => gm.UserId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<MessagesChat>()
               .HasOne(m => m.Sender)
               .WithMany(u => u.MessagesSentUser)  // User có thể gửi nhiều tin nhắn
               .HasForeignKey(m => m.SenderId)
               .OnDelete(DeleteBehavior.Restrict); // Tránh xóa cascade

            // Thiết lập quan hệ giữa MessagesChat và Group (nếu là tin nhắn nhóm)
            builder.Entity<MessagesChat>()
                .HasOne(m => m.Group)
                .WithMany(g => g.Messages)  // Nhóm có thể có nhiều tin nhắn
                .HasForeignKey(m => m.GroupId)
                .OnDelete(DeleteBehavior.Cascade); // Tin nhắn bị xóa khi nhóm bị xóa


        }

    }
}
