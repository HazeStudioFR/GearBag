namespace GearBag.Engine.Common.Functions;

internal static class sKeys
{
    internal static string IntructKey(this Keys key)
    {
        return $"~{key.GetInstructionalId()}~";
    }

    internal static string IntructKey(this ControllerButtons key)
    {
        return $"~{key.GetInstructionalId()}~";
    }

    internal static string IntructKey(this MouseButtons key)
    {
        return $"~{InstructionalKey.MouseLeft.GetId()}~";
    }

    public static bool IsControllerInUse()
    {
        if (!Game.IsControllerConnected) return false;
        return !NativeFunction.Natives.IS_USING_KEYBOARD_AND_MOUSE<bool>();
    }


    public static bool IsKeyPressed(Keys modifierKey, Keys mainKey, ControllerButtons modifierButton, ControllerButtons mainButton)
    {
        var isModifierPressed = false;
        var isMainKeyPressed = false;

        if (IsControllerInUse())
        {
            isModifierPressed = modifierButton == ControllerButtons.None || Game.IsControllerButtonDownRightNow(modifierButton);
            isMainKeyPressed = Game.IsControllerButtonDown(mainButton) || Game.IsControllerButtonDownRightNow(mainButton);
        }
        else
        {
            isModifierPressed = modifierKey == Keys.None || Game.IsKeyDownRightNow(modifierKey);
            isMainKeyPressed = Game.IsKeyDown(mainKey) || Game.IsKeyDownRightNow(mainKey);
        }

        return isModifierPressed && isMainKeyPressed;
    }

    public static string KeybindDisplay(Keys modifierKey, Keys mainKey)
    {
        var keys = string.Empty;
        if (modifierKey != Keys.None) keys = $"{modifierKey.IntructKey()} + ";
        keys += $"{mainKey.IntructKey()}";
        return keys;
    }

    public static string KeybindDisplay(ControllerButtons modifierKey, ControllerButtons mainKey)
    {
        var keys = string.Empty;
        if (modifierKey != ControllerButtons.None) keys = $"{modifierKey.IntructKey()} + ";
        keys += $"{mainKey.IntructKey()}";
        return keys;
    }
}