using System.Collections.Generic;
#if UNITY_EDITOR
using System.Diagnostics;
#endif
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameLokal.Toolkit
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [Title("Configurations")]
        public bool useEasySave;
        [HideIf("useEasySave")]
        public string defaultFilename = "gamelokal_savedata";
        public bool saveOnApplicationPause = true;
        public bool saveOnApplicationQuit = true;
        public bool useAutoSave = true;
        [ShowIf("useAutoSave"), Range(1, 100), SuffixLabel("minute(s)")]
        public float autoSaveInterval = 5;
        
        /// <summary>
        /// Used to remember filename that is currently player using
        /// </summary>
        private string lastSaveFilename = "";
        
        private List<IGameSave> gameSaves = new List<IGameSave>();

        private const string FILE_EXTENSION = ".gls";

        protected override void Awake()
        {
            base.Awake();
            if (useEasySave)
            {
                defaultFilename = ES3Settings.defaultSettings.path;
            }
        }

        private void Start()
        {
            if (useAutoSave)
            {
                InvokeRepeating(nameof(AutoSave), autoSaveInterval.MinuteToSecond(), autoSaveInterval.MinuteToSecond());
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (string.IsNullOrEmpty(lastSaveFilename) || !pauseStatus || !saveOnApplicationPause) return;

            Save(lastSaveFilename);
        }

        private void OnApplicationQuit()
        {
            if (string.IsNullOrEmpty(lastSaveFilename) || !saveOnApplicationQuit) return;

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
                saveFileName = defaultFilename;
            }
            
            lastSaveFilename = saveFileName;
            
            // Save without Easy Save 3
            /*var wrappers = gameSaves.Select(gameSave => new JsonWrapper
            {
                uniqueName = gameSave.GetUniqueName(), value = JsonUtility.ToJson(gameSave.GetSaveData())
            }).ToList();

            SaveLoad.WriteFile(JsonHelper.ToJson(wrappers, true),saveFileName + FILE_EXTENSION);*/
            
            //Save With Easy Save 3
            foreach (var gameSave in gameSaves)
            {
                var uniqueName = gameSave.GetUniqueName();
                ES3.Save(uniqueName, gameSave.GetSaveData());
            }
            
            Debug.Log($"Game saved to {Application.persistentDataPath}/{saveFileName + FILE_EXTENSION}");
        }

        public void Load(string loadFileName = "")
        {
            if (string.IsNullOrEmpty(loadFileName))
            {
                loadFileName = defaultFilename;
            }

            if (!useEasySave)
            {
                if (!SaveLoad.FileExist(loadFileName + FILE_EXTENSION))
                {
                    Debug.LogWarning($"No save file with name {loadFileName} is found");
                    return;
                }
            }
            else
            {
                if (!ES3.FileExists(defaultFilename))
                {
                    Debug.LogWarning($"No save file with name {loadFileName} is found");
                    return;
                }
            }
            
            
            lastSaveFilename = loadFileName;
            
            // Load without Easy Save 3
            /*
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
            */
            
            // Load with Easy Save 3
            foreach (var gameSave in gameSaves)
            {
                var uniqueName = gameSave.GetUniqueName();
                if(ES3.KeyExists(uniqueName))
                    gameSave.OnLoad(ES3.Load(uniqueName));
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
        
#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        public void OpenPersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }
#endif

        [System.Serializable]
        private class JsonWrapper
        {
            public string uniqueName;
            public string value;
        }
    }
}