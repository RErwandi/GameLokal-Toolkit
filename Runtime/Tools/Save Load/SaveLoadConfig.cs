#if UNITY_EDITOR
using System.Diagnostics;
#endif
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace GameLokal.Toolkit
{
    [GlobalConfig("Resources/GameLokal Toolkit/")]
    public class SaveLoadConfig : GlobalConfig<SaveLoadConfig>
    {
        public string defaultFilename = "gamelokal_savedata";
        public bool saveOnApplicationPause = true;
        public bool saveOnApplicationQuit = true;
        public bool useAutoSave = true;
        [ShowIf("useAutoSave"), Range(1, 100), SuffixLabel("minute(s)")]
        public float autoSaveInterval = 5;

#if UNITY_EDITOR
        [Button(ButtonSizes.Large)]
        public void OpenPersistentDataPath()
        {
            Process.Start(Application.persistentDataPath);
        }
#endif
    }
}