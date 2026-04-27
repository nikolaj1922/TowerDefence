namespace _Project.Scripts.Infrastructure.Constants
{
    public static class GameConstants
    {
        public const string MENU_SCENE = "Menu";
        public const string LEVEL_SCENE = "Level";
        
        public const float PANEL_OFFSET = 200f;
        public const float ENEMY_SPAWN_OFFSET = 1f;
        public const float TOWER_PLACEMENT_RAYCAST_DISTANCE = 100f;
        public const float MAX_ANGLE_TO_ATTACK = 10f;
        public const float ATTACK_RECOIL = 0.03f;
        public const float CASTLE_COLLAPSE_HEALTH_PERCENT = 0.4f;
        public const int MAX_META_COIN_COUNT = 10_000;
        public const string PLAYER_PROGRESS = "PlayerProgress";
        
        public const string WEAPON_BASE_INJECT_ID = "WeaponBase";
        public const string WEAPON_HEAD_INJECT_ID = "WeaponHead";
        public const string PROJECTILE_POINT_INJECT_ID = "ProjectileSpawnPoint";
        public const string LOADING_CURTAIN_ASSET_INJECT_ID = "LoadingCurtain";
        
        public const string REWARDED_ADS_ID = "cbnurzmrqzqb21hb";
        public const string INTERSTITIAL_ADS_ID = "w5vixbeaunmzmjq5";
        public const string LEVEL_PLAY_APP_ID = "261f1e15d";
        
        public const string ENEMY_REMOTE_CONFIG_KEY = "Enemies";
        public const string TOWER_REMOTE_CONFIG_KEY = "Towers";
        public const string WEAPON_REMOTE_CONFIG_KEY = "Weapons";
        public const string GAME_REMOTE_CONFIG_KEY = "Game";
        public const string WAVES_REMOTE_CONFIG_KEY = "Waves";
        public const string UPGRADES_REMOTE_CONFIG_KEY = "Upgrades";
    }
}