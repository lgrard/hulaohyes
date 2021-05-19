using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MainMenu : MonoBehaviour
{
    private EventSystem eventSystem;
    private GameObject activeMenu;
    private GameObject previousMenu = null;

    [SerializeField] Text versionContainer;
    [SerializeField] GameObject playerAssigner;

    [Header("Menus")]
    [SerializeField] List<GameObject> menuList;

    [Header("First Selected Buttons")]
    [SerializeField] List<GameObject> firstButtonList;

    private void OnEnable()
    {
        eventSystem = GetComponent<EventSystem>();
        EventSystem.current = eventSystem;
        ChangeMenu(0);

        versionContainer.text += Application.version;
    }

    private void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
    }

    public void LoadLevel(int pLevelIndex)
    {
        StartCoroutine(LevelLoader.LoadLevel(pLevelIndex));
    }

    private void ChangeMenu(int pMenuIndex)
    {
        previousMenu = activeMenu;
        activeMenu = menuList[pMenuIndex];
        
        if(previousMenu != null) previousMenu.SetActive(false);
        activeMenu.SetActive(true);

        eventSystem.SetSelectedGameObject(firstButtonList[pMenuIndex]);
    }

    public void BackButton()
    {
        ChangeMenu(0);
    }

    public void NextMenu(int pMenuIndex)
    {
        ChangeMenu(pMenuIndex);
    }

    public void QuitGame() => Application.Quit();
}
