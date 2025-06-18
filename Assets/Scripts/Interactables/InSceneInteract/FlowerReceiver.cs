using Project.Helper;
using Project.Inventory;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    public class FlowerReceiver : ItemReceiver
    {
        [SerializeField] private GameObject cutFlower;
        public override bool TryUseItem(ItemData draggedItem)
        {
            // Check for a valid combination
            if (draggedItem.CanCombine(itemRepresentation.itemID))
            {
                ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                // CUSTOM LOGIC -----
                if (spriteRenderer != null && draggedItem.itemID == 59)
                {
                    itemRepresentation = result;

                    cutFlower.SetActive(true);
                    gameObject.SetActive(false);
                    CursorManager.Instance.SetNormalCursor();

                    return true;
                }
                // CUSTOM LOGIC ----
            }

            Debug.Log("Can't use this item on the flower.");
            return false;
        }
    }
}
