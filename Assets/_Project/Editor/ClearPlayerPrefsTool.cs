using UnityEditor;
using UnityEngine;

namespace _Project.Editor
{
    public class ClearPlayerPrefsTool : EditorWindow
    {
        [MenuItem("Tools/Clear Prefs")]
        private static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
        }
    }
}