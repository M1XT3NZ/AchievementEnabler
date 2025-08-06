using Colossal.Logging;
using Colossal.PSI.Common;
using Colossal.Serialization.Entities;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Unity.Entities;

namespace AchievementEnabler
{
    public class Mod : IMod
    {
        public static ILog _log = LogManager.GetLogger($"{nameof(AchievementEnabler)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {
            _log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                _log.Info($"Current mod asset at {asset.path}");

            World.DefaultGameObjectInjectionWorld.CreateSystem<UnlockerSystem>();
        }

        public void OnDispose()
        {
            _log.Info(nameof(OnDispose));
        }
    }

    public partial class UnlockerSystem : GameSystemBase
    {
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            PlatformManager.instance.achievementsEnabled = true;
            PlatformManager.instance.onAchievementUpdated += (backend, id) =>
            {
                Mod._log.Info($"Achievement updated: {id}");
                PlatformManager.instance.GetAchievement(id, out var achi);
                PlatformManager.instance.IndicateAchievementProgress(id,achi.progress);
            };
            Mod._log.Info("Achievements enabled");
        }

        protected override void OnUpdate() 
        { }
    }
}