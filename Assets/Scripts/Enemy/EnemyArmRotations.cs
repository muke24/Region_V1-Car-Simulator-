using UnityEngine;

public class EnemyArmRotations : MonoBehaviour
{
    [SerializeField]
    private GameObject arms = null;
    [SerializeField]
    private GameObject lookCam = null;
    [SerializeField]
    private GameObject head = null;

    [SerializeField]
    private float y;

    // Update is called once per frame
    void Update()
    {
        y = 0f + (lookCam.transform.localRotation.x / 10 * 3);

        head.transform.rotation = lookCam.transform.localRotation;
        if (lookCam.transform.localRotation.x > 0)
        {
            arms.transform.localPosition = new Vector3(0.14f, y, -0.075f);
        }
    }
}
