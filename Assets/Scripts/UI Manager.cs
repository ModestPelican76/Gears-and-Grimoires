using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public GameObject settingsMenu;
    public GameObject pauseMenu;
    public GameObject warningMenu;
    public Scene tutorial;
    public void PlayGame()
    {
        SceneManager.LoadScene("Tutorial");
    }
    public void OpenSettings()
    {
        settingsMenu.SetActive(true);
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void CloseSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void pauseGame()
    {
        if (pauseMenu.activeInHierarchy)
        {
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
        }
    }
    public void attemptToLeave()
    {
        warningMenu.SetActive(true);
    }
    public void cancel()
    {
        pauseMenu.SetActive(true);
        warningMenu.SetActive(false);
    }
}