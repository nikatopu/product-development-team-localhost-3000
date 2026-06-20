using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public interface IBreakingChangeService
{
    List<BreakingChangeInfo> DetectChanges(AnalysisResult previous, AnalysisResult current);
}
