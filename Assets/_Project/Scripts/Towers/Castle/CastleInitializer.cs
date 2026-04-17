using Zenject;
using UnityEngine;
using _Project.Scripts.Towers.Castle.States;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Infrastructure.GameConstants;

namespace _Project.Scripts.Towers.Castle
{
    public class CastleInitializer : ICastleInitializer
    {
        private readonly ITowerService _towerService;
        
        [Inject]
        public CastleInitializer(ITowerService towerService) => _towerService = towerService;

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