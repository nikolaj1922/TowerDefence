using System.Collections.Generic;

namespace _Project.Scripts.Services.Analytics
{
    public class AnalyticsService : IAnalyticsService
    {
        private readonly IAnalyticsClient _client;
        
        public AnalyticsService(IAnalyticsClient client)
        {
            _client = client;
        }
        
        public void GameStarted(int metaCurrencyTotal)
        {
            _client.LogEvent("game_started", new Dictionary<string, object>
            {
                { "meta_currency_total", metaCurrencyTotal }
            });
        }

        public void WaveStarted(int waveNumber, int enemiesToSpawn)
        {
            _client.LogEvent("wave_started", new Dictionary<string, object>()
            {
                { "wave_number", waveNumber },
                { "enemies_to_spawn", enemiesToSpawn },
            });
        }

        public void WaveCompleted(int waveNumber, int towersBuilt, int coinsRemaining)
        {
            _client.LogEvent("wave_completed", new Dictionary<string, object>()
            {
                { "wave_number", waveNumber },
                { "towers_built", towersBuilt },
                { "coins_remaining", coinsRemaining },
            });
        }

        public void TowerBuilt(int waveNumber, int coinSpent, int coinsRemaining, int towersTotal)
        {
            _client.LogEvent("tower_built", new Dictionary<string, object>()
            {
                { "wave_number", waveNumber },
                { "coins_spent", coinSpent },
                { "coins_remaining", coinsRemaining },
                { "towers_total", towersTotal }
            });
        }

        public void BuildRejected(string reason, int waveNumber)
        {
            _client.LogEvent("build_rejected", new Dictionary<string, object>()
            {
                { "reason", reason },
                { "wave_number", waveNumber }
            });
        }

        public void CastleDamaged(int waveNumber, float castleHpRemaining)
        {
            _client.LogEvent("castle_damaged", new Dictionary<string, object>()
            {
                { "wave_number", waveNumber },
                { "castle_hp_remaining", castleHpRemaining }
            });
        }

        public void GameOver(int wavesSurvived, int enemiesKilled, int towersBuilt, int metaEarned)
        {
            _client.LogEvent("game_over", new Dictionary<string, object>()
            {
                { "waves_survived", wavesSurvived },
                { "enemies_killed", enemiesKilled },
                { "towers_built", towersBuilt },
                { "meta_earned", metaEarned }
            });
        }

        public void SessionRestarted(int wavesSurvived)
        {
            _client.LogEvent("session_restarted", new Dictionary<string, object>()
            {
                { "waves_survived", wavesSurvived }
            });
        }

        public void ReturnedToMenu(int wavesSurvived, int metaCurrencyTotal)
        {
            _client.LogEvent("returned_to_menu", new Dictionary<string, object>()
            {
                { "waves_survived", wavesSurvived },
                { "meta_currency_total", metaCurrencyTotal }
            });
        }
    }
}