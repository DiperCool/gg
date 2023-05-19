using CleanArchitecture.Domain.Entities;
using CleanArchitecture.Domain.Entities.EmployeeProfiles;
using CleanArchitecture.Domain.Entities.Events;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<User> Users { get; }
    DbSet<ConfirmEmail> ConfirmEmails { get; }
    DbSet<Employee> Employees { get; }
    DbSet<Profile> Profiles { get; }
    DbSet<MediaFile> MediaFiles { get; }
    DbSet<RefreshToken> RefreshTokens { get; }
    DbSet<RestorePassword> RestorePasswords { get; }
    DbSet<EmployeeRole> EmployeeRoles { get; }
    DbSet<EmployeeProfile> EmployeeProfiles  { get; }
    DbSet<OrganizerProfile> OrganizerProfiles  { get; }
    DbSet<NewsEditorProfile> NewsEditorProfiles  { get; }
    DbSet<Team> Teams { get; }
    DbSet<TeamUser> TeamUsers { get; }
    DbSet<Teammate> Teammates { get; }

    DbSet<Event> Events { get; }
    DbSet<PlayerStats> PlayerStats { get; }
    DbSet<UserLogging> UserLogging { get; }
    DbSet<ExTeamUser> ExTeamUsers { get; }
    DbSet<Ban> Bans { get; }
    DbSet<ShadowBan> ShadowBans { get; }
    DbSet<Domain.Entities.News> News { get; }
    DbSet<EmployeeLog> EmployeeLogs { get; }
    DbSet<EventOnReview> EventOnReviews { get; }
    DbSet<EventApproved> EventApproved { get; }
    DbSet<EventModerator> EventModerators { get; }
    DbSet<GroupModerator> GroupModerators { get; }
    DbSet<Participant> Participants { get;  }
    DbSet<EventParticipant> EventParticipants { get;  }

    DbSet<Prize> Prizes { get; }
    DbSet<PlacementPrize> PlacementPrizes { get; }
    DbSet<UsersEventParticipants> UsersEventParticipants { get;  }
    DbSet<ParticipantsUser> UsersParticipants { get;  }

    DbSet<Group> Groups { get; }
    DbSet<Stage> Stages { get; }
    DbSet<UserStatistic> UserStatistics { get; }
    DbSet<ShopItem> ShopItems { get; }
    DbSet<ShopItemUser> ShopItemUsers { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
