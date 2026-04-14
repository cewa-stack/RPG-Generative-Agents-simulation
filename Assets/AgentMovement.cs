using UnityEngine;
using UnityEngine.AI;

public class AgentMovement : MonoBehaviour
{
    private NavMeshAgent navAgent;
    private Animator animator;

    private void Awake()
    {
        navAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        
        navAgent.updateRotation = false;
        navAgent.updateUpAxis = false;
    }

    public void SetDestination(Vector2 target)
    {
        if (navAgent.isOnNavMesh)
        {
            navAgent.SetDestination(target);
        }
    }

    private void Update()
    {
        bool isMoving = navAgent.velocity.magnitude > 0.1f;
        animator.SetBool("isWalking", isMoving);
        
        if (isMoving)
        {
            animator.SetFloat("Horizontal", navAgent.velocity.x);
            animator.SetFloat("Vertical", navAgent.velocity.y);
        }
    }
}