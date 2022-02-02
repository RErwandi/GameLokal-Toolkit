using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [System.Serializable]
    public class Dialogue 
    {
        [HorizontalGroup("Split", 0.5f, LabelWidth = 70)]
        [BoxGroup("Split/Character")]
        [ValueDropdown("availableCharacter")]
        public string character;
        
        [BoxGroup("Split/Character")]
        [ValueDropdown("availableExpression")]
        public string expression;
        
        [BoxGroup("Split/Settings")]
        public DialoguePortraitAlignment alignment;

        [BoxGroup("Split/Settings")]
        public float speed = 25;

        [BoxGroup("Text"), TextArea, HideLabel]
        public string text;

        public List<string> availableCharacter => CharacterConfig.Instance.GetAvailableNames;

        public List<string> availableExpression => CharacterConfig.Instance.GetAvailableExpression;
    }
    
    public enum DialoguePortraitAlignment {Left, Right}
}