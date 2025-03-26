namespace GearBag.Engine.World;

internal static class Camera
{
    internal static void TraceForEntity(Action Code, out HitResult hitResult, Rage.Camera camera = null)
    {
        Vector3 camPos;
        Vector3 camRot;

        if (camera == null)
        {
            camPos = NativeFunction.Natives.GET_GAMEPLAY_CAM_COORD<Vector3>();
            camRot = NativeFunction.Natives.GET_GAMEPLAY_CAM_ROT<Vector3>(2);
        }
        else
        {
            camPos = camera.Position;
            camRot = camera.Direction;
        }


        // Convert Euler angles to a forward direction vector.
        var pitch = camRot.X * (float)Math.PI / 180f;
        var yaw = camRot.Z * (float)Math.PI / 180f;
        var camForward = new Vector3(-(float)Math.Sin(yaw) * (float)Math.Cos(pitch), (float)Math.Cos(yaw) * (float)Math.Cos(pitch), (float)Math.Sin(pitch));

        var rayDistance = 5f;
        var rayEnd = camPos + camForward * rayDistance;

        var traceResult = Rage.World.TraceLine(camPos, rayEnd, TraceFlags.IntersectEverything, Global.PlayerPed);
        hitResult = traceResult;
        Code();
    }
}