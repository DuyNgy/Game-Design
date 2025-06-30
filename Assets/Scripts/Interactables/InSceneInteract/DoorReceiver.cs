using Project.Inventory;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Project.Audio;
using UnityEngine.UI;

namespace Project.Interactable.InSceneInteract
{
    public class DoorReceiver : ItemReceiver
    {
        public bool isLocked = false;
        public string sceneToLoad;
        public GameObject opendoor;
        public GameObject bgdoor;
        
        public override bool TryUseItem(ItemData draggedItem)
        {
            // Check for a valid combination
            if (draggedItem.CanCombine(itemRepresentation.itemID) && isLocked == true)
            {
                ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                // CUSTOM LOGIC -----
                if (spriteRenderer != null && itemRepresentation.itemName == "Key")
                {
                    itemRepresentation = result;
                    spriteRenderer.sprite = itemRepresentation.icon;

                }

                return true;
                // CUSTOM LOGIC ----
            }

            Debug.Log("Can't use this item on the Door.");
            Object.FindFirstObjectByType<CustomAudioManager>().Play("wrong");
            return false;
        }

        protected override async void Interact()
        {
            if (isLocked == false)
            {
                gameObject.SetActive(false);
                bgdoor.SetActive(false);
                opendoor.SetActive(true);

                FindFirstObjectByType<CustomAudioManager>().Play("door");
                await Task.Delay(1500);
                Debug.Log("Changing Scene.");
                SceneManager.LoadScene(sceneToLoad);

                return;
            }
        }
    }
}