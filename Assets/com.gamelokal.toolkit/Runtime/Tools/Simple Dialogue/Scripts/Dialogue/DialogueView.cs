using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GameLokal.Toolkit
{
    public class DialogueView : MonoBehaviour
    {
        public GameObject overlay;
        public TextMeshProUGUI characterName;
        public TextMeshProUGUI textbox;
        public Image portrait;
        public Button nextButton;

        protected virtual void OnEnable()
        {
            nextButton.onClick.AddListener(Finish);
        }
        
        protected virtual void OnDisable()
        {
            nextButton.onClick.RemoveListener(Finish);
        }

        public virtual void Show(Dialogue dialogue)
        {
            characterName.text = dialogue.character;
            textbox.text = dialogue.text;
            overlay.SetActive(dialogue.useOverlay);
            
            var characterPortrait = CharacterConfig.Instance.GetCharacterExpression(dialogue.character, dialogue.expression);
            if (characterPortrait == null)
            {
                portrait.gameObject.SetActive(false);
            }
            else
            {
                portrait.gameObject.SetActive(true);
                portrait.sprite = characterPortrait;
            }

            if (dialogue.useEvent && !string.IsNullOrEmpty(dialogue.eventName))
            {
                GameEvent.Trigger(dialogue.eventName);
            }
        }

        protected virtual void Finish()
        {
            DialogueManager.Instance.Next();
        }
    }
}