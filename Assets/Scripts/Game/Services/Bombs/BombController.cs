using System;
using MyCompany.Game.Model;

namespace MyCompany.Game.Services
{
    public class BombController
    {
        public event Action<BombModel> Exploded; 
        
        private readonly BombModel _model;
        private BombBehaviour _behaviour;
        public BombController(BombModel model)
        {
            _model = model;
            _behaviour = new MoveToTargetBombBehaviour(this);
        }

        public bool IsExploded => _model.IsExploded;

        public void Process(float fixedDeltaTime)
        {
            if (_behaviour == null)
            {
                return;
            }

            _behaviour = _behaviour.Process(fixedDeltaTime);
        }
        
        private void InvokeExplode()
        {
            Exploded?.Invoke(_model);
        }

        private abstract class BombBehaviour
        {
            protected readonly BombController Controller;
            protected BombModel Model => Controller._model;
            protected BombBehaviour(BombController controller)
            {
                Controller = controller;
            }

            public abstract BombBehaviour Process(float fixedDeltaTime);
        }
        
        private class MoveToTargetBombBehaviour : BombBehaviour
        {
            public MoveToTargetBombBehaviour(BombController controller): base(controller)
            {
                Model.CurrentTime = 0f;
                Model.TotalMoveDuration = (Model.TargetPosition - Model.StartPosition).magnitude / Model.Description.Speed;
                Model.Changed = true;
            }

            public override BombBehaviour Process(float fixedDeltaTime)
            {
                Model.CurrentTime += fixedDeltaTime;
                Model.Changed = true;
                return Model.CurrentTime >= Model.TotalMoveDuration
                    ? (BombBehaviour) new ExplodeBombBehaviour(Controller)
                    : this;
            }
        }
        
        private class ExplodeBombBehaviour : BombBehaviour
        {
            public ExplodeBombBehaviour(BombController controller): base(controller)
            {
                Controller.InvokeExplode();
                Model.Changed = true;
            }

            public override BombBehaviour Process(float fixedDeltaTime)
            {
                return null;
            }
        }
    }
}