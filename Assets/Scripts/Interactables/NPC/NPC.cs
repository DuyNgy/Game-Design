using Unity.VisualScripting;
using UnityEngine;
using Project.Dialogue;
using Project.Dialogue.Data;
using UnityEngine.AI;

namespace Project.Interactable.NPCs
{
    public class NPC : Interactables
    {
        [SerializeField] private DialogueData dialogueData;
        [SerializeField] private string initialDialogueID;
        [SerializeField] private Animator animator;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        protected override void Start()
        {
            base.Start();
        }

        public void PlayTalkingAnimation()
        {
            if (animator != null)
            {
                animator.SetBool("Talking", true);
            }
        }

        public void StopTalkingAnimation()
        {
            if (animator != null)
            {
                animator.SetBool("Talking", false);
            }
        }

        public void SetDialogueData(DialogueData data)
        {
            dialogueData = data;
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();
            // Check if the player is close enough to interact with the NPC
            if (DialogueManager.Instance.CurrentSpeakingNPC == this && !IsPlayerClose())
            {
                DialogueManager.Instance.EndDialogue();
            }
        }

        protected override void Interact()
        {
            if (dialogueData == null)
            {
                Debug.LogError($"No dialogue data assigned to NPC: {gameObject.name}");
                return;
            }

            DialogueManager.Instance.StartDialogue(dialogueData, initialDialogueID, this);
        }

        private bool IsPlayerClose()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distance = Vector3.Distance(transform.position, player.transform.position);
                return distance <= 5.0f;
            }
            else
            {
                return false;
            }
        }
    }
}
