using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pause : MonoBehaviour
{
    public GameObject pauseGO;
    public Slider healthBarSlider;
    public Text healthTxt;

    public static GameObject pause;
    public static Slider healthBar;
    public static Text healthText;

    private void Awake()
    {
        pause = pauseGO;
        healthBar = healthBarSlider;
        healthText = healthTxt;
    }

    private void Update()
    {
        if (pauseGO.activeSelf && !Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;            
        }

        if (!pauseGO.activeSelf && Cursor.visible)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;            
        }
    }
}
