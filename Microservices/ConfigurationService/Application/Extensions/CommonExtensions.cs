namespace Ecosystem.ConfigurationService.Application.Extensions;

public static class CommonExtensions
{
    public static bool ToBool(this object source)
    {
        if (source is null) return false;
        var s = source.ToString();
        if (string.IsNullOrWhiteSpace(s)) return false;

        if (bool.TryParse(s, out var b)) return b;
        if (int.TryParse(s, out var i)) return i != 0;
        return s.Trim().Equals("true", StringComparison.OrdinalIgnoreCase);
    }

    public static int ToInt32(this object source) => Convert.ToInt32(source);

    public static ParallelOptions MaxDegreeOfThreads()
    {
        var maxThreads = Convert.ToInt32(Math.Ceiling(Environment.ProcessorCount * 0.75));
        return new ParallelOptions { MaxDegreeOfParallelism = maxThreads };
    }
}
