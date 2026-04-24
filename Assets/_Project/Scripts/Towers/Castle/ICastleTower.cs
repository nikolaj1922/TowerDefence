using System;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.UI.HealthBar;

namespace _Project.Scripts.Towers.Castle
{
    public interface ICastleTower
    {
        event Action OnCastleDestroy;
        event Action<float> OnCastleDamaged;
        HealthModel HealthModel { get; }
        void SetStateMachine(StateMachine stateMachine);
        void RestoreHp();
        void BreakWeapon();
        void RestoreWeapon();
    }
}