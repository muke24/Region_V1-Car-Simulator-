using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    public GameObject mainMenuUI;
    public GameObject modeSelect;

    public void GameModeSelect()
    {
        mainMenuUI.SetActive(false);
        modeSelect.SetActive(true);
    }

    public void BackToMainMenu()
    {
        mainMenuUI.SetActive(true);
        modeSelect.SetActive(false);
        GameMode.singleplayer = false;
        GameMode.multiplayer = false;
    }
}
