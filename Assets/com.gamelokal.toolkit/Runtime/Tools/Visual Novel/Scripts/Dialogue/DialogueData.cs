using System.Collections.Generic;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [CreateAssetMenu(order = 0, fileName = "Dialogue", menuName = "GameLokal/Dialogue")]
    public class DialogueData : ScriptableObject
    {
        public List<Dialogue> dialogues = new List<Dialogue>();
    }
}