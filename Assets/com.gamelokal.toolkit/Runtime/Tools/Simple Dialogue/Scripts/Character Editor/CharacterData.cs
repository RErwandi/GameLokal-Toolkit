using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [System.Serializable]
    public class CharacterData
    {
        [LabelWidth(20)]
        [ValueDropdown("availableCharacter")]
        public string character;
        [TableList(DrawScrollView = false)]
        public List<CharacterExpression> expressions = new List<CharacterExpression>();
        
        private List<string> availableCharacter => CharacterConfig.Instance.GetAvailableNames;
    }
    
    [System.Serializable]
    public class CharacterExpression
    {
        [ValueDropdown("availableExpression")]
        public string expression;
        [PreviewField(Alignment = ObjectFieldAlignment.Center)]
        public Sprite sprite;
        
        private List<string> availableExpression => CharacterConfig.Instance.GetAvailableExpression;
    }
}