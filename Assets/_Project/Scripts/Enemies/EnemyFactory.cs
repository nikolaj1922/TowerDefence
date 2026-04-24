using System;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Enemies.States;
using _Project.Scripts.Database.Enemies;
using _Project.Scripts.Enemies.Behaviour;
using _Project.Scripts.Infrastructure.Constants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemies
{
    public class EnemyFactory : IEnemyFactory
    {
        private readonly EnemyPool _orksPool;
        private readonly IEnemySpawner _enemySpawner;
        private readonly EnemyDatabase _enemyDatabase;

        private Action _onDeathCached;
        private Action _addCoinsCached;

        public EnemyFactory(
            EnemyPool orksPool,
            IEnemySpawner enemySpawner,
            EnemyDatabase enemyDatabase
        )
        {
            _orksPool = orksPool;
            _enemySpawner = enemySpawner;
            _enemyDatabase = enemyDatabase;
        }

        public void CreateEnemy(EnemyType type, Action onDeath)
        {
            Vector3 spawnPoint = _enemySpawner.GetRandomSpawnPoint(GameConstants.ENEMY_SPAWN_OFFSET);
            Enemy enemy = GetEnemy(type);
            
            enemy.Agent.Warp(spawnPoint);
            Physics.SyncTransforms();
            enemy.ResetComponents();

            if (!enemy.IsInitialized)
                InitializeEnemy(enemy, type, onDeath);
            
            if (enemy.StateMachine != null)
                enemy.StateMachine.ResetState();
            else
                enemy.SetStateMachine(CreateStateMachine(enemy));
            
            enemy.gameObject.SetActive(true);
        }

        public void StopActiveEnemies()
        {
            foreach (var enemy in _orksPool.ActiveEnemies)
                enemy.ToIdle();   
        }

        public void DespawnAllEnemies()
        {
            foreach (EnemyType enemyType in Enum.GetValues(typeof(EnemyType)))
                GetEnemyPool(enemyType).DespawnAll();
        }
        
        private StateMachine CreateStateMachine(Enemy enemy)
        {
            StateMachine enemyStateMachine = new(
                new IState[] {
                    new EnemyMoveState(enemy.Mover, enemy.Agent, enemy.Animator),
                    new EnemyIdleState(enemy.Mover, enemy.Attack, enemy.Animator), 
                    new EnemyAttackState(enemy.Attack),
                    new EnemyDeathState(enemy.Animator, enemy.Agent),
                },
                new ITransition[]
                {
                    new Transition<EnemyMoveState, EnemyAttackState>(() => enemy.Mover.IsTargetReached),
                    new Transition<EnemyIdleState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyMoveState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyAttackState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0)
                }
            );

            return enemyStateMachine;
        }

        private void InitializeEnemy(Enemy enemy, EnemyType type, Action onDeath)
        {
            enemy.Initialize(onDeath, GetEnemyConfig(type).CoinsReward);
            enemy.Death.Initialize(onDeath: () => GetEnemyPool(type).Despawn(enemy));
            enemy.SetInitialized();
        }

        private Enemy GetEnemy(EnemyType type)
        {
            return type switch
            {
                EnemyType.Ork => _orksPool.Spawn(),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown enemy type")
            };
        }

        private EnemyConfig GetEnemyConfig(EnemyType type)
        {
            return type switch
            {
                EnemyType.Ork => _enemyDatabase.GetConfig(EnemyType.Ork),
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown enemy type")
            };
        }

        private EnemyPool GetEnemyPool(EnemyType type)
        {
            return type switch
            {
                EnemyType.Ork => _orksPool,
                _ => throw new ArgumentOutOfRangeException(nameof(type), type, "Unknown enemy type")
            };
        }
    }
}
