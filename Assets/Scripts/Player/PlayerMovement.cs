using UnityEngine;
using Project.Helper;
using Project.Interactable;
using Unity.VisualScripting;
using UnityEngine.EventSystems;

namespace Project.Player
{
    /// <summary>
    /// Class responsible for handling the player movement.
    /// </summary>
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private MouseRaycast mouseRaycast;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] private float valueAboveGround = 0f;
        [SerializeField] private float minDistanceToInteractable = 5f;

        private bool isMoving = false;
        private Vector2 targetPosition;
        private GameObject currentInteractable;
        private Rigidbody2D rb;
        public bool ignoreGroundCheck = false;


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.bodyType = RigidbodyType2D.Dynamic;
            rb.gravityScale = 0;
            rb.constraints = RigidbodyConstraints2D.FreezeRotation;
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        // Update is called once per frame
        void Update()
        {
            HandleMovement();
            CheckSpriteLayer();
        }

        void OnTriggerExit2D(Collider2D other)
        {
            if (other.CompareTag("Ground") && !ignoreGroundCheck)
            {
                minDistanceToInteractable = 100f;
                if (isMoving)
                {
                    isMoving = false;
                }
            }
        }


        /// <summary>
        /// Handles the movement of the player if we are clicking on "Ground".
        /// It will continously move the player towards the click position until the player reaches it.
        /// Unless the player clicks on an interactable object, in which case it will stop moving once close enough.
        /// </summary>
        private void HandleMovement()
        {
            GameObject gameObjectHit = mouseRaycast.GetGameObject();
            bool isGround = gameObjectHit != null && gameObjectHit.CompareTag("Ground");

            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("GameObject hit: " + gameObjectHit?.name);
                if (isGround && !EventSystem.current.IsPointerOverGameObject())
                {
                    Debug.Log("Clicked on ground: " + gameObjectHit.name);
                    targetPosition = mouseRaycast.GetMousePosition();
                    currentInteractable = null;
                    isMoving = true;
                    ignoreGroundCheck = false;
                }
                else if (gameObjectHit != null && gameObjectHit.GetComponent<Interactables>() != null)
                {
                    Interactables interactable = gameObjectHit.GetComponent<Interactables>();

                    targetPosition = (Vector2)gameObjectHit.transform.position;
                    currentInteractable = gameObjectHit;

                    if (IsNearGround(gameObjectHit.transform))
                    {
                        isMoving = true;
                    }
                    else if (!interactable.CompareTag("Item"))
                    {
                        isMoving = true;
                    }
                    else
                    {
                        Debug.Log("Interactable too far from ground, skipping movement.");
                        interactable.ForceInteract();
                    }
                }
            }

            if (isMoving)
            {
                Vector2 currentPosition = transform.position;
                transform.position = Vector2.MoveTowards(currentPosition, targetPosition, moveSpeed * Time.deltaTime);
                // Check if we should stop based on the type of target
                if (currentInteractable != null)
                {
                    float distanceToTarget = Vector2.Distance(transform.position, targetPosition);
                    if (distanceToTarget <= minDistanceToInteractable)
                    {
                        isMoving = false;
                    }
                }
                else if ((Vector2)transform.position == targetPosition)
                {
                    isMoving = false;
                    Debug.Log("Reached target position: " + targetPosition);
                    if (ignoreGroundCheck && currentInteractable == null)
                    {
                        ignoreGroundCheck = false;
                    }
                }
            }
        }

        /// <summary>
        /// Checks the Y position of the player and sets the sorting layer accordingly.
        /// This is used to ensure that the player sprite is rendered above or below other sprites.
        /// </summary>
        private void CheckSpriteLayer()
        {
            if (spriteRenderer != null)
            {
                // Set the sorting layer based on the Y position of the player
                if (transform.position.y < valueAboveGround)
                {
                    spriteRenderer.sortingLayerID = SortingLayer.NameToID("PlayerAbove");
                }
                else
                {
                    spriteRenderer.sortingLayerID = SortingLayer.NameToID("PlayerBelow");
                }
            }
        }

        /// <summary>
        /// Returns the min distance the player needs to be to the interactable object.
        /// </summary>
        public float GetMinDistanceToInteractable()
        {
            return minDistanceToInteractable;
        }

        /// <summary>
        /// Returns the min distance the player needs to be to the interactable object.
        /// </summary>
        public void ResetMinDistanceToInteractable()
        {
            minDistanceToInteractable = 5;
        }

        private bool IsNearGround(Transform target)
        {
            Collider2D[] nearbyGround = Physics2D.OverlapCircleAll(target.position, 5f);

            foreach (Collider2D col in nearbyGround)
            {
                if (col.CompareTag("Ground"))
                {
                    return true;
                }
            }

            return false;
        }

        public Vector2 GetMoveDirection()
        {
            if (isMoving)
                return (targetPosition - (Vector2)transform.position).normalized;
            else
                return Vector2.zero;
        }

        public void MovePlayerTo(Vector2 position)
        {
            ignoreGroundCheck = true; // Ignore ground check when moving to a new position
            targetPosition = position;
            Debug.Log($"Moving player to position: {targetPosition}");
            isMoving = true;
            currentInteractable = null; // Reset current interactable when moving to a new position
        }
    }
}
