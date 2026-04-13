using Zenject;
using UnityEngine;
using _Project.Scripts.Towers.Castle.States;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle
{
    public class CastleInitializer
    {
        private readonly TowerService _towerService;
        
        [Inject]
        public CastleInitializer(TowerService towerService) => _towerService = towerService;

        public CastleTower CreateCastle(
            Vector3 position, 
            float damageMultiplier, 
            float attackSpeedMultiplier)
        {
            Tower tower = _towerService.Create(
                TowerType.Castle, 
                position,
                damageMultiplier,
                attackSpeedMultiplier);

            if (tower.TryGetComponent(out CastleTower castle))
                castle.SetStateMachine(CreateCastleStateMachine(castle));
            
            return castle;
        }

        private StateMachine CreateCastleStateMachine(CastleTower castle)
        {
            return new StateMachine(
                new IState[]
                {
                    new EntireState(),
                    new CollapseState(castle)
                },
                new ITransition[]
                {
                    new Transition<EntireState, CollapseState>(
                        () => castle.HealthModel.CurrentHealth / castle.HealthModel.MaxHealth < GameConstants.CASTLE_COLLAPSE_HEALTH_PERCENT)
                }
            );
        }
        
    }
}