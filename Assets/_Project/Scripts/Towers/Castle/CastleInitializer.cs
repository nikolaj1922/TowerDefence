using Zenject;
using UnityEngine;
using _Project.Scripts.Towers.Castle.States;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle
{
    public class CastleInitializer : ICastleInitializer
    {
        private readonly ITowerService _towerService;
        
        [Inject]
        public CastleInitializer(ITowerService towerService) => _towerService = towerService;

        public ICastleTower CreateCastle(
            Vector3 position, 
            float damageMultiplier, 
            float attackSpeedMultiplier)
        {
            Tower tower = _towerService.Create(
                TowerType.Castle, 
                position,
                damageMultiplier,
                attackSpeedMultiplier);

            if (tower.TryGetComponent(out ICastleTower castle))
                castle.SetStateMachine(CreateCastleStateMachine(castle));
            
            return castle;
        }

        private StateMachine CreateCastleStateMachine(ICastleTower castle)
        {
            return new StateMachine(
                new IState[]
                {
                    new InitialState(castle),
                    new BrokenWeaponState(castle)
                },
                new ITransition[]
                {
                    new Transition<InitialState, BrokenWeaponState>(
                        () => castle.HealthModel.CurrentHealth / castle.HealthModel.MaxHealth 
                              < GameConstants.CASTLE_COLLAPSE_HEALTH_PERCENT),
                    new Transition<BrokenWeaponState, InitialState>(
                        () => castle.HealthModel.CurrentHealth / castle.HealthModel.MaxHealth
                              > GameConstants.CASTLE_COLLAPSE_HEALTH_PERCENT)
                }
            );
        }
        
    }
}