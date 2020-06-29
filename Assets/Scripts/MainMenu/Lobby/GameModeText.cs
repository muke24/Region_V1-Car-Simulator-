using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameModeText : MonoBehaviour
{
    public Text gamemodeText;

    // Start is called before the first frame update
    void Start()
    {
        if (GameMode.captureTheFlag)
        {
            gamemodeText.text = "Capture The Flag";
        }
        if (GameMode.teamDeathMatch)
        {
            gamemodeText.text = "Team Death Match";
        }
        if (GameMode.freeForAll)
        {
            gamemodeText.text = "Free For All";
        }
    }
}
