using System;
using UnityEngine;
using _Project.Scripts.Services.NetworkChecker;
using Cysharp.Threading.Tasks;

namespace _Project.Scripts.Services.SaveLoad
{
    public class SaveLoad : ISaveLoad
    {
        private bool _cloudSaveInitialized;
        private readonly INetworkChecker _networkChecker;
        private readonly ILocalSaveService _localSaveService;
        private readonly IRemoteSaveService _remoteSaveService;
        public PlayerProgress PlayerProgress { get; private set; }

        public SaveLoad(
            INetworkChecker networkChecker, 
            ILocalSaveService localSaveService,
            IRemoteSaveService remoteSaveService)
        {
            _networkChecker = networkChecker;
            _localSaveService = localSaveService;
            _remoteSaveService = remoteSaveService;
        }

        public async UniTask InitializeRemoteSave()
        {
            bool isNetworkAvailable = await _networkChecker.CheckNetwork();

            if (!isNetworkAvailable)
                return;
            
            await _remoteSaveService.Initialize();

            _cloudSaveInitialized = true;
            Debug.Log($"Cloud save initialized successfully");
        }
        
        public async UniTask SaveProgress()
        {
            bool isNetworkAvailable = await _networkChecker.CheckNetwork();
            
            PlayerProgress.updatedAtTimeStamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            
            string json = ProgressToJson();
            _localSaveService.Save(json);

            if (_cloudSaveInitialized && isNetworkAvailable)
                await _remoteSaveService.Save(json);
        }
        
        public async UniTask LoadProgress()
        {
            bool isNetworkAvailable = await _networkChecker.CheckNetwork();
            
            if (isNetworkAvailable)
            {
                PlayerProgress remoteProgress = await _remoteSaveService.Load();

                if (remoteProgress == null)
                {
                    PlayerProgress = _localSaveService.Load();
                    return;
                }

                PlayerProgress localProgress = _localSaveService.GetProgress();

                if (localProgress == null)
                {
                    PlayerProgress = remoteProgress;
                    _localSaveService.Save(ProgressToJson());
                    return;
                }

                if (CheckIsLocalProgressFresher(localProgress, remoteProgress))
                {
                    PlayerProgress = localProgress;
                    await _remoteSaveService.Save(ProgressToJson());
                    return;
                }
                
                PlayerProgress = remoteProgress;
                _localSaveService.Save(ProgressToJson());
            }
            else
            {
                PlayerProgress = _localSaveService.Load();
            }
        }

        private string ProgressToJson() => JsonUtility.ToJson(PlayerProgress);
        
        private bool CheckIsLocalProgressFresher(PlayerProgress localProgress, PlayerProgress cloudProgress) =>
            localProgress.updatedAtTimeStamp > cloudProgress.updatedAtTimeStamp;
    }
}