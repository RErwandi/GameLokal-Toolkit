using System;
using UnityEngine;
using UnityEngine.UI;

namespace GameLokal.Toolkit
{
    public class SaveLoadExamples : MonoBehaviour, IGameSave
    {
        [System.Serializable]
        public class SavedClass
        {
            public string myString;
            public CsvTestClass testScriptable;
        }

        public SavedClass savedClass;
        public InputField inputField;

        private void Start()
        {
            SaveLoadManager.Instance.Initialize(this);
        }

        private void Update()
        {
            savedClass.myString = inputField.text;
        }

        public void Save()
        {
            SaveLoadManager.Instance.Save();
        }

        public void Load()
        {
            SaveLoadManager.Instance.Load();
        }

        public string GetUniqueName()
        {
            return gameObject.name;
        }

        public object GetSaveData()
        {
            return savedClass;
        }

        public Type GetSaveDataType()
        {
            return typeof(SavedClass);
        }

        public void ResetData()
        {
            savedClass = new SavedClass();
        }

        public void OnLoad(object generic)
        {
            savedClass = (SavedClass)generic;
            inputField.text = savedClass.myString;
        }
    }
}