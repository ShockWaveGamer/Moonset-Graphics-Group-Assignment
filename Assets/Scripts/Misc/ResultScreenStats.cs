using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ResultScreenStats : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] TMP_Text finalTime, deathCount;

    #endregion

    #region variables



    #endregion

    private void Awake()
    {
        Debug.Log(PlayerPrefs.GetString("FinalTime") + " : " + PlayerPrefs.GetInt("DeathCount"));

        finalTime.SetText("Final Time: " + PlayerPrefs.GetString("FinalTime"));
        deathCount.SetText("Total Deaths: " + PlayerPrefs.GetInt("DeathCount").ToString());
    }
}
