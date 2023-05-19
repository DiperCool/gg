using CleanArchitecture.Domain.Entities.Events;

namespace CleanArchitecture.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public Profile Profile { get; set; } = null!;
        public bool EmailConfirmed { get; set; }
        public string Password { get; set; }= String.Empty;
        public int Coins { get; set; }
        public List<TeamUser> TeamUsers { get; set; } = new();
        public List<Team> CreatedTeams { get; set; } = new();
        public List<UserLogging> UserLogging { get; set; } = new();
        public List<ExTeamUser> ExTeams { get; set; } = new();
        public ActiveUserLogging ActiveUserLogging { get; set; } = null!;
        public Ban Ban { get; set; } = null!;
        public ShadowBan ShadowBan { get; set; } = null!;
        public UserStatistic Statistic { get; set; } = null!;
        public Guid? DeletedById { get; set; }
        public Employee? DeletedBy { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime RegisteredAt { get; set; }
        public List<EventOnReview> OnReviews { get; set; } = new();
        public List<EventApproved> Approved { get; set; } = new();
        public List<Winner> EventWinners { get; set; } = new();
        public List<ShopItemUser> PurchasedShopItems { get; set; } = new();
        public List<UsersEventParticipants> Matches { get; set; } = new();
        public List<ParticipantsUser> ParticipantsUsers { get; set; } = new();

    }
}