using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class MovimientoNpcPorSupermercado : MonoBehaviour
{
    [Header("Waypoints")]
    [SerializeField] private Transform[] waypoints;

    [Header("Movimiento")]

    [SerializeField] private float agentSpeed = 3.5f;

    [SerializeField] private float waitTime = 1f;
    [SerializeField] private float nextWaypointThreshold = 0.3f;
    [SerializeField] private bool loop = true;
    [SerializeField] private bool randomStart = false;

    [Header("Avoidance")]
    [SerializeField][Range(0, 99)] private int avoidancePriority = 50;

    private NavMeshAgent agent;
    private int currentIndex = 0;
    private bool waiting = false;
    private Coroutine waitCoroutine;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        if (agent == null)
        {
            Debug.LogWarning($"{nameof(MovimientoNpcPorSupermercado)} requiere un NavMeshAgent en el mismo GameObject.");
            enabled = false;
            return;
        }

        agent.speed = agentSpeed;
        agent.avoidancePriority = avoidancePriority;

        if (waypoints == null || waypoints.Length == 0)
        {
            Debug.LogWarning($"{nameof(MovimientoNpcPorSupermercado)}: no hay waypoints asignados.");
            enabled = false;
            return;
        }

        if (randomStart)
        {
            currentIndex = Random.Range(0, waypoints.Length);
        }
        else
        {
            currentIndex = 0;
        }

        agent.SetDestination(waypoints[currentIndex].position);
    }

    void Update()
    {
        if (agent == null || waypoints == null || waypoints.Length == 0) return;

        if (agent.pathPending) return;

        if (agent.pathStatus == NavMeshPathStatus.PathInvalid)
        {
            TryMoveToNext();
            return;
        }

        if (!waiting && !agent.pathPending && agent.remainingDistance <= Mathf.Max(agent.stoppingDistance, nextWaypointThreshold))
        {
            if (waitCoroutine != null) StopCoroutine(waitCoroutine);
            waitCoroutine = StartCoroutine(WaitAtWaypoint());
        }
    }

    private IEnumerator WaitAtWaypoint()
    {
        waiting = true;
        yield return new WaitForSeconds(waitTime);
        waiting = false;
        TryMoveToNext();
    }

    private void TryMoveToNext()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        int nextIndex = currentIndex + 1;
        if (nextIndex >= waypoints.Length)
        {
            if (loop)
            {
                nextIndex = 0;
            }
            else
            {
                return;
            }
        }

        currentIndex = nextIndex;
        agent.SetDestination(waypoints[currentIndex].position);
    }

    public void ForceNextWaypoint()
    {
        if (waitCoroutine != null) StopCoroutine(waitCoroutine);
        waiting = false;
        TryMoveToNext();
    }

    public void SetAgentSpeed(float newSpeed)
    {
        if (agent != null)
        {
            agent.speed = newSpeed;
        }
    }


    void OnDrawGizmos()
    {
        if (waypoints == null || waypoints.Length == 0) return;

        Gizmos.color = Color.green;
        for (int i = 0; i < waypoints.Length; i++)
        {
            if (waypoints[i] == null) continue;
            Gizmos.DrawWireSphere(waypoints[i].position, 0.25f);

            int next = (i + 1);
            if (loop)
            {
                next %= waypoints.Length;
            }

            if (next < waypoints.Length && waypoints[next] != null)
            {
                Gizmos.color = Color.cyan;
                Gizmos.DrawLine(waypoints[i].position, waypoints[next].position);
            }
        }
    }
}