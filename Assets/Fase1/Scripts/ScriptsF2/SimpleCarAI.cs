using UnityEngine;

public class SimpleCarAI : MonoBehaviour
{
    [Header("Waypoints")]
    public Transform[] waypoints;

    [Header("Velocidade")]
    public float minSpeed = 4f;
    public float maxSpeed = 7f;
    public float rotateSpeed = 4f;
    public float stopDistance = 1f;

    [Header("Detecção")]
    public float safeDistance = 6f;        // detectar carro na frente
    public float tooCloseDistance = 3f;    // detectar carro atrás muito perto
    public LayerMask carLayer;

    [Header("Boost")]
    public float frontBoostMultiplier = 3f;   // **3x a velocidade base**
    public float frontBoostDuration = 2.5f;   // dura bem mais
    public float rearStopTime = 2f;           // o de trás para

    float baseSpeed;
    float currentSpeed;
    float boostTimer = 0f;           // boost do carro da frente
    float rearStopTimer = 0f;        // parada do carro de trás

    int currentWP = 0;

    void Start()
    {
        baseSpeed = Random.Range(minSpeed, maxSpeed);
        currentSpeed = baseSpeed;
        currentWP = GetClosestWaypointIndex();
    }

    int GetClosestWaypointIndex()
    {
        int closest = 0;
        float dist = Mathf.Infinity;

        for (int i = 0; i < waypoints.Length; i++)
        {
            float d = Vector3.Distance(transform.position, waypoints[i].position);
            if (d < dist)
            {
                dist = d;
                closest = i;
            }
        }

        return (closest + 1) % waypoints.Length;
    }

    void Update()
    {
        DetectCars();
        ApplyMovement();
    }

    void DetectCars()
    {
        // 1 — Checar carro na frente
        if (Physics.Raycast(transform.position + transform.up, transform.forward,
                             out RaycastHit frontHit, safeDistance, carLayer))
        {
            // desacelera suavemente
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed * 0.3f, Time.deltaTime * 3f);
        }
        else
        {
            // volta ao normal quando livre
            currentSpeed = Mathf.Lerp(currentSpeed, baseSpeed, Time.deltaTime * 1.5f);
        }


        // 2 — Checar carro atrás (trigger do boost)
        if (Physics.Raycast(transform.position + transform.up, -transform.forward,
                             out RaycastHit backHit, tooCloseDistance, carLayer))
        {
            // SOMENTE quem está NA FRENTE ganha boost
            SimpleCarAI backAI = backHit.collider.GetComponent<SimpleCarAI>();

            if (backAI != null)
            {
                // o carro de trás PARA
                backAI.rearStopTimer = rearStopTime;

                // o carro da frente ganha boost forte
                boostTimer = frontBoostDuration;
            }
        }
    }

    void ApplyMovement()
    {
        // efeito de parada do carro de trás
        if (rearStopTimer > 0)
        {
            rearStopTimer -= Time.deltaTime;
            return; // não anda
        }

        // BOOST ATIVO = velocidade multiplicada
        if (boostTimer > 0)
        {
            boostTimer -= Time.deltaTime;
            currentSpeed = baseSpeed * frontBoostMultiplier;
        }

        MoveTowardsWaypoint();
    }

    void MoveTowardsWaypoint()
    {
        Transform wp = waypoints[currentWP];
        Vector3 dir = (wp.position - transform.position).normalized;

        // rotação suave
        Quaternion look = Quaternion.LookRotation(dir);
        transform.rotation = Quaternion.Slerp(transform.rotation, look, rotateSpeed * Time.deltaTime);

        // mover
        transform.position += transform.forward * currentSpeed * Time.deltaTime;

        // chegou no waypoint?
        if (Vector3.Distance(transform.position, wp.position) < stopDistance)
        {
            currentWP = (currentWP + 1) % waypoints.Length;
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position + transform.up,
            transform.position + transform.up + transform.forward * safeDistance);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(transform.position + transform.up,
            transform.position + transform.up - transform.forward * tooCloseDistance);
    }
}
