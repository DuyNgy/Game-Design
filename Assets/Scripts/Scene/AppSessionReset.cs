using System.Collections.Generic;
using Project.Dialogue.Data;
using UnityEngine;

public class AppSessionReset : MonoBehaviour
{
    void Awake()
    {
        // Clear this only at the beginning of a full game session
        // Add any Prefs that arent supposed to stay between sessions
        PlayerPrefs.DeleteKey("isLit");

        // Reset all DialogueData ScriptableObjects
        List<DialogueData> allDialogueData = new List<DialogueData>();
        allDialogueData.AddRange(Resources.LoadAll<DialogueData>("DialogueData/Level 1"));
        allDialogueData.AddRange(Resources.LoadAll<DialogueData>("DialogueData/Level 2"));

        foreach (var dialogueData in allDialogueData)
        {
            foreach (var dialogueLine in dialogueData.dialogueLines)
            {
                // Reset the dialogue line's choices
                foreach (var choice in dialogueLine.Choices)
                {
                    choice.isRemoved = false; // Reset choices to not removed
                }

                foreach (var reward in dialogueLine.Rewards)
                {
                    reward.isGiven = false; // Reset rewards to not given
                }
            }
        }
    }
}