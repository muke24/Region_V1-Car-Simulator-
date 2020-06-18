#region This code is written by Peter Thompson
using UnityEngine;

public class GameMode : MonoBehaviour
{
    static GameMode instance;

    // Bool to check if the game mode should be singleplayer or multiplayer
    public static bool singleplayer   = false;
    public static bool multiplayer    = false;

    // Bools to check the mode with how many number of players
    public static bool mode2Players   = false;
    public static bool mode10Players  = false;
    public static bool mode100Players = false;

    // Bools for certain game modes
    public static bool freeForAll     = false;
    public static bool teamDeathMatch = false;
    public static bool captureTheFlag = false;
    public static bool battleRoyale   = false;

    // Singleton
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    // Sets all the bools to false when this void has been called
    public static void SetAllValuesToFalse()
    {
        singleplayer    = false;
        multiplayer     = false;

        mode2Players    = false;
        mode10Players   = false;
        mode100Players  = false;

        freeForAll      = false;
        teamDeathMatch  = false;
        captureTheFlag  = false;
        battleRoyale    = false;
    }

	#region Network Modes
	public void SinglePlayer()
    {
        // Sets the mode to singleplayer
        singleplayer    = true;
        multiplayer     = false;
    }

    public void MultiPlayer()
    {
        // Sets the mode to multiplayer
        singleplayer    = false;
        multiplayer     = true;
    }
	#endregion

	#region Game Modes
	public void OneVerseOne()
    {
        // Sets the game mode to 1v1
        mode2Players    = true;
        mode10Players   = false;
        mode100Players  = false;
    }

    public void FiveVerseFive()
    {
        // Sets the game mode to 5v5
        mode2Players    = false;
        mode10Players   = true;
        mode100Players  = false;
    }

    public void BattleRoyale()
    {
        // Sets the game mode to battle royale
        mode2Players    = false;
        mode10Players   = false;
        mode100Players  = true;
    }
	#endregion

	#region Game Types
    public void FFA()
    {
        freeForAll      = true;
        teamDeathMatch  = false;
        captureTheFlag  = false;
        battleRoyale    = false;
    }

    public void TDM()
    {
        freeForAll      = false;
        teamDeathMatch  = true;
        captureTheFlag  = false;
        battleRoyale    = false;
    }

    public void CTF()
    {
        freeForAll      = false;
        teamDeathMatch  = false;
        captureTheFlag  = true;
        battleRoyale    = false;
    }

    public void BR()
    {
        freeForAll      = false;
        teamDeathMatch  = false;
        captureTheFlag  = false;
        battleRoyale    = true;
    }
	#endregion
}
// This code is written by Peter Thompson
#endregion