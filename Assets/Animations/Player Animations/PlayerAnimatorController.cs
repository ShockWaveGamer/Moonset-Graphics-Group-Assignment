using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PlayerAnimatorController
{
    public static void UpdatePlayerAnim(PlayerAnimTriggerStates state, Animator anim)
    {
        switch (state)
        {
            case PlayerAnimTriggerStates.Grapple:
                anim.SetTrigger("Grapple");
                return;
            case PlayerAnimTriggerStates.Jump:
                anim.SetTrigger("Jump");
                return;
            case PlayerAnimTriggerStates.Landed:
                anim.SetTrigger("Landed");
                return;
            case PlayerAnimTriggerStates.Slide:
                anim.SetTrigger("Slide");
                return;
            default: 
                Debug.LogWarning($"Incorrect call for UpdatePlayerAnim! State: {state} not Valid");
                return;
        }
    }

    public static void UpdatePlayerAnim(PlayerAnimBooleanStates state, bool value, Animator anim)
    {
        switch (state)
        {
            case PlayerAnimBooleanStates.IsGrappled:
                anim.SetBool("isGrappled", value);
                return;
            case PlayerAnimBooleanStates.IsGrounded:
                anim.SetBool("isGrounded", value);
                return;
            case PlayerAnimBooleanStates.IsMoving:
                anim.SetBool("isMoving", value);
                return;
            case PlayerAnimBooleanStates.IsSliding:
                anim.SetBool("IsSliding", value);
                return;
            case PlayerAnimBooleanStates.IsSprinting:
                anim.SetBool("isSprinting", value);
                return;
            default:
                Debug.LogWarning($"Incorrect call for UpdatePlayerAnim! State: {state} not Valid");
                return;
        }
    }
}

public enum PlayerAnimBooleanStates
{
    IsGrappled,
    IsGrounded,
    IsMoving,
    IsSprinting,
    IsSliding
}

public enum PlayerAnimTriggerStates
{
    Grapple,
    Jump,
    Landed,
    Slide
}
