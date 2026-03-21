using Zenject;
using UnityEngine;
using _Project.Scripts.Tower;
using _Project.Scripts.Configs;
using _Project.Scripts.Repositories;
using _Project.Scripts.Enemy.States;
using _Project.Scripts.Database.EnemyDatabase;
using _Project.Scripts.Infrastructure;
using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Enemy
{
    public class EnemyFactory
    {
        private readonly DiContainer _container;
        private readonly Transform _castle;
        private readonly EnemySpawner _enemySpawner;
        private readonly EnemyPrefabsDatabase _enemyPrefabsDatabase;
        private readonly EnemyConfigsRepository _enemyConfigsRepository;

        public EnemyFactory(
            EnemyConfigsRepository enemyConfigsRepository,
            EnemySpawner enemySpawner,
            EnemyPrefabsDatabase enemyPrefabsDatabase,
            CastleController castle,
            DiContainer container)
        {
            _enemySpawner = enemySpawner;
            _enemyConfigsRepository = enemyConfigsRepository;
            _castle = castle.transform;
            _enemyPrefabsDatabase = enemyPrefabsDatabase;
            _container = container;
        }

        public void CreateEnemy(EnemyType type)
        {
            EnemyConfig enemyConfig = _enemyConfigsRepository.ForEnemy(type);
            GameObject enemyPrefab = _enemyPrefabsDatabase.Get(type);
            Vector3 spawnPoint = _enemySpawner.GetRandomSpawnPoint(GameConstants.EnemySpawnOffset);
            
            EnemyController enemyController =  _container.InstantiatePrefabForComponent<EnemyController>(
                enemyPrefab,
                spawnPoint, 
                Quaternion.FromToRotation(spawnPoint, _castle.position),
                null);
            
            EnemyAttack enemyAttack = enemyController.GetComponent<EnemyAttack>();
            EnemyAgentMover enemyMover = enemyController.GetComponent<EnemyAgentMover>();
            EnemyAnimator enemyAnimator = enemyController.GetComponent<EnemyAnimator>();

            StateMachine enemyStateMachine = new(
                new IState[] {
                    new EnemyIdleState(enemyMover, enemyAttack, enemyAnimator), 
                    new EnemyAttackState(enemyAttack),
                    new EnemyDeathState(enemyAnimator),
                    new EnemyMoveState(enemyMover, enemyAnimator),
                    new EnemyVictoryState(enemyMover, enemyAttack, enemyAnimator)
                },
                new ITransition[]
                {
                    new Transition<EnemyIdleState, EnemyMoveState>(() => enemyMover.IsMoving),
                    new Transition<EnemyMoveState, EnemyAttackState>(() => enemyMover.IsCastleReached),
                    new Transition<EnemyIdleState, EnemyDeathState>(() => enemyController.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyMoveState, EnemyDeathState>(() => enemyController.HealthModel.CurrentHealth <= 0),
                    new Transition<EnemyAttackState, EnemyDeathState>(() => enemyController.HealthModel.CurrentHealth <= 0),
                }
                );
            
            
            enemyAttack.Initialize(enemyConfig.damage, enemyConfig.attackCooldown, enemyConfig.attackRange);
            enemyMover.Initialize(enemyConfig.speed, enemyConfig.attackRange, _castle.position);
            
            enemyController.SetStateMachine(enemyStateMachine);
            enemyController.InitHealth(enemyConfig.health);
        }
    }
}