using System.IO;
using UnityEngine;

namespace GameLokal.Toolkit
{
    public static class SaveLoad
    {
        public static void WriteFile(string json, string filename)
        {
            File.WriteAllText(FilePath(filename), json);
        }

        public static string Read(string filename)
        {
            var path = FilePath(filename);
            if (File.Exists(path))
            {
                using (var reader = new StreamReader(path))
                {
                    return reader.ReadToEnd();
                }
            }
            
            Debug.Log("File not found");
            return "";
        }

        public static bool FileExist(string filename)
        {
            return File.Exists(FilePath(filename));
        }

        private static string FilePath(string filename)
        {
            return Path.Combine(Application.persistentDataPath, filename);
        }
    }
}