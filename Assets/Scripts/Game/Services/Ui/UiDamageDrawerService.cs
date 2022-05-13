using System.Collections.Generic;
using System.Linq;
using MyCompany.Core.Ui;
using MyCompany.Game.Descriptions;
using MyCompany.Game.Model;
using MyCompany.Library.Pool;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class UiDamageDrawerService : ServiceBase
    {
        private class DamageViewData
        {
            public int LastHealthValue;
            public float LastShownTime;
        }

        private UiController _uiController;
        private LevelModel _levelModel;
        private GameObjectPool _pool;
        private GameDescriptionAccessor _gameDescription;
        private GameObject _root;
        private readonly Dictionary<CreatureModel, DamageViewData> _damageViewData = new Dictionary<CreatureModel, DamageViewData>();
        private readonly List<DamageView> _views = new List<DamageView>();
        private UnityEventProxy _unityEvent;
        private CreatureDrawerService _creatureDrawer;
        private MainCamera _camera;

        public override void Initialize()
        {
            _uiController = PermanentServices.GetService<UiController>();
            _camera = PermanentServices.GetService<MainCamera>();
            
            _gameDescription = DataServices.GetService<GameDescriptionAccessor>();
            
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.CreatureAdded += HandleCreatureAdded;
            _levelModel.CreatureRemoved += HandlerCreatureRemoved;

            _unityEvent = CurrentServices.GetService<UnityEventProxy>();
            _unityEvent.Updated += HandleUpdated;

            _creatureDrawer = CurrentServices.GetService<CreatureDrawerService>();

            _root = _uiController.CreatePanel("DamageViews");
            _pool = new GameObjectPool(_gameDescription.GameDescription.DamageViewPrefab, _root.transform, 20);
        }

        public override void Dispose()
        {
            foreach (var view in _views)
            {
                view.Dispose();
            }
            
            _pool.Dispose();
            
            Object.Destroy(_root);
            
            _levelModel.CreatureAdded -= HandleCreatureAdded;
            _levelModel.CreatureRemoved -= HandlerCreatureRemoved;
            
            _unityEvent.Updated -= HandleUpdated;
        }

        private void HandleUpdated()
        {
            foreach (var creature in _damageViewData.Keys.ToArray())
            {
                DrawDamageIfNeeded(creature);
            }
        }

        private void DrawDamageIfNeeded(CreatureModel creature, bool forceShow = false)
        {
            var health = creature[EffectTargetProperty.Health];
            var damageViewData = _damageViewData[creature];
            
            var difference = health.Value - damageViewData.LastHealthValue;
            if (difference != 0 && (Time.time - damageViewData.LastShownTime > 0.5f || forceShow))
            {
                StartShowDamageView(creature, difference);
                damageViewData.LastHealthValue = health.Value;
                damageViewData.LastShownTime = Time.time;
            }
        }

        private void StartShowDamageView(CreatureModel creature, int damage)
        {
            var position = _creatureDrawer.GetCreatureTooltipPosition(creature);
            var screenPosition = _camera.WorldToScreenPoint(position);
            var handler = _pool.Get();
            var view = new DamageView(handler);
            view.Completed += HandleViewShowCompleted;
            view.Show(screenPosition, damage.ToString(), 2f);
            _views.Add(view);
        }

        private void HandleViewShowCompleted(DamageView view)
        {
            _views.Remove(view);
        }

        private void HandleCreatureAdded(CreatureModel creature)
        {
            var health = creature[EffectTargetProperty.Health];
            _damageViewData.Add(creature, new DamageViewData {LastShownTime = 0f, LastHealthValue = health.Value});
        }
        
        private void HandlerCreatureRemoved(CreatureModel creature)
        {
            DrawDamageIfNeeded(creature, true);
            _damageViewData.Remove(creature);
        }
    }
}