using UnityEngine;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;
using _Project.Scripts.Services.Upgrade;
using _Project.Scripts.Tower.Castle.States;

namespace _Project.Scripts.Tower.Castle
{
    public class CastleInitializer
    {
        private readonly TowerService _towerService;
        private readonly UpgradeService _upgradeService;

        public CastleInitializer(TowerService towerService, UpgradeService upgradeService)
        {
            _towerService = towerService;
            _upgradeService = upgradeService;
        }

        public Castle CreateCastle(Vector3 position)
        {
            Castle castle = (Castle)_towerService.Create(
                TowerType.Castle, 
                position, 
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_DAMAGE_ID), 
                _upgradeService.GetUpgradeMultiplier(UpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
                );
            
            castle.SetStateMachine(CreateCastleStateMachine(castle));

            return castle;
        }
        
        private StateMachine CreateCastleStateMachine(Castle castle)
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