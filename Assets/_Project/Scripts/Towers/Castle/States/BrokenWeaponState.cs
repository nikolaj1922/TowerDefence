using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle.States
{
    public class BrokenWeaponState: IEnterableState
    {
        private readonly ICastleTower _castle;
        
        public BrokenWeaponState(ICastleTower castle) => _castle = castle;

        public void Enter() => _castle.BreakWeapon();
    }
}