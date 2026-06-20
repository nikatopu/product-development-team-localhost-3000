using ApiDocGen.Hubs;
using ApiDocGen.Models.Responses;
using Microsoft.AspNetCore.SignalR;

namespace ApiDocGen.Services;

public class AnalysisNotifier : IAnalysisNotifier
{
    private readonly IHubContext<AnalysisHub> _hub;

    public AnalysisNotifier(IHubContext<AnalysisHub> hub) => _hub = hub;

    public Task NotifyStarted(string repoUrl) =>
        _hub.Clients.Group(AnalysisHub.GroupName(repoUrl))
            .SendAsync("AnalysisStarted", new { repoUrl, stage = "Cloning repository", percent = 0 });

    public Task NotifyCloningComplete(string repoUrl) =>
        _hub.Clients.Group(AnalysisHub.GroupName(repoUrl))
            .SendAsync("AnalysisProgress", new { repoUrl, stage = "Parsing source files", percent = 40 });

    public Task NotifyAnalysisProgress(string repoUrl, string stage, int percent) =>
        _hub.Clients.Group(AnalysisHub.GroupName(repoUrl))
            .SendAsync("AnalysisProgress", new { repoUrl, stage, percent });

    public Task NotifyComplete(string repoUrl, AnalysisResult result, List<BreakingChangeInfo> breakingChanges) =>
        _hub.Clients.Group(AnalysisHub.GroupName(repoUrl))
            .SendAsync("AnalysisComplete", new
            {
                repoUrl,
                totalRoutes = result.Metadata.TotalRoutes,
                totalControllers = result.Metadata.TotalControllers,
                breakingChangeCount = breakingChanges.Count(c => c.Severity == "Breaking"),
                breakingChanges
            });

    public Task NotifyFailed(string repoUrl, string error) =>
        _hub.Clients.Group(AnalysisHub.GroupName(repoUrl))
            .SendAsync("AnalysisFailed", new { repoUrl, error });
}
