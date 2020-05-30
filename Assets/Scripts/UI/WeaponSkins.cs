#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSkins : MonoBehaviour
{
    [SerializeField]
    private int skinID = 0;
    [SerializeField]
    private Text skinNameText = null;
    [SerializeField]
    private Shader rainbow = null;
    [SerializeField]
    private Shader defaultSkin = null;
    [SerializeField]
    private Renderer rend = null;

    // Start is called before the first frame update
    void Start()
    {
        skinID = 0;
        defaultSkin = Shader.Find("Standard");
        rainbow = Shader.Find("_Shaders/Rainbow");
    }

    // Update is called once per frame
    void Update()
    {
        if (skinID == 0)
        {
            skinNameText.text = "Default";
            rend.material.shader = defaultSkin;
        }
        if (skinID == 1)
        {
            skinNameText.text = "Rainbow";
            rend.material.shader = rainbow;
        }
    }

    public void ChangeSkinRight()
    {
        skinID++;
        if (skinID > 1)
        {
            skinID = 0;
        }
        if (skinID < 0)
        {
            skinID = 1;
        }
    }
    public void ChangeSkinLeft()
    {
        skinID--;
        if (skinID > 1)
        {
            skinID = 0;
        }
        if (skinID < 0)
        {
            skinID = 1;
        }
    }
}
// This code is written by Peter Thompson
#endregion