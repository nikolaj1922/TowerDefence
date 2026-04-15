using CartoonFX;
using UnityEngine;

namespace _Project.Scripts.Infrastructure.ObjectsPool
{
    public class PooledVFX: MonoBehaviour
    {
        private ObjectsPool<CFXR_Effect> _pool;
        private CFXR_Effect _effect;
        private ParticleSystem _particleSystem;
        
        public void Init(ObjectsPool<CFXR_Effect> pool, CFXR_Effect effect)
        {
            _pool = pool;
            _effect = effect;
            _particleSystem = effect.GetComponent<ParticleSystem>();

            var main = _particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        
        private void OnParticleSystemStopped() => _pool.Release(_effect);
    }
}