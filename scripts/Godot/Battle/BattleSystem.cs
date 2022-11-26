using Godot;
using Rpg2d.Battle;
using Rpg2d.Battle.Actions;
using Rpg2d.Battle.Actors;
using Rpg2d.Godot.Battle.Actors;
using Rpg2d.Skills;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Godot.Battle
{
    public class BattleSystem : Node
    {

        private UnitSlot _leftUnit;
        private UnitSlot _upUnit;
        private UnitSlot _rightUnit;
        private UnitSlot _bottomUnit;
        private Node _enemiesRoot;
        [Export]
        private PackedScene _enemyModel;
        private List<EnemySlot> _enemies = new List<EnemySlot>();
        private TargetSelector _targetSelector;
        private IActionDispatcher _actionDispatcher = new ActionDispatcher();
        private EnemyTroopResource _troop;
        private BattlePhaseEnum _phase;
        public BattlePhaseEnum Phase
        {
            get => _phase;
            set
            {
                _phase = value;
                PhaseChanged?.Invoke(value);
            }
        }
        public Action<BattlePhaseEnum> PhaseChanged;
        public TargetSelector TargetSelector => _targetSelector;
        public IEnumerable<EnemySlot> Enemies => _enemies;

        public void Init(BattleSystemContext context)
        {
            var partyNode = GetNode("../Party");
            _enemiesRoot = GetNode("../Enemies");
            _leftUnit = partyNode.GetNode<UnitSlot>("LeftUnit");
            _upUnit = partyNode.GetNode<UnitSlot>("UpUnit");
            _rightUnit = partyNode.GetNode<UnitSlot>("RightUnit");
            _bottomUnit = partyNode.GetNode<UnitSlot>("BottomUnit");
            SetupUnit(_leftUnit, context.PartyLeftUnit);
            SetupUnit(_upUnit, context.PartyUpUnit);
            SetupUnit(_rightUnit, context.PartyRightUnit);
            SetupUnit(_bottomUnit, context.PartyBottomUnit);
            _targetSelector = GetNode<TargetSelector>("../TargetSelector");

            SetupTroop(context.Troop);
            _actionDispatcher.AllActionsFinished += AllActionFinished;
            Phase = BattlePhaseEnum.Initialization;
            StartPartyTurn();
        }

        private void SetupUnit(UnitSlot unit, UnitResource unitResource)
        {
            if (unitResource != null)
            {
                unit.SetUnit(unitResource);
            }
            unit.ActionDispatcher = _actionDispatcher;
        }

        private void SetupTroop(EnemyTroopResource troop)
        {
            _troop = troop;
            for (int i = 0; i < troop.Enemies.Length; i++)
            {
                var enemySlot = _enemyModel.Instance<EnemySlot>();
                _enemiesRoot.AddChild(enemySlot);
                enemySlot.Position = troop.Positions[i];
                enemySlot.SetEnemy(troop.Enemies[i]);
                enemySlot.ActionDispatcher = _actionDispatcher;
                enemySlot.Died += _targetSelector.Next;
                _enemies.Add(enemySlot);
            }
            _targetSelector.Init(_enemies);
        }

        private void EnemyActionFinished(BattleAction obj)
        {
            if (!_enemies.Any(unit => unit.CanAct))
            {
                StartPartyTurn();
            }
        }

        private void StartPartyTurn()
        {
            foreach (var unit in EnumerateUnits())
            {
                unit.ActionEnabled = true;
            }
            _targetSelector.Enabled = true;
            _targetSelector.First();
            Phase = BattlePhaseEnum.PartyTurn;
        }

        public void AllActionFinished()
        {
            if (EnumerateUnits().All(unit => unit.IsDead))
            {
                EndBattle(BattlePhaseEnum.EnemyVictory);
            }
            else if (_enemies.All(unit => unit.IsDead))
            {
                EndBattle(BattlePhaseEnum.PartyVictory);
            }
            if (Phase == BattlePhaseEnum.PartyTurn || Phase == BattlePhaseEnum.EnemyTurn)
            {
                IEnumerable<IBattlerSlot> slots;
                Action callback;
                if (Phase == BattlePhaseEnum.PartyTurn)
                {
                    slots = EnumerateUnits();
                    callback = StartEnemyTurn;
                }
                else
                {
                    slots = _enemies;
                    callback = StartPartyTurn;
                }
                if (!slots.Any(unit => unit.CanAct || unit.IsActing))
                {
                    callback();
                }
            }
        }

        private void EndBattle(BattlePhaseEnum phase)
        {
            foreach (var unit in EnumerateUnits())
            {
                unit.ActionEnabled = false;
            }
            TargetSelector.Enabled = false;
            Phase = phase;
        }

        private async void StartEnemyTurn()
        {
            Phase = BattlePhaseEnum.EnemyTurn;
            _targetSelector.Enabled = false;
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            foreach (var enemy in _enemies)
            {
                enemy.ActionEnabled = true;
            }
            var ai = _troop.AI;
            var actions = ai.GetActions(new ActionContext
            {
                Enemies = _enemies,
                Party = EnumerateUnits()
            });
            while (actions.MoveNext())
            {
                var action = actions.Current;
                if (action is WaitAction waitAction)
                {
                    await ToSignal(GetTree().CreateTimer(waitAction.Seconds), "timeout");
                }
                else
                {
                    action.Owner.PerformAction(action);
                }
            }
        }

        public IEnumerable<UnitSlot> EnumerateUnits() => EnumerateAllUnits().Where(unit => unit.HasUnit);

        public IEnumerable<UnitSlot> EnumerateAllUnits()
        {
            yield return _upUnit;
            yield return _rightUnit;
            yield return _leftUnit;
            yield return _bottomUnit;
        }
    }
}
