using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    GameManager gameManager;

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        gameManager.currentMenuScene = "Pause Menu";
    }

    public void TogglePause()
    {
        gameManager.TogglePause();
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(gameManager.currentScene);
        SceneManager.UnloadSceneAsync("Pause Menu");
    }

    public void OpenMenuScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Additive);
        SceneManager.UnloadSceneAsync("Pause Menu");
    }

    public void MainMenu()
    {
        SceneManager.LoadScene("Title Screen");
        SceneManager.UnloadSceneAsync("Pause Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
