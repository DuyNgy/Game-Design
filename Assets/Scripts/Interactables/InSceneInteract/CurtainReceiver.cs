using System.Collections;
using Project.Dialogue.Data;
using Project.Interactable.NPCs;
using Project.Inventory;
using UnityEngine;

namespace Project.Interactable.InSceneInteract
{
    public class CurtainReceiver : ItemReceiver
    {
        public GameObject fire;
        public GameObject guard;
        public GameObject omi_curtain;
        public DialogueData omi_panic_dialogueData;
        public DialogueData guard_panic_dialogueData;
        public GameObject omi;
        public GameObject villageDoor;
        public override bool TryUseItem(ItemData draggedItem)
        {
            if (PlayerPrefs.GetInt("isLit", 0) == 1)
            {
                Debug.Log("IT IS ALREADY BURNING!!!!");
            }
            else
            {
                // Check for a valid combination
                if (draggedItem.CanCombine(itemRepresentation.itemID))
                {
                    ItemData result = draggedItem.GetCombinationResult(itemRepresentation.itemID);
                    Debug.Log($"Combined {draggedItem.itemName} with {itemRepresentation.itemName} to get {result.itemName}");

                    // CUSTOM LOGIC -----
                    if (spriteRenderer != null && draggedItem.itemID == 56)
                    {
                        itemRepresentation = result;

                        fire.SetActive(true);
                        fire.GetComponent<Animator>().SetBool("isLit", true);
                        PlayerPrefs.SetInt("isLit", 1);
                        PlayerPrefs.Save();

                        StartCoroutine(wait());

                        Debug.Log("IT BUUUURNS!!!");
                        return true;
                    }
                    // CUSTOM LOGIC ----
                    Debug.Log("Can't use this item on the Curtain.");
                }
            }
            return false;
        }

        private IEnumerator wait()
        {
            yield return new WaitForSeconds(2f);
            omi_curtain.GetComponent<Animator>().SetBool("Panic", true);
            omi_curtain.GetComponent<NPC>().SetDialogueData(omi_panic_dialogueData);
            yield return new WaitForSeconds(1f);
            guard.GetComponent<NPCMovement>().WalkTo(new Vector2(-10, -3.5f));
            guard.GetComponent<NPC>().SetDialogueData(guard_panic_dialogueData);
            villageDoor.SetActive(true);
        }
    }
}