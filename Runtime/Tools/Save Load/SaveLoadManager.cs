using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GameLokal.Toolkit
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        /// <summary>
        /// Used to remember filename that is currently player using
        /// </summary>
        private string lastSaveFilename = "";
        
        private List<IGameSave> gameSaves = new List<IGameSave>();
        private SaveLoadConfig config;

        private const string FILE_EXTENSION = ".gls";

        protected override void Awake()
        {
            base.Awake();
            config = SaveLoadConfig.Instance;
        }

        private void Start()
        {
            if (config.useAutoSave)
            {
                InvokeRepeating(nameof(AutoSave), config.autoSaveInterval.MinuteToSecond(), config.autoSaveInterval.MinuteToSecond());
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (string.IsNullOrEmpty(lastSaveFilename) || !pauseStatus || !config.saveOnApplicationPause) return;

            Save(lastSaveFilename);
        }

        private void OnApplicationQuit()
        {
            if (string.IsNullOrEmpty(lastSaveFilename) || !config.saveOnApplicationQuit) return;

            Save(lastSaveFilename);
        }

        private void AutoSave()
        {
            if (string.IsNullOrEmpty(lastSaveFilename))
            {
                Debug.Log("Attempting auto save but no current save file is used at the moment.");
                return;
            }
            
            Save(lastSaveFilename);
            Debug.Log("Auto Saving...");
        }

        public void Initialize(IGameSave gameSave)
        {
            gameSaves.Add(gameSave);
        }

        public void Save(string saveFileName = "")
        {
            if (string.IsNullOrEmpty(saveFileName))
            {
                saveFileName = config.defaultFilename;
            }
            
            lastSaveFilename = saveFileName;
            var wrappers = gameSaves.Select(gameSave => new JsonWrapper {uniqueName = gameSave.GetUniqueName(), value = JsonUtility.ToJson(gameSave.GetSaveData())}).ToList();

            SaveLoad.WriteFile(JsonHelper.ToJson(wrappers, true),saveFileName + FILE_EXTENSION);
            Debug.Log($"Game saved to {Application.persistentDataPath}/{saveFileName + FILE_EXTENSION}");
        }

        public void Load(string loadFileName = "")
        {
            if (string.IsNullOrEmpty(loadFileName))
            {
                loadFileName = config.defaultFilename;
            }

            if (!SaveLoad.FileExist(loadFileName + FILE_EXTENSION))
            {
                Debug.LogWarning($"No save file with name {loadFileName} is found");
                return;
            }
            
            lastSaveFilename = loadFileName;
            
            var json = SaveLoad.Read(loadFileName + FILE_EXTENSION);
            var wrappers = JsonHelper.FromJson<JsonWrapper>(json);
            foreach (var gameSave in gameSaves)
            {
                foreach (var generic in from wrapper in wrappers where wrapper.uniqueName == gameSave.GetUniqueName() select JsonUtility.FromJson(wrapper.value, gameSave.GetSaveDataType()))
                {
                    gameSave.ResetData();
                    gameSave.OnLoad(generic);
                }
            }
            
            Debug.Log($"Game loaded from {Application.persistentDataPath}/{loadFileName + FILE_EXTENSION}");
        }

        /// <summary>
        /// Prevent auto-save until saving or loading is occured
        /// </summary>
        public void ResetLastSavedFilename()
        {
            lastSaveFilename = "";
            Debug.Log($"Last saved filename has been cleared. Auto-save is now turned off until saving or loading is occured.");
        }

        [System.Serializable]
        private class JsonWrapper
        {
            public string uniqueName;
            public string value;
        }
    }
}