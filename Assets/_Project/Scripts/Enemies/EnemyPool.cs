using System.Collections.Generic;
using Zenject;

namespace _Project.Scripts.Enemies
{
    public class EnemyPool : MonoMemoryPool<Enemy>
    {
        public List<Enemy> ActiveEnemies { get; private set; } = new();

        protected override void OnSpawned(Enemy item)
        {
            base.OnSpawned(item);
            ActiveEnemies.Add(item);
        }

        protected override void OnDespawned(Enemy enemy) => enemy.gameObject.SetActive(false);
    }
}
