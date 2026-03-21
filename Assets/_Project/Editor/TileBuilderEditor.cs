using System;
using UnityEditor;
using UnityEngine;
using _Project.Scripts.Level.Builder;


namespace _Project.Editor
{
    [CustomEditor(typeof(TileBuilder))]
    [CanEditMultipleObjects]
    public class TileBuilderEditor: UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector(); 
            TileBuilder tile = (TileBuilder)target;
              
            GUIStyle headerStyle = new()
            { 
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12,
                fontStyle = FontStyle.Bold,
            };
                
            headerStyle.normal.textColor = Color.white;
              
            GUILayout.Label("Tiles", headerStyle,  GUILayout.ExpandWidth(true));

            TwoButtons("Start", tile.PlaceStart, "Straight", tile.PlaceStraight);
            TwoButtons("Split", tile.PlaceSplit, "Dirt", tile.PlaceDirt);
            TwoButtons("Corner", tile.PlaceCorner, "Base", tile.PlaceBase);
              
            GUILayout.Label("Rotation", headerStyle, GUILayout.ExpandWidth(true));
              
            TwoButtons("Left", () => tile.RotateTile(new Vector3(0, 90, 0)), "Right", () => tile.RotateTile(new Vector3(0, 90, 0)));
        }

        private void TwoButtons(string a, Action aCallback, string b, Action bCallback)
        {   
            GUILayout.BeginHorizontal();
            DrawButton(a, aCallback);
            DrawButton(b, bCallback);
            GUILayout.EndHorizontal();
        }
        
        private void DrawButton(string label, Action callback)
        {
            if (GUILayout.Button(label))
                callback?.Invoke();
        }
    }
}