using UnityEngine;
using UnityEngine.UI;
using Project.Dialogue.Data;
using System.Collections.Generic;
using TMPro;
using Project.Inventory;
using Unity.VisualScripting;

namespace Project.Dialogue
{
    /// <summary>
    /// Class to manage the dialogue UI and display dialogue lines and choices.
    /// </summary>
    public class DialogueUI : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI dialogueText;
        [SerializeField] private Transform choicesContainer;

        public void DisplayDialogue(DialogueLine dialogue)
        {
            dialogueText.text = $"{dialogue.SpeakerName}: {dialogue.Text}";
            if (CheckConditions(dialogue.Conditions) == false)
            {
                Debug.Log("Dialogue conditions not met, skipping dialogue.");
                dialogueText.text = "...";
                return;
            }
            DisplayChoices(dialogue.Choices);
        }

        private void DisplayChoices(List<DialogueChoice> choices)
        {
            // Clear existing choices
            foreach (Transform child in choicesContainer)
            {
                Destroy(child.gameObject);
            }

            // Create new choice buttons
            for (int i = 0; i < choices.Count; i++)
            {
                if (choices[i].Conditions != null && choices[i].Conditions.Count > 0)
                {
                    if (!CheckChoiceConditions(choices[i])) continue; // Skip this choice if conditions are not met
                }

                if (choices[i].isRemoved)
                {
                    continue;
                }

                var choice = choices[i];
                // Create a new GameObject for the button
                GameObject buttonObj = new GameObject($"Choice Button {i}");
                buttonObj.transform.SetParent(choicesContainer, false);

                // Add required components
                Button button = buttonObj.AddComponent<Button>();
                Image buttonImage = buttonObj.AddComponent<Image>();
                VerticalLayoutGroup layoutGroup = buttonObj.AddComponent<VerticalLayoutGroup>();
                // Setup VerticalLayoutGroup
                layoutGroup.childControlHeight = true;
                layoutGroup.childControlWidth = true;
                layoutGroup.childForceExpandHeight = false;
                layoutGroup.childForceExpandWidth = false;
                layoutGroup.childAlignment = TextAnchor.UpperLeft;
                layoutGroup.padding = new RectOffset(25, 25, 25, 25);

                // Create text child object
                GameObject textObj = new GameObject("Choice Text");
                textObj.transform.SetParent(buttonObj.transform, false);

                // Add text component
                TextMeshProUGUI tmpText = textObj.AddComponent<TextMeshProUGUI>();
                tmpText.text = choice.Text;
                tmpText.alignment = TextAlignmentOptions.Left;
                tmpText.fontSize = 35;
                tmpText.color = Color.black;

                Debug.Log($"Choice {i}: {choice.Text}");
                int choiceIndex = i;
                button.onClick.AddListener(() => DialogueManager.Instance.MakeChoice(choiceIndex));
            }
        }

        public bool CheckChoiceConditions(DialogueChoice dialogueChoice)
        {
            // Check conditions here and skip if not met
            bool conditionsMet = true;
            foreach (var condition in dialogueChoice.Conditions)
            {
                switch (condition.Type)
                {
                    case ConditionType.HasItem:
                        // Check if the player has the item
                        if (!InventoryManager.Instance.HasItemWithID(condition.ItemID))
                        {
                            conditionsMet = false;
                        }
                        break;
                }
            }

            return conditionsMet;
        }

        public bool CheckConditions(List<DialogueCondition> conditions)
        {
            // Check conditions here and skip if not met
            bool conditionsMet = true;
            foreach (var condition in conditions)
            {
                switch (condition.Type)
                {
                    case ConditionType.HasItem:
                        // Check if the player has the item
                        if (!InventoryManager.Instance.HasItemWithID(condition.ItemID))
                        {
                            conditionsMet = false;
                        }
                        break;
                }
            }

            return conditionsMet;
        }
    }
}