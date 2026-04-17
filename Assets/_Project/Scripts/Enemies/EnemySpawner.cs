using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemySpawner : IEnemySpawner
    {
        private const int MAX_SIDES = 4;
        private const float SPAWN_HEIGHT = 1f;
        
        private readonly float _viewHeight;
        private readonly float _viewWidth;

        public EnemySpawner(float viewHeight, float viewWidth)
        {
            _viewHeight = viewHeight;
            _viewWidth = viewWidth;
        }
        
        public Vector3 GetRandomSpawnPoint(float offset)
        {
            Vector3 center = Vector3.zero;
            
            SpawnSide side = (SpawnSide)Random.Range(0, MAX_SIDES);

            float x = 0f;
            float z = 0f;

            switch (side)
            {
                case SpawnSide.Left:
                    x = center.x - _viewWidth / 2 - offset;
                    z = Random.Range(center.y - _viewHeight / 2, center.y + _viewHeight / 2);
                    break;

                case SpawnSide.Right:
                    x = center.x + _viewWidth / 2 + offset;
                    z = Random.Range(center.y - _viewHeight / 2, center.y + _viewHeight / 2);
                    break;

                case SpawnSide.Top:
                    x = Random.Range(center.x - _viewWidth / 2, center.x + _viewWidth / 2);
                    z = center.y + _viewHeight / 2 + offset;
                    break;

                case SpawnSide.Bottom:
                    x = Random.Range(center.x - _viewWidth / 2, center.x + _viewWidth / 2);
                    z = center.y - _viewHeight / 2 - offset;
                    break;
            }
            
            return new Vector3(x, SPAWN_HEIGHT, z);
        }
        
        private enum SpawnSide
        {
            Left,
            Right,
            Top,
            Bottom
        }
    }
}