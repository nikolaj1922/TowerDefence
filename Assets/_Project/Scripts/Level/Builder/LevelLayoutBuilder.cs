using UnityEngine;

namespace _Project.Scripts.Logic.Game.Builder
{
    public class LevelLayoutBuilder : MonoBehaviour
    {
        [field: SerializeField] public GameObject LevelLayout { get; private set; }
        [field: SerializeField] public GameObject LevelBasePrefab { get; private set; }
        [field: SerializeField] public int Width { get; private set; }
        [field: SerializeField] public int Height { get; private set; }
    }
}