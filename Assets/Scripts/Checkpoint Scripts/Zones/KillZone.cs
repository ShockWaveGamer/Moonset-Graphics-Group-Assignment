using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class KillZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerController>(out PlayerController player))
        {
            player.Die();
        }
    }
    
    private void OnDrawGizmos()
    {
        if (TryGetComponent<BoxCollider>(out BoxCollider collider))
        {
            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawCube(transform.position, collider.size);

            Gizmos.color = new Color(1, 0, 0, 0.25f);
            Gizmos.DrawWireCube(transform.position, collider.size);
        }
    }
}
