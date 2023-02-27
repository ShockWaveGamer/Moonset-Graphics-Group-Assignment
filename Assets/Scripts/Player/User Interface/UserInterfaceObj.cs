using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "User Interface Object", menuName = "ScriptableObjs/User Interface", order = 0)]

public class UserInterfaceObj : ScriptableObject
{
    [Header("Values")]

    public float colorLerpSpeed, sizeLerpSpeed;

    public Color defaultColor, grappleTargetColor, grappleTargetReadyColor;
}
