using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HUDCheckpoint : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] float checkSmoothTime, checkSpeed;
    [SerializeField] int flickers;
    [SerializeField] float flickerSpeed;

    [Space]

    [SerializeField] bool test;
    [SerializeField] string testCheckpointName;
    [SerializeField] Color testCheckpointColor;

    #endregion

    #region variables

    Image bar;
    Image fill;
    TMP_Text text;

    float checkCurrentSpeed;

    [SerializeField] bool fillActive;

    #endregion
    
    private void Awake()
    {
        bar = transform.Find("Bar").GetComponent<Image>();
        fill = bar.transform.Find("Bar Fill").GetComponent<Image>();
        text = bar.transform.Find("Bar Text").GetComponent<TMP_Text>();

        bar.gameObject.SetActive(false);
    }

    private void Update()
    {
        if (!fillActive && test)
        {
            test = false;
            StartCoroutine(CheckFill(testCheckpointName, testCheckpointColor));
        }
    }

    public IEnumerator CheckFill(string text, Color color)
    {
        if (fillActive) yield break;

        StartCoroutine(FlickerActive(true));
        fillActive = true;
        fill.fillAmount = 0;
        
        this.text.text = text;
        this.text.color = new Color(color.r, color.g, color.b, 1f);

        bar.color = new Color(color.r, color.g, color.b, 0.25f);
        fill.color = new Color(color.r, color.g, color.b, 1f);
        
        while (fillActive)
        {
            fill.fillAmount = Mathf.SmoothDamp(fill.fillAmount, 1, ref checkCurrentSpeed, checkSmoothTime, checkSpeed);

            yield return new WaitForEndOfFrame();

            if (fill.fillAmount >= 0.99f) 
            {
                fill.fillAmount = 1;
                StartCoroutine(FindObjectOfType<LevelProgressManager>().EnableNextPoint());
                fillActive = false;
            }
        }

        StartCoroutine(FlickerActive(false));
        fillActive = false;

        IEnumerator FlickerActive(bool active)
        {
            int count = 0;
            while (count < flickers)
            {
                bar.gameObject.SetActive(active);
                yield return new WaitForSeconds(flickerSpeed);

                bar.gameObject.SetActive(!active);
                yield return new WaitForSeconds(flickerSpeed);

                count++;
            }

            bar.gameObject.SetActive(active);
        }
    }

    public void StopBar()
    {
        fillActive = false;
    }
}
