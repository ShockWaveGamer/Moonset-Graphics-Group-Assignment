using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.UI;

public class HUDWaypoints : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField] float waypointSpeed;

    #endregion

    #region variables

    GameObject waypointImageObj;

    Transform currentPoint;

    Image wpImage;
    RectTransform wpTrans;

    #endregion

    private void Awake()
    {
        waypointImageObj = transform.Find("Waypoint Obj").gameObject;

        wpImage = waypointImageObj.GetComponent<Image>();
        wpTrans = waypointImageObj.GetComponent<RectTransform>();
    }

    private void Update()
    {
        if (currentPoint)
        {
            if (Vector3.Angle(Camera.main.transform.forward, currentPoint.position - Camera.main.transform.position) < 90)
            {
                if (!waypointImageObj.activeSelf) waypointImageObj.SetActive(true);
            } 
            else if (waypointImageObj.activeSelf) waypointImageObj.SetActive(false);

            wpTrans.position = Vector3.Lerp(wpTrans.position, Camera.main.WorldToScreenPoint(currentPoint.position), Time.deltaTime * waypointSpeed);
        }
        else if (waypointImageObj.activeSelf) waypointImageObj.SetActive(false);
    }

    public void SetWaypoint(Transform point)
    {
        currentPoint = point;
    }

    public void DisableWaypoint()
    {
        currentPoint = null;
    }
}
