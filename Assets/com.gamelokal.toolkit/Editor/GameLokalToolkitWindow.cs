using Sirenix.OdinInspector.Editor;
using Sirenix.Utilities;
using Sirenix.Utilities.Editor;
using UnityEditor;


namespace GameLokal.Toolkit
{
    public class GameLokalToolkitWindow : OdinMenuEditorWindow
    {

        [MenuItem("Tools/GameLokal/Preferences")]
        private static void OpenWindow()
        {
            var window = GetWindow<GameLokalToolkitWindow>(title:"Game Lokal Preferences");
            window.position = GUIHelper.GetEditorWindowRect().AlignCenter(800, 500);
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            var tree = new OdinMenuTree();
            tree.Add("Save and Load", SaveLoadConfig.Instance);
            return tree;
        }
    }
}