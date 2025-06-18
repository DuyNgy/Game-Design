using UnityEngine;
using Project.Dialogue.Data;
using System.Collections.Generic;
using Project.Interactable.NPCs;
using Project.Inventory;
using UnityEngine.SceneManagement;

namespace Project.Dialogue
{
    /// <summary>
    /// Class to manage the dialogue system, including starting dialogues, making choices, and checking conditions.
    /// </summary>
    public class DialogueManager : MonoBehaviour
    {
        private static DialogueManager instance;
        public static DialogueManager Instance => instance;
        public NPC CurrentSpeakingNPC => currentSpeakingNPC;

        [SerializeField] private DialogueUI dialogueUI;
        [SerializeField] private InventoryUI inventoryUI;
        private DialogueData currentDialogueData;
        private DialogueLine currentDialogue;
        private GameObject currentDialogueUIObject;
        private NPC currentSpeakingNPC;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else
                Destroy(gameObject);
        }

        public void StartDialogue(DialogueData dialogueData, string dialogueID, NPC speaker)
        {
            if (currentDialogueUIObject != null)
            {
                Destroy(currentDialogueUIObject);
            }

            if (dialogueData == null || dialogueID == "") return;
            currentSpeakingNPC = speaker;

            currentSpeakingNPC.PlayTalkingAnimation();

            var dialogeUIObject = Instantiate(dialogueUI);
            currentDialogueUIObject = dialogeUIObject.gameObject;
            currentDialogueData = dialogueData;
            var dialogueLine = dialogueData.GetDialogueLine(dialogueID);

            if (dialogueLine != null)
            {
                currentDialogue = dialogueLine;
                dialogeUIObject.DisplayDialogue(dialogueLine);
            }

            if (currentDialogue.Rewards != null && currentDialogue.Rewards.Count > 0)
            {
                GiveRewards(currentDialogue.Rewards);
            }
        }

        public void EndDialogue()
        {
            if (currentSpeakingNPC != null)
            {
                currentSpeakingNPC.StopTalkingAnimation();
            }

            if (currentDialogueUIObject != null)
            {
                Destroy(currentDialogueUIObject);
                currentDialogueUIObject = null;
            }
            currentSpeakingNPC = null;
            currentDialogueData = null;
            currentDialogue = null;
        }

        public void MakeChoice(int choiceIndex)
        {
            choiceIndex = Mathf.Clamp(choiceIndex, 0, currentDialogue.Choices.Count - 1);
            if (currentDialogue == null) return;
            var choice = currentDialogue.Choices[choiceIndex];

            if (choice.shouldBeRemoved)
            {
                choice.isRemoved = true; // Mark the choice as used
            }

            if (CheckConditions(choice.Conditions))
            {
                StartDialogue(currentDialogueData, choice.NextDialogueID, currentSpeakingNPC);
            }
        }

        private bool CheckConditions(List<DialogueCondition> conditions)
        {
            if (conditions == null) return true;

            foreach (var condition in conditions)
            {
                switch (condition.Type)
                {
                    case ConditionType.HasItem:
                        break;
                }
            }
            return true;
        }

        private void GiveRewards(List<DialogueReward> rewards)
        {
            if (rewards == null) return;

            foreach (var reward in rewards)
            {
                if (reward.isGiven) continue; // Skip if the reward has already been given
                switch (reward.Type)
                {
                    case RewardType.Item:
                        Debug.Log($"Giving rewards {reward.Item.itemName} for dialogue {currentDialogue.DialogueID}");
                        for (int i = 0; i < reward.Amount; i++)
                        {
                            InventoryManager.Instance.AddItem(reward.Item);
                            inventoryUI.UpdateInventoryUI();
                            reward.isGiven = true; // Mark the reward as given
                        }
                        break;
                    case RewardType.SceneToLoad:
                        SceneManager.LoadScene(reward.SceneToLoad);
                        break;
                }
            }
        }
    }
}