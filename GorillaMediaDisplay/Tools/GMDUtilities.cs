using System;

namespace GorillaMediaDisplay.Tools;

public class GMDUtilities
{
    public static string FormatTime(double seconds)
    {
        TimeSpan time = TimeSpan.FromSeconds(seconds);
        return time.ToString(@"mm\:ss");
    }
}