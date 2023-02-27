using System.Collections;
using System.Collections.Generic;
//using UnityEditor.ShaderGraph.Drawing.Inspector.PropertyDrawers;
using UnityEngine;

[CreateAssetMenu(fileName = "Movement Physics Object", menuName = "ScriptableObjs/Physics/Movement", order = 0)]

public class MovementPhysicsObj : ScriptableObject
{
    [Header("Grounded")]

    public float walkSpeed;
    
    public float sprintSpeed;

    public float acceleration;
    
    public float deceleration;

    [Header("Jump")]

    public float jumpPower;

    public float airStrafeSpeed;

    public float airStrafeSpeedCap;

    public float jumpCooldown; 

    public float coyoteTime;

    [Header("Rotation")]

    public float rotationSpeed;

    [Range(0, 1)]
    public float tiltExtreme;

    [Header("Slow Motion")]

    [Range(0f, 1f)] public float minVignetteIntensity, maxVignetteIntensity;
    [Range(0f, 1f)] public float slowIntensity;
    public float slowSmoothSpeed;
}
