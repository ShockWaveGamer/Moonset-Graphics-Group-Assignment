using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.InputSystem.Processors;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class SettingsMenu : MonoBehaviour
{
    #region components

    GameManager gameManager;

    InputAction look;

    #endregion

    #region inspector

    TMP_InputField mouseSensitivityXText, mouseSensitivityYText;
    Slider mouseSensitivityXSlider, mouseSensitivityYSlider;

    #endregion

    #region variables

    [HideInInspector] public float mouseSensitivityX, mouseSensitivityY;

    #endregion
    
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentMenuScene = "Settings Menu";

        look = FindObjectOfType<PlayerInput>().actions["Look"];

        mouseSensitivityXText = transform.Find("Mouse Sensitivity").Find("Horizontal Sensitivity").Find("InputField (TMP)").GetComponent<TMP_InputField>();
        mouseSensitivityYText = transform.Find("Mouse Sensitivity").Find("Vertical Sensitivity").Find("InputField (TMP)").GetComponent<TMP_InputField>();

        mouseSensitivityXSlider = transform.Find("Mouse Sensitivity").Find("Horizontal Sensitivity").Find("Slider").GetComponent<Slider>();
        mouseSensitivityYSlider = transform.Find("Mouse Sensitivity").Find("Vertical Sensitivity").Find("Slider").GetComponent<Slider>();

        SetVerticalSensitivity(PlayerPrefs.GetFloat("MouseVerticalSensitivity", 1f));
        SetHorizontalSensitivity(PlayerPrefs.GetFloat("MouseHorizontalSensitivity", 1f));
    }

    #region set mouse sensitivity

    public void SetVerticalSensitivity(TMP_InputField input)
    {
        SetVerticalSensitivity(float.Parse(input.text));
    }

    public void SetVerticalSensitivity(Slider input)
    {
        SetVerticalSensitivity(input.value);
    }

    private void SetVerticalSensitivity(float input) 
    {
        mouseSensitivityY = Mathf.Clamp(input, 0.1f, 2f);
        mouseSensitivityYText.text = mouseSensitivityY.ToString("0.0");
        mouseSensitivityYSlider.value = mouseSensitivityY;

        PlayerPrefs.SetFloat("MouseVerticalSensitivity", mouseSensitivityY);

        look.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scaleVector2(x={mouseSensitivityX}, y={mouseSensitivityY})" });
    }

    public void SetHorizontalSensitivity(TMP_InputField input)
    {
        SetHorizontalSensitivity(float.Parse(input.text));
    }

    public void SetHorizontalSensitivity(Slider input)
    {
        SetHorizontalSensitivity(input.value);
    }
    
    public void SetHorizontalSensitivity(float input)
    {
        mouseSensitivityX = Mathf.Clamp(input, 0.1f, 2f);
        mouseSensitivityXText.text = mouseSensitivityX.ToString("0.0");
        mouseSensitivityXSlider.value = mouseSensitivityX;

        PlayerPrefs.SetFloat("MouseHorizontalSensitivity", mouseSensitivityX);

        look.ApplyBindingOverride(new InputBinding { overrideProcessors = $"scaleVector2(x={mouseSensitivityX}, y={mouseSensitivityY})" });
    }

    #endregion

    public void OpenMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Settings Menu");
    }
}
