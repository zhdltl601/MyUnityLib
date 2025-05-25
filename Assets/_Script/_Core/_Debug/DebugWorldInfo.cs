using UnityEngine;

internal struct DebugWorldInfo
{
    public Vector3 worldPosition;
    public string message;
    public float sizeRatio;

    public DebugWorldInfo(Vector3 worldPosition, string message, float sizeRatio = 1)
    {
        this.worldPosition = worldPosition;
        this.message = message;
        this.sizeRatio = sizeRatio;
    }

    public readonly Vector3 GetScreenPos(Camera camera)
    {
        Vector3 result = camera.WorldToScreenPoint(worldPosition, Camera.MonoOrStereoscopicEye.Mono);
        return result;
    }
}
