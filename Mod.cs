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
        public static ILog LOG = LogManager.GetLogger($"{nameof(AchievementEnabler)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {
            LOG.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                LOG.Info($"Current mod asset at {asset.path}");

            World.DefaultGameObjectInjectionWorld.CreateSystem<UnlockerSystem>();
        }

        public void OnDispose()
        {
            LOG.Info(nameof(OnDispose));
        }
    }

    public partial class UnlockerSystem : GameSystemBase
    {
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            PlatformManager.instance.achievementsEnabled = true;
            PlatformManager.instance.onAchievementUpdated += (backend, id) =>
            {
                Mod.LOG.Info($"Achievement updated: {id}");
                PlatformManager.instance.GetAchievement(id, out var achi);
                PlatformManager.instance.IndicateAchievementProgress(id,achi.progress);
            };
            Mod.LOG.Info("Achievements enabled");
        }

        protected override void OnUpdate() 
        { }
    }
}