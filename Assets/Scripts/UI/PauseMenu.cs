using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] GameObject firstSelectedButton;

    public void Resume() => LevelLoader.TogglePauseMenu();
    public void Quit() => LevelLoader.LoadLevel(LevelLoader.mainMenu);

    private void Start()
    {
        EventSystem.current.SetSelectedGameObject(firstSelectedButton);
    }
}
