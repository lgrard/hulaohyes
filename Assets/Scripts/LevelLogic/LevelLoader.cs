using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using hulaohyes;

public static class LevelLoader
{
    public const int mainMenu = 0;
    private const int pauseMenu = 1;
    public const int level0 = 2;
    public const int level1 = 3;

    public static void LoadLevel(int pLevel)
    {
        SceneManager.LoadScene(pLevel);
    }

    public static bool TogglePauseMenu()
    {
        GameManager lGameManager = GameManager.getInstance();

        if (lGameManager.isPaused)
        {
            SceneManager.UnloadSceneAsync(SceneManager.GetSceneAt(1));
            lGameManager.isPaused = false;
            Time.timeScale = 1f;
        }

        else
        {
            SceneManager.LoadScene(pauseMenu,LoadSceneMode.Additive);
            lGameManager.isPaused = true;
            Time.timeScale = 0f;
        }

        return lGameManager.isPaused;
    }
}
