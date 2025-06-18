using Project.Helper;
using Project.Inventory;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    public class BridgeReceiver : ItemReceiver
    {
        // Dialogue related
        [SerializeField] private GameObject bridge;
        [SerializeField] private GameObject snail;

        public override bool TryUseItem(ItemData draggedItem)
        {
            // Check for a valid combination
            if (draggedItem.CanCombine(itemRepresentation.itemID))
            {
                ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                // CUSTOM LOGIC -----
                if (spriteRenderer != null && draggedItem.itemID == 69)
                {
                    bridge.SetActive(true);
                    snail.GetComponent<BoxCollider2D>().enabled = true;
                    InventoryManager.Instance.RemoveItem(draggedItem);
                    return true;
                }

                // CUSTOM LOGIC ----
            }

            Debug.Log("Can't use this item on the bridge.");
            CursorManager.Instance.SetPutCursor();
            return false;
        }
    }
}
