using System;
using System.Collections.Generic;
using MyCompany.Game.Model;
using MyCompany.Library.Services;
using MyCompany.Library.UnityEvent;
using UnityEngine;

namespace MyCompany.Game.Services
{
    public class EffectsProcessingService : ServiceBase
    {
        private readonly Dictionary<Type, IEffectProcessorCreator> _registeredProcessors;
        private readonly List<IEffectProcessor> _records = new List<IEffectProcessor>();
        private readonly List<IEffectProcessor> _markedToRemoveRecords = new List<IEffectProcessor>();
        
        private LevelModel _levelModel;
        private UnityEventProxy _unityEvent;

        public EffectsProcessingService(Dictionary<Type, IEffectProcessorCreator> registeredProcessors)
        {
            _registeredProcessors = registeredProcessors;
        }

        public override void Initialize()
        {
            _levelModel = CurrentServices.GetService<LevelModel>();
            _levelModel.EffectAssigned += HandleEffectAssigned;
            _levelModel.EffectRemoved += HandleEffectRemoved;

            _unityEvent = CurrentServices.GetService<UnityEventProxy>();
            _unityEvent.FixedUpdated += HandleFixedUpdate;
        }
        
        public override void Dispose()
        {
            _levelModel.EffectAssigned -= HandleEffectAssigned;
            _levelModel.EffectRemoved -= HandleEffectRemoved;
            _unityEvent.FixedUpdated -= HandleFixedUpdate;
        }

        private void HandleFixedUpdate()
        {
            float fixedDeltaTime = Time.fixedDeltaTime;
            
            foreach (var record in _records)
            {
                if (record.Process(fixedDeltaTime) == false)
                {
                    _markedToRemoveRecords.Add(record);
                }
            }

            foreach (var record in _markedToRemoveRecords)
            {
                record.Remove();
            }
            
            _markedToRemoveRecords.Clear();
        }
        
        private void HandleEffectAssigned(EffectModelBase effectModel)
        {
            var modelType = effectModel.GetType();
            if (_registeredProcessors.TryGetValue(modelType, out var creator) == false)
            {
                Debug.LogError($"Effect processor for type {modelType} not found");
                return;
            }

            var record = creator.Create(effectModel);
            _records.Add(record);
        }

        private void HandleEffectRemoved(EffectModelBase effectModel)
        {
            for (var i = 0; i < _records.Count; i++)
            {
                if (_records[i].EffectModel == effectModel)
                {
                    _records.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public interface IEffectProcessorCreator
    {
        IEffectProcessor Create(EffectModelBase effectModel);
    }

    public interface IEffectProcessor
    {
        EffectModelBase EffectModel { get; }
        bool Process(float deltaTime);
        void Remove();
    }
}