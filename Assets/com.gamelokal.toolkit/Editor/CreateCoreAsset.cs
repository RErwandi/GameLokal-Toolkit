namespace GameLokal.Toolkit
{
	using System.IO;
	using UnityEditor;

	public class CreateCoreAsset
	{
		// PRIVATE METHODS: -----------------------------------------------------------------------

		public static string GetSelectionPath()
		{
			string path = "Assets";
			foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
			{
				path = AssetDatabase.GetAssetPath(obj);
				if (File.Exists(path))
				{
					path = Path.GetDirectoryName(path);
				}

				break;
			}

			return path;
		}
	}
}