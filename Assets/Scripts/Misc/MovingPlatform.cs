using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    #region components



    #endregion

    #region inspector

    [SerializeField]
    float maxSpeed, smoothTime;

    [SerializeField]
    Transform[] points;

    [SerializeField]
    bool MoveWithPlayer;

    #endregion

    #region variables

    Vector3 currentSpeed = new Vector3();
    bool isMoving;

    #endregion

    private void Awake()
    {
        transform.position = points[0].position;
    }

    private void Update()
    {
        if (!MoveWithPlayer && !isMoving) StartCoroutine(StartMoving());
    }

    private IEnumerator StartMoving() 
    {
        isMoving = true;

        for (int i = 1; i <= points.Length; i++) // for each point along the platform's path...
            while (Vector3.Distance(transform.position, points[i % points.Length].position) > 0.1f) { //while the distance between the platform and current point is greater than 0.1...
                transform.position = Vector3.SmoothDamp(transform.position, points[i % points.Length].position, ref currentSpeed, smoothTime, maxSpeed); //move to next point
                yield return new WaitForEndOfFrame();
            }

        isMoving = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        if (MoveWithPlayer && !isMoving && collision.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            StartCoroutine(StartMoving());
        }
    }


    private void OnDrawGizmos()
    {
        if (points == null || points.Length < 2) return;

        for (int i = 0; i < points.Length; i++)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawSphere(points[i].position, 0.5f);
            
            Gizmos.color = Color.green;
            Gizmos.DrawLine(points[i].position, points[(i + 1) % points.Length].position);
        }
    }
}
