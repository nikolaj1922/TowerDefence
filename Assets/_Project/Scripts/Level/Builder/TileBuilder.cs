using Unity.AI.Navigation;
using UnityEngine;
using UnityEditor;

namespace _Project.Scripts.Level.Builder
{
    public class TileBuilder : MonoBehaviour
    {
        private MeshFilter _meshFilter;
        private NavMeshModifier _navMeshModifier;

        [Header("Model"), SerializeField]
        private GameObject _model;

        [Header("Meshes")]
        [SerializeField] private Mesh _baseMesh;
        [SerializeField] private Mesh _straightMesh;
        [SerializeField] private Mesh _cornerMesh;
        [SerializeField] private Mesh _dirtMesh;
        [SerializeField] private Mesh _splitMesh;
        [SerializeField] private Mesh _startMesh;

        private void OnValidate()
        {
            if (_meshFilter == null)
                _meshFilter = _model.GetComponent<MeshFilter>();
            if(_navMeshModifier == null)
                _navMeshModifier = _model.GetComponent<NavMeshModifier>();
            
        }

        public void PlaceStraight() =>  ChangeTile(_straightMesh);

        public void PlaceCorner() => ChangeTile(_cornerMesh);

        public void PlaceSplit() => ChangeTile(_splitMesh);

        public void PlaceDirt() => ChangeTile(_dirtMesh);
        
        public void PlaceStart() => ChangeTile(_startMesh);

        public void PlaceBase() => ChangeTile(_baseMesh);
        
        public void RotateTile(Vector3 angle)
        {
            TileBuilder[] selectedTiles = GetSelectedTiles();
            
            foreach (var tile in selectedTiles)
            {
                Undo.RecordObject(tile.transform, "Rotate Tile");
                tile._model.transform.Rotate(angle);
            }
        } 
        
        private void ChangeTile(Mesh newMesh, NavMeshAreaState navMeshAreaState = NavMeshAreaState.Walkable)
        {
            TileBuilder[] selectedTiles = GetSelectedTiles();

            foreach (var tile in selectedTiles)
            {
                Undo.RecordObject(tile._meshFilter, "Change Tile Mesh");
                tile._meshFilter.sharedMesh = newMesh;
                tile._navMeshModifier.overrideArea = true;
                tile._navMeshModifier.area = (int)navMeshAreaState;
            }
        }

        private TileBuilder[] GetSelectedTiles() => 
            Selection.GetFiltered<TileBuilder>(SelectionMode.Editable | SelectionMode.ExcludePrefab);
    }

    enum NavMeshAreaState
    {
        Walkable,
        NotWalkable,
    }
}

