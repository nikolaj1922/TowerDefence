using UnityEngine;
using _Project.Scripts.Weapons;
using _Project.Scripts.UI.CoinCounter;
using _Project.Scripts.Database.Towers;

namespace _Project.Scripts.Towers
{
    public class TowerService : ITowerService
    {
        private readonly ITowerFactory _towerFactory;
        private readonly IWeaponFactory _weaponFactory;
        private readonly TowerDatabase _towerDatabase;
        private readonly CoinCounterModel _coinCounterModel;
        
        public TowerService(
            ITowerFactory towerFactory, 
            IWeaponFactory weaponFactory,
            TowerDatabase towerDatabase,
            CoinCounterModel coinCounterModel
            )
        {
            _coinCounterModel = coinCounterModel;
            _towerFactory = towerFactory;
            _towerDatabase = towerDatabase;
            _weaponFactory = weaponFactory;
        }
        
        public Tower Create(TowerType towerType, Vector3 position, float damageMultiplier, float attackSpeedMultiplier)
        {
            Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerDatabase.GetConfig(towerType).weaponType, 
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