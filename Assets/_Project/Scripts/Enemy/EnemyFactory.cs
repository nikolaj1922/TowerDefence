using System;
using UnityEngine;
using _Project.Scripts.Enemy.States;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy
{
    public class EnemyFactory
    {
        private readonly EnemySpawner _enemySpawner;
        private readonly EnemyPool _orksPool;

        public EnemyFactory(
            EnemySpawner enemySpawner,
            EnemyPool orksPool
        )
        {
            _enemySpawner = enemySpawner;
            _orksPool = orksPool;
        }

        public void CreateEnemy(EnemyType type, Action onDeath)
        {
            Vector3 spawnPoint = _enemySpawner.GetRandomSpawnPoint(GameConstants.EnemySpawnOffset);
            Enemy enemy = GetEnemy(type);

            enemy.Agent.Warp(spawnPoint);
            Physics.SyncTransforms();
            
            enemy.Death.Initialize(onDeath: () => GetEnemyPool(type).Despawn(enemy));
            enemy.HealthModel.OnDeath += onDeath;
            enemy.HealthModel.OnDeath += enemy.Death.Die;
            
            enemy.SetStateMachine(CreateStateMachine(enemy));
        }

        private StateMachine CreateStateMachine(Enemy enemy)
        {
            StateMachine enemyStateMachine = new(
                new IState[] {
                    new EnemyIdleState(enemy.Mover, enemy.Attack, enemy.Animator), 
                    new EnemyAttackState(enemy.Attack),
                    new EnemyDeathState(enemy.Animator, enemy.Agent),
                    new EnemyMoveState(enemy.Mover, enemy.Animator),
                    new EnemyVictoryState(enemy.Mover, enemy.Attack, enemy.Animator)
                },
                new ITransition[]
                {
                    new Transition<EnemyIdleState, EnemyMoveState>(() => enemy.Mover.IsMoving),
                    new Transition<EnemyMoveState, EnemyAttackState>(() => enemy.Mover.IsCastleReached),
                    new Transition<EnemyIdleState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyMoveState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyAttackState, EnemyDeathState>(() => enemy.HealthModel.CurrentHealth <= 0),
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