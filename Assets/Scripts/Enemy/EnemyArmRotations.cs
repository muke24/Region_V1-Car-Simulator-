#region This code is written by Peter Thompson
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

    private GameObject player;
    private CurrentCar currentCar;

    public float speed;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        currentCar = GameObject.FindGameObjectWithTag("Manager").GetComponent<CurrentCar>();
    }

    // Update is called once per frame
    void Update()
    {
        y = 0f + (lookCam.transform.rotation.x / 10 * 3);

        head.transform.rotation = lookCam.transform.rotation;
        if (lookCam.transform.rotation.x > 0)
        {
            arms.transform.localPosition = new Vector3(0.14f, y, -0.075f);
        }
    }

    private void FixedUpdate()
    {
        if (!Car.inCar)
        {
            Quaternion originalRot = transform.rotation;
            lookCam.transform.LookAt(player.GetComponent<Rigidbody>().transform.position);
            Quaternion NewRot = transform.rotation;
            transform.rotation = originalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
        }

        if (Car.inCar)
        {
            Quaternion originalRot = transform.rotation;
            lookCam.transform.LookAt(currentCar.currentCar.transform.position);
            Quaternion NewRot = transform.rotation;
            transform.rotation = originalRot;
            transform.rotation = Quaternion.Lerp(transform.rotation, NewRot, speed * Time.deltaTime);
        }
    }
}
// This code is written by Peter Thompson
#endregion