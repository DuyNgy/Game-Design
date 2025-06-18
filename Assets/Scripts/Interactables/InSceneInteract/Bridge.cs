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

            if (player.transform.position.y > 4)
            {
                targetPositionLand = new Vector2(-1.7f, 1.25f);
                Debug.Log("Moving player to land position: " + targetPositionLand);
                playerMovement.MovePlayerTo(targetPositionLand);
            }
            else
            {
                targetPositionTree = new Vector2(7.5f, 4.75f);
                Debug.Log("Moving player to tree position: " + targetPositionTree);
                playerMovement.MovePlayerTo(targetPositionTree);
            }
        }
    }
}