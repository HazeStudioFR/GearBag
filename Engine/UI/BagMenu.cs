using GearBag.Engine.Common.Functions;
using GearBag.Engine.Common.UI;

namespace GearBag.Engine.UI;

internal static class BagMenu
{
    internal static UIMenu s_bagMenu = BaseMenu.CreateMenu("GearBag Menu");

    private static UIMenuItem s_placeBagItem = BaseMenu.CreateItem("Place Gear Bag", "Place your ~b~Gear Bag~s~ on the ground. This is how you can access it.", s_bagMenu);

    private static UIMenuItem s_restockAmmo = BaseMenu.CreateItem("Restock Ammunition", "Restock ammunition for the weapons you have stored on your person.", s_bagMenu);
    private static UIMenuItem s_restockArmour = BaseMenu.CreateItem("Replace Armour", "Replace ~b~your~s~ bulletproof vest with a new, ~b~undamaged~s~ one.", s_bagMenu);
    private static UIMenuItem s_injectEpi = BaseMenu.CreateItem("Self-Heal", "Inject yourself with epinephrine to replenish your health.", s_bagMenu);


    internal static void Initialize()
    {
        s_bagMenu.OnMenuOpen += sender =>
        {
            if (NativeFunction.Natives.IS_ENTITY_ATTACHED_TO_ANY_PED<bool>(InterfaceHandler.s_gearBag))
            {
                s_bagMenu.ResetMenu();
            }
            else
            {
                s_bagMenu.ResetMenu(false);
                s_bagMenu.AddItems(s_placeBagItem, s_injectEpi, s_restockAmmo, s_restockArmour);
            }

            s_bagMenu.CurrentSelection = 0;
        };


        s_bagMenu.ResetMenu();
        s_placeBagItem.Activated += (sender, item) =>
        {
            if (!InterfaceHandler.s_gearBag)
            {
                LogHandler.Trace("Player tried placing gear bag. Gear bag is still stored in the vehicle.");
                return;
            }

            Global.PlayerPed.Tasks.PlayAnimation("missheist_agency2aig_13", "pickup_briefcase", 2500, 2f, 1f, 0f, AnimationFlags.None).WaitForCompletion();

            if (!NativeFunction.Natives.IS_ENTITY_ATTACHED_TO_ANY_PED<bool>(InterfaceHandler.s_gearBag))
            {
                InterfaceHandler.s_gearBag.AttachTo(Global.PlayerPed, Global.PlayerPed.GetBoneIndex(PedBoneId.RightHand), new Vector3(0.41f, -0.02f, -0.02f), new Rotator(-70.0f, -80 - 0f, 0.0f));
                NativeFunction.Natives.SET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed, "move_ped_wpn_jerrycan_generic");
                item.Text = "Place Gear Bag";
            }
            else
            {
                InterfaceHandler.s_gearBag.Detach();
                NativeFunction.Natives.PLACE_OBJECT_ON_GROUND_PROPERLY<bool>(InterfaceHandler.s_gearBag);
                NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed);
                Global.PlayerPed.Tasks.Clear();
                item.Text = "Pickup Gear Bag";
            }

            sender.Close(false);
        };

        s_injectEpi.Activated += (sender, item) =>
        {
            Game.DisplayHelp("You have injected epinephrine. You cannot use your gear bag until the epinephrine has ran out.");
            LogHandler.Trace($"Player has injected Epinephrine. Starting Health [{Global.PlayerPed.Health}] - Max Health [{Global.PlayerPed.MaxHealth}]");
            while (Global.PlayerPed.Health != Global.PlayerPed.MaxHealth)
            {
                GameFiber.Wait(999);
                if (Settings.ENABLE_CONTROLLER_VIBRATIONS) NativeFunction.Natives.x48B3886C1358D0D5(1, 200, 120);
                Global.PlayerPed.Health++;
                GameFiber.Yield();
            }

            sender.Close(false);
        };

        s_restockAmmo.Activated += (sender, item) =>
        {
            Global.PlayerPed.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 3780, 2f, 1f, 0f, AnimationFlags.None);
            GameFiber.Wait(1200);
            if (sKeys.IsControllerInUse() && Settings.ENABLE_CONTROLLER_VIBRATIONS)
            {
                NativeFunction.Natives.x48B3886C1358D0D5(1, 500, 100);
                GameFiber.Wait(1000);
                NativeFunction.Natives.x48B3886C1358D0D5(1, 500, 100);
            }

            foreach (var weapon in Global.PlayerPed.Inventory.Weapons)
                //gives players 3 mags
                weapon.Ammo = (short)(weapon.MagazineSize * 3);
        };

        s_restockArmour.Activated += (sender, item) =>
        {
            Global.PlayerPed.Tasks.PlayAnimation("anim@gangops@facility@servers@bodysearch@", "player_search", 3780, 2f, 1f, 0f, AnimationFlags.None);
            GameFiber.Wait(1200);
            if (sKeys.IsControllerInUse() && Settings.ENABLE_CONTROLLER_VIBRATIONS)
            {
                NativeFunction.Natives.x48B3886C1358D0D5(1, 500, 200);
                GameFiber.Wait(1000);
                NativeFunction.Natives.x48B3886C1358D0D5(1, 500, 250);
            }

            NativeFunction.Natives.SET_PED_ARMOUR(Global.PlayerPed, 100);
        };
    }

    private static void ResetMenu(this UIMenu menu, bool addPlaceItem = true)
    {
        menu.MenuItems.Clear();
        menu.RefreshIndex();
        if (addPlaceItem) menu.AddItem(s_placeBagItem);
    }
}