using Zenject;
using System.Collections.Generic;
using _Project.Scripts.Enemies.Behaviour;

namespace _Project.Scripts.Enemies
{
    public class EnemyPool : MonoMemoryPool<Enemy>
    {
        public List<Enemy> ActiveEnemies { get; } = new();

        public void DespawnAll()
        {
            for (int i = ActiveEnemies.Count - 1; i >= 0; i--)
                OnDespawned(ActiveEnemies[i]);
        }

        protected override void OnSpawned(Enemy enemy)
        {
            base.OnSpawned(enemy);
            ActiveEnemies.Add(enemy);
        }

        protected override void OnDespawned(Enemy enemy)
        {
            enemy.gameObject.SetActive(false);
            ActiveEnemies.Remove(enemy);
        }
    }
}
