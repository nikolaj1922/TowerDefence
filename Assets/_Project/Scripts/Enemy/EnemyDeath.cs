using System;
using UnityEngine;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        private event Action OnDeath;
        
        private const float DEATH_TIME = 2f;
        
        private void OnDisable() => OnDeath = null;

        public void Initialize(Action onDeath = null) => OnDeath = onDeath;

        public void Die()
        {
            GetComponent<Collider>().enabled = false;
            DieRoutineAsync().Forget();
        }

        private async UniTaskVoid DieRoutineAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(DEATH_TIME), cancellationToken: this.GetCancellationTokenOnDestroy());

            float elapsed = 0f;
            while (elapsed < DEATH_TIME)
            {
                transform.position += Vector3.down * Time.deltaTime;
                elapsed += Time.deltaTime;

                await UniTask.Yield(PlayerLoopTiming.Update, this.GetCancellationTokenOnDestroy());
            }

            OnDeath?.Invoke();
        }
    }
}