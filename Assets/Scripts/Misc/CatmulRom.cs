using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CatmulRom
{
    public static float Float(float p0, float p1, float p2, float p3, float t)
    {
        return 0.5f * (2.0f * p1 + t * (-p0 + p2) + t * t * (2.0f * p0 - 5.0f * p1 + 4.0f * p2 - p3) + t * t * t * (-p0 + 3.0f * p1 - 3.0f * p2 + p3));
    }
    
    public static Vector3 Vector3(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3, float t)
    {
        float x = Float(p0.x, p1.x, p2.x, p3.x, t);
        float y = Float(p0.y, p1.y, p2.y, p3.y, t);
        float z = Float(p0.z, p1.z, p2.z, p3.z, t);
        return new Vector3(x, y, z);
    }
}
