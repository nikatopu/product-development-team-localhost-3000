using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IAnalysisNotifier
{
    Task NotifyStarted(string repoUrl);
    Task NotifyCloningComplete(string repoUrl);
    Task NotifyAnalysisProgress(string repoUrl, string stage, int percent);
    Task NotifyComplete(string repoUrl, AnalysisResult result, List<BreakingChangeInfo> breakingChanges);
    Task NotifyFailed(string repoUrl, string error);
}
