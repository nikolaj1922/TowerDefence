using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Enemies
{
    public class EnemyDeath : MonoBehaviour
    {
        private const float DEATH_TIME = 2f;
        
        private Action _onDeath;

        public void Initialize(Action onDeath = null) => _onDeath = onDeath;

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

            _onDeath?.Invoke();
        }
    }
}