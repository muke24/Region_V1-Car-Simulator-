#region This code is written by Peter Thompson
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float maxHealth = 100;
    public float curHealth = 100;

    public MeshRenderer[] mesh;

    public PostProcessVolume postProcessVignette;
    public Vignette vignette = null;
    
    public Slider healthBar;
    public Text healthText;

    public float whatIntesityShouldBe;
    public float healthRegenTimer = 10f;

    public bool wasShot = false;
    public bool healthIsRegening = false;

    private void Awake()
    {
        mesh = FindObjectsOfType<MeshRenderer>();
    }

    // Start is called before the first frame update
    void Start()
	{
        curHealth = maxHealth;

        postProcessVignette.profile.TryGetSettings(out vignette);

        foreach (MeshRenderer allMesh in mesh)
        {
            allMesh.probeAnchor = transform;            
        }

        healthBar = Pause.healthBar;
        healthText = Pause.healthText;
    }

    private void Update()
    {
        //whatIntesityShouldBe = (100 - curHealth) / 100 / 2;

        if (curHealth < 0f)
        {
            curHealth = 0f;
        }

        healthBar.value = curHealth;
        healthText.text = "HP: " + curHealth.ToString();

        vignette.intensity.value = (100 - curHealth) / 100 / 2;

        if (curHealth > 0)
        {
            if (wasShot)
            {
                healthRegenTimer -= Time.deltaTime;
                healthIsRegening = false;
            }
            if (wasShot && healthRegenTimer <= 0)
            {
                healthRegenTimer = 10f;
                wasShot = false;
                healthIsRegening = true;
            }

            if (healthIsRegening && curHealth < 100f)
            {
                curHealth += Time.deltaTime * 6f;
            }

            if (curHealth > 100f)
            {
                curHealth = 100f;
            }
            if (healthIsRegening && curHealth == 100)
            {
                healthIsRegening = false;
            }
        }        
    }
}
// This code is written by Peter Thompson
#endregion