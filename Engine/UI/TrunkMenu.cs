using GearBag.Engine.Common.Extension;
using GearBag.Engine.Common.UI;
using Object = Rage.Object;

namespace GearBag.Engine.UI;

internal static class VehicleMenu
{
    internal static UIMenu s_vehicleMenu = BaseMenu.CreateMenu("Vehicle Menu");

    internal static UIMenuItem s_vehicleTruckItem = BaseMenu.CreateItem("Open Trunk", "Open your vehicle's truck to access your gearbag.", s_vehicleMenu);

    internal static UIMenuCheckboxItem s_realisticWeaponSystem = BaseMenu.CreateCheckBox("Enable Weapons System", "~b~Enable~s~ or ~b~Disable~s~ GearBag's weapon system.", s_vehicleMenu);
    internal static UIMenuItem s_vehicleGearBag = BaseMenu.CreateItem("Grab Gear Bag", "Grab your ~b~gear bag~s~ from your car", s_vehicleMenu);
    internal static UIMenuItem s_vehicleTrafficCones = BaseMenu.CreateItem("Grab Traffic Cones", "Grab your ~b~traffic cones~s~ from your car", s_vehicleMenu);
    internal static UIMenuItem s_vehicleRemoveTrafficCones = BaseMenu.CreateItem("Pickup All Traffic Cones", "Pickup and remove all ~b~placed~s~ traffic cones.", s_vehicleMenu);

    internal static void Initialize()
    {
        s_vehicleMenu.ResetMenu();
        s_vehicleTruckItem.Activated += (sender, item) =>
        {
            if (!Global.PlayerPed.LastVehicle)
            {
                LogHandler.Warn("Last vehicle could not be found. Enter then leave the vehicle and try again.");
                return;
            }

            if (Global.PlayerPed.DistanceTo(Global.PlayerPed.LastVehicle.RearPosition) >= 2f)
            {
                LogHandler.Warn("Walk to the rear of your vehicle to open the trunk.");
                GameFiber.WaitUntil(() => Global.PlayerPed.DistanceTo(Global.PlayerPed.LastVehicle.RearPosition) < 2f);
            }

            Global.PlayerPed.Tasks.PlayAnimation("rcmnigel3_trunk", "out_trunk_trevor", 2300, 2f, 1f, 0f, AnimationFlags.None);
            GameFiber.Wait(1420);
            if (Global.PlayerPed.LastVehicle.Doors[5].IsValid()) Global.PlayerPed.LastVehicle.OpenDoor(5);
            else LogHandler.Trace($"Current Vehicle [{Global.PlayerPed.LastVehicle.Model.Name}] does not have a door index of [5] / [Trunk]");
            
            s_vehicleMenu.ResetMenu(false);
            s_vehicleMenu.AddItems(s_realisticWeaponSystem, s_vehicleGearBag, s_vehicleTrafficCones);
            if (InterfaceHandler.s_trafficCones.Count > 0) s_vehicleMenu.AddItem(s_vehicleRemoveTrafficCones);
            s_vehicleMenu.RefreshIndex();
        };

        s_vehicleGearBag.Activated += (sender, item) =>
        {
            Global.PlayerPed.Tasks.PlayAnimation("anim@heists@narcotics@trash", "pickup", 2800, 2f, 1f, 0f, AnimationFlags.None).WaitForCompletion();
            
            if (!InterfaceHandler.s_gearBag.Exists())
            {
                NativeFunction.Natives.SET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed, "move_ped_wpn_jerrycan_generic");
                InterfaceHandler.s_gearBag = new Object("xm_prop_x17_bag_01d", Global.PlayerPed.Position);
                InterfaceHandler.s_gearBag.AttachTo(Global.PlayerPed, Global.PlayerPed.GetBoneIndex(PedBoneId.RightHand), new Vector3(0.41f, -0.02f, -0.02f), new Rotator(-70.0f, -80-0f, 0.0f));
                item.Text = "Return Gear Bag";
            }
            else
            {
                if (InterfaceHandler.s_gearBag.Exists()) InterfaceHandler.s_gearBag.Delete();
                NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed);
                Global.PlayerPed.Tasks.Clear();
                item.Text = "Grab Gear Bag";
            }

            s_vehicleMenu.Close(false);
        };

        s_vehicleTrafficCones.Activated += (sender, item) =>
        {
            Global.PlayerPed.Tasks.PlayAnimation("anim@heists@narcotics@trash", "pickup", 2800, 2f, 1f, 0f, AnimationFlags.None).WaitForCompletion();
            
            if (!InterfaceHandler.s_trafficCone.Exists())
            {
                NativeFunction.Natives.SET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed, "move_ped_wpn_jerrycan_generic");
                InterfaceHandler.s_trafficCone = new Object("prop_mp_cone_02", Global.PlayerPed.Position);
                InterfaceHandler.s_trafficCone.AttachTo(Global.PlayerPed, Global.PlayerPed.GetBoneIndex(PedBoneId.RightHand), new Vector3(0.74f, 0.0f, 0.0f), new Rotator(0.0f, -90f, 0.0f));
                item.Text = "Return Traffic Cones";
            }
            else
            {
                if (InterfaceHandler.s_trafficCone.Exists()) InterfaceHandler.s_trafficCone.Delete();
                NativeFunction.Natives.RESET_PED_WEAPON_MOVEMENT_CLIPSET(Global.PlayerPed);
                Global.PlayerPed.Tasks.Clear();
                item.Text = "Grab Traffic Cones";
            }

            s_vehicleMenu.Close(false);
        };

        s_vehicleRemoveTrafficCones.Activated += (sender, item) =>
        {
            foreach (var cone in InterfaceHandler.s_trafficCones)
            {
                if (cone.Exists()) cone.Delete();
            }
            InterfaceHandler.s_trafficCones.Clear();
        };
        
        s_realisticWeaponSystem.Checked = Settings.ENABLE_REALISTIC_WEAPONS_SYSTEM;


        s_vehicleMenu.OnMenuClose += _ =>
        {
            s_vehicleMenu.ResetMenu();
            Global.PlayerPed.LastVehicle.CloseDoors();
        };
    }

    private static void ResetMenu(this UIMenu menu, bool addTrunkItem = true)
    {
        menu.MenuItems.Clear();
        menu.RefreshIndex();
        if (addTrunkItem) menu.AddItem(s_vehicleTruckItem);
    }
}