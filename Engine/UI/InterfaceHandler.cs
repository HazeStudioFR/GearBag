using GearBag.Engine.Common.Functions;
using GearBag.Engine.Player;
using Object = Rage.Object;

namespace GearBag.Engine.UI;

internal static class InterfaceHandler
{
    internal static MenuPool s_menuPool = new();
    internal static Object s_gearBag;
    internal static Object s_trafficCone;
    internal static List<Object> s_trafficCones = [];
    private static bool s_messageDisplayed;
    private static bool s_canSpawn = true;


    internal static void Initialize()
    {
        GameFiber.StartNew(delegate
        {
            while (Main.onDuty)
            {
                GameFiber.Yield();
                s_menuPool.ProcessMenus();

                if (sKeys.IsKeyPressed(Settings.INTERACTION_MENU_MODIFIER, Settings.INTERACTION_MENU_KEY, Settings.INTERACTION_MENU_BUTTON_MODIFIER, Settings.INTERACTION_MENU_BUTTON))
                {
                    if (s_gearBag && Global.PlayerPed.DistanceTo(Global.PlayerPed.LastVehicle) > 4f)
                        BagMenu.s_bagMenu.Visible = true;
                    else
                        TrunkMenu.s_trunkMenu.Visible = true;
                }

                if (s_trafficCone)
                {
                    if (!s_messageDisplayed)
                    {
                        s_messageDisplayed = true;
                        Game.DisplayHelp(!sKeys.IsControllerInUse() ? $"Press {MouseButtons.Left.IntructKey()} To ~b~Place~s~ Traffic Cone." : $"Press {ControllerButtons.DPadDown.IntructKey()} To ~b~Place~s~ Traffic Cone.", true);
                    }

                    DisableAttacking();

                    var isInputPressed = Game.GetMouseState().IsLeftButtonDown || Game.IsControllerButtonDown(ControllerButtons.DPadDown);

                    if (isInputPressed && !UIMenu.IsAnyMenuVisible && s_canSpawn)
                    {
                        s_canSpawn = false;
                        Global.PlayerPed.Tasks.PlayAnimation("anim@mp_fireworks", "place_firework_4_cone", 2200, 2f, 1f, 0f, AnimationFlags.None);
                        GameFiber.Wait(1700);
                        var cone = new Object("prop_mp_cone_02", Global.PlayerPed.GetOffsetPositionFront(1.2f));
                        NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY<bool>(cone);
                        cone.IsPersistent = true;
                        cone.IsPositionFrozen = true;
                        NativeFunction.Natives.SET_OBJECT_FORCE_VEHICLES_TO_AVOID(cone, true);
                        s_trafficCones.Add(cone);
                    }
                    else if (!isInputPressed)
                    {
                        s_canSpawn = true;
                    }
                }
                else if (s_messageDisplayed)
                {
                    NativeFunction.Natives.CLEAR_HELP(true);
                    s_messageDisplayed = false;
                }
            }

            if (Settings.DISABLE_HEALTH_REGEN) NativeFunction.Natives.SET_PLAYER_HEALTH_RECHARGE_MULTIPLIER(Global.PlayerPed, 0f);
            // if (Settings.ENABLE_REALISTIC_WEAPONS_SYSTEM) Inventory.RemoveStoredWeapons();
        }, "UI Handler");
    }

    private static void DisableAttacking()
    {
        Game.DisableControlAction(0, GameControl.Attack, true);
        Game.DisableControlAction(0, GameControl.Attack2, true);
        Game.DisableControlAction(0, GameControl.MeleeAttack1, true);
        Game.DisableControlAction(0, GameControl.MeleeAttack2, true);
        Game.DisableControlAction(0, GameControl.MeleeAttackAlternate, true);
        Game.DisableControlAction(0, GameControl.MeleeAttackHeavy, true);
        Game.DisableControlAction(0, GameControl.MeleeAttackLight, true);
    }
}