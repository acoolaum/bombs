using System;
using System.Collections.Generic;
using DG.Tweening;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Logic;
using MyCompany.Game.Model;
using MyCompany.Game.Services;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;

namespace MyCompany.Game.States
{
    public class PlayGameState : GameStateBase
    {
        private Tween _endGameTween;
        public override void Stop()
        {
                     
        }
        protected override void OnStarted()
        {
            var levelDescription = GetLevelDescription();
            ServiceContainer.Add(GameLayerName, new UnityEventProxy());
            ServiceContainer.Add(GameLayerName, new LevelModel(levelDescription));
            ServiceContainer.Add(GameLayerName, new SimpleWallCreatingService());
            ServiceContainer.Add(GameLayerName, new CreatureSpawnService());
            ServiceContainer.Add(GameLayerName, new BombSpawnService());
            ServiceContainer.Add(GameLayerName, new BombMovementService());
            
            ServiceContainer.Add(GameLayerName, new ExplosionCalculationService());
            ServiceContainer.Add(GameLayerName, new ExplosionDamageAssigner());
            ServiceContainer.Add(GameLayerName, new EffectsProcessingService(new Dictionary<Type, IEffectProcessorCreator>()
            {
                {typeof(InstantDamageEffectModel), new InstantDamageProcessorCreator()},
                {typeof(ContinuousDamageEffectModel), new ContinuousDamageProcessorCreator()}
            }));
            ServiceContainer.Add(GameLayerName, new CreatureDiedService());
            
            ServiceContainer.Add(GameLayerName, new LevelDrawerService());
            ServiceContainer.Add(GameLayerName, new CreatureDrawerService());
            ServiceContainer.Add(GameLayerName, new BombDrawerService());
            ServiceContainer.Add(GameLayerName, new ExplosionDrawer());
            ServiceContainer.Add(GameLayerName, new UiDamageDrawerService());
            
            //ServiceContainer.Add(GameLayerName, new DebugExplosionDrawer());

            ServiceContainer.InitializeLayer(GameLayerName);
            
            _endGameTween = DOVirtual.DelayedCall(levelDescription.GameDuration, () =>
            {
                StateMachine.Start(new FinishGameState());
            });
        }

        private LevelDescription GetLevelDescription()
        {
            var levels = DescriptionAccessor.GameDescription.Levels;
            var levelDescription = levels[PlayerModel.GamePlayedCounter % levels.Length];
            return levelDescription;
        }
    }
}