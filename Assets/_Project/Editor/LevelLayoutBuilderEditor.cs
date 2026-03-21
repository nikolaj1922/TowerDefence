using UnityEditor;
using UnityEngine;
using _Project.Scripts.Level.Builder;

namespace _Project.Editor
{
    [CustomEditor(typeof(LevelLayoutBuilder))]
    public class LevelLayoutBuilderEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            
            var builder = (LevelLayoutBuilder)target;

            if (GUILayout.Button("Build Level Layout"))
            {
                GenerateLayout(builder.LevelLayout, builder.LevelBasePrefab, builder.Width, builder.Height);
            }

            if (GUILayout.Button("Clear"))
            {
                var parent = builder.LevelLayout.transform;

                for (var i = parent.childCount - 1; i >= 0; i--)
                {
                    var child = parent.GetChild(i);
                    Undo.DestroyObjectImmediate(child.gameObject);
                }
            }
        }
        
        private static void GenerateLayout(GameObject levelLayout, GameObject levelPrefab, int width, int height)
        {
            var halfX = (width - 1) * 0.5f;
            var halfZ = (height - 1) * 0.5f;

            for (var x = 0; x < width; x++)
            {
                for (var z = 0; z < height; z++)
                {
                    var go = (GameObject)PrefabUtility.InstantiatePrefab(levelPrefab, levelLayout.transform);
                    Undo.RegisterCreatedObjectUndo(go, "Build Level Layout");

                    go.transform.localPosition = new Vector3(x - halfX, 0f, z - halfZ);
                    go.transform.localRotation = Quaternion.identity;
                }
            }
        }
    }
}