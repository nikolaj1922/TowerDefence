using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle.States
{
    public class CollapseState: IEnterableState
    {
        private readonly Castle _castle;
        
        public CollapseState(Castle castle) => _castle = castle;

        public void Enter() => _castle.Collapse();

    }
   
}