using System;
using _Project.Scripts.Configs;
using _Project.Scripts.Database.Game;
using _Project.Scripts.Logic.Level.Services.Interfaces;
using _Project.Scripts.Services.TowerUpgrade;
using _Project.Scripts.Towers.Castle;
using UnityEngine;
using Zenject;

namespace _Project.Scripts.Logic.Level.Services
{
    public class CastleService : IInitializable, IDisposable, ICastleService
    {
        public event Action<float> OnDamaged;
        public event Action OnDestroyed;
        
        private readonly ICastleInitializer _initializer;
        private readonly ITowerUpgradeService _upgradeService;
        private readonly GameDatabase _db;

        private ICastleTower _castle;

        public CastleService(
            ICastleInitializer initializer,
            ITowerUpgradeService upgradeService,
            GameDatabase db)
        {
            _initializer = initializer;
            _upgradeService = upgradeService;
            _db = db;
        }

        public void Initialize()
        {
            GameDTO gameConfig = _db.GetConfig(); 
            
            _castle = _initializer.CreateCastle(
                new Vector3(gameConfig.castlePositionX, gameConfig.castlePositionY,  gameConfig.castlePositionZ),
                _upgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_DAMAGE_ID),
                _upgradeService.GetUpgradeMultiplier(TowerUpgradeIdMatcher.CASTLE_ATTACK_SPEED_ID)
            );

            _castle.OnCastleDamaged += OnCastleDamaged;
            _castle.OnCastleDestroy += OnCastleDestroyed;
        }

        public void Dispose()
        {
            _castle.OnCastleDamaged -= OnCastleDamaged;
            _castle.OnCastleDestroy -= OnCastleDestroyed;
        }

        public void Restore() => _castle.RestoreHp();

        private void OnCastleDamaged(float damage) => OnDamaged?.Invoke(damage);
        
        private void OnCastleDestroyed() => OnDestroyed?.Invoke();
    }
}