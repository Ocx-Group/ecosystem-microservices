using Newtonsoft.Json;

namespace Ecosystem.AccountService.Application.Helpers;

public static class JsonExtensions
{
    public static string ToJsonString(this object source)
        => JsonConvert.SerializeObject(source);
}