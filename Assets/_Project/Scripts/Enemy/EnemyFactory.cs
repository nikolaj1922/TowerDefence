using System;
using Zenject;
using UnityEngine;
using _Project.Scripts.Configs;
using _Project.Scripts.Logic.Coins;
using _Project.Scripts.Enemy.States;
using _Project.Scripts.Logic.Health;
using _Project.Scripts.ConfigRepositories;
using _Project.Scripts.Infrastructure.GameConstants;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy
{
    public class EnemyFactory
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly EnemyPool _orksPool;
        private readonly CoinCounterModel _coinCounterModel;
        private readonly EnemyConfigsRepository _configsRepository;
        private readonly HealthModel _castleHealthModel;
        
        public EnemyFactory(
            [Inject(Id = "CastleHealthModel")] HealthModel healthModel,
            EnemyConfigsRepository configsRepository,
            CoinCounterModel coinCounterModel,
            EnemySpawner enemySpawner,
            EnemyPool orksPool
        )
        {
            _configsRepository = configsRepository;
            _coinCounterModel = coinCounterModel;
            _enemySpawner = enemySpawner;
            _orksPool = orksPool;
            _castleHealthModel = healthModel;
        }

        public void CreateEnemy(EnemyType type, Action onDeath)
        {
            Vector3 spawnPoint = _enemySpawner.GetRandomSpawnPoint(GameConstants.ENEMY_SPAWN_OFFSET);
            Enemy enemy = GetEnemy(type);
            
            enemy.Agent.Warp(spawnPoint);
            Physics.SyncTransforms();
            enemy.ResetComponents();

            if (!enemy.isInitialized)
                InitializeEnemy(enemy, type, onDeath);
            
            if (enemy.StateMachine != null)
                enemy.StateMachine.ResetState();
            else
                enemy.SetStateMachine(CreateStateMachine(enemy));
            
            enemy.gameObject.SetActive(true);
        }

        private void InitializeEnemy(Enemy enemy, EnemyType type, Action onDeath)
        {
            enemy.isInitialized = true;
            enemy.Death.Initialize(onDeath: () => GetEnemyPool(type).Despawn(enemy));
            enemy.HealthModel.OnDeath += onDeath;
            enemy.HealthModel.OnDeath += enemy.Death.Die;
            enemy.HealthModel.OnDeath += () => _coinCounterModel.AddCoins(GetEnemyConfig(type).coinsReward);
        }
        
        private StateMachine CreateStateMachine(Enemy enemy)
        {
            StateMachine enemyStateMachine = new(
                new IState[] {
                    new EnemyMoveState(enemy.Mover, enemy.Agent, enemy.Animator),
                    new EnemyIdleState(enemy.Mover, enemy.Attack, enemy.Animator), 
                    new EnemyAttackState(enemy.Attack),
                    new EnemyDeathState(enemy.Animator, enemy.Agent),
                    new EnemyVictoryState(enemy.Mover, enemy.Attack, enemy.Animator)
                },
                new ITransition[]
                {
                    new Transition<EnemyMoveState, EnemyAttackState>(() => enemy.Mover.IsTargetReached),
                    new Transition<EnemyIdleState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyMoveState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyAttackState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyAttackState, EnemyIdleState>(() => _castleHealthModel.CurrentHealth <= 0),
                    new Transition<EnemyMoveState, EnemyIdleState>(() => _castleHealthModel.CurrentHealth <= 0),
                }
            );

            return enemyStateMachine;
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
                EnemyType.Ork => _configsRepository.Get(EnemyType.Ork),
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
