using Cinemachine;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    //contains components
    #region components
    #endregion

    //contains values visable in the inspector
    #region inspector

    [SerializeField]
    public float minLevelYKillLevel = -1000, maxLevelYKillLevel = 1000;
    
    #endregion

    //contains values not visable in the inspector
    #region variables

    Canvas canvas;
    
    GameObject virtualCamera;

    PlayerController playerController;

    [HideInInspector] public int deathCount;

    [HideInInspector] public bool isPaused;

    [HideInInspector] public string currentScene, currentMenuScene;

    #endregion

    private void Awake()
    {
        canvas = GetComponentInChildren<Canvas>();
        virtualCamera = FindObjectOfType<CinemachineVirtualCamera>().gameObject;
        playerController = FindObjectOfType<PlayerController>();

        Time.timeScale = 1;
        virtualCamera.SetActive(true);
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        currentScene = SceneManager.GetActiveScene().name;
    }

    public void TogglePause()
    {
        if (!playerController) return;

        if (!isPaused)
        {
            isPaused = true;

            virtualCamera.SetActive(false);

            SceneManager.LoadScene("Pause Menu", LoadSceneMode.Additive);

            playerController.movement.Disable();
            playerController.look.Disable();
            playerController.jump.Disable();
            playerController.sprint.Disable();
            playerController.slowMotionAim.Disable();
            playerController.grapple.Disable();
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            playerController.StopAllCoroutines();
            Time.timeScale = 0;
        }
        else if (SceneManager.sceneCount > 1)
        {
            isPaused = false;

            virtualCamera.SetActive(true);

            SceneManager.UnloadSceneAsync(currentMenuScene);

            playerController.movement.Enable();
            playerController.look.Enable();
            playerController.jump.Enable();
            playerController.sprint.Enable();
            playerController.slowMotionAim.Enable();
            playerController.grapple.Enable();

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            playerController.StopAllCoroutines();
            Time.timeScale = 1;
        }
    }

    public void CompleteLevel()
    {
        PlayerPrefs.SetString("FinalTime", FindObjectOfType<LevelTimer>().timer.text);
        PlayerPrefs.SetInt("DeathCount", deathCount);
        SceneManager.LoadScene("ResultsScreen");
    }
}
