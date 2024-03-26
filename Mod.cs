using Colossal.Logging;
using Colossal.Serialization.Entities;
using Game;
using Game.Modding;
using Game.SceneFlow;
using Unity.Entities;

namespace AchievementEnabler
{
    public class Mod : IMod
    {
        public static ILog log = LogManager.GetLogger($"{nameof(AchievementEnabler)}.{nameof(Mod)}").SetShowsErrorsInUI(false);

        public void OnLoad(UpdateSystem updateSystem)
        {
            log.Info(nameof(OnLoad));

            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out var asset))
                log.Info($"Current mod asset at {asset.path}");

            World.DefaultGameObjectInjectionWorld.CreateSystem<UnlockerSystem>();
        }

        public void OnDispose()
        {
            log.Info(nameof(OnDispose));
        }
    }

    public partial class UnlockerSystem : GameSystemBase
    {
        protected override void OnGameLoadingComplete(Purpose purpose, GameMode mode)
        {
            Colossal.PSI.Common.PlatformManager.instance.achievementsEnabled = true;
        }

        protected override void OnUpdate() 
        { }
    }
}
