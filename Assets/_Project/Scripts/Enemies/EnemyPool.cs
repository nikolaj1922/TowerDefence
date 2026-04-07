using Zenject;

namespace _Project.Scripts.Enemies
{
    public class EnemyPool : MonoMemoryPool<Enemy>
    {
        protected override void OnDespawned(Enemy enemy) => enemy.gameObject.SetActive(false);
    }
}
