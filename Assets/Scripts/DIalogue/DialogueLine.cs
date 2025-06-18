using System;
using System.Collections.Generic;
using Project.Inventory;
using UnityEngine;

namespace Project.Dialogue.Data
{
    /// <summary>
    /// General class to hold the basic structure of a dialogue line.
    /// /summary>
    [Serializable]
    public class DialogueLine
    {
        public string SpeakerName;
        public string DialogueID;
        [TextArea] public string Text;
        public List<DialogueChoice> Choices;
        public List<DialogueCondition> Conditions;
        public List<DialogueReward> Rewards;
    }

    /// <summary>
    /// Class to hold the structure of a dialogue choice.
    /// /summary>
    [Serializable]
    public class DialogueChoice
    {
        public string Text;
        public string NextDialogueID;
        public bool shouldBeRemoved = true;
        [HideInInspector] public bool isRemoved = false;
        public List<DialogueCondition> Conditions;
    }

    /// <summary>
    /// Class to hold the structure of a dialogue condition.
    /// /summary>
    [Serializable]
    public class DialogueCondition
    {
        public ConditionType Type;
        public int ItemID;
    }

    /// <summary>
    /// Class to hold the structure of a dialogue reward.
    /// /summary>
    [Serializable]
    public class DialogueReward
    {
        public RewardType Type;
        public ItemData Item;
        public int Amount;
        public string SceneToLoad;
        public bool isGiven = false;
    }

    /// <summary>
    /// Enum to define the types of conditions that can be checked in a dialogue.
    /// /summary>
    public enum ConditionType
    {
        HasItem,
        QuestComplete
    }

    /// <summary>
    /// Enum to define the types of rewards that can be given in a dialogue.
    /// /summary>
    public enum RewardType
    {
        Item,
        SceneToLoad
    }
}