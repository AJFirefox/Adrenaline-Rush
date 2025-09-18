using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
   public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game is quitting...");   
    }

    public void LoadScene(string Menu)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 4);
    }
}
