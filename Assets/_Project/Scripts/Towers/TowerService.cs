using UnityEngine;
using _Project.Scripts.Weapons;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Logic.Towers;
using _Project.Scripts.UI.TowerCreation;
using _Project.Scripts.ConfigRepositories;

namespace _Project.Scripts.Towers
{
    public class TowerService
    {
        private readonly TowerFactory _towerFactory;
        private readonly WeaponFactory _weaponFactory;
        private readonly TowerConfigsRepository _towerConfigsRepository;
        private readonly CoinCounterModel _coinCounterModel;
        
        private CreateTowerPanel _createTowerPanel;
        
        public TowerService(
            TowerFactory towerFactory, 
            WeaponFactory weaponFactory,
            TowerConfigsRepository towerConfigsRepository,
            CoinCounterModel coinCounterModel
            )
        {
            _coinCounterModel = coinCounterModel;
            _towerFactory = towerFactory;
            _towerConfigsRepository = towerConfigsRepository;
            _weaponFactory = weaponFactory;
        }
        
        public Tower Create(TowerType towerType, Vector3 position, float damageMultiplier, float attackSpeedMultiplier)
        {
            Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(towerType).WeaponType, 
                    tower.WeaponPoint.transform.position, 
                    tower.WeaponPoint.transform, 
                    damageMultiplier, 
                    attackSpeedMultiplier);
            
            if (tower.TryGetComponent(out IWeaponMountOwner weaponMountOwner))
                weaponMountOwner.SetWeapon(weapon);
            
            return tower;
        }

        public void CreateAndPurchase(TowerType towerType, Vector3 position, int coinPrice, float damageMultiplier, float attackSpeedMultiplier)
        {
            Create(towerType, position, damageMultiplier, attackSpeedMultiplier);
            Purchase(coinPrice);
        }
        
        private void Purchase(int coinPrice) => _coinCounterModel.RemoveCoins(coinPrice);
    }
}