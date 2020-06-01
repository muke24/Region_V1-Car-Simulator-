#region This code is written by Peter Thompson
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public string[] qualityNames;

    public GameObject pausePanel;

    public bool low;
    public bool lowMedium;
    public bool medium;
    public bool highMedium;
    public bool high;

    public Button lowButton;
    public Button lowMedButton;
    public Button medButton;
    public Button highMedButton;
    public Button highButton;

    // Start is called before the first frame update
    void Start()
    {
        qualityNames = QualitySettings.names;

        if (QualitySettings.GetQualityLevel() == 0)
        {
			#region Change button colour to selected graphics
			ColorBlock colourLow = lowButton.colors;
            colourLow.normalColor = Color.yellow;
            colourLow.selectedColor = Color.yellow;
            lowButton.colors = colourLow;

            ColorBlock colourLowMed = lowMedButton.colors;
            colourLowMed.normalColor = Color.white;
            colourLowMed.selectedColor = Color.white;
            lowMedButton.colors = colourLowMed;

            ColorBlock colourMed = medButton.colors;
            colourMed.normalColor = Color.white;
            colourMed.selectedColor = Color.white;
            medButton.colors = colourMed;

            ColorBlock colourHighMed = highMedButton.colors;
            colourHighMed.normalColor = Color.white;
            colourHighMed.selectedColor = Color.white;
            highMedButton.colors = colourHighMed;

            ColorBlock colourHigh = highButton.colors;
            colourHigh.normalColor = Color.white;
            colourHigh.selectedColor = Color.white;
            highButton.colors = colourHigh;
			#endregion

			low = true;
            lowMedium = false;
            medium = false;
            highMedium = false;
            high = false;
        }

        if (QualitySettings.GetQualityLevel() == 1)
        {
            #region Change button colour to selected graphics
            ColorBlock colourLow = lowButton.colors;
            colourLow.normalColor = Color.white;
            colourLow.selectedColor = Color.white;
            lowButton.colors = colourLow;

            ColorBlock colourLowMed = lowMedButton.colors;
            colourLowMed.normalColor = Color.yellow;
            colourLowMed.selectedColor = Color.yellow;
            lowMedButton.colors = colourLowMed;

            ColorBlock colourMed = medButton.colors;
            colourMed.normalColor = Color.white;
            colourMed.selectedColor = Color.white;
            medButton.colors = colourMed;

            ColorBlock colourHighMed = highMedButton.colors;
            colourHighMed.normalColor = Color.white;
            colourHighMed.selectedColor = Color.white;
            highMedButton.colors = colourHighMed;

            ColorBlock colourHigh = highButton.colors;
            colourHigh.normalColor = Color.white;
            colourHigh.selectedColor = Color.white;
            highButton.colors = colourHigh;
            #endregion

            low = false;
            lowMedium = true;
            medium = false;
            highMedium = false;
            high = false;
        }

        if (QualitySettings.GetQualityLevel() == 2)
        {
            #region Change button colour to selected graphics
            ColorBlock colourLow = lowButton.colors;
            colourLow.normalColor = Color.white;
            colourLow.selectedColor = Color.white;
            lowButton.colors = colourLow;

            ColorBlock colourLowMed = lowMedButton.colors;
            colourLowMed.normalColor = Color.white;
            colourLowMed.selectedColor = Color.white;
            lowMedButton.colors = colourLowMed;

            ColorBlock colourMed = medButton.colors;
            colourMed.normalColor = Color.yellow;
            colourMed.selectedColor = Color.yellow;
            medButton.colors = colourMed;

            ColorBlock colourHighMed = highMedButton.colors;
            colourHighMed.normalColor = Color.white;
            colourHighMed.selectedColor = Color.white;
            highMedButton.colors = colourHighMed;

            ColorBlock colourHigh = highButton.colors;
            colourHigh.normalColor = Color.white;
            colourHigh.selectedColor = Color.white;
            highButton.colors = colourHigh;
            #endregion

            low = false;
            lowMedium = false;
            medium = true;
            highMedium = false;
            high = false;
        }

        if (QualitySettings.GetQualityLevel() == 3)
        {
            #region Change button colour to selected graphics
            ColorBlock colourLow = lowButton.colors;
            colourLow.normalColor = Color.white;
            colourLow.selectedColor = Color.white;
            lowButton.colors = colourLow;

            ColorBlock colourLowMed = lowMedButton.colors;
            colourLowMed.normalColor = Color.white;
            colourLowMed.selectedColor = Color.white;
            lowMedButton.colors = colourLowMed;

            ColorBlock colourMed = medButton.colors;
            colourMed.normalColor = Color.white;
            colourMed.selectedColor = Color.white;
            medButton.colors = colourMed;

            ColorBlock colourHighMed = highMedButton.colors;
            colourHighMed.normalColor = Color.yellow;
            colourHighMed.selectedColor = Color.yellow;
            highMedButton.colors = colourHighMed;

            ColorBlock colourHigh = highButton.colors;
            colourHigh.normalColor = Color.white;
            colourHigh.selectedColor = Color.white;
            highButton.colors = colourHigh;
            #endregion

            low = false;
            lowMedium = false;
            medium = false;
            highMedium = true;
            high = false;
        }

        if (QualitySettings.GetQualityLevel() == 4)
        {
            #region Change button colour to selected graphics
            ColorBlock colourLow = lowButton.colors;
            colourLow.normalColor = Color.white;
            colourLow.selectedColor = Color.white;
            lowButton.colors = colourLow;

            ColorBlock colourLowMed = lowMedButton.colors;
            colourLowMed.normalColor = Color.white;
            colourLowMed.selectedColor = Color.white;
            lowMedButton.colors = colourLowMed;

            ColorBlock colourMed = medButton.colors;
            colourMed.normalColor = Color.white;
            colourMed.selectedColor = Color.white;
            medButton.colors = colourMed;

            ColorBlock colourHighMed = highMedButton.colors;
            colourHighMed.normalColor = Color.white;
            colourHighMed.selectedColor = Color.white;
            highMedButton.colors = colourHighMed;

            ColorBlock colourHigh = highButton.colors;
            colourHigh.normalColor = Color.yellow;
            colourHigh.selectedColor = Color.yellow;
            highButton.colors = colourHigh;
            #endregion

            low = false;
            lowMedium = false;
            medium = false;
            highMedium = false;
            high = true;
        }
    }

    private void Update()
    {
        if (pausePanel.activeSelf)
        {
            pausePanel.GetComponent<RectTransform>().sizeDelta = new Vector2(Screen.width * 20f, Screen.height * 20f);            
        }
    }

    public void LowQuality()            // Quality level 0
    {
        #region Change button colour to selected graphics
        ColorBlock colourLow = lowButton.colors;
        colourLow.normalColor = Color.yellow;
        colourLow.selectedColor = Color.yellow;
        lowButton.colors = colourLow;

        ColorBlock colourLowMed = lowMedButton.colors;
        colourLowMed.normalColor = Color.white;
        colourLowMed.selectedColor = Color.white;
        lowMedButton.colors = colourLowMed;

        ColorBlock colourMed = medButton.colors;
        colourMed.normalColor = Color.white;
        colourMed.selectedColor = Color.white;
        medButton.colors = colourMed;

        ColorBlock colourHighMed = highMedButton.colors;
        colourHighMed.normalColor = Color.white;
        colourHighMed.selectedColor = Color.white;
        highMedButton.colors = colourHighMed;

        ColorBlock colourHigh = highButton.colors;
        colourHigh.normalColor = Color.white;
        colourHigh.selectedColor = Color.white;
        highButton.colors = colourHigh;
        #endregion

        low = true;
        lowMedium = false;
        medium = false;
        highMedium = false;
        high = false;
        QualitySettings.SetQualityLevel(0);
    }

    public void LowMediumQuality()      // Quality level 1
    {
        #region Change button colour to selected graphics
        ColorBlock colourLow = lowButton.colors;
        colourLow.normalColor = Color.white;
        colourLow.selectedColor = Color.white;
        lowButton.colors = colourLow;

        ColorBlock colourLowMed = lowMedButton.colors;
        colourLowMed.normalColor = Color.yellow;
        colourLowMed.selectedColor = Color.yellow;
        lowMedButton.colors = colourLowMed;

        ColorBlock colourMed = medButton.colors;
        colourMed.normalColor = Color.white;
        colourMed.selectedColor = Color.white;
        medButton.colors = colourMed;

        ColorBlock colourHighMed = highMedButton.colors;
        colourHighMed.normalColor = Color.white;
        colourHighMed.selectedColor = Color.white;
        highMedButton.colors = colourHighMed;

        ColorBlock colourHigh = highButton.colors;
        colourHigh.normalColor = Color.white;
        colourHigh.selectedColor = Color.white;
        highButton.colors = colourHigh;
        #endregion

        low = false;
        lowMedium = true;
        medium = false;
        highMedium = false;
        high = false;
        QualitySettings.SetQualityLevel(1);
    }

    public void MediumQuality()         // Quality level 2
    {
        #region Change button colour to selected graphics
        ColorBlock colourLow = lowButton.colors;
        colourLow.normalColor = Color.white;
        colourLow.selectedColor = Color.white;
        lowButton.colors = colourLow;

        ColorBlock colourLowMed = lowMedButton.colors;
        colourLowMed.normalColor = Color.white;
        colourLowMed.selectedColor = Color.white;
        lowMedButton.colors = colourLowMed;

        ColorBlock colourMed = medButton.colors;
        colourMed.normalColor = Color.yellow;
        colourMed.selectedColor = Color.yellow;
        medButton.colors = colourMed;

        ColorBlock colourHighMed = highMedButton.colors;
        colourHighMed.normalColor = Color.white;
        colourHighMed.selectedColor = Color.white;
        highMedButton.colors = colourHighMed;

        ColorBlock colourHigh = highButton.colors;
        colourHigh.normalColor = Color.white;
        colourHigh.selectedColor = Color.white;
        highButton.colors = colourHigh;
        #endregion

        low = false;
        lowMedium = false;
        medium = true;
        highMedium = false;
        high = false;
        QualitySettings.SetQualityLevel(2);
    }

    public void HighMediumQuality()     // Quality level 3
    {
        #region Change button colour to selected graphics
        ColorBlock colourLow = lowButton.colors;
        colourLow.normalColor = Color.white;
        colourLow.selectedColor = Color.white;
        lowButton.colors = colourLow;

        ColorBlock colourLowMed = lowMedButton.colors;
        colourLowMed.normalColor = Color.white;
        colourLowMed.selectedColor = Color.white;
        lowMedButton.colors = colourLowMed;

        ColorBlock colourMed = medButton.colors;
        colourMed.normalColor = Color.white;
        colourMed.selectedColor = Color.white;
        medButton.colors = colourMed;

        ColorBlock colourHighMed = highMedButton.colors;
        colourHighMed.normalColor = Color.yellow;
        colourHighMed.selectedColor = Color.yellow;
        highMedButton.colors = colourHighMed;

        ColorBlock colourHigh = highButton.colors;
        colourHigh.normalColor = Color.white;
        colourHigh.selectedColor = Color.white;
        highButton.colors = colourHigh;
        #endregion

        low = false;
        lowMedium = false;
        medium = false;
        highMedium = true;
        high = false;
        QualitySettings.SetQualityLevel(3);
    }

    public void HighQuality()           // Quality level 4
    {
        #region Change button colour to selected graphics
        ColorBlock colourLow = lowButton.colors;
        colourLow.normalColor = Color.white;
        colourLow.selectedColor = Color.white;
        lowButton.colors = colourLow;

        ColorBlock colourLowMed = lowMedButton.colors;
        colourLowMed.normalColor = Color.white;
        colourLowMed.selectedColor = Color.white;
        lowMedButton.colors = colourLowMed;

        ColorBlock colourMed = medButton.colors;
        colourMed.normalColor = Color.white;
        colourMed.selectedColor = Color.white;
        medButton.colors = colourMed;

        ColorBlock colourHighMed = highMedButton.colors;
        colourHighMed.normalColor = Color.white;
        colourHighMed.selectedColor = Color.white;
        highMedButton.colors = colourHighMed;

        ColorBlock colourHigh = highButton.colors;
        colourHigh.normalColor = Color.yellow;
        colourHigh.selectedColor = Color.yellow;
        highButton.colors = colourHigh;
        #endregion

        low = false;
        lowMedium = false;
        medium = false;
        highMedium = false;
        high = true;
        QualitySettings.SetQualityLevel(4);
    }
}
// This code is written by Peter Thompson
#endregion