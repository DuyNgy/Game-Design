using Project.Dialogue.Data;
using Project.Interactable.NPCs;
using Project.Inventory;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    public class SnailReceiver : ItemReceiver
    {
        [SerializeField] private NPC snailNPC;
        [SerializeField] private DialogueData newSnailDialogue;
        public override bool TryUseItem(ItemData draggedItem)
        {
            // Check for a valid combination
            if (draggedItem.CanCombine(itemRepresentation.itemID))
            {
                ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                // CUSTOM LOGIC -----
                if (spriteRenderer != null && draggedItem.itemID == 61)
                {
                    InventoryManager.Instance.RemoveItem(draggedItem);
                    snailNPC.SetDialogueData(newSnailDialogue);
                    spriteRenderer.sprite = result.icon;
                    return true;
                }
                // CUSTOM LOGIC ----
            }

            Debug.Log("Can't use this item on the Snail.");
            return false;
        }
    }
}
