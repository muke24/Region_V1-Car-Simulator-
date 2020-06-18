using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamemodeSelect : MonoBehaviour
{
    public Toggle ToggleFFA;
    public Toggle ToggleTDM;
    public Toggle ToggleCTF;

    public int gameMode = 0;

    public void OnPlay()
    {
        if (ToggleFFA.isOn && ToggleTDM.isOn && ToggleCTF.isOn)
        {
            gameMode = Random.Range(1, 4);

            if (gameMode == 1)
            {
                GameMode.freeForAll     = true;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = false;
                Debug.Log("Random Gamemode has been chosen. The chosen gamemode is Free For All");
            }

            if (gameMode == 2)
            {
                GameMode.freeForAll     = false;
                GameMode.teamDeathMatch = true;
                GameMode.captureTheFlag = false;
                print("Random Gamemode has been chosen. The chosen gamemode is Team Death Match");
            }

            if (gameMode == 3)
            {
                GameMode.freeForAll     = false;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = true;
                print("Random Gamemode has been chosen. The chosen gamemode is Capture The Flag");
            }            
        }

        if (ToggleFFA.isOn && ToggleTDM.isOn && !ToggleCTF.isOn)
        {
            gameMode = Random.Range(1, 3);

            if (gameMode == 1)
            {
                GameMode.freeForAll     = true;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = false;
                print("Random Gamemode has been chosen. The chosen gamemode is Free For All");
            }

            if (gameMode == 2)
            {
                GameMode.freeForAll     = false;
                GameMode.teamDeathMatch = true;
                GameMode.captureTheFlag = false;
                print("Random Gamemode has been chosen. The chosen gamemode is Team Death Match");
            }
        }

        if (ToggleFFA.isOn && !ToggleTDM.isOn && ToggleCTF.isOn)
        {
            gameMode = Random.Range(1, 3);

            if (gameMode == 1)
            {
                GameMode.freeForAll     = true;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = false;
                print("Random Gamemode has been chosen. The chosen gamemode is Free For All");
            }

            if (gameMode == 2)
            {
                GameMode.freeForAll     = false;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = true;
                print("Random Gamemode has been chosen. The chosen gamemode is Capture The Flag");
            }
        }

        if (!ToggleFFA.isOn && ToggleTDM.isOn && ToggleCTF.isOn)
        {
            gameMode = Random.Range(1, 3);

            if (gameMode == 1)
            {
                GameMode.freeForAll = false;
                GameMode.teamDeathMatch = true;
                GameMode.captureTheFlag = false;
                print("Random Gamemode has been chosen. The chosen gamemode is Team Death Match");
            }

            if (gameMode == 2)
            {
                GameMode.freeForAll = false;
                GameMode.teamDeathMatch = false;
                GameMode.captureTheFlag = true;
                print("Random Gamemode has been chosen. The chosen gamemode is Capture The Flag");
            }
        }

        if (ToggleFFA.isOn && !ToggleTDM.isOn && !ToggleCTF.isOn)
        {
            GameMode.freeForAll = true;
            GameMode.teamDeathMatch = false;
            GameMode.captureTheFlag = false;
            print("The chosen gamemode is Free For All");
        }

        if (!ToggleFFA.isOn && ToggleTDM.isOn && !ToggleCTF.isOn)
        {
            GameMode.freeForAll = false;
            GameMode.teamDeathMatch = true;
            GameMode.captureTheFlag = false;
            print("The chosen gamemode is Team Death Match");
        }

        if (!ToggleFFA.isOn && !ToggleTDM.isOn && ToggleCTF.isOn)
        {
            GameMode.freeForAll = false;
            GameMode.teamDeathMatch = false;
            GameMode.captureTheFlag = true;
            print("The chosen gamemode is Capture The Flag");
        }
    }
}
