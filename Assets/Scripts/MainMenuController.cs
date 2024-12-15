using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenuController : MonoBehaviour
{
    private PlayableDirector openMenuDir;
    private bool menuOpened = false;

    public void openGameModeMenu()
    {
        if (!menuOpened)
        {
            openMenuDir = GameObject.Find("Game Modes Menu Timeline").GetComponent<PlayableDirector>();
            openMenuDir.Play();
            menuOpened = false;
        }
    }

    public void StartGame(int game_mode)
    {
        if (game_mode == 0)
        {
            // set Timed Game Mode
        }
        else if (game_mode == 1)
        {
            // set Survival Game Mode
        }
        
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
