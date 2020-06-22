using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PlayerFX : MonoBehaviour
{
    public MeshRenderer[] mesh;
    public PostProcessVolume postProcessVignette;
    public Vignette vignette = null;

    private void Awake()
    {
        mesh = FindObjectsOfType<MeshRenderer>();
        postProcessVignette.profile.TryGetSettings(out vignette);
    }

    // Start is called before the first frame update
    void Start()
    {
        foreach (MeshRenderer allMesh in mesh)
        {
            allMesh.probeAnchor = transform;
        }        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        vignette.intensity.value = (100 - GetComponent<Player>().curHealth) / 100 / 2;
    }
}
