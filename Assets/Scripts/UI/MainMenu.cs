using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private EventSystem eventSystem;

    [SerializeField] GameObject mainMenu;
    [SerializeField] GameObject levelMenu;
    [SerializeField] GameObject assignMenu;
    [SerializeField] GameObject playerAssigner;

    private void Start()
    {
        eventSystem = EventSystem.current;
    }

    public void OnPlayButton()
    {
        mainMenu.SetActive(false);
        assignMenu.SetActive(true);
        playerAssigner.SetActive(true);
    }

    public void OnAssignBackButton()
    {
        playerAssigner.SetActive(false);
        assignMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void OnLevelBackButton()
    {
        levelMenu.SetActive(false);
        assignMenu.SetActive(true);
        playerAssigner.SetActive(true);
    }
}
