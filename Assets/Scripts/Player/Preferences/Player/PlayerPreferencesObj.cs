using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Player Preferences Object", menuName = "ScriptableObjs/Preferences/Player", order = 0)]

public class PlayerPreferencesObj : ScriptableObject
{
    [Header("Controls")]

    public bool useToggleSprint;
}

