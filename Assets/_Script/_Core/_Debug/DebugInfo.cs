using System;
using UnityEngine;

public struct DebugInfo
{
    public Vector3 worldPosition;
    internal bool is3D;
    public string message;
    public float sizeRatio;

    public DebugInfo(Vector3 worldPosition, string message, float sizeRatio = 1)
    {
        this.worldPosition = worldPosition;
        this.message = message;
        this.sizeRatio = sizeRatio;
        is3D = false;
    }
    public readonly Vector3 GetScreenPos(Camera camera)
    {
        Vector3 result = camera.WorldToScreenPoint(worldPosition, Camera.MonoOrStereoscopicEye.Mono);
        return result;
    }
}
