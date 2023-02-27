using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    Transform respawnPoint;
    [HideInInspector] public Vector3 pointScale;
    
    private void Awake()
    {
        respawnPoint = transform.GetChild(0);

        pointScale = transform.localScale;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            LevelProgressManager levelProgressManager = FindObjectOfType<LevelProgressManager>();
            levelProgressManager.currentPointIndex = levelProgressManager.checkPoints.IndexOf(this);

            StartCoroutine(FindObjectOfType<HUDCheckpoint>().CheckFill(gameObject.name, GetComponent<Renderer>().material.GetColor("_Color")));
            GetComponent<Collider>().enabled = false;

            player.respawnPoint = respawnPoint.position;
        }
    }
}
