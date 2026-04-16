namespace _Project.Scripts.Infrastructure.Constants
{
    public static class GameConstants
    {
        public const float PANEL_OFFSET = 200f;
        public const string MENU_SCENE = "Menu";
        public const string LEVEL_SCENE = "Level";
        public const float ENEMY_SPAWN_OFFSET = 1f;
        public const float TOWER_PLACEMENT_RAYCAST_DISTANCE = 100f;
        public const float MAX_ANGLE_TO_ATTACK = 10f;
        public const float ATTACK_RECOIL = 0.03f;
        public const float CASTLE_COLLAPSE_HEALTH_PERCENT = 0.4f;
        public const string PLAYER_PROGRESS = "PlayerProgress";
        public const string WEAPON_BASE_INJECT_ID = "WeaponBase";
        public const string WEAPON_HEAD_INJECT_ID = "WeaponHead";
        public const string PROJECTILE_POINT_INJECT_ID = "ProjectileSpawnPoint";

        public const string ENEMY_CONFIG_ASSET_LABEL = "EnemyConfig";
        public const string TOWER_CONFIG_ASSET_LABEL = "TowerConfig";
        public const string WEAPON_CONFIG_ASSET_LABEL = "WeaponConfig";
        public const string GAME_CONFIG_ASSET_LABEL = "GameConfig";
        
        public const string LOADING_CURTAIN_INJECT_ID = "LoadingCurtain";
    }
}