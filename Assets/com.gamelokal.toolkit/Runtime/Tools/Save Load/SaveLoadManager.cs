using System.Collections.Generic;
#if UNITY_EDITOR
using System.Diagnostics;
#endif
using Sirenix.OdinInspector;
using UnityEngine;
using Debug = UnityEngine.Debug;

namespace GameLokal.Toolkit
{
    public class SaveLoadManager : Singleton<SaveLoadManager>
    {
        [InfoBox("If set to false, you need to call StoreCachedFile() to write save data in memory to permanent local file")]
        public bool storeCachedFileAfterSaving = true;
        public bool saveOnApplicationPause = true;
        public bool saveOnApplicationQuit = true;
        public bool useAutoSave = true;
        [ShowIf("useAutoSave"), Range(1, 100), SuffixLabel("minute(s)")]
        public float autoSaveInterval = 5;

        private List<IGameSave> gameSaves = new List<IGameSave>();

        private void Start()
        {
            if (useAutoSave)
            {
                InvokeRepeating(nameof(Save), autoSaveInterval.MinuteToSecond(), autoSaveInterval.MinuteToSecond());
            }
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (!saveOnApplicationPause) return;

            Save();
        }

        private void OnApplicationQuit()
        {
            if (!saveOnApplicationQuit) return;

            Save();
        }

        public void Initialize(IGameSave gameSave)
        {
            gameSaves.Add(gameSave);
        }

        public void Save()
        {
            var settings = new ES3Settings(ES3.Location.Cache);
            
            foreach (var gameSave in gameSaves)
            {
                var uniqueName = gameSave.GetUniqueName();
                ES3.Save(uniqueName, gameSave.GetSaveData(), settings);
            }

            if (storeCachedFileAfterSaving)
            {
                StoreCachedFile();
            }
        }

        public void StoreCachedFile()
        {
            ES3.StoreCachedFile();
            Debug.Log($"Game saved to {ES3Settings.defaultSettings.path}");
        }

        public void LoadCachedFile()
        {
            ES3.CacheFile(ES3Settings.defaultSettings.path);
            Debug.Log($"Game loaded from {ES3Settings.defaultSettings.path}");
        }

        public void Load()
        {
            if (!ES3.FileExists(ES3Settings.defaultSettings.path))
            {
                Debug.LogWarning($"No save file is found");
                return;
            }

            LoadCachedFile();
            
            var settings = new ES3Settings(ES3.Location.Cache);
            foreach (var gameSave in gameSaves)
            {
                var uniqueName = gameSave.GetUniqueName();
                if(ES3.KeyExists(uniqueName))
                    gameSave.OnLoad(ES3.Load(uniqueName, settings));
            }
            
            Debug.Log($"Game loaded from {ES3Settings.defaultSettings.path}");
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