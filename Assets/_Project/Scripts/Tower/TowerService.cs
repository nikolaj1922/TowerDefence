using UnityEngine;
using _Project.Scripts.Weapon;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.UI.CreateTowerPanel;

namespace _Project.Scripts.Tower
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
        
        public Tower Create(TowerType towerType, Vector3 position)
        {
            Tower tower = _towerFactory.CreateTower(towerType, position);
            Weapon.Weapon weapon = 
                _weaponFactory.CreateWeapon(
                    _towerConfigsRepository.Get(towerType).weaponType, 
                    tower.WeaponPoint.transform.position, 
                    tower.WeaponPoint.transform);
            
            tower.SetWeapon(weapon);
            
            return tower;
        }

        public Tower CreateAndPurchase(TowerType towerType, Vector3 position, int coinPrice)
        {
            Tower tower = Create(towerType, position);
            Purchase(coinPrice);

            return tower;
        }
        
        private void Purchase(int coinPrice) => _coinCounterModel.RemoveCoins(coinPrice);
    }
}