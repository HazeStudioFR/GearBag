namespace GearBag;

internal static class Settings
{
    private static InitializationFile _ini;

    internal static bool DISABLE_HEALTH_REGEN = true;
    internal static bool ENABLE_CONTROLLER_VIBRATIONS = true;

    internal static Keys INTERACTION_MENU_KEY = Keys.F7;
    internal static Keys INTERACTION_MENU_MODIFIER = Keys.None;

    internal static ControllerButtons INTERACTION_MENU_BUTTON = ControllerButtons.LeftThumb;
    internal static ControllerButtons INTERACTION_MENU_BUTTON_MODIFIER = ControllerButtons.LeftShoulder;

    internal static void Initialize()
    {
        _ini = new InitializationFile("Plugins/LSPDFR/GearBag.ini");
        _ini.Create();
        LogHandler.Trace("Initializing user settings from config.ini");

        DISABLE_HEALTH_REGEN = _ini.ReadBoolean("Gameplay", "DISABLE_HEALTH_REGEN", DISABLE_HEALTH_REGEN);
        LogHandler.Trace($"DISABLE_HEALTH_REGEN: {DISABLE_HEALTH_REGEN}");

        ENABLE_CONTROLLER_VIBRATIONS = _ini.ReadBoolean("Gameplay", "ENABLE_CONTROLLER_VIBRATIONS", ENABLE_CONTROLLER_VIBRATIONS);
        LogHandler.Trace($"ENABLE_CONTROLLER_VIBRATIONS: {ENABLE_CONTROLLER_VIBRATIONS}");

        INTERACTION_MENU_KEY = _ini.ReadEnum("Keyboard", "INTERACTION_MENU_KEY", INTERACTION_MENU_KEY);
        LogHandler.Trace($"INTERACTION_MENU_KEY: {INTERACTION_MENU_KEY}");
        INTERACTION_MENU_MODIFIER = _ini.ReadEnum("Keyboard", "INTERACTION_MENU_MODIFIER", INTERACTION_MENU_MODIFIER);

        INTERACTION_MENU_BUTTON = _ini.ReadEnum("Keyboard", "INTERACTION_MENU_BUTTON", INTERACTION_MENU_BUTTON);
        LogHandler.Trace($"INTERACTION_MENU_BUTTON: {INTERACTION_MENU_BUTTON}");

        INTERACTION_MENU_BUTTON_MODIFIER = _ini.ReadEnum("Keyboard", "INTERACTION_MENU_BUTTON_MODIFIER", INTERACTION_MENU_BUTTON_MODIFIER);
        LogHandler.Trace($"INTERACTION_MENU_BUTTON_MODIFIER: {INTERACTION_MENU_BUTTON_MODIFIER}");
    }
}