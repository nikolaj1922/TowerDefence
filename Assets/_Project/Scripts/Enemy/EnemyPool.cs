using Zenject;

namespace _Project.Scripts.Enemy
{
    public class EnemyPool : MonoMemoryPool<Enemy>
    {
        protected override void OnSpawned(Enemy enemy)
        {
            enemy.ResetComponents();
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(Enemy enemy) => enemy.gameObject.SetActive(false);
    }
}