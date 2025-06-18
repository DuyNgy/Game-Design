using Project.Player;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimatorController : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    private Animator animator;
    private Vector3 lastPosition;
    private float movementThreshold = 0.01f; // Avoid false movement triggers
    public PlayerMovement playerMovement;


    void Start()
    {
        animator = GetComponent<Animator>();
        lastPosition = transform.position;

        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 currentPosition = transform.position;
        Vector3 delta = currentPosition - lastPosition;

        float speed = delta.magnitude / Time.deltaTime;
        if (speed < movementThreshold)
            speed = 0f;

        animator.SetFloat("Speed", speed);

        // Flip based on movement direction instead of raw delta
        Vector2 moveDir = playerMovement.GetMoveDirection();
        bool isIntentionallyMoving = moveDir.sqrMagnitude > 0.5f;

        // Only stop mirroring if player is intentionally moving
        if (isIntentionallyMoving && animator.GetBool("Mirroring"))
        {
            animator.SetBool("Mirroring", false);
        }

        if (moveDir.x > 0.01f)
            spriteRenderer.flipX = false;
        else if (moveDir.x < -0.01f)
            spriteRenderer.flipX = true;

        lastPosition = currentPosition;
    }
}