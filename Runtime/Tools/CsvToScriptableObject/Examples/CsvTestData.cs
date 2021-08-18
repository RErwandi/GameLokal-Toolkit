using Sirenix.OdinInspector;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [System.Serializable]
    public class CsvTestData
    {
        public string firstName;
        public string lastName;
        public int day;
        public int month;
        public int year;
        public float price;
        public int[] friendList;
        [PreviewField]
        public Sprite icon;
    }
}