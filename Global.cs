global using LSPD_First_Response.Mod.API;
global using Rage;
global using System;
global using System.Windows.Forms;
global using RAGENativeUI;
global using System.Reflection;
global using System.Collections.Generic;
global using System.Drawing;
global using RAGENativeUI.Elements;
global using Rage.Native;
global using GearBag.Engine.Common.Misc;

internal static class Global
{
    internal static readonly Random Random = new(DateTime.Now.Millisecond);

    internal static Ped PlayerPed
    {
        get
        {
            while (!Game.LocalPlayer.Character) GameFiber.Yield();
            return Game.LocalPlayer.Character;
        }
    }

    internal static string RandomString(params string[] strings)
    {
        return strings[Random.Next(strings.Length)];
    }
}