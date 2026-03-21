using UnityEngine;
using System.Collections;

namespace _Project.Scripts.Infrastructure.CoroutineRunner
{
    public class CoroutineRunner : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public Coroutine Run(IEnumerator coroutine)
        {
            return StartCoroutine(coroutine);
        }

        public void Stop(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
    }
}