using System;
using UnityEngine.Events;

namespace GameLokal.Toolkit
{
    public class DialogueManager : Singleton<DialogueManager>
    {
        protected override bool ShouldNotDestroyOnLoad()
        {
            return false;
        }

        public DialogueView leftDialogue;
        public DialogueView rightDialogue;
        public UnityEvent onDialogueStart;
        public UnityEvent onDialogueFinish;

        private int iDialogue;
        private DialogueData currentDialogueData;
        private Action onFinishCallback;
        
        protected virtual void Start()
        {
            HideDialogue();
        }

        protected virtual void HideDialogue()
        {
            leftDialogue.gameObject.SetActive(false);
            rightDialogue.gameObject.SetActive(false);
        }
        
        public virtual void PlayDialogue(DialogueData data, Action onFinish)
        {
            iDialogue = 0;
            currentDialogueData = data;
            onFinishCallback = onFinish;
            ShowCurrentDialogue();
        }

        protected virtual void ShowDialogue(Dialogue dialogue)
        {
            HideDialogue();
            if (dialogue.alignment == DialoguePortraitAlignment.Left)
            {
                leftDialogue.gameObject.SetActive(true);
                leftDialogue.Show(dialogue);
            }
            else
            {
                rightDialogue.gameObject.SetActive(true);
                rightDialogue.Show(dialogue);
            }
        }

        protected virtual void ShowCurrentDialogue()
        {
            ShowDialogue(currentDialogueData.dialogues[iDialogue]);
        }

        public virtual void Next()
        {
            iDialogue++;
            if (iDialogue < currentDialogueData.dialogues.Count)
            {
                ShowCurrentDialogue();
            }
            else
            {
                EndDialogue();
            }
        }

        protected virtual void EndDialogue()
        {
            onFinishCallback?.Invoke();
            HideDialogue();
        }
    }
}