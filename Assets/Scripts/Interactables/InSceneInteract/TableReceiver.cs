using System.Collections;
using Project.Dialogue.Data;
using Project.Interactable.NPCs;
using Project.Inventory;
using Project.Player;
using Unity.VisualScripting;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    public class TableReceiver : ItemReceiver
    {
        [SerializeField] private GameObject[] honeyObjects;
        private int honeyCount;

        // Dialogue related
        [SerializeField] private NPC ant;
        [SerializeField] private DialogueData antAllHoneyDialogue;
        [SerializeField] private GameObject player;
        [SerializeField] private DialogueData antMirrorHoneyDialogue;

        public override bool TryUseItem(ItemData draggedItem)
        {
            // Check for a valid combination
            if (draggedItem.CanCombine(itemRepresentation.itemID))
            {
                ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                // CUSTOM LOGIC -----
                if (spriteRenderer != null && (draggedItem.itemID == 60 || draggedItem.itemID == 63 || draggedItem.itemID == 68))
                {
                    honeyCount++;
                    honeyObjects[honeyCount - 1].SetActive(true);
                    InventoryManager.Instance.RemoveItem(draggedItem);

                    if (honeyCount >= 3)
                    {
                        ant.SetDialogueData(antAllHoneyDialogue);
                    }
                    return true;
                }

                if (spriteRenderer != null && draggedItem.itemID == 56 && honeyCount >= 3)
                {
                    Vector3 mirrorPosition = new Vector2(-1.7f, -4.5f);
                    player.GetComponent<PlayerMovement>().MovePlayerTo(mirrorPosition);
                    if (player.transform.position != mirrorPosition)
                    {
                        StartCoroutine(wait());
                    }

                    ant.SetDialogueData(antMirrorHoneyDialogue);
                    // TODO: stop animation when dialogue ends
                }
                // CUSTOM LOGIC ----
            }

            Debug.Log("Can't use this item on the table.");
            return false;
        }

        public IEnumerator wait()
        {
            yield return new WaitForSeconds(1f);
            player.GetComponent<SpriteRenderer>().flipX = false;
            player.GetComponent<Animator>().SetBool("Mirroring", true);
        }
    }
}
