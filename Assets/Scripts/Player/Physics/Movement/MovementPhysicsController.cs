using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.InputSystem;

public static class MovementPhysicsController
{
    public static void Movement(PlayerController player)
    {
        Vector2 input = player.movement.ReadValue<Vector2>();
        Vector3 movementInput = new Vector3(input.x, 0, input.y).normalized;
        bool inputActive = movementInput.magnitude >= 0.1f;

        Vector3 forwardVector = player.isGrounded ?
            Vector3.Cross(player.groundNormal, -player.camFollowTargetTransform.right).normalized :
            Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized;
        Vector3 rightVector = player.isGrounded ?
            Vector3.Cross(player.groundNormal, player.camFollowTargetTransform.forward).normalized :
            Camera.main.transform.right;

        Debug.DrawRay(player.transform.position, forwardVector, Color.blue);
        Debug.DrawRay(player.transform.position, rightVector, Color.red);
        
        Vector3 directionalInput = (forwardVector * movementInput.z + rightVector * movementInput.x).normalized;

        if (player.isGrounded)
        {
            switch (player.groundTypeTag)
            {
                case "SlideGround":
                    SlideOnGround(player, directionalInput);
                    break;
                default:
                    WalkOnGround(player, directionalInput);
                    break;
            }

            PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimBooleanStates.IsSprinting, player.isSprinting, player.animator);
            PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimBooleanStates.IsMoving, input.magnitude >= 0.1f, player.animator);
        }
        else if (player.grappleJoint && player.grappleHookObj)
        {
            player.body.AddForce(directionalInput * player.movementPhyObj.airStrafeSpeed, ForceMode.Acceleration);
            RotatePlayer(player, player.body.velocity, Vector3.Normalize(player.grappleHookObj.transform.position - player.transform.position));
        }
        else
        {
            Vector3 currentHorizontalVelocity = new Vector3(player.body.velocity.x, 0, player.body.velocity.z);

            bool canMoveRht = Mathf.Abs(currentHorizontalVelocity.x) < player.movementPhyObj.airStrafeSpeedCap || 
                directionalInput.x / Mathf.Abs(directionalInput.x) != currentHorizontalVelocity.x / Mathf.Abs(currentHorizontalVelocity.x);
             
            if (canMoveRht) player.body.AddForce(directionalInput.x * Vector3.right * player.movementPhyObj.airStrafeSpeed, ForceMode.Acceleration);

            bool canMoveFwd = Mathf.Abs(currentHorizontalVelocity.z) < player.movementPhyObj.airStrafeSpeedCap ||
                directionalInput.z / Mathf.Abs(directionalInput.z) != currentHorizontalVelocity.z / Mathf.Abs(currentHorizontalVelocity.z);
            
            if (canMoveFwd) player.body.AddForce(directionalInput.z * Vector3.forward * player.movementPhyObj.airStrafeSpeed, ForceMode.Acceleration);

            RotatePlayer(player, forwardVector, Vector3.up);
        }
        
        PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimBooleanStates.IsGrappled, player.grappleJoint, player.animator);
        PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimBooleanStates.IsGrounded, player.isGrounded, player.animator);
        PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimBooleanStates.IsSliding, player.groundTypeTag == "SlideGround", player.animator);
        if (inputActive) player.actionStarted = true;
    }

    private static void WalkOnGround(PlayerController player, Vector3 input)
    {
        Vector3 newVelocity = input * (player.isSprinting ? player.movementPhyObj.sprintSpeed : player.movementPhyObj.walkSpeed) * (input.magnitude >= 0.1f ? 1 : 0);
        if (player.groundCollider.TryGetComponent<Rigidbody>(out Rigidbody groundbody)) player.transform.parent = groundbody.transform;
        else player.transform.parent = GameObject.FindObjectOfType<GameManager>().transform;
        player.body.velocity = Vector3.Lerp(player.body.velocity, newVelocity, input.magnitude >= 0.1f ? player.movementPhyObj.acceleration : player.movementPhyObj.deceleration * Time.deltaTime);

        Vector3 playerForwardDirection = input.magnitude >= 0.1f ? input : Vector3.Cross(player.transform.right, player.groundNormal);
        RotatePlayer(player, playerForwardDirection, Vector3.up);
    }

    private static void SlideOnGround(PlayerController player, Vector3 input) 
    {
        player.body.AddForce(new Vector3(player.groundNormal.x, 1, player.groundNormal.z).normalized * player.groundNormal.y);

        RotatePlayer(player, player.body.velocity, Vector3.up);

        if (!player.animator.GetCurrentAnimatorStateInfo(0).IsName("Slide"))
            PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Slide, player.animator);
    }
    
    public static void Jump(PlayerController player)
    {
        if (player.jumpCooldown || !player) return;
        if (player.grappleJoint) { GrapplingPhysicsController.GrappleLunge(player); return; }

        if (player.isGrounded)
        {
            player.jumpCooldown = true;
            player.isGrounded = false;
            player.StartCoroutine(StartJumpForce());
        }
        else if (player.doubleJumpReady)
        {
            player.doubleJumpReady = false;
            player.StartCoroutine(StartDoubleJumpForce());
        }
       
        IEnumerator StartJumpForce()
        {
            PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Jump, player.animator);
            
            yield return new WaitForSeconds(0.1f);
            
            Physics.Raycast(player.transform.position, -player.transform.up, out RaycastHit hit, player.GetComponent<CapsuleCollider>().height / 1.5f);
            Vector3 jumpVector = Vector3.Lerp(hit.normal, Vector3.up, 0.5f).normalized;
            player.body.AddForce(jumpVector * player.movementPhyObj.jumpPower, ForceMode.Impulse);
            
            yield return new WaitForSeconds(player.movementPhyObj.jumpCooldown);
            
            player.jumpCooldown = false;
        }

        IEnumerator StartDoubleJumpForce()
        {
            PlayerAnimatorController.UpdatePlayerAnim(PlayerAnimTriggerStates.Jump, player.animator);

            yield return new WaitForSeconds(0.1f);

            Vector3 currentHorizontalVelocity = new Vector3(player.body.velocity.x, 0, player.body.velocity.z);

            Vector3 forwardVec = Vector3.Cross(Camera.main.transform.right, Vector3.up).normalized;
            Vector3 rightVec = Vector3.Cross(Camera.main.transform.forward, Vector3.up).normalized;

            Vector3 input = player.movement.ReadValue<Vector2>();
            Vector3 jumpVector = (forwardVec * input.y + rightVec * -input.x) * currentHorizontalVelocity.magnitude;

            Vector3 newHorizontalVelocity = Vector3.Lerp(currentHorizontalVelocity, jumpVector, 0.5f);
            Vector3 newVerticalVelocity = (player.movementPhyObj.jumpPower > player.body.velocity.y ? player.movementPhyObj.jumpPower : player.body.velocity.y) * Vector3.up;

            player.body.velocity = (newHorizontalVelocity + newVerticalVelocity) * 1.5f;

            yield return new WaitForSeconds(player.movementPhyObj.jumpCooldown);

            player.jumpCooldown = false;
        }
    }

    public static void SprintPerformed(PlayerController player)
    {
        if (player.playerPreferencesObj.useToggleSprint) player.isSprinting = !player.isSprinting; //toggle sprint state for toggle sprint
    }

    private static void RotatePlayer(PlayerController player, Vector3 forwardVector, Vector3 upVector)
    {
        player.body.transform.rotation = Quaternion.Lerp(player.body.transform.rotation, Quaternion.LookRotation(forwardVector, upVector), Time.deltaTime * player.movementPhyObj.rotationSpeed);
    }
}
