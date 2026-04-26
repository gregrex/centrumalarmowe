using Alarm112.Application.Interfaces;
using Alarm112.Application.Services;
using Alarm112.Infrastructure.Persistence;

namespace Alarm112.Api;

internal static class ServiceCollectionExtensions
{
    public static bool AddAlarm112Services(this IServiceCollection services, IConfiguration configuration, string contentRootPath)
    {
        var dataRoot = Path.GetFullPath(
            Path.Combine(contentRootPath, configuration["ContentBundles:DataRoot"] ?? "../../data"));
        services.AddSingleton<IContentBundleLoader>(_ => new JsonContentBundleLoader(dataRoot));

        var pgConnStr = configuration.GetConnectionString("Main");
        var usingPostgres = !string.IsNullOrWhiteSpace(pgConnStr);
        if (usingPostgres)
        {
            services.AddSingleton<ISessionStore>(sp =>
                new PostgresSessionStore(pgConnStr!, sp.GetRequiredService<ILogger<PostgresSessionStore>>()));
        }
        else
        {
            services.AddSingleton<ISessionStore, InMemorySessionStore>();
        }

        services.AddSignalR();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddOutputCache();

        services.AddSingleton<ISessionService, SessionService>();
        services.AddSingleton<IBotDirector, BotDirector>();
        services.AddSingleton<IReferenceDataService, ReferenceDataService>();
        services.AddSingleton<ILobbyService, LobbyService>();
        services.AddSingleton<IContentValidationService, ContentValidationService>();
        services.AddSingleton<IQuickPlayService, QuickPlayService>();
        services.AddSingleton<ICityMapService, CityMapService>();
        services.AddSingleton<IOperationsBoardService, OperationsBoardService>();
        services.AddSingleton<IRoundRuntimeService, RoundRuntimeService>();
        services.AddSingleton<IThemePackService, ThemePackService>();
        services.AddSingleton<IHomeFlowService, HomeFlowService>();
        services.AddSingleton<ICampaignEntryService, CampaignEntryService>();
        services.AddSingleton<IRuntimeBootstrapService, RuntimeBootstrapService>();
        services.AddSingleton<IMissionFlowService, MissionFlowService>();
        services.AddSingleton<IMissionRuntimeService, MissionRuntimeService>();
        services.AddSingleton<IPlayableRuntimeService, PlayableRuntimeService>();
        services.AddSingleton<IRuntimePolishService, RuntimePolishService>();
        services.AddSingleton<IQuasiProductionDemoService, QuasiProductionDemoService>();
        services.AddSingleton<IRuntimeUiFlowService, RuntimeUiFlowService>();
        services.AddSingleton<INearFinalSliceService, NearFinalSliceService>();
        services.AddSingleton<IShowcaseDemoService, ShowcaseDemoService>();
        services.AddSingleton<IReviewBuildService, ReviewBuildService>();
        services.AddSingleton<IReleaseCandidateService, ReleaseCandidateService>();
        services.AddSingleton<IAndroidPreviewService, AndroidPreviewService>();
        services.AddSingleton<IInternalTestService, InternalTestService>();
        services.AddSingleton<IFinalHandoffService, FinalHandoffService>();
        services.AddSingleton<IRealAndroidBuildService, RealAndroidBuildService>();
        services.AddHostedService<BotTickHostedService>();

        return usingPostgres;
    }
}
