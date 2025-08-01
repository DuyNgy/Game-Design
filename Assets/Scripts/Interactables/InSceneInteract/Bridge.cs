using Project.Dialogue;
using Project.Helper;
using Project.Player;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    /// <summary>
    /// Represents a bridge interactable in the scene.
    /// </summary>
    public class Bridge : Interactables
    {
        [SerializeField] private GameObject player;
        private Vector2 targetPositionLand;
        private Vector2 targetPositionTree;

        protected override void Interact()
        {
            PlayerMovement playerMovement = player.GetComponent<PlayerMovement>();

            // Set ignoreGroundCheck to true and will be maintained until reaching destination
            playerMovement.ignoreGroundCheck = true;
            DialogueManager.Instance.EndDialogue(); // End any ongoing dialogue when picking up an item

            if (player.transform.position.y > 4)
            {
                targetPositionLand = new Vector2(-1.7f, -3.25f);
                Debug.Log("Moving player to land position: " + targetPositionLand);
                playerMovement.MovePlayerTo(targetPositionLand);
            }
            else
            {
                targetPositionTree = new Vector2(7.5f, 1.75f);
                Debug.Log("Moving player to tree position: " + targetPositionTree);
                playerMovement.MovePlayerTo(targetPositionTree);
            }
        }
    }
}