using System;

namespace _Project.Scripts.Logic.Level.Services.Interfaces
{
    public interface ICastleService
    {
        event Action<float> OnDamaged;
        event Action OnDestroyed;
        void Restore();
    }
}