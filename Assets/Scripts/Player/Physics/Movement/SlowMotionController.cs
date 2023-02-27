using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public static class SlowMotionController
{
    public static void SetUpVolumeVignette(PlayerController player)
    {
        Volume volume = GameObject.FindObjectOfType<Volume>();
        if (volume.sharedProfile.TryGet(out Vignette vignette)) vignette.intensity.value = player.movementPhyObj.minVignetteIntensity;
        else Debug.LogError("Vignette overide Attached to the Volume!");
    }

    public static void StartSlowMotionCoroutine(PlayerController player)
    {
        if (!player || player.isSlowed) return;

        player.StartCoroutine(StartSlowMotion(player));

        IEnumerator StartSlowMotion(PlayerController player)
        {
            Volume volume = GameObject.FindObjectOfType<Volume>();
            if (volume.sharedProfile.TryGet(out Vignette vignette))
            {
                player.isSlowed = true;

                while (player && player.slowT != 1f && player.isSlowed)
                {
                    player.slowT = Mathf.MoveTowards(player.slowT, 1f, Time.unscaledDeltaTime * player.movementPhyObj.slowSmoothSpeed);

                    vignette.intensity.value = Mathf.Lerp(player.movementPhyObj.minVignetteIntensity, player.movementPhyObj.maxVignetteIntensity, player.slowT);
                    Time.timeScale = Mathf.Lerp(1f, player.movementPhyObj.slowIntensity, player.slowT);
                    Time.fixedDeltaTime = Mathf.Lerp(1f * 0.02f, player.movementPhyObj.slowIntensity * 0.02f, player.slowT);

                    yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
                }
            };
        }
    }

    public static void StopSlowMotionCoroutine(PlayerController player)
    {
        if (!player || !player.isSlowed) return;

        player.StartCoroutine(StopSlowMotion(player));

        IEnumerator StopSlowMotion(PlayerController player)
        {
            Volume volume = GameObject.FindObjectOfType<Volume>();
            if (volume.sharedProfile.TryGet(out Vignette vignette))
            {
                player.isSlowed = false;

                while (player && player.slowT != 0f && !player.isSlowed)
                {
                    player.slowT = Mathf.MoveTowards(player.slowT, 0f, Time.unscaledDeltaTime * player.movementPhyObj.slowSmoothSpeed);

                    vignette.intensity.value = Mathf.Lerp(player.movementPhyObj.minVignetteIntensity, player.movementPhyObj.maxVignetteIntensity, player.slowT);
                    Time.timeScale = Mathf.Lerp(1f, player.movementPhyObj.slowIntensity, player.slowT);
                    Time.fixedDeltaTime = Mathf.Lerp(1f * 0.02f, player.movementPhyObj.slowIntensity * 0.02f, player.slowT);

                    yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);
                }
            };
        }
    }
}
