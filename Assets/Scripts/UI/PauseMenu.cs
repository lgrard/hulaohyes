using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedButton;

    public void Resume() => LevelLoader.TogglePauseMenu();
    public void Quit()
    {
        EventSystem.current.enabled = false;
        StartCoroutine(LevelLoader.LoadLevel(LevelLoader.mainMenu));
    }

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
