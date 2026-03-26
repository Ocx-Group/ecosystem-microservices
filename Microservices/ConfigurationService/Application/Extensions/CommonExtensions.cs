namespace Ecosystem.ConfigurationService.Application.Extensions;

public static class CommonExtensions
{
    public static bool ToBool(this object source)
    {
        bool result;
        try { result = Convert.ToBoolean(source); }
        catch (Exception) { result = Convert.ToBoolean(Convert.ToInt32(source)); }
        return result;
    }

    public static int ToInt32(this object source) => Convert.ToInt32(source);

    public static ParallelOptions MaxDegreeOfThreads()
    {
        var maxThreads = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75));
        return new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
    }
}
