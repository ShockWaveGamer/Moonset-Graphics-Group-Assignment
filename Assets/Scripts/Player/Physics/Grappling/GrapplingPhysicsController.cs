using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public static class GrapplingPhysicsController
{
    public static void StartGrapple(PlayerController player)
    {
        if (!player) return;

        if (Vector3.Distance(player.GetComponent<Rigidbody>().position, player.ReticleTarget().point) >= player.grapplingPhysicsObj.maxLength || !player.ReticleTarget().collider) return;

        switch (player.ReticleTarget().collider.tag)
        {
            case "GrappleTarget":
                StartSwing(player);
                return;
            case "GrappleDraggable":
                StartDrag(player);
                return;
        }

    }

    private static void StartSwing(PlayerController player)
    {
        player.transform.parent = GameObject.FindObjectOfType<GameManager>().transform;

        player.grappleJoint = player.AddComponent<SpringJoint>();

        player.grappleJoint.autoConfigureConnectedAnchor = false;
        player.StartCoroutine(UpdateAnchor());
        player.StartCoroutine(ScaleGrapple(player));

        player.grappleScale = Vector3.Distance(player.transform.position, player.ReticleTarget().point);

        player.grappleJoint.maxDistance = player.grappleScale * player.grapplingPhysicsObj.maxDistanceMultiple;
        player.grappleJoint.minDistance = player.grappleScale * player.grapplingPhysicsObj.minDistanceMultiple;
        
        player.grappleJoint.spring = player.grapplingPhysicsObj.springForce;
        player.grappleJoint.damper = player.grapplingPhysicsObj.damperForce;
        player.grappleJoint.massScale = player.grapplingPhysicsObj.massScale;
        
        player.GetComponent<LineRenderer>().positionCount = 2;

        player.doubleJumpReady = true;

        PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Grapple, player.animator);

        IEnumerator UpdateAnchor()
        {
            Transform connectedAnchor = new GameObject().transform;
            connectedAnchor.SetPositionAndRotation(player.ReticleTarget().point, Quaternion.LookRotation(player.ReticleTarget().normal));
            connectedAnchor.SetParent(player.ReticleTarget().collider.transform);

            player.grappleHookObj = GameObject.Instantiate(player.grapplingPhysicsObj.hookPrefab, connectedAnchor.position, Quaternion.identity, connectedAnchor).transform;

            while (player.grappleJoint)
            {
                player.grappleJoint.connectedAnchor = connectedAnchor.position;

                player.grappleHookObj.LookAt(player.transform);

                if (player.grappleLineRenderer.positionCount == 2)
                {
                    player.grappleLineRenderer.SetPosition(0, connectedAnchor.position);
                    player.grappleLineRenderer.SetPosition(1, player.grapplingHandObj.position);
                }

                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator ScaleGrapple(PlayerController player)
        {
            while (player.grappleScale != player.grapplingPhysicsObj.minLength)
            {
                if (!player.grappleJoint) yield break;

                player.grappleScale = Mathf.SmoothDamp(player.grappleScale, player.grapplingPhysicsObj.minLength, ref player.grappleScaleSpeed, player.grapplingPhysicsObj.scaleSpeed);
                player.grappleJoint.maxDistance = player.grappleScale * player.grapplingPhysicsObj.maxDistanceMultiple;
                player.grappleJoint.minDistance = player.grappleScale * player.grapplingPhysicsObj.minDistanceMultiple;
                yield return new WaitForEndOfFrame();
            }
        }
    }

    public static void GrappleLunge(PlayerController player)
    {
        if (!player.grappleJoint) return;

        Vector3 vectorToAnchor = player.grappleJoint.connectedAnchor - player.transform.position;
        Vector3 lungeForce = Vector3.Lerp(vectorToAnchor * player.grapplingPhysicsObj.lungeForce, -Physics.gravity * vectorToAnchor.magnitude * player.grapplingPhysicsObj.lungeGravityComp, 0.5f);
        player.body.velocity = Vector3.Lerp(player.body.velocity, lungeForce, 0.25f);

        CancelGrapple(player);
    }

    public static void CancelGrapple(PlayerController player)
    {
        if (player.grappleJoint)
        {
            GameObject.Destroy(player.grappleJoint);
            if (player.grappleHookObj) GameObject.Destroy(player.grappleHookObj.gameObject);
            
            player.grappleLineRenderer.positionCount = 0;
        }
    }

    public static void StartDrag(PlayerController player)
    {
        player.grappleJoint = player.ReticleTarget().collider.gameObject.AddComponent<SpringJoint>();

        player.grappleJoint.autoConfigureConnectedAnchor = false;
        player.StartCoroutine(UpdateAnchor());
        player.StartCoroutine(ScaleGrapple(player));

        player.grappleScale = Vector3.Distance(player.transform.position, player.ReticleTarget().point);

        player.grappleJoint.maxDistance = player.grappleScale * player.grapplingPhysicsObj.maxDistanceMultiple;
        player.grappleJoint.minDistance = player.grappleScale * player.grapplingPhysicsObj.minDistanceMultiple;

        player.grappleJoint.spring = player.grapplingPhysicsObj.springForce;
        player.grappleJoint.damper = player.grapplingPhysicsObj.damperForce;
        player.grappleJoint.massScale = player.grapplingPhysicsObj.massScale;

        player.GetComponent<LineRenderer>().positionCount = 2;

        IEnumerator UpdateAnchor()
        {
            Transform connectedAnchor = new GameObject().transform;
            connectedAnchor.SetPositionAndRotation(player.ReticleTarget().point, Quaternion.LookRotation(player.ReticleTarget().normal));
            connectedAnchor.SetParent(player.ReticleTarget().collider.transform);

            player.grappleHookObj = GameObject.Instantiate(player.grapplingPhysicsObj.hookPrefab, player.grappleJoint.connectedAnchor, Quaternion.identity, connectedAnchor).transform;

            while (player.grappleJoint)
            {
                player.grappleJoint.connectedAnchor = connectedAnchor.position;

                player.grappleHookObj.LookAt(player.transform);
                
                player.grappleLineRenderer.SetPosition(0, player.grapplingHandObj.position);
                player.grappleLineRenderer.SetPosition(1, player.grappleJoint.connectedAnchor);
                
                yield return new WaitForEndOfFrame();
            }
        }

        IEnumerator ScaleGrapple(PlayerController player)
        {
            while (player.grappleScale != player.grapplingPhysicsObj.minLength)
            {
                if (!player.grappleJoint) yield break;

                player.grappleScale = Mathf.SmoothDamp(player.grappleScale, player.grapplingPhysicsObj.minLength, ref player.grappleScaleSpeed, 1f);
                player.grappleJoint.maxDistance = player.grappleScale * player.grapplingPhysicsObj.maxDistanceMultiple;
                player.grappleJoint.minDistance = player.grappleScale * player.grapplingPhysicsObj.minDistanceMultiple;
                yield return new WaitForEndOfFrame();
            }
        }
    }
    
    public static void DrawLine(PlayerController player)
    {
        if (player.grappleLineRenderer.positionCount == 2)
        {
            player.grappleLineRenderer.SetPosition(0, player.grappleJoint.connectedAnchor);
            player.grappleLineRenderer.SetPosition(1, player.grapplingHandObj.position);
        }
    }
}
