using UnityEngine;

public class WallBang : MonoBehaviour
{
    // The higher the value the less the damage is cut off
    [Range(0, 1)]
    public float damageCutOffMultiplier = 1f;
}