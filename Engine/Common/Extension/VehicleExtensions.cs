namespace GearBag.Engine.Common.Extension;

internal static class VehicleExtensions
{
    internal static void OpenDoor(this Vehicle vehicle, int door)
    {
        if (vehicle.Exists())
        {
            GameFiber.Wait(100);
            vehicle.Doors[door].Open(false, true);
            GameFiber.Wait(100);
        }
    }

    public static void CloseDoors(this Vehicle vehicle)
    {
        if (vehicle.Exists())
            NativeFunction.Natives.SET_VEHICLE_DOORS_SHUT(vehicle, true);
        else
            return;
    }
}