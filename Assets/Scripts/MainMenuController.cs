using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Playables;

public class MainMenuController : MonoBehaviour
{
    private PlayableDirector openMenuDir;
    public IntVariable gameMode;
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
            // set Timed Game Mode (1)
            gameMode.Value = 1;
        }
        else if (game_mode == 1)
        {
            // set Survival Game Mode (0)
            gameMode.Value = 0;
        }
        
        SceneManager.LoadSceneAsync(1);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
