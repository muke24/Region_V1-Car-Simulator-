﻿using UnityEngine;
using UnityEngine.SceneManagement;

public class ToGreyBoxLevel : MonoBehaviour
{
    public void ToGreyboxLevel()
	{
		SceneManager.LoadScene(3);
	}

	public void ToBattleRoyale()
	{
		SceneManager.LoadScene(4);
	}
}
