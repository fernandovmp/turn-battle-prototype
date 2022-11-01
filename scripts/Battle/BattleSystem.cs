using Godot;
using Rpg2d.UI.Battle;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Rpg2d.Battle
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
        private BattleUi _battleUi;
        private IActionDispatcher _actionDispatcher = new ActionDispatcher();
        private bool _partyTurn;
        private EnemyTroopResource _troop;

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
            _battleUi = GetNode<BattleUi>("../CanvasLayer");
            _targetSelector.SelectedTargetChanged += _battleUi.UpdateTargetHud;
            _targetSelector.EnableChanged += _battleUi.ShowTargetHud;
            SetupTroop(context.Troop);
            _battleUi.InitUnitHuds(EnumerateUnits());
            _actionDispatcher.AllActionsFinished += AllActionFinished;
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
                var enemyNode = _enemyModel.Instance();
                _enemiesRoot.AddChild(enemyNode);
                var node2D = enemyNode.GetNode<Node2D>("AnimatedSprite");
                var enemySlot = enemyNode.GetNode<EnemySlot>(".");
                node2D.Position = troop.Positions[i];
                enemySlot.SetEnemy(troop.Enemies[i]);
                enemySlot.ActionDispatcher = _actionDispatcher;
                enemySlot.Died += _targetSelector.Next;
                enemySlot.DamageRecived += _battleUi.DisplayDamageText;
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
            _partyTurn = true;
            _targetSelector.First();
        }

        public override void _Input(InputEvent inputEvent)
        {
        }

        public void AllActionFinished()
        {
            IEnumerable<IBattlerSlot> slots;
            Action callback;
            if (_partyTurn)
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

        private async void StartEnemyTurn()
        {
            _partyTurn = false;
            _targetSelector.Enabled = false;
            await ToSignal(GetTree().CreateTimer(1), "timeout");
            foreach (var enemy in _enemies)
            {
                enemy.ActionEnabled = true;
            }
            var ai = _troop.AI;
            var actions = ai.GetActions(new BattleContext
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
                    foreach (var target in action.TargetGroup.GetTargets())
                    {
                        action.Skill.Cast(new Skills.CastContext
                        {
                            Caster = action.Owner,
                            Skill = action.Skill,
                            Target = target
                        });
                    }
                }
            }
        }

        public IEnumerable<UnitSlot> EnumerateUnits() => EnumerateAllUnits().Where(unit => unit.HasUnit);

        public IEnumerable<UnitSlot> EnumerateAllUnits()
        {
            yield return _leftUnit;
            yield return _upUnit;
            yield return _rightUnit;
            yield return _bottomUnit;
        }
    }
}
