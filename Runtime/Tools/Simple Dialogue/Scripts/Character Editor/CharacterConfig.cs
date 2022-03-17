using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [GlobalConfig("_GameLokal/Configs/")]
    public class CharacterConfig : GlobalConfig<CharacterConfig>
    {
        [HorizontalGroup("Database")]
        [Title("Characters Name Database")]
        public String[] availableCharacter;

        [HorizontalGroup("Database")]
        [Title("Characters Expression Database")]
        public string[] availableCharacterExpressions;
        
        [Title("Characters Creations")]
        [TableList(DrawScrollView = false)]
        public List<CharacterData> characters = new List<CharacterData>();

        public Sprite GetCharacterExpression(string name, string expression)
        {
            return (from c in characters from e in c.expressions where c.character == name && e.expression == expression select e.sprite).FirstOrDefault();
        }
        
        public List<string> GetAvailableNames
        {
            get
            {
                List<string> names = new List<string>();
                foreach (var c in availableCharacter)
                {
                    names.Add(c);
                }

                return names;
            }
        }

        public List<string> GetAvailableExpression
        {
            get
            {
                List<string> expression = new List<string>();
                foreach (var e in availableCharacterExpressions)
                {
                    expression.Add(e);
                }

                return expression;
            }
        }
    }
}