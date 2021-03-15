using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    public void Resume() => LevelLoader.TogglePauseMenu();
    public void Quit() => LevelLoader.LoadLevel(LevelLoader.mainMenu);
}
