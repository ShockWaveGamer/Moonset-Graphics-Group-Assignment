using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AstraTrack : MonoBehaviour
{
    public enum TrackType
    {
        LayeredIntensity,
        LayeredConditional,
        Simple
    }

    public List<AstraClip> clips = new List<AstraClip>();
    public float layeringVal = 0.0f;
    public float volume = 1.0f;
    public float VolumeChangeSpeed = 0.1f
        ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        foreach (AstraClip track in clips)
        {
            if (track.isEnabled && layeringVal >= track.enableThreshold)
            {
                track.clip.GetComponent<AudioSource>().volume = Mathf.Lerp(track.clip.GetComponent<AudioSource>().volume, volume, VolumeChangeSpeed);
            }
            else
            {
                track.clip.GetComponent<AudioSource>().volume = Mathf.Lerp(track.clip.GetComponent<AudioSource>().volume, 0.0f, VolumeChangeSpeed);
            }
        }
    }
}
