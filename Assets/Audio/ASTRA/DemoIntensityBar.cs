using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoIntensityBar : MonoBehaviour
{
    public AstraTrack track;
    public Image bar;
    void Update()
    {
        bar.fillAmount = track.layeringVal;
    }
}
