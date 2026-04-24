using System;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace _Project.Scripts.Logic.Level.Services.Interfaces
{
    public interface ILevelUIService
    {
        event Action<int> OnCreateTower;
        void ShowTowerPanel(Vector3 pos);
        UniTask ShowEndModal(string title);
        void ShowContinueForAdsModal();
    }
}