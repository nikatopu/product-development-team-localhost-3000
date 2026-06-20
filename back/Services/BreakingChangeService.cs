using ApiDocGen.Models.Responses;

namespace ApiDocGen.Services;

public class BreakingChangeService : IBreakingChangeService
{
    public List<BreakingChangeInfo> DetectChanges(AnalysisResult previous, AnalysisResult current)
    {
        var changes = new List<BreakingChangeInfo>();

        var prevRoutes = previous.Routes.ToDictionary(r => RouteKey(r), r => r);
        var currRoutes = current.Routes.ToDictionary(r => RouteKey(r), r => r);

        // Removed endpoints
        foreach (var (key, prevRoute) in prevRoutes)
        {
            if (!currRoutes.ContainsKey(key))
            {
                changes.Add(new BreakingChangeInfo
                {
                    ChangeType = "EndpointRemoved",
                    Severity = "Breaking",
                    AffectedEndpoint = $"{prevRoute.HttpMethod} {prevRoute.Path}",
                    OldValue = $"{prevRoute.HttpMethod} {prevRoute.Path}"
                });
                continue;
            }

            var currRoute = currRoutes[key];

            // Removed required parameters
            foreach (var prevParam in prevRoute.Parameters.Where(p => p.IsRequired))
            {
                var currParam = currRoute.Parameters.FirstOrDefault(p =>
                    string.Equals(p.Name, prevParam.Name, StringComparison.OrdinalIgnoreCase));

                if (currParam == null)
                {
                    changes.Add(new BreakingChangeInfo
                    {
                        ChangeType = "ParameterRemoved",
                        Severity = "Breaking",
                        AffectedEndpoint = $"{prevRoute.HttpMethod} {prevRoute.Path}",
                        AffectedField = prevParam.Name,
                        OldValue = $"{prevParam.Name}: {prevParam.Type}"
                    });
                }
                else if (!string.Equals(currParam.Type, prevParam.Type, StringComparison.OrdinalIgnoreCase))
                {
                    changes.Add(new BreakingChangeInfo
                    {
                        ChangeType = "ParameterTypeChanged",
                        Severity = "Breaking",
                        AffectedEndpoint = $"{prevRoute.HttpMethod} {prevRoute.Path}",
                        AffectedField = prevParam.Name,
                        OldValue = prevParam.Type,
                        NewValue = currParam.Type
                    });
                }
            }

            // Removed request body fields
            if (prevRoute.RequestBody != null)
                DetectPropertyChanges(
                    changes,
                    $"{prevRoute.HttpMethod} {prevRoute.Path}",
                    "RequestBodyField",
                    prevRoute.RequestBody.Properties,
                    currRoute.RequestBody?.Properties ?? []);

            // Removed response fields (for 2xx responses)
            var prevOk = prevRoute.Responses.Where(r => r.StatusCode is >= 200 and < 300).ToList();
            var currOk = currRoute.Responses.Where(r => r.StatusCode is >= 200 and < 300).ToList();

            foreach (var prevResp in prevOk)
            {
                var currResp = currOk.FirstOrDefault(r => r.StatusCode == prevResp.StatusCode);
                DetectPropertyChanges(
                    changes,
                    $"{prevRoute.HttpMethod} {prevRoute.Path}",
                    $"ResponseField[{prevResp.StatusCode}]",
                    prevResp.Properties,
                    currResp?.Properties ?? []);

                if (currResp == null && prevResp.Properties.Count > 0)
                {
                    changes.Add(new BreakingChangeInfo
                    {
                        ChangeType = "ResponseTypeChanged",
                        Severity = "Breaking",
                        AffectedEndpoint = $"{prevRoute.HttpMethod} {prevRoute.Path}",
                        OldValue = $"{prevResp.StatusCode}: {prevResp.TypeName}",
                        NewValue = "removed"
                    });
                }
            }
        }

        // Added endpoints (non-breaking, informational)
        foreach (var (key, currRoute) in currRoutes)
        {
            if (!prevRoutes.ContainsKey(key))
            {
                changes.Add(new BreakingChangeInfo
                {
                    ChangeType = "EndpointAdded",
                    Severity = "NonBreaking",
                    AffectedEndpoint = $"{currRoute.HttpMethod} {currRoute.Path}",
                    NewValue = $"{currRoute.HttpMethod} {currRoute.Path}"
                });
            }
        }

        return changes;
    }

    private static void DetectPropertyChanges(
        List<BreakingChangeInfo> changes,
        string endpoint,
        string context,
        List<PropertyInfo> prev,
        List<PropertyInfo> curr)
    {
        foreach (var prevProp in prev)
        {
            var currProp = curr.FirstOrDefault(p =>
                string.Equals(p.Name, prevProp.Name, StringComparison.OrdinalIgnoreCase));

            if (currProp == null)
            {
                changes.Add(new BreakingChangeInfo
                {
                    ChangeType = $"{context}Removed",
                    Severity = "Breaking",
                    AffectedEndpoint = endpoint,
                    AffectedField = prevProp.Name,
                    OldValue = $"{prevProp.Name}: {prevProp.Type}"
                });
            }
            else if (!string.Equals(currProp.Type, prevProp.Type, StringComparison.OrdinalIgnoreCase))
            {
                changes.Add(new BreakingChangeInfo
                {
                    ChangeType = $"{context}TypeChanged",
                    Severity = "Breaking",
                    AffectedEndpoint = endpoint,
                    AffectedField = prevProp.Name,
                    OldValue = prevProp.Type,
                    NewValue = currProp.Type
                });
            }
        }
    }

    private static string RouteKey(RouteInfo r) =>
        $"{r.HttpMethod.ToUpperInvariant()}:{r.Path.ToLowerInvariant()}";
}
