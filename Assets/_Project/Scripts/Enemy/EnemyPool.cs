using Zenject;

namespace _Project.Scripts.Enemy
{
    public class EnemyPool : MonoMemoryPool<EnemyController>
    {
        protected override void OnSpawned(EnemyController enemy)
        {
            enemy.gameObject.SetActive(true);
        }

        protected override void OnDespawned(EnemyController enemy)
        {
            enemy.gameObject.SetActive(false);
        }
    }
}