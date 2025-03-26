using GearBag.Engine.UI;

namespace GearBag;

public class Main : Plugin
{
    internal static bool onDuty;

    public override void Finally()
    {
    }

    public override void Initialize()
    {
        Functions.OnOnDutyStateChanged += DutyStateHandler;
        AppDomain.CurrentDomain.DomainUnload += (_, _) => OnUnload();
    }

    private static void DutyStateHandler(bool onDuty)
    {
        Main.onDuty = onDuty;
        if (onDuty)
            GameFiber.StartNew(OnLoad);
        else
            OnUnload();
    }

    private static void OnLoad()
    {
        LogHandler.Trace($"Loading Version {Assembly.GetExecutingAssembly().GetName().Version}");
        Settings.Initialize();
        InterfaceHandler.Initialize();
        BagMenu.Initialize();
        TrunkMenu.Initialize();
        VehicleMenu.Initialize();
        LogHandler.Trace("Successfully loaded with all dependencies");
        GameFiber.Yield();
    }

    private static void OnUnload()
    {
        LogHandler.Trace("Unloading...");

        if (InterfaceHandler.s_gearBag) InterfaceHandler.s_gearBag.Delete();
        if (InterfaceHandler.s_trafficCone) InterfaceHandler.s_trafficCone.Delete();
        foreach (var cone in InterfaceHandler.s_trafficCones) if (cone)cone.Delete();
        NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed);
        Global.PlayerPed.Tasks.Clear();
        LogHandler.Trace("Successfully unloaded and cleaned up");
    }
}