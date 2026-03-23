using UnityEngine;

namespace _Project.Scripts.Enemy
{
    [RequireComponent(typeof(Animator))]
    public class EnemyAnimator :  MonoBehaviour
    {
        private static readonly int Attack = Animator.StringToHash("Attack");
        private static readonly int Die = Animator.StringToHash("Die");
        private static readonly int Victory = Animator.StringToHash("Victory");
        private static readonly int Run = Animator.StringToHash("Run");
        private static readonly int Idle = Animator.StringToHash("Idle");
        
        private Animator _animator;

        private void Awake() => _animator = GetComponent<Animator>();

        public void PlayMove() => _animator.Play(Run);
        
        public void PlayAttack() => _animator.Play(Attack);
        
        public void PlayDeath() => _animator.Play(Die);
        
        public void PlayVictory() => _animator.Play(Victory);
        
        public void PlayIdle() => _animator.Play(Idle);
    }
}