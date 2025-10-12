using UnityEngine;
using UnityEngine.SceneManagement;

public class Heaven_Script : MonoBehaviour
{
    public GameObject endUI;
    public GameObject conversationManager;
    public Playerv2 player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void endGame()
    {
        player.preventInput();
        conversationManager.SetActive(false);
        endUI.SetActive(true);
    }
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
