using UnityEngine;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator :  MonoBehaviour
    {
        private readonly int _attack = Animator.StringToHash("Attack");
        private readonly int _die = Animator.StringToHash("Die");
        private readonly int _run = Animator.StringToHash("Run");
        private readonly int _idle = Animator.StringToHash("Idle");
        private Animator _animator;

        private void Awake() => _animator = GetComponent<Animator>();

        public void PlayMove() => _animator.Play(_run);
        
        public void PlayAttack() => _animator.Play(_attack);
        
        public void PlayDeath() => _animator.Play(_die);
        
        public void PlayIdle() => _animator.Play(_idle);
    }
}