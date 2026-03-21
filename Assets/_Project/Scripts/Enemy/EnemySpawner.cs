using UnityEngine;

namespace _Project.Scripts.Enemy
{
    public class EnemySpawner
    {
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
            
            int side = Random.Range(0, 4);

            float x = 0f;
            float z = 0f;

            switch (side)
            {
                case 0:
                    x = center.x - _viewWidth / 2 - offset;
                    z = Random.Range(center.y - _viewHeight / 2, center.y + _viewHeight / 2);
                    break;

                case 1:
                    x = center.x + _viewWidth / 2 + offset;
                    z = Random.Range(center.y - _viewHeight / 2, center.y + _viewHeight / 2);
                    break;

                case 2:
                    x = Random.Range(center.x - _viewWidth / 2, center.x + _viewWidth / 2);
                    z = center.y + _viewHeight / 2 + offset;
                    break;

                case 3:
                    x = Random.Range(center.x - _viewWidth / 2, center.x + _viewWidth / 2);
                    z = center.y - _viewHeight / 2 - offset;
                    break;
            }

            return new Vector3(x, 1f, z);
        }
    }
}