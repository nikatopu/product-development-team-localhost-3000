using Microsoft.AspNetCore.SignalR;

namespace ApiDocGen.Hubs;

public class AnalysisHub : Hub
{
    public async Task JoinAnalysis(string repoUrl) =>
        await Groups.AddToGroupAsync(Context.ConnectionId, GroupName(repoUrl));

    public async Task LeaveAnalysis(string repoUrl) =>
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GroupName(repoUrl));

    public static string GroupName(string repoUrl) =>
        $"analysis:{repoUrl.Trim().ToLowerInvariant()}";
}
