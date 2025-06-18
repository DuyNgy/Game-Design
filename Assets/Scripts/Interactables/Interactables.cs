using UnityEngine;
using Project.Helper;
using Project.Inventory;
using Project.Player;

namespace Project.Interactable
{
    /// <summary>
    /// Base class for all interactable objects in the game.
    /// </summary>
    public abstract class Interactables : MonoBehaviour
    {
        private MouseRaycast mouseRaycast;
        private SpriteOutline spriteOutline;
        private bool isHighlighted = false;
        private bool waitingForPlayerToGetClose = false;
        private GameObject hoveredObject;

        protected virtual void Start()
        {
            spriteOutline = gameObject.AddComponent<SpriteOutline>();
            mouseRaycast = GameObject.FindGameObjectWithTag("Mouse").GetComponent<MouseRaycast>();
        }

        protected virtual void Update()
        {
            hoveredObject = mouseRaycast.GetGameObject();
            bool shouldHighlight = false;

            if (hoveredObject != null)
            {
                // Check if the hovered object is this interactable
                shouldHighlight = hoveredObject == gameObject;
            }

            // Only update highlight if the state needs to change
            if (shouldHighlight != isHighlighted)
            {
                HandleHoverEffect(shouldHighlight);
            }

            // Check if we're waiting for player to get close
            if (waitingForPlayerToGetClose)
            {
                PlayerMovement player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
                if (player != null)
                {
                    float distance = Vector3.Distance(transform.position, player.transform.position);
                    if (distance <= player.GetMinDistanceToInteractable())
                    {
                        // Stop waiting for player to get close and start interaction
                        waitingForPlayerToGetClose = false;
                        Interact();
                        player.ResetMinDistanceToInteractable();
                    }
                }
            }
            // Only start interaction process when clicked
            else if (Input.GetMouseButtonDown(0) && shouldHighlight)
            {
                waitingForPlayerToGetClose = true;
            }
        }

        /// <summary>
        /// Handles the hover effect by showing or hiding the outline.
        /// </summary>
        private void HandleHoverEffect(bool shouldHighlight)
        {
            if (spriteOutline != null)
            {
                spriteOutline.ShowOutline(shouldHighlight);
                isHighlighted = shouldHighlight;
            }

            if (shouldHighlight)
            {
                // Check tags only if we're actually hovering this object
                switch (hoveredObject.tag)
                {
                    case "Door":
                        CursorManager.Instance.SetDoorCursor();
                        break;
                    case "NPC":
                        CursorManager.Instance.SetDialogueCursor();
                        break;
                    case "Item":
                        CursorManager.Instance.SetGrabCursor();
                        break;
                    case "Put":
                        CursorManager.Instance.SetPutCursor();
                        break;
                    default:
                        CursorManager.Instance.SetNormalCursor();
                        break;
                }
            }
            else
            {
                CursorManager.Instance.SetNormalCursor();
            }

            hoveredObject = null;
        }

        /// <summary>
        /// Allows forcing an interaction immediately, bypassing proximity logic.
        /// </summary>
        public void ForceInteract()
        {
            waitingForPlayerToGetClose = false;
            Interact();
        }

        protected abstract void Interact();
    }
}