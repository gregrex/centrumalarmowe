using Alarm112.Application.Interfaces;
using Alarm112.Application.Services;
using Alarm112.Contracts;
using Alarm112.Infrastructure.Persistence;
using Microsoft.Extensions.Logging.Abstractions;

namespace Alarm112.Api.Tests;

/// <summary>
/// Unit tests for BotDirector service.
/// Verifies bot tick logic: skip when no bots, skip when no profiles, handle missing sessions gracefully.
/// </summary>
public class BotDirectorTests
{
    private static string FindSolutionRoot()
    {
        var dir = new DirectoryInfo(AppContext.BaseDirectory);
        while (dir is not null)
        {
            if (File.Exists(Path.Combine(dir.FullName, "Alarm112.sln")))
                return dir.FullName;
            dir = dir.Parent!;
        }
        return AppContext.BaseDirectory;
    }

    private static InMemorySessionStore CreateStore() => new();

    private static SessionService CreateSessionService(InMemorySessionStore store) =>
        new SessionService(store, NullLogger<SessionService>.Instance);

    private static BotDirector CreateBotDirectorWithStub(InMemorySessionStore store, SessionService svc) =>
        new BotDirector(store, svc, new StubContentBundleLoader());

    private static BotDirector CreateBotDirectorReal(InMemorySessionStore store, SessionService svc)
    {
        var dataRoot = Path.Combine(FindSolutionRoot(), "data");
        return new BotDirector(store, svc, new JsonContentBundleLoader(dataRoot));
    }

    private static SessionSnapshotDto MakeSnapshot(
        string sessionId,
        bool hasBotRole = true,
        bool hasHumanRole = false,
        string incidentStatus = "pending",
        string unitStatus = "available")
    {
        var roles = new List<RoleSlotDto>
        {
            new("Dispatcher", HasHuman: hasHumanRole, HasBot: hasBotRole, OccupantId: null),
            new("CallOperator", HasHuman: true, HasBot: false, OccupantId: "player-1"),
        };
        var incidents = new List<IncidentDto>
        {
            new("inc-001", "Fire at Market", "Fire", "High", "Zone-A", incidentStatus)
        };
        var units = new List<DispatchUnitDto>
        {
            new("unit-001", "FireTruck", unitStatus, "Zone-A")
        };
        return new SessionSnapshotDto(
            SessionId: sessionId,
            SessionCode: "TEST",
            State: "Active",
            Roles: roles,
            Incidents: incidents,
            Units: units,
            Alerts: []);
    }

    [Fact]
    public async Task ExecuteBotTick_NonExistentSession_DoesNotCrash()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorWithStub(store, svc);

        await bot.ExecuteBotTickAsync("non-existent-session", CancellationToken.None);
    }

    [Fact]
    public async Task ExecuteBotTick_EmptyProfiles_DoesNotModifySession()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorWithStub(store, svc); // null config -> empty profiles

        store.Save(MakeSnapshot("sess-no-profiles", hasBotRole: true));

        await bot.ExecuteBotTickAsync("sess-no-profiles", CancellationToken.None);

        var after = store.TryGet("sess-no-profiles");
        Assert.NotNull(after);
        Assert.Equal("pending", after!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_SessionWithBotRoleAndRealProfiles_DispatchesPendingIncident()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorReal(store, svc);

        store.Save(MakeSnapshot("sess-with-bot", hasBotRole: true));

        await bot.ExecuteBotTickAsync("sess-with-bot", CancellationToken.None);

        var after = store.TryGet("sess-with-bot");
        Assert.NotNull(after);
        Assert.Equal("dispatched", after!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_NoHumanNoBotRole_LeavesIncidentPending()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorReal(store, svc);

        // No bot enabled on any role
        store.Save(MakeSnapshot("sess-no-bots", hasBotRole: false));

        await bot.ExecuteBotTickAsync("sess-no-bots", CancellationToken.None);

        var after = store.TryGet("sess-no-bots");
        Assert.Equal("pending", after!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_AllIncidentsResolved_DoesNotCrash()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorReal(store, svc);

        store.Save(MakeSnapshot("sess-resolved", hasBotRole: true, incidentStatus: "resolved"));

        await bot.ExecuteBotTickAsync("sess-resolved", CancellationToken.None);

        var after = store.TryGet("sess-resolved");
        Assert.Equal("resolved", after!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_NoAvailableUnits_LeavesIncidentPending()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorReal(store, svc);

        store.Save(MakeSnapshot("sess-no-units", hasBotRole: true, unitStatus: "dispatched"));

        await bot.ExecuteBotTickAsync("sess-no-units", CancellationToken.None);

        var after = store.TryGet("sess-no-units");
        Assert.Equal("pending", after!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_SecondCallAfterDispatch_IsIdempotent()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorReal(store, svc);

        store.Save(MakeSnapshot("sess-multi", hasBotRole: true));

        await bot.ExecuteBotTickAsync("sess-multi", CancellationToken.None);
        Assert.Equal("dispatched", store.TryGet("sess-multi")!.Incidents.First().Status);

        // Second tick: no pending incidents left, no crash
        await bot.ExecuteBotTickAsync("sess-multi", CancellationToken.None);
        Assert.Equal("dispatched", store.TryGet("sess-multi")!.Incidents.First().Status);
    }

    [Fact]
    public async Task ExecuteBotTick_CancelledToken_DoesNotThrow()
    {
        var store = CreateStore();
        var svc = CreateSessionService(store);
        var bot = CreateBotDirectorWithStub(store, svc);

        store.Save(MakeSnapshot("sess-cancel", hasBotRole: true));

        using var cts = new CancellationTokenSource();
        cts.Cancel();

        // Stub returns early — no async delay to cancel, should complete without exception
        var ex = await Record.ExceptionAsync(() =>
            bot.ExecuteBotTickAsync("sess-cancel", cts.Token));
        Assert.Null(ex);
    }
}

/// <summary>
/// Stub that returns default(T) for all loads.
/// BotDirector handles null profiles gracefully by falling back to empty array.
/// </summary>
internal sealed class StubContentBundleLoader : IContentBundleLoader
{
    public string DataRoot => AppContext.BaseDirectory;

    public Task<T> LoadContentAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => Task.FromResult(default(T)!);

    public Task<T> LoadConfigAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => Task.FromResult(default(T)!);

    public Task<T> LoadReferenceAsync<T>(string fileName, CancellationToken cancellationToken = default)
        => Task.FromResult(default(T)!);
}
