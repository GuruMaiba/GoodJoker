using System;
using System.Data.Entity;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
//using GoodJoker.Migrations;

namespace GoodJoker.Models
{
    public static class IdentityExtensions
    {
        public static T GetUserId<T>(this IIdentity identity) where T : IConvertible
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            if (ci != null)
            {
                var id = ci.FindFirst(ClaimTypes.NameIdentifier);
                if (id != null)
                {
                    return (T)Convert.ChangeType(id.Value, typeof(T), CultureInfo.InvariantCulture);
                }
            }
            return default(T);
        }

        public static string GetUserRole(this IIdentity identity)
        {
            if (identity == null)
            {
                throw new ArgumentNullException("identity");
            }
            var ci = identity as ClaimsIdentity;
            string role = "";
            if (ci != null)
            {
                var id = ci.FindFirst(ClaimsIdentity.DefaultRoleClaimType);
                if (id != null)
                    role = id.Value;
            }
            return role;
        }
    }

    public class ApplicationDbContext : DbContext
    {
        // Пользователь
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Assess> Assess { get; set; }
        public DbSet<InfoSite> InfoSite { get; set; }

        // Новости
        public DbSet<News> News { get; set; }
        public DbSet<ContentNews> ContentsNews { get; set; }
        public DbSet<ImageNews> ImagesNews { get; set; }
        public DbSet<CommentNews> CommentsNews { get; set; }
        public DbSet<Tag> Tags { get; set; }

        // Лоттереи
        public DbSet<Lottery> Lotteries { get; set; }
        public DbSet<Prize> Prizes { get; set; }
        public DbSet<Finger> Fingers { get; set; }
        public DbSet<MandatoryInfo> MandatoryInfo { get; set; }
        public DbSet<ReturnMandatoryInfo> ReturnMandatoryInfo { get; set; }
        public DbSet<CommentLottery> CommentsLottery { get; set; }
        public DbSet<CommentChatLottery> ChatLottery { get; set; }
        public DbSet<LinkLottery> LinkLotteries { get; set; }
        public DbSet<RefreshWinner> RefreshWinners { get; set; }

        // Ежедневные лоттереи
        public DbSet<EverydayLott> Everyday { get; set; }
        public DbSet<WinnersEverydayLott> WinnersEveryday { get; set; }

        // Студии
        public DbSet<Studio> Studios { get; set; }
        public DbSet<StudioTeam> StudiosTeam { get; set; }
        public DbSet<StudioRoleMember> StudioRolesMember { get; set; }

        // Реклама
        public DbSet<Advert> Adverts { get; set; }
        public DbSet<PlaceAdvert> PlacesAdverts { get; set; }

        // Города
        public DbSet<City> Cities { get; set; }
        public DbSet<RegionCity> Regions { get; set; }

        // Диалоги
        public DbSet<Dialog> Dialogs { get; set; }
        public DbSet<MessageDialog> MessagesDialog { get; set; }

        // Видео
        public DbSet<Video> Videos { get; set; }
        public DbSet<TypeVideo> TypesVideos { get; set; }

        // Социальные сети
        public DbSet<SocialNetwork> SocialNetworks { get; set; }

        // Настройки
        public DbSet<OptionUser> OptionUsers { get; set; }
        public DbSet<OptionStudio> OptionStudios { get; set; }
        public DbSet<LinkStudio> LinkStudios { get; set; }

        // Архив
        public DbSet<ArchiveLottery> ArchiveLotteries { get; set; }
        public DbSet<ArchiveWinner> ArchiveWinners { get; set; }
        public DbSet<ArchiveWinnerEveryday> ArchiveWinnersEveryday { get; set; }

        // Уведомления
        public DbSet<Notice> Notices { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Участники ежедневки
            modelBuilder.Entity<User>()
                .HasMany(u => u.Everyday)
                .WithMany(l => l.Participants)
                .Map(m =>
                {
                    // Ссылка на промежуточную таблицу
                    m.ToTable("ParticipantsEvr");

                    // Настройка внешних ключей промежуточной таблицы
                    m.MapLeftKey("UserId");
                    m.MapRightKey("EverydayId");
                });

            // Лайки к ежедневке
            modelBuilder.Entity<User>()
                .HasMany(u => u.LikesEveryday)
                .WithMany(l => l.Likes)
                .Map(m =>
                {
                    // Ссылка на промежуточную таблицу
                    m.ToTable("LikesEvr");

                    // Настройка внешних ключей промежуточной таблицы
                    m.MapLeftKey("UserId");
                    m.MapRightKey("EverydayId");
                });

            // Участники лоттереи
            modelBuilder.Entity<User>()
                .HasMany(u => u.Lotteries)
                .WithMany(l => l.Users)
                .Map(m =>
                {
                    // Ссылка на промежуточную таблицу
                    m.ToTable("ParticipantsLott");

                    // Настройка внешних ключей промежуточной таблицы
                    m.MapLeftKey("UserId");
                    m.MapRightKey("LotteriesId");
                });

            // Бан чата (может дать организатор)
            modelBuilder.Entity<User>()
                .HasMany(u => u.BanCommentLotteries)
                .WithMany(l => l.BanComment)
                .Map(m =>
                {
                    // Ссылка на промежуточную таблицу
                    m.ToTable("BanCommentLotteries");

                    // Настройка внешних ключей промежуточной таблицы
                    m.MapLeftKey("UserId");
                    m.MapRightKey("LotteriesId");
                });

            // Города пользователя
            modelBuilder.Configurations.Add(new OptionUser.OptionUserMapping());

            // Диалоги, создатель и приглашённый
            modelBuilder.Configurations.Add(new Dialog.DialogMapping());
        }

        public ApplicationDbContext() : base("Local") // ServerDbConnection
        {
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<ApplicationDbContext, Configuration>());
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}