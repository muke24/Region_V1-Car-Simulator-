using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class MeshDeformation : MonoBehaviour
{
    public MeshFilter[] meshes;
    public Collider[] colliders;
    [Range(0.1f, 100.0f)] 
    public float damagePerHit = 0.5f;
    [Range(0.01f, 1000.0f)] 
    public float areaToDeform = 0.1f;
    [Range(0.1f, 10.0f)] 
    public float maximumDistance = 0.4f;

    private int numberOfImpacts = 0;
    private float nextTimeImpact = 0.0f;
    private Vector3 sumPositionImpacts = Vector3.zero, sumSpeedImpacts = Vector3.zero;
    private Vector3[][] meshesOriginals;
    private Rigidbody rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();

        meshes = new MeshFilter[1];
        colliders = new Collider[1];

        meshes[0] = GetComponent<MeshFilter>();
        colliders[0] = GetComponent<Collider>();
    }

    void OnEnable()
    {
        // Adjusts the size of the vector to the number of vertices
        meshesOriginals = new Vector3[meshes.Length][]; 
        for (int i = 0; i < meshes.Length; i++)
        {
            Mesh mesh = meshes[i].mesh;
            // Takes the original location of each vertex and puts it in the vector
            meshesOriginals[i] = mesh.vertices; 
            mesh.MarkDynamic();
        }
    }
    void OnDisable()
    {
        for (int i = 0; i < meshes.Length; i++)
        {
            Mesh mesh = meshes[i].mesh;

            // Relocates vertices in their original places
            mesh.vertices = meshesOriginals[i];

            // Recalculates the normals
            mesh.RecalculateNormals();

            // recalculates the bounds
            mesh.RecalculateBounds(); 
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (meshes.Length > 0 && colliders.Length > 0)
        {
            int impactCount = 0;
            Vector3 impactPosition = Vector3.zero, impactVelocity = Vector3.zero;
            foreach (ContactPoint contact in collision.contacts)
            {
                float dragRatio = Vector3.Dot(rigid.GetPointVelocity(contact.point), contact.normal);

                // If it has enough speed to be a collision
                if (dragRatio < -0.6f || collision.relativeVelocity.sqrMagnitude > 3.0f)
                {
                    // Increases the number of collisions that occur
                    impactCount++;
                    // Takes the places of impacts
                    impactPosition += contact.point;
                    // Takes impact speeds
                    impactVelocity += collision.relativeVelocity; 
                }
            }

            // If collision has more than 0 impacts
            if (impactCount > 0)
            {
                float invCount = 1.0f / impactCount;
                impactPosition *= invCount;
                impactVelocity *= invCount;
                numberOfImpacts++;
                sumPositionImpacts += transform.InverseTransformPoint(impactPosition);
                sumSpeedImpacts += transform.InverseTransformDirection(impactVelocity);
            }
        }
    }
    void FixedUpdate()
    {
        if (meshes.Length > 0 && colliders.Length > 0)
        {
            // If the time between impacts is greater or equal to 0.2 and the impact count is greater than 0
            if (Time.time - nextTimeImpact >= 0.2f && numberOfImpacts > 0)
            {
                float invCount = 1.0f / numberOfImpacts;
                sumPositionImpacts *= invCount;
                sumSpeedImpacts *= invCount;
                Vector3 impactVelocity = Vector3.zero;

                if (sumSpeedImpacts.sqrMagnitude > 1.5f)
                {
                    impactVelocity = transform.TransformDirection(sumSpeedImpacts) * 0.02f;
                }

                // If the normalized impact speed is greater than 0
                if (impactVelocity.sqrMagnitude > 0.0f)
                { 
                    Vector3 contactPoint = transform.TransformPoint(sumPositionImpacts);
                    for (int i = 0, c = meshes.Length; i < c; i++)
                    {
                        // Calls the method that deforms the meshes and sends the information
                        DeformMesh(meshes[i].mesh, meshesOriginals[i], meshes[i].transform, contactPoint, impactVelocity);
                    }
                }

                numberOfImpacts = 0;
                sumPositionImpacts = Vector3.zero;
                sumSpeedImpacts = Vector3.zero;
                nextTimeImpact = Time.time + 0.2f * Random.Range(-0.4f, 0.4f);
            }
        }
    }

    float DeformMesh(Mesh mesh, Vector3[] originalMesh, Transform localTransform, Vector3 contactPoint, Vector3 contactVelocity)
    {
        Vector3[] vertices = mesh.vertices;
        Vector3 localContactPoint = localTransform.InverseTransformPoint(contactPoint);
        Vector3 localContactForce = localTransform.InverseTransformDirection(contactVelocity);
        float sqrMaxDeform = maximumDistance * maximumDistance;
        float totalDamage = 0.0f;
        int damagedVertices = 0;
        for (int i = 0; i < vertices.Length; i++)
        {
            // Distance between the collision point and the mesh
            float dist = (localContactPoint - vertices[i]).sqrMagnitude; 
            if (dist < areaToDeform)
            {
                // Gets the damage
                Vector3 damage = (localContactForce * ((areaToDeform * 2.0f) - Mathf.Sqrt(dist)) / (areaToDeform * 2.0f)) * (damagePerHit * (1 + rigid.velocity.magnitude * 0.2f));
                // Applies the damage to the vertex
                vertices[i] += damage;
                // Subtracts the original position of the damage to reach the new position of the vertex
                Vector3 deform = vertices[i] - originalMesh[i]; 
                if (deform.sqrMagnitude > sqrMaxDeform)
                {
                    // Normalizes extreme mesh deforms
                    vertices[i] = originalMesh[i] + deform.normalized * maximumDistance;
                }
                totalDamage += damage.magnitude;
                damagedVertices++;
            }
        }

        // Apply the new vertices to the mesh
        mesh.vertices = vertices; 
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        return damagedVertices > 0 ? totalDamage / damagedVertices : 0.0f;
    }
}