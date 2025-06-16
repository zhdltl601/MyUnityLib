using System;
using UnityEngine;

public struct DebugInfo
{
    public Vector3 worldPosition;
    internal bool is3D;
    public string message;
    public float sizeRatio;

    public DebugInfo(string message, float sizeRatio = 1, Vector3 worldPosition = default)
        : this(message, sizeRatio, false, worldPosition)
    {
    }
    internal DebugInfo(string message, float sizeRatio = 1, bool is3D = false, Vector3 worldPosition = default)
    {
        this.worldPosition = worldPosition;
        this.message = message;
        this.sizeRatio = sizeRatio;
        this.is3D = is3D;
    }
    public readonly Vector3 GetScreenPos(Camera camera)
    {
        Vector3 result = camera.WorldToScreenPoint(worldPosition, Camera.MonoOrStereoscopicEye.Mono);
        return result;
    }
}
