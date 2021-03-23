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
    public const int loadingScreen = 4;

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

    public static IEnumerator LoadLevel(int pLevel)
    {
        yield return null;

        int lLastSceneIndex = SceneManager.GetActiveScene().buildIndex;
        bool lHasPauseMenu = SceneManager.GetSceneByBuildIndex(pauseMenu).isLoaded;

        SceneManager.LoadScene(loadingScreen, LoadSceneMode.Additive);
        yield return new WaitForSecondsRealtime(0.75f);

        AsyncOperation operation = SceneManager.LoadSceneAsync(pLevel, LoadSceneMode.Additive);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f) operation.allowSceneActivation = true;
            yield return null;
        }

        SceneManager.UnloadSceneAsync(lLastSceneIndex);
        if (lHasPauseMenu) SceneManager.UnloadSceneAsync(pauseMenu);
        SceneManager.UnloadSceneAsync(loadingScreen);
        Time.timeScale = 1f;

        yield return null;
    }
}
