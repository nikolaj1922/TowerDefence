namespace _Project.Scripts.Tower.Weapon
{
    public class Weapon1
    {
        private WeaponAim1 _aim;
        private WeaponAttack1 _attack;
        private WeaponTargetFinder1 _targetFinder;

        public Weapon1(WeaponAim1 aim, WeaponAttack1 attack, WeaponTargetFinder1 finder)
        {
            _aim = aim;
            _attack = attack;
            _targetFinder = finder;
        }

        public void Tick(float deltaTime)
        {
            _aim.Tick(deltaTime);
        }
    }
}