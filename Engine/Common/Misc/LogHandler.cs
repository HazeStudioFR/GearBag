namespace GearBag.Engine.Common.Misc;

internal static class LogHandler
{
    private const string PrefixTrace = "[GearBag] [TRACE] ";
    private const string PrefixWarn = "[GearBag] [WARN] ";
    private const string PrefixFatal = "[GearBag] [FATAL] ";

    internal static void Trace(string message)
    {
        Game.LogTrivial($"{PrefixTrace}{message}");
    }

    internal static void Warn(string message, bool displayNotification = true)
    {
        if (displayNotification)
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "GearBag", "Warning Alert", message);
        Game.LogTrivial($"{PrefixWarn}{message.Replace("~n~", ",").Replace("~s~", "").Replace("~y~", "")}");
    }

    internal static void Fatal(string message, bool displayNotification = true)
    {
        if (displayNotification)
            Game.DisplayNotification("commonmenu", "mp_alerttriangle", "GearBag", "Fatal Alert", message);
        Game.LogTrivial($"{PrefixFatal}{message.Replace("~n~", ",").Replace("~s~", "").Replace("~r~", "")}");
    }
}