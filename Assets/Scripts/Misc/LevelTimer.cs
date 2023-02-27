using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LevelTimer : MonoBehaviour
{
    public TMP_Text timer;
    public PlayerController player;
    bool runTimer = false;
    public bool isRunning = false;
    public float timerCounter;

    public string timerOut;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        runTimer = player.actionStarted;
        
        if (runTimer && !isRunning)
        {
            isRunning = true;
        }
        
        if (isRunning)
        {
            timerCounter += Time.unscaledDeltaTime;
        }

        // format timerCounter to a format of Minutes:Seconds to 2 decimal places and output it to timer.text
        

        float minutes = Mathf.Floor(timerCounter / 60);
        float seconds = timerCounter % 60;
        timerOut = minutes.ToString() + ":" + seconds.ToString("F2");
        timer.text = timerOut;
    }

}
