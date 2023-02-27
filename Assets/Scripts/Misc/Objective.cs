using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Objective : MonoBehaviour
{
    #region Components



    #endregion

    #region Inspector



    #endregion

    #region Variables



    #endregion

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if (TryGetComponent<Rigidbody>(out Rigidbody body)) Destroy(body);
            if (TryGetComponent<Collider>(out Collider collider)) Destroy(collider);
            transform.parent = collision.transform;
            transform.localPosition = new Vector3(0, 1, -0.5f);
            transform.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }
}
