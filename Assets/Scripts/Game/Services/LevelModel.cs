using System;
using System.Collections.Generic;
using MyCompany.Core.Data;
using MyCompany.Game.Descriptions;
using MyCompany.Library.Services;
using UnityEngine;
using Random = UnityEngine.Random;

namespace MyCompany.Game.Model
{
    public class LevelModel : ServiceBase, IWallChecker
    {
        public event Action<CreatureModel> CreatureAdded;
        public event Action<CreatureModel> CreatureRemoved; 
        
        public event Action<EffectModelBase> EffectAssigned;
        public event Action<EffectModelBase> EffectRemoved; 

        public event Action<BombModel> BombAdded;
        public event Action<BombModel> BombExploded; 
        
        public LevelDescription LevelDescription { get; }
        public MapData<bool> WallCells { get; }

        private readonly List<CreatureModel> _creatures = new List<CreatureModel>();
        private readonly List<BombModel> _bombs = new List<BombModel>();
        
        private int _occupiedByWallCellsCount;
        
        public LevelModel(LevelDescription description)
        {
            LevelDescription = description;
            WallCells = new MapData<bool>(LevelDescription.Size);
            WallCells.ElementChanged += HandleWallCellsElementChanged;
        }

        public Vector2Int GetFreeRandomCell()
        {
            var size = WallCells.Size;
            var index = Random.Range(0, size.x * size.y - _occupiedByWallCellsCount);
           
            var currentIndex = -1;
            for (int x = 0; x < size.x; x++)
            {
                for (int y = 0; y < size.y; y++)
                {
                    if (WallCells[x, y] == false)
                    {
                        currentIndex++;
                        if (index == currentIndex)
                        {
                            return new Vector2Int(x, y);
                        }
                    }
                }
            }
            
            throw new InvalidOperationException ("Free random cell not found");
        }
        
        public CreatureModel AddCreature(CreatureDescription creatureDescription, Vector2Int position)
        {
            var creature = new CreatureModel
            {
                Description = creatureDescription,
                Position = position
            };
            
            creature.AddProperty(new CreatureModelProperty
                {Type = EffectTargetProperty.Health, Value = creatureDescription.MaxHealth});
            creature.AddProperty(new CreatureModelProperty
                {Type = EffectTargetProperty.Speed, Value = creatureDescription.MaxSpeed});
            
            creature.EffectAssigned += HandleEffectAssigned;
            creature.EffectRemoved += HandleEffectRemoved;
            _creatures.Add(creature);
            CreatureAdded?.Invoke(creature);
            return creature;
        }
        
        public void RemoveCreature(CreatureModel creature)
        {
            _creatures.Remove(creature);
            creature.EffectAssigned -= HandleEffectAssigned;
            creature.EffectRemoved -= HandleEffectRemoved;
            CreatureRemoved?.Invoke(creature);
        }
        
        public void AddBomb(BombDescription bombDescription, Vector2Int position)
        {
            var bombModel = new BombModel
            {
                Description = bombDescription,
                TargetPosition = position,
                StartPosition = new Vector2Int(Random.Range(0, WallCells.Size.x), WallCells.Size.y + 1),
                Changed = true
            };
            _bombs.Add(bombModel);
            BombAdded?.Invoke(bombModel);
        }

        public void ExplodeBomb(BombModel bombModel)
        {
            bombModel.IsExploded = true;
            bombModel.Changed = true;
            BombExploded?.Invoke(bombModel);
        }
        
        public bool IsWallHere(int mapPositionX, int mapPositionY)
        {
            if (mapPositionX < 0 || mapPositionY < 0 ||
                mapPositionX > WallCells.Size.x - 1 ||
                mapPositionY > WallCells.Size.y - 1)
            {
                return true;
            }

            return WallCells[mapPositionX, mapPositionY];
        }
        
        public bool IsCreatureInCell(Vector2Int mapCell)
        {
            foreach (var creature in _creatures)
            {
                if (creature.Position == mapCell)
                {
                    return true;
                }
            }

            return false;
        }
        
        public List<CreatureModel> GetCreaturesInCircle(Vector2Int centerInMapCoords, int radiusInMapCoords)
        {
            int sqrtRadius = radiusInMapCoords * radiusInMapCoords;
            var result = new List<CreatureModel>(); 
            foreach (var creature in _creatures)
            {
                if ((creature.Position - centerInMapCoords).sqrMagnitude <= sqrtRadius)
                {
                    result.Add(creature);
                }
            }

            return result;
        }

        private void HandleEffectAssigned(EffectModelBase effect)
        {
            EffectAssigned?.Invoke(effect);
        }
        
        private void HandleEffectRemoved(EffectModelBase effect)
        {
            EffectRemoved?.Invoke(effect);
        }

        private void HandleWallCellsElementChanged(MapData<bool>.MapDataElementChangedArgs args)
        {
            if (args.NewValue)
            {
                _occupiedByWallCellsCount++;
            }
            else
            {
                _occupiedByWallCellsCount--;
            }
        }
    }

    public interface IWallChecker
    {
        bool IsWallHere(int mapPositionX, int mapPositionY);
    }
}