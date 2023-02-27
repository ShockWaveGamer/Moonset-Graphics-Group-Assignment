using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class DemoInteraction : MonoBehaviour
{

    GameControls controls;
    float val = 0.0f;
    public float sensitivity = 10.0f;
    float barAmount = 0.0f;
    public AstraTrack track;
    // Start is called before the first frame update
    void Awake()
    {
        controls = new GameControls();
    }
    private void OnEnable()
    {
        controls.Enable();
        
    }
    private void OnDisable()
    {
        controls.Disable();
    }
    // Update is called once per frame
    void Update()
    {
        val = controls.UI.Navigate.ReadValue<Vector2>().x;
        barAmount += val / sensitivity;
        track.layeringVal = barAmount;
    }
}
