using _Project.Scripts.Infrastructure.StateMachine;

namespace _Project.Scripts.Towers.Castle.States
{
    public class CollapseState: IEnterableState
    {
        private readonly CastleTower _castle;
        
        public CollapseState(CastleTower castle) => _castle = castle;

        public void Enter() => _castle.Collapse();

    }
   
}