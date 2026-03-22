using System;
using UnityEngine;
using System.Collections;

namespace _Project.Scripts.Enemy
{
    public class EnemyDeath : MonoBehaviour
    {
        private Action _onDeath;
        
        public void Initialize(Action onDeath = null) => _onDeath = onDeath;
        
        public void Die()
        {
            GetComponent<Collider>().enabled = false;
            StartCoroutine(DieRoutine());
        }

        private IEnumerator DieRoutine()
        {
            yield return new WaitForSeconds(2f);

            float elapsed = 0;

            while (elapsed < 2f)
            {
                transform.position += 1f * Time.deltaTime * Vector3.down;
                elapsed += Time.deltaTime;
                
                yield return null;
            }
            
            _onDeath?.Invoke();
        }
    }
}