using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelProgressManager : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] public List<CheckPoint> checkPoints;
    [SerializeField] Material startShader, checkpointShader, endShader;
    [SerializeField] float checkpointSpeed;

    #endregion

    #region variables

    HUDWaypoints hudWaypointsManager;

    [HideInInspector] public int currentPointIndex = 0;

    #endregion

    private void Start()
    {
        hudWaypointsManager = FindObjectOfType<HUDWaypoints>();

        #region checkPoints setup

        foreach (CheckPoint checkPoint in checkPoints) SetShader(checkPoint.GetComponent<Renderer>(), checkpointShader);

        SetShader(checkPoints[0].GetComponent<Renderer>(), startShader);
        SetShader(checkPoints[checkPoints.Count - 1].GetComponent<Renderer>(), endShader);

        //Sets the ordered shader for the checkpoints in the level
        void SetShader(Renderer renderer, Material material) { renderer.material = material; }

        //enables the first point
        hudWaypointsManager.SetWaypoint(checkPoints[0].transform);

        #endregion
    }

    public IEnumerator EnableNextPoint()
    {
        float t = 0;

        while (t != 1)
        {
            t = Mathf.MoveTowards(t, 1, Time.unscaledDeltaTime * checkpointSpeed);
            for (int i = 0; i <= currentPointIndex; i++) checkPoints[i].transform.localScale = Vector3.Lerp(checkPoints[i].pointScale, new Vector3(0, 1, 0), t);
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
        }

        t = 1;

        for (int i = 0; i <= currentPointIndex; i++) 
        {
            checkPoints[i].transform.localScale = new Vector3(0, 1, 0);
            checkPoints[i].gameObject.SetActive(false);
        }

        if (currentPointIndex != checkPoints.Count - 1)
        {
            currentPointIndex++;

            checkPoints[currentPointIndex].transform.localScale = new Vector3(0, 1, 0);
            checkPoints[currentPointIndex].gameObject.SetActive(true);

            hudWaypointsManager.SetWaypoint(checkPoints[currentPointIndex].transform);

            t = 0;

            while (t != 1)
            {
                t = Mathf.MoveTowards(t, 1, Time.unscaledDeltaTime * checkpointSpeed);
                checkPoints[currentPointIndex].transform.localScale = Vector3.Lerp(new Vector3(0, 1, 0), checkPoints[currentPointIndex].pointScale, t);
                yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
            }
        }
        else
        {
            FindObjectOfType<GameManager>().CompleteLevel();
        }
    }   
}
