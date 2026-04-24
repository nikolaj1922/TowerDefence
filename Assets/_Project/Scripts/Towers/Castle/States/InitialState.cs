using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle.States
{
    public class InitialState : IEnterableState
    {
        private readonly ICastleTower _castle;
        
        public InitialState(ICastleTower castle) => _castle = castle;

        public void Enter() => _castle.RestoreWeapon();
    }
}

