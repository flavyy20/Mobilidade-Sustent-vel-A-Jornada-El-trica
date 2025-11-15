using UnityEngine;

public class SimpleCarAI : MonoBehaviour
{
    public Transform[] waypoints;
    public float speed = 5f;
    public float rotateSpeed = 3f;
    public float stopDistance = 1f;

    private int currentIndex = 0;

    void Update()
    {
        if (waypoints.Length == 0) return;

        Transform target = waypoints[currentIndex];

        // Rotação em direção ao próximo waypoint
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, rotateSpeed * Time.deltaTime);

        // Movimento
        float dist = Vector3.Distance(transform.position, target.position);

        if (dist > stopDistance)
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }
        else
        {
            // vai para o próximo ponto
            currentIndex = (currentIndex + 1) % waypoints.Length;
        }
    }
}
