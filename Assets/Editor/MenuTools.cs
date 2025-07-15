using UnityEditor;
using static System.IO.Directory;
using static System.IO.Path;
using static UnityEngine.Application;
using static UnityEditor.AssetDatabase;

namespace HeroicArcade
{
    public static class MenuTools
    {
        [MenuItem("Tools/Setup/Create Default Folders")]
        public static void CreateDefaultFolders()
        {
            CreateDirectories("_Project", "Animations", "Crosshairs", "Materials", "Prefabs/Resources", "Scenes", "Scripts", "SOs");
            CreateDirectories("External");
            Refresh();
        }

        public static void CreateDirectories(string root, params string[] subDirs)
        {
            var fullPath = Combine(dataPath, root);
            CreateDirectory(fullPath);
            foreach (var newDir in subDirs)
            {
                CreateDirectory(Combine(fullPath, newDir));
            }
        }
    }
}