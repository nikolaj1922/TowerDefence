using Zenject;
using UnityEngine;
using _Project.Scripts.Weapons;
using _Project.Scripts.Logic.Towers;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Towers.Castle.States;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle
{
    public class CastleInitializer
    {
        private readonly TowerFactory _towerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TowerConfigsRepository _towerConfigsRepository;

        [Inject]
        public CastleInitializer(
            TowerFactory towerFactory,
            WeaponFactory weaponFactory,
            TowerConfigsRepository  towerConfigsRepository
            )
        {
            _towerConfigsRepository = towerConfigsRepository;
            _weaponFactory = weaponFactory;
            _towerFactory = towerFactory;
        }

        public CastleTower CreateCastle(
            Vector3 position, 
            float damageMultiplier, 
            float attackSpeedMultiplier)
        {
            Tower tower = _towerFactory.CreateTower(TowerType.Castle, position);
            Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(TowerType.Castle).WeaponType, 
                    tower.WeaponPoint.transform.position, 
                    tower.WeaponPoint.transform,
                    damageMultiplier,
                    attackSpeedMultiplier);

            if (tower.TryGetComponent(out IWeaponMountOwner weaponMountOwner))
                weaponMountOwner.SetWeapon(weapon);

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