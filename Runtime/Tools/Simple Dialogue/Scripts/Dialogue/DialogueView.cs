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

        private void OnEnable()
        {
            nextButton.onClick.AddListener(Finish);
        }
        
        private void OnDisable()
        {
            nextButton.onClick.RemoveListener(Finish);
        }

        public void Show(Dialogue dialogue)
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
        }

        private void Finish()
        {
            DialogueManager.Instance.Next();
        }
    }
}