using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Grappling Physics Object", menuName = "ScriptableObjs/Physics/Grappling Hook", order = 1)]

public class GrapplingPhysicsObj : ScriptableObject
{
    [Header("Prefabs")]

    public GameObject hookPrefab;

    [Header("Properties")]

    public float minLength;

    public float maxLength;

    [Range(0, 1)]
    public float maxDistanceMultiple;

    [Range(0, 1)]
    public float minDistanceMultiple;

    public float springForce;
    
    public float damperForce;
    
    public float massScale;

    public float lungeForce;

    public float lungeGravityComp;

    public float scaleSpeed;
}
