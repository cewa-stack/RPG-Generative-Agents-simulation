using UnityEngine;

public class AgentMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 3f;
    private Vector2 targetPosition;
    private Rigidbody2D rb;
    private Animator animator;
    private bool isMoving = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        targetPosition = transform.position;
    }

    public void SetDestination(Vector2 newTarget)
    {
        targetPosition = newTarget;
        isMoving = true;
    }

    private void FixedUpdate()
    {
        if (isMoving)
        {
            Vector2 currentPos = rb.position;
            float distance = Vector2.Distance(currentPos, targetPosition);

            if (distance > 0.1f)
            {
                Vector2 direction = (targetPosition - currentPos).normalized;
                rb.MovePosition(currentPos + direction * moveSpeed * Time.fixedDeltaTime);
                UpdateAnimation(direction, true);
            }
            else
            {
                isMoving = false;
                UpdateAnimation(Vector2.zero, false);
            }
        }
    }

    private void UpdateAnimation(Vector2 dir, bool walking)
    {
        if (animator == null) return;
        
        animator.SetBool("isWalking", walking);
        
        if (walking)
        {
            animator.SetFloat("Horizontal", dir.x);
            animator.SetFloat("Vertical", dir.y);
        }
    }
}