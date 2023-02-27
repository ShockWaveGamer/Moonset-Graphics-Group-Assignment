using System;
using UnityEngine;
using UnityEngine.UI;

public static class UserInterfaceController
{
    public static void UpdateReticle(PlayerController player)
    {
        if (!player.ReticleTarget().collider)
        {
            DefaultReticleState(player.reticle, player.userInterfaceObj);
            return;
        }

        switch (player.ReticleTarget().collider.tag)
        {
            case "GrappleTarget":
                if (Vector3.Distance(player.transform.position, player.ReticleTarget().point) <= player.grapplingPhysicsObj.maxLength)
                {
                    SetReticleColor(player.reticle, player.userInterfaceObj.grappleTargetReadyColor, player.userInterfaceObj);
                    SetReticleSize(player.reticle, 50f, player.userInterfaceObj);
                }
                else
                {
                    SetReticleColor(player.reticle, player.userInterfaceObj.grappleTargetColor, player.userInterfaceObj);
                    
                    float newSize = Vector3.Distance(player.transform.position, player.ReticleTarget().point) * 2 - player.grapplingPhysicsObj.maxLength;
                    SetReticleSize(player.reticle, newSize, player.userInterfaceObj);
                }
                return;
            /*case "MovableObject":
                if (player.ReticleTarget().collider && Vector3.Distance(player.transform.position, player.ReticleTarget().point) <= player.grapplingPhysicsObj.maxLength) SetReticle(player.reticle, 1f, Color.magenta, player.userInterfaceObj);
                else DefaultReticleState(player.reticle, player.userInterfaceObj);
                return;*/
            default:
                DefaultReticleState(player.reticle, player.userInterfaceObj);
                return;
        }
    }

    private static void DefaultReticleState(Image reticle, UserInterfaceObj userInterfaceObj)
    {
        SetReticleSize(reticle, 100f, userInterfaceObj);
        SetReticleColor(reticle, userInterfaceObj.defaultColor, userInterfaceObj);
    }

    private static void SetReticleSize(Image reticle, float size, UserInterfaceObj userInterfaceObj)
    {
        reticle.rectTransform.sizeDelta = Vector2.Lerp(reticle.rectTransform.sizeDelta, new Vector2(size, size), userInterfaceObj.sizeLerpSpeed * Time.unscaledDeltaTime);
    }

    private static void SetReticleColor(Image reticle, Color color, UserInterfaceObj userInterfaceObj)
    {
        reticle.color = Color.Lerp(reticle.color, color, userInterfaceObj.colorLerpSpeed * Time.unscaledDeltaTime);
    }
}
