using UnityEngine;
using UnityEngine.AI;

public class AIControl : MonoBehaviour
{
    private Zombie zombie;
    private NavMeshAgent agent;

    [SerializeField] Transform[ ] Waypoint;

    private int count;

    private Animator animator;
    private Transform target;

    void Start()
    {
        zombie = GetComponent<Zombie>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        InvokeRepeating(nameof(MoveNext), 0, 2);
    }

    public void NewTarget(Transform p_target)
    {
        CancelInvoke(nameof(MoveNext));
        target = p_target;
    }

    public void ResetTarget()
    {
        target = null;
        InvokeRepeating(nameof(MoveNext), 0, 2);
    }

    public void MoveNext()
    {
        if (target == null)
        {
            if (agent.velocity == Vector3.zero)
            {
                agent.SetDestination(Waypoint[count++].position);

                if (count >= Waypoint.Length)
                {
                    count = 0;
                }
            }
        }
    }

    void Update()
    {
        if (zombie.health <= 0)
        {
            CancelInvoke(nameof(MoveNext));
        }
        else
        {
            if (target != null)
            {
                agent.SetDestination(target.position);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            NewTarget(other.transform);
        }    
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Character"))
        {
            ResetTarget();
        }
    }
}
