using System.Reflection;
using CleanArchitecture.Application.Common.Interfaces;
using CleanArchitecture.Domain.Common;
using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext,  IApplicationDbContext
{

    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;
    public ApplicationDbContext(
        DbContextOptions<ApplicationDbContext> options,IDateTime dateTime,ICurrentUserService currentUserService) : base(options)
    {
        _dateTime=dateTime;
        _currentUserService=currentUserService;
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<ConfirmEmail> ConfirmEmails => Set<ConfirmEmail>();
    public DbSet<Employee> Employees  => Set<Employee>();
    public DbSet<Profile> Profiles => Set<Profile>();
    public DbSet<MediaFile> MediaFiles => Set<MediaFile>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<RestorePassword> RestorePasswords => Set<RestorePassword>();
    public DbSet<EmployeeRole> EmployeeRoles => Set<EmployeeRole>();
    public DbSet<EmployeeProfile> EmployeeProfiles => Set<EmployeeProfile>();
    public DbSet<OrganizerProfile> OrganizerProfiles => Set<OrganizerProfile>();
    public DbSet<NewsEditorProfile> NewsEditorProfiles => Set<NewsEditorProfile>();
    public DbSet<Team> Teams => Set<Team>();
    public DbSet<TeamUser> TeamUsers => Set<TeamUser>();
    public DbSet<Teammate> Teammates => Set<Teammate>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<PlayerStats> PlayerStats => Set<PlayerStats>();
    public DbSet<UserLogging> UserLogging => Set<UserLogging>();
    public DbSet<ExTeamUser> ExTeamUsers => Set<ExTeamUser>();
    public DbSet<Ban> Bans => Set<Ban>();
    public DbSet<ShadowBan> ShadowBans => Set<ShadowBan>();
    public DbSet<News> News => Set<News>();
    public DbSet<EmployeeLog> EmployeeLogs => Set<EmployeeLog>();
    public DbSet<EventOnReview> EventOnReviews => Set<EventOnReview>();
    public DbSet<EventApproved> EventApproved => Set<EventApproved>();
    public DbSet<EventModerator> EventModerators => Set<EventModerator>();
    public DbSet<GroupModerator> GroupModerators=> Set<GroupModerator>();
    public DbSet<Participant> Participants => Set<Participant>();
    public DbSet<EventParticipant> EventParticipants => Set<EventParticipant>();
    public DbSet<Prize> Prizes => Set<Prize>();
    public DbSet<PlacementPrize> PlacementPrizes => Set<PlacementPrize>();
    public DbSet<UsersEventParticipants> UsersEventParticipants => Set<UsersEventParticipants>();
    public DbSet<ParticipantsUser> UsersParticipants => Set<ParticipantsUser>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Stage> Stages => Set<Stage>();
    public DbSet<UserStatistic> UserStatistics => Set<UserStatistic>();
    public DbSet<ShopItem> ShopItems => Set<ShopItem>();
    public DbSet<ShopItemUser> ShopItemUsers => Set<ShopItemUser>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        builder.Entity<Employee>()
            .HasOne(e => e.CreatedBy)
            .WithMany(c => c.CreatedEmployees);
        builder.Entity<Employee>()
            .HasOne(e => e.DeletedBy)
            .WithMany(c => c.DeletedEmployees);
        builder.Entity<User>()
            .HasOne(x => x.DeletedBy)
            .WithMany(x => x.DeletedUsers);
        builder.Entity<Team>()
            .HasOne(x => x.Creator)
            .WithMany(x => x.CreatedTeams);


        builder.Entity<News>()
            .HasOne(x => x.Background)
            .WithOne(x => x.BackgroundNews)
            .HasForeignKey<MediaFile>(x => x.BackgroundNewsId);
        builder.Entity<News>()
            .HasMany(x => x.Pictures)
            .WithOne(x => x.News);
        
        builder.Entity<UsersEventParticipants>()
            .HasOne<User>(sc => sc.User)
            .WithMany(s => s.Matches);

        builder.Entity<UsersEventParticipants>()
            .HasOne<EventParticipant>(sc => sc.Participant)
            .WithMany(s => s.Users);

        base.OnModelCreating(builder);
    }
     public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        
        
        IEnumerable<PlayerStats> removeStatsEnumerable = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is PlayerStats)
            .Select(e => e.Entity).Cast<PlayerStats>();
        foreach (PlayerStats playerStats in removeStatsEnumerable)
        {
            UserStatistic? userStatistic = await base.Set<UserStatistic>().FirstOrDefaultAsync(x=>x.UserId==playerStats.UserId, cancellationToken: cancellationToken);
            if(userStatistic==null) continue;
            userStatistic.Kills -= playerStats.Kills;
            Update(userStatistic);
        }
        
        
        
        IEnumerable<PlayerStats> addStatsEnumerable = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added && e.Entity is PlayerStats)
            .Select(e => e.Entity).Cast<PlayerStats>();
        foreach (PlayerStats playerStats in addStatsEnumerable)
        {
            UserStatistic? userStatistic = await base.Set<UserStatistic>().FirstOrDefaultAsync(x=>x.UserId==playerStats.UserId, cancellationToken: cancellationToken);
            if(userStatistic==null) continue;
            userStatistic.Kills += playerStats.Kills;
            Update(userStatistic);
        }
        
        
        
        foreach (var entry in ChangeTracker.Entries<AuditableEntity>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.Created =  _dateTime.Now.ToUniversalTime();
                    break;

                case EntityState.Modified:
                    entry.Entity.LastModified = _dateTime.Now.ToUniversalTime();
                    break;
            }
        }
        var result = await base.SaveChangesAsync(cancellationToken);


        return result;
    }

}
